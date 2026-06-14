using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TravelQuotesApi.Interfaces;
using TravelQuotesApi.Models;

namespace TravelQuotesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuotesController : ControllerBase
    {
        private readonly IRepository<Quote> _quoteRepository;

        public QuotesController(IRepository<Quote> quoteRepository)
        {
            _quoteRepository = quoteRepository;
        }

        // GET: api/Quotes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Quote>>> GetQuotes()
        {
            var quotes = await _quoteRepository.GetAllAsync();
            return Ok(quotes);
        }

        // GET: api/Quotes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Quote>> GetQuote(int id)
        {
            var quote = await _quoteRepository.GetByIdAsync(id);
            return Ok(quote);
        }

        // POST: api/Quotes
        [HttpPost]
        public async Task<ActionResult<Quote>> PostQuote(Quote quote)
        {
            await _quoteRepository.AddAsync(quote);
            return CreatedAtAction(nameof(GetQuote), new { id = quote.Id }, quote);
        }

        // PUT: api/Quotes/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> PutQuote(int id, Quote quote)
        {
            if (id != quote.Id)
            {
                return BadRequest();
            }

            await _quoteRepository.UpdateAsync(quote);
            return NoContent();
        }

        // DELETE: api/Quotes/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteQuote(int id)
        {
            var quote = await _quoteRepository.GetByIdAsync(id);
            await _quoteRepository.DeleteAsync(id);
            return NoContent();
        }
    }
}
