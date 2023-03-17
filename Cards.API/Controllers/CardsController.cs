using Cards.API.Interfaces;
using Cards.API.Interfaces.RepositoryChilds;
using Cards.API.Models.ContextModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Cards.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CardsController : Controller
    {
        private readonly ICardRepository _cardRep;
        private readonly ILogger<CardsController> _logger;
        public CardsController(ICardRepository cardRep, ILogger<CardsController> logger)
        {
            _cardRep = cardRep;
            _logger = logger;
        }

        //Get all cards
        [HttpGet]
        public async Task<IActionResult> GetAllCards()
        {
            try
            {
                var cards = await _cardRep.GetAll();
                return Ok(cards);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500);
            }
        }

        //Get one card
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetCard(Guid id)
        {
            try
            {
                var cards = await _cardRep.Get(id);
                if (cards != null)
                    return Ok(cards);
                else
                    return NotFound("Card not found");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500);
            }
        }

        //Create card
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddCard([FromBody] Card value)
        {
            try
            {
                value.Id = Guid.NewGuid();
                await _cardRep.Post(value);
                return CreatedAtAction(nameof(GetCard), new { id = value.Id }, value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500);
            }
        }

        //Update card
        [HttpPut]
        [Route("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCard(Guid id, [FromBody] Card card)
        {
            try
            {
                await _cardRep.Put(id, card);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500);
            }
        }

        //Delete card
        [HttpDelete]
        [Route("{id:guid}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCard(Guid id)
        {
            try
            {
                await _cardRep.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(500);
            }
        }
    }
}
