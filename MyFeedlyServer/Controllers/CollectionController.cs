using System;
using System.Linq;
using Contracts;
using Microsoft.AspNetCore.Mvc;
using MyFeedlyServer.Models;

namespace MyFeedlyServer.Controllers
{
    [Route("api/collection")]
    public class CollectionController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;

        public CollectionController(ILoggerManager logger, IRepositoryWrapper repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet("user/{userId}")]
        public IActionResult GetCollectionsByUserId(int userId)
        {
            try
            {
                var collections = _repository.Collection.FindByCondition(c => c.User.Id == userId).Select(c => new CollectionModel(c));

                _logger.LogInfo($"Returned {nameof(collections)} by {nameof(userId)}: {userId}");

                return Ok(collections);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside {nameof(GetCollectionsByUserId)} action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
