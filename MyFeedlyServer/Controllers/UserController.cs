using Contracts;
using Microsoft.AspNetCore.Mvc;
using MyFeedlyServer.Models;
using System;
using System.Linq;

namespace MyFeedlyServer.Controllers
{
    [Route("api/user")]
    public class UserController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;

        public UserController(ILoggerManager logger, IRepositoryWrapper repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet]
        public IActionResult GetAllUsers()
        {
            try
            {
                var users = _repository.User.GetAllUsers().Select(u => new UserModel(u));

                _logger.LogInfo($"Returned all {nameof(users)} from database.");

                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside {nameof(GetAllUsers)} action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            try
            {
                var user = new UserModel(_repository.User.GetUserById(id));

                if (user.IsNull())
                {
                    _logger.LogError($"{nameof(user)} with id: {id}, hasn't been found in db.");
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo($"Returned {nameof(user)} with id: {id}");
                    return Ok(user);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong inside {nameof(GetUserById)} action: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}
