using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI_Identity.Entities;

namespace WebAPI_Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IssuesController : ControllerBase
    {
        private readonly DataContext _context;

        public IssuesController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Issues
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Issue>>> GetIssue()
        {
            return await _context.Issue.ToListAsync();
        }

        // GET: api/Issues/5
        [HttpGet("{Id}")]
        public async Task<ActionResult<Issue>> GetIssue(int Id)
        {
            var issue = await _context.Issue.FindAsync(Id);

            if (issue == null)
            {
                return NotFound();
            }

            return issue;
        }

        // PUT: api/Issues/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{Id}")]
        public async Task<IActionResult> PutIssue(int Id, Issue issue)
        {
            if (Id != issue.Id)
            {
                return BadRequest();
            }

            _context.Entry(issue).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IssueExists(Id))
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

        // POST: api/Issues
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Issue>> PostIssue(Issue issue)
        {
            _context.Issue.Add(issue);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetIssue", new { id = issue.Id }, issue);
        }

        // DELETE: api/Issues/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Issue>> DeleteIssue(int Id)
        {
            var issue = await _context.Issue.FindAsync(Id);
            if (issue == null)
            {
                return NotFound();
            }

            _context.Issue.Remove(issue);
            await _context.SaveChangesAsync();

            return issue;
        }

        private bool IssueExists(int Id)
        {
            return _context.Issue.Any(e => e.Id == Id);
        }
    }
}
