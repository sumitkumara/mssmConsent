using Microsoft.AspNetCore.Mvc;
using MssmConsent.Models.ViewModel;
using MssmConsent.ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MssmConsent.Controllers
{
    [Route("api/Consents")]
    [ApiController]
    public class ConsentsApiController : ControllerBase
    {
        private readonly IConsentService _consentService;

        public ConsentsApiController(IConsentService consentService)
        {
            _consentService = consentService;
        }


        // GET api/<ConsentsApiController>/5
        [HttpGet]
        [Route("{id}/LanguageId/{languageId?}")]
        [Route("{id}")]
        public async Task<IActionResult> Get(int id, int? languageId)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            var result = await _consentService.GetConsent(id, languageId);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // POST api/<ConsentsApiController>
        [HttpPost]
        public async Task<IActionResult> Post(ConsentViewModel consent)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);
            var result = await _consentService.AddConsent(consent);
            if (result == null)
                return null;

            return CreatedAtAction(nameof(Get), new { id = result.ID }, result);
        }

        // PUT api/<ConsentsApiController>/5
        [HttpPut]
        [Route("{id}/LanguageId/{languageId?}")]
        [Route("{id}")]
        public async Task<IActionResult> Put(int id, int? languageId, [FromBody] ConsentViewModel consent)
        {
            if (!ModelState.IsValid)
                return ValidationProblem(ModelState);
            if (id != consent.ID)
                return BadRequest("Url and request body consent id's don't match");

            if (languageId.HasValue)
            {
                if (languageId != consent.LanguageID)
                    return BadRequest("Url and request body language id's don't match");

                var response = await _consentService.AddConsentLanguage(consent);
                if (response == null) return null;
                return CreatedAtAction(nameof(Get), new { id = response.ID }, response);
            }

            var result = await _consentService.UpdateConsent(consent);
            if (result == null)
                return null;

            return CreatedAtAction(nameof(Get), new { id = result.ID }, result);
        }


        // DELETE api/<ConsentsApiController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }

            await _consentService.DeleteConsent(id);

            return Ok($"Consent with id = {id} is deleted successfully");
        }
    }
}
