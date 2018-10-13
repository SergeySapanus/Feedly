using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyFeedlyServer.Contracts;
using MyFeedlyServer.Contracts.Repositories;
using MyFeedlyServer.Entities.Entities;
using MyFeedlyServer.Entities.Extensions;
using MyFeedlyServer.Entities.Models;
using MyFeedlyServer.Filters;
using MyFeedlyServer.Resources;

namespace MyFeedlyServer.Controllers
{
    [Route("api/user")]
    public class UserController : BaseController
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;

        public UserController(ILoggerManager logger, IRepositoryWrapper repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet("all", Name = nameof(GetAllUsers))]
        public IActionResult GetAllUsers()
        {
            var users = _repository.User.GetAllUsers().Select(u => new UserGetModel(u));

            _logger.LogInfo(Resource.LogInfoGetAllUsers);
            return Ok(users);
        }

        [Authorize]
        [HttpGet]
        public IActionResult GetUser()
        {
            var autorizedUserId = AuthorizedUserId;

            if (!autorizedUserId.HasValue)
            {
                _logger.LogError(Resource.LogErrorUserIsNotAutorized);
                return Unauthorized();
            }

            var user = new UserGetModel(_repository.User.GetUserById(autorizedUserId.Value));
            if (user.IsNull())
            {
                _logger.LogError(string.Format(Resource.LogErrorGetByIsNull, nameof(user), nameof(autorizedUserId), autorizedUserId.Value));
                return NotFound();
            }

            _logger.LogInfo(string.Format(Resource.LogInfoGetById, nameof(user), autorizedUserId));
            return Ok(user);
        }

        [Authorize]
        [HttpGet("collection")]
        public IActionResult GetUserWithCollections()
        {
            var autorizedUserId = AuthorizedUserId;

            if (!autorizedUserId.HasValue)
            {
                _logger.LogError(Resource.LogErrorUserIsNotAutorized);
                return Unauthorized();
            }

            var user = new UserWithCollectionsGetModel(_repository.User.GetUserById(autorizedUserId.Value));

            if (user.IsNull())
            {
                _logger.LogError(string.Format(Resource.LogErrorGetByIsNull, nameof(user), nameof(autorizedUserId), autorizedUserId));
                return NotFound();
            }

            _logger.LogInfo(string.Format(Resource.LogInfoGetById, nameof(user), autorizedUserId));
            return Ok(user);
        }

        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [HttpPost]
        public IActionResult CreateUser([FromBody]UserCreateOrUpdateModel user)
        {
            _repository.User.CreateUser(user.GetEntity());

            return CreatedAtRoute(nameof(GetAllUsers), new { id = user.Id }, new EntityModel<User>(user.GetEntity()));
        }

        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [Authorize]
        [HttpPut]
        public IActionResult UpdateUser([FromBody]UserCreateOrUpdateModel user)
        {
            var autorizedUserId = AuthorizedUserId;

            if (!autorizedUserId.HasValue)
            {
                _logger.LogError(Resource.LogErrorUserIsNotAutorized);
                return Unauthorized();
            }

            var dbUser = _repository.User.GetUserById(autorizedUserId.Value);
            if (dbUser.IsNull())
            {
                _logger.LogError(string.Format(Resource.LogErrorGetByIsNull, nameof(user), nameof(autorizedUserId), autorizedUserId.Value));
                return NotFound();
            }

            _repository.User.UpdateUser(dbUser, user.GetEntity());

            return NoContent();
        }

        [Authorize]
        [HttpDelete]
        public IActionResult DeleteUser()
        {
            var autorizedUserId = AuthorizedUserId;

            if (!autorizedUserId.HasValue)
            {
                _logger.LogError(Resource.LogErrorUserIsNotAutorized);
                return Unauthorized();
            }

            var user = _repository.User.GetUserById(autorizedUserId.Value);
            if (user.IsNull())
            {
                _logger.LogError(string.Format(Resource.LogErrorGetByIsNull, nameof(user), nameof(autorizedUserId), autorizedUserId.Value));
                return NotFound();
            }

            _repository.User.DeleteUser(user);

            return NoContent();
        }
    }
}
