using System;
using System.Linq;
using Contracts;
using Contracts.Repositories;
using Entities;
using Entities.Extensions;
using Entities.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFeedlyServer.Extensions;
using MyFeedlyServer.Resources;

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
            var users = _repository.User.GetAllUsers().Select(u => new UserGetModel(u));

            _logger.LogInfo(Resource.LogInfoGetAllUsers);
            return Ok(users);
        }

        [Authorize]
        [HttpGet("{id}", Name = nameof(GetUserById))]
        public IActionResult GetUserById(int id)
        {
            UserGetModel user;

            if (!this.IsSameUser(id))
            {
                _logger.LogError(string.Format(Resource.LogErrorGetByIsNull, nameof(user), nameof(id), id));
                return NotFound();
            }

            user = new UserGetModel(_repository.User.GetUserById(id));
            if (user.IsNull())
            {
                _logger.LogError(string.Format(Resource.LogErrorGetByIsNull, nameof(user), nameof(id), id));
                return NotFound();
            }

            _logger.LogInfo(string.Format(Resource.LogInfoGetById, nameof(user), id));
            return Ok(user);
        }

        [Authorize]
        [HttpGet("{id}/collection")]
        public IActionResult GetUserWithCollections(int id)
        {
            UserWithCollectionsGetModel user;

            if (!this.IsSameUser(id))
            {
                _logger.LogError(string.Format(Resource.LogErrorGetByIsNull, nameof(user), nameof(id), id));
                return NotFound();
            }

            user = new UserWithCollectionsGetModel(_repository.User.GetUserById(id));

            if (user.IsNull())
            {
                _logger.LogError(string.Format(Resource.LogErrorGetByIsNull, nameof(user), nameof(id), id));
                return NotFound();
            }

            _logger.LogInfo(string.Format(Resource.LogInfoGetById, nameof(user), id));
            return Ok(user);
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody]UserCreateOrUpdateModel user)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError(string.Format(Resource.LogErrorInvalidModel, nameof(user), ModelState.GetAllErrors()));
                return BadRequest(Resource.Status400BadRequestInvalidModel);
            }

            _repository.User.CreateUser(user.GetEntity());

            return CreatedAtRoute(nameof(GetUserById), new { id = user.Id }, new EntityModel<User>(user.GetEntity()));
        }

        [Authorize]
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody]UserCreateOrUpdateModel user)
        {
            if (!this.IsSameUser(id))
            {
                _logger.LogError(string.Format(Resource.LogErrorGetByIsNull, nameof(user), nameof(id), id));
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                _logger.LogError(string.Format(Resource.LogErrorInvalidModel, nameof(user), ModelState.GetAllErrors()));
                return BadRequest(Resource.Status400BadRequestInvalidModel);
            }

            var dbUser = _repository.User.GetUserById(id);
            if (dbUser.IsNull())
            {
                _logger.LogError(string.Format(Resource.LogErrorGetByIsNull, nameof(user), nameof(id), id));
                return NotFound();
            }

            _repository.User.UpdateUser(dbUser, user.GetEntity());

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            User user;

            if (!this.IsSameUser(id))
            {
                _logger.LogError(string.Format(Resource.LogErrorGetByIsNull, nameof(user), nameof(id), id));
                return NotFound();
            }

            user = _repository.User.GetUserById(id);
            if (user.IsNull())
            {
                _logger.LogError(string.Format(Resource.LogErrorGetByIsNull, nameof(user), nameof(id), id));
                return NotFound();
            }

            _repository.User.DeleteUser(user);

            return NoContent();
        }
    }
}
