using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Blockchain.Investments.Core.Infrastructure;
using Blockchain.Investments.Core.Domain;
using Blockchain.Investments.Core.Repositories;

namespace Blockchain.Investments.Api.Controllers
{
    [Route("api/[controller]")]
    public class SecurityController : Controller
    {
        private readonly ILogger<SecurityController> _logger;
        private IRepository<Security> _repo;
        
        public SecurityController (ILogger<SecurityController> logger, IRepository<Security> repo)
        {
            _logger = logger;
            _repo = repo;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<Security> Get()
        {
            _logger.LogInformation(LoggingEvents.LIST_ITEMS, "Listing all items");
            return _repo.FindAll();
        }

        // GET api/values/5
        [HttpGet("{id:length(24)}")]
        public IActionResult Get(string id)
        {
            _logger.LogInformation(LoggingEvents.GET_ITEM, "Getting item {0}", id);
            
            var currency = _repo.FindByObjectId(id);
            if (currency == null)
            {
                _logger.LogWarning(LoggingEvents.GET_ITEM_NOTFOUND, "GetById({ID}) NOT FOUND", id);
                return NotFound();
            }
            return new ObjectResult(currency);
        }

        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody]Security security)
        {
            if (security == null)
            {
                return BadRequest();
            }
            var createdCurrency = _repo.Create(security);
            _logger.LogInformation(LoggingEvents.INSERT_ITEM, "Item {0} Created", createdCurrency.UniqueId);
            
            return CreatedAtRoute("default", new { id = security.UniqueId}, security);
        }

        // PUT api/values/5
        [HttpPut]
        public IActionResult Put([FromBody]Security security)
        {
            if (security == null || string.IsNullOrEmpty(security.UniqueId))
            {
                return BadRequest();
            }

            var currentMarket = _repo.FindByObjectId(security.UniqueId);
            if (currentMarket == null)
            {
                _logger.LogWarning(LoggingEvents.GET_ITEM_NOTFOUND, "Update({0}) NOT FOUND", security.UniqueId);
                return NotFound();
            }
            
            _repo.Update(security.UniqueId, security);
            _logger.LogInformation(LoggingEvents.UPDATE_ITEM, "Item {0} Updated", security.UniqueId);
            return Accepted(security);
        }

        // DELETE api/values/5
        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var currency = _repo.FindByObjectId(id);
            if (currency == null)
            {
                return NotFound();
            }
 
            _repo.Remove(id);
            _logger.LogInformation(LoggingEvents.DELETE_ITEM, "Item {0} Deleted", id);
            return new OkResult();
        }
    }
}
