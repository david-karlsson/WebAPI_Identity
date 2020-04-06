using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI_Identity.Entities;
using Microsoft.AspNetCore.Authorization;
using WebAPI_Identity.Models;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http;
using System.Net.Http.Headers;

namespace WebAPI_Identity.Controllers
{


    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DataContext _context;



        public IConfiguration Configuration { get; }


        public UsersController(DataContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<User>> Register([FromBody]RegisterModel model)
        {


            if(_context.Users.Any(user =>user.Email == model.Email))
            return BadRequest();

            try {

                var user = new User()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email

                };

                user.CreatePasswordHash(model.Password);

                _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok();


            }

            catch
            {
                return BadRequest();
            }

        }





        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ActionResult<User>> Login([FromBody]LoginModel model)
        {


            if (string.IsNullOrEmpty(model.Email) || string.IsNullOrEmpty(model.Password))
                return BadRequest("Email or user not found");
            var user = await _context.Users.SingleOrDefaultAsync(user => user.Email == model.Email);

            if (user == null)
                return BadRequest("user not found");

            if (!user.VerifyPasswordHash(model.Password))
                return BadRequest("passwords don't match");


            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(Configuration.GetSection("Secret").Value);
            var tokenDescpriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString())
                }),

                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)

            };
            var token = tokenHandler.CreateToken(tokenDescpriptor);
            var tokenString = tokenHandler.WriteToken(token);


            var baseAddress = new Uri("https://localhost:44318/api/");

            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {
                httpClient.DefaultRequestHeaders.Add("Authorization", "Bearer " + "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IjEiLCJuYmYiOjE1ODQ2MTA4NDUsImV4cCI6MTU4NTIxNTY0NSwiaWF0IjoxNTg0NjEwODQ1fQ.zamhOsuQIX3eXlx5AvouegneZ1hmp2WkeMBwBdZ_nbo");


                using (var response = await httpClient.GetAsync("user/list{?organizationId}"))
                {
                    string responseData = await response.Content.ReadAsStringAsync();
                }


                using (var requestMessage =
            new HttpRequestMessage(HttpMethod.Get, "https://localhost:44318/api/"))
                {
                    requestMessage.Headers.Authorization =
                        new AuthenticationHeaderValue("Bearer", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IjEiLCJuYmYiOjE1ODQ2MTA4NDUsImV4cCI6MTU4NTIxNTY0NSwiaWF0IjoxNTg0NjEwODQ1fQ.zamhOsuQIX3eXlx5AvouegneZ1hmp2WkeMBwBdZ_nbo");
                    await httpClient.SendAsync(requestMessage);
                }







                return Ok(
                new
                {
                    id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Token = tokenString
                }

                );





            }
        }

        //public override Task<AuthenticationState> GetAuthenticationStateAsync()
        //{
        //    var identity = new ClaimsIdentity(new[]
        //    {
        //        new Claim(ClaimTypes.Name, "mrfibuli"),
        //    }, "Fake authentication type");

        //    var user = new ClaimsPrincipal(identity);

        //    return Task.FromResult(new AuthenticationState(user));
        //}




        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return user;
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }




    }



}
