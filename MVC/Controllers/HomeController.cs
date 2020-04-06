using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MVC.Models;
using MVC.Models.Case;
using MVC.Models.Customer;
using MVC.Models.User;
using Newtonsoft.Json;

namespace MVC.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }



        public async Task<IActionResult> Index()
        {

            using (var client = new HttpClient())
            {
                var principal = HttpContext.User as ClaimsPrincipal;
                var token = principal?.Claims.FirstOrDefault(c => c.Type == "token").Value;
                var role = principal?.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role).Value;

                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                // GET CASES
                var caseResponse = await client.GetAsync("http://localhost:63539/api/customercases");
                var resultCases = JsonConvert.DeserializeObject<IEnumerable<IssueModel>>(await caseResponse.Content.ReadAsStringAsync());

                // GET CUSTOMER
                var customerResponse = await client.GetAsync("http://localhost:63539/api/customers");
                var resultCustomers = JsonConvert.DeserializeObject<IEnumerable<CustomerModel>>(await customerResponse.Content.ReadAsStringAsync());

                ViewBag.Cases = resultCases;
                ViewBag.Customers = resultCustomers;
                ViewBag.Role = role;

                return View();
            }



        }



















        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterModel model)
        {
            using (var client = new HttpClient())
            {
                HttpContent request = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("http://localhost:63539/api/users/register", request);

                if (response.IsSuccessStatusCode)
                    return RedirectToAction("Login");

                return View();
            }
        }


        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(UserLoginModel model)
        {

            using (var client = new HttpClient())
            {
                HttpContent request = new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json");
                var response = await client.PostAsync("http://localhost:63539/api/users/login", request);

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeObject<UserLoginResponseModel>(await response.Content.ReadAsStringAsync());

                    var claims = new List<Claim>()
                    {
                        new Claim("userId", result.Id.ToString()),
                        new Claim(ClaimTypes.Email, result.Email),
               
                        new Claim("token", result.Token)
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    var authenticationProperties = new AuthenticationProperties();

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity), authenticationProperties);

                    return RedirectToAction("Index");
                }

                return BadRequest();

            }
        }


        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index");
        }







        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
