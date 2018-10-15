using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using MyFeedlyServer.Contracts;
using MyFeedlyServer.Contracts.Repositories;
using MyFeedlyServer.Entities.Contracts;
using MyFeedlyServer.Entities.Extensions;
using MyFeedlyServer.Filters;
using MyFeedlyServer.Models;
using MyFeedlyServer.Models.Extensions;
using MyFeedlyServer.Resources;
using Swashbuckle.AspNetCore.Annotations;

namespace MyFeedlyServer.Controllers
{
    [SwaggerTag("Create, read, update and delete Users")]
    [Route("api/user")]
    public class UserController : BaseController
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly IDataProtector _dataProtector;

        public UserController(ILoggerManager logger, IRepositoryWrapper repository, IDataProtectionProvider dataProtectionProvider)
        {
            _logger = logger;
            _repository = repository;
            _dataProtector = dataProtectionProvider.CreateProtector(GetDataProtectionPurpose());
        }

        [SwaggerOperation(
            Summary = "Get all users",
            Description = "Get all users",
            OperationId = "GetAllUsers"
        )]
        [SwaggerResponse((int)HttpStatusCode.OK, "List of users", typeof(UserGetModel))]
        [HttpGet("all", Name = nameof(GetAllUsers))]
        public IActionResult GetAllUsers()
        {
            var users = _repository.User.GetAllUsers().Select(u => new UserGetModel(u));

            _logger.LogInfo(Resource.LogInfoGetAllUsers);
            return Ok(users);
        }

        [SwaggerOperation(
            Summary = "Get authorized user",
            Description = "Get authorized user",
            OperationId = "GetUser"
        )]
        [SwaggerResponse((int)HttpStatusCode.OK, "Info about authorized user", typeof(UserGetModel))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Authorized user hasn't been found in db")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, "User hasn't been authorized")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [Authorize]
        [HttpGet]
        public IActionResult GetUser()
        {
            var autorizedUserId = AuthorizedUserId;

            var user = new UserGetModel(_repository.User.GetUserById(autorizedUserId));
            if (user.IsNull())
            {
                _logger.LogError(string.Format(Resource.LogErrorGetByIsNull, nameof(user), nameof(autorizedUserId), autorizedUserId));
                return NotFound();
            }

            _logger.LogInfo(string.Format(Resource.LogInfoGetById, nameof(user), autorizedUserId));
            return Ok(user);
        }

        [SwaggerOperation(
            Summary = "Get authorized user with collections",
            Description = "Get authorized user with collections",
            OperationId = "GetUserWithCollections"
        )]
        [SwaggerResponse((int)HttpStatusCode.OK, "Info about authorized user and his collections", typeof(UserWithCollectionsGetModel))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Authorized user hasn't been found in db")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, "User hasn't been authorized")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [Authorize]
        [HttpGet("collection")]
        public IActionResult GetUserWithCollections()
        {
            var autorizedUserId = AuthorizedUserId;

            var user = new UserWithCollectionsGetModel(_repository.User.GetUserById(autorizedUserId));
            if (user.IsNull())
            {
                _logger.LogError(string.Format(Resource.LogErrorGetByIsNull, nameof(user), nameof(autorizedUserId), autorizedUserId));
                return NotFound();
            }

            _logger.LogInfo(string.Format(Resource.LogInfoGetById, nameof(user), autorizedUserId));
            return Ok(user);
        }

        [SwaggerOperation(
            Summary = "Create user",
            Description = "Create user",
            OperationId = "CreateUser"
        )]
        [SwaggerResponse((int)HttpStatusCode.Created, "User created successfully", typeof(EntityGetModel<IEntity>))]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [HttpPost]
        public IActionResult CreateUser([FromBody]UserCreateOrUpdateModel user)
        {
            var entity = user.GetEntity();
            entity.Password = _dataProtector.Protect(entity.Password);

            _repository.User.CreateUser(entity);

            return CreatedAtRoute(nameof(GetAllUsers), new { id = user.Id }, new EntityGetModel<IEntity>(entity));
        }

        [SwaggerOperation(
            Summary = "Update user",
            Description = "Update user",
            OperationId = "UpdateUser"
        )]
        [SwaggerResponse((int)HttpStatusCode.NoContent, "User updated successfully")]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Authorized user hasn't been found in db")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, "User hasn't been authorized")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [Authorize]
        [HttpPut]
        public IActionResult UpdateUser([FromBody]UserCreateOrUpdateModel user)
        {
            var autorizedUserId = AuthorizedUserId;

            var dbUser = _repository.User.GetUserById(autorizedUserId);
            if (dbUser.IsNull())
            {
                _logger.LogError(string.Format(Resource.LogErrorGetByIsNull, nameof(user), nameof(autorizedUserId), autorizedUserId));
                return NotFound();
            }

            var entity = user.GetEntity();
            entity.Password = _dataProtector.Protect(entity.Password);

            _repository.User.UpdateUser(dbUser, entity);

            return NoContent();
        }

        [SwaggerOperation(
            Summary = "Delete user",
            Description = "Delete user",
            OperationId = "DeleteUser"
        )]
        [SwaggerResponse((int)HttpStatusCode.NoContent, "User deleted successfully")]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Authorized user hasn't been found in db")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, "User hasn't been authorized")]
        [Authorize]
        [HttpDelete]
        public IActionResult DeleteUser()
        {
            var autorizedUserId = AuthorizedUserId;

            var user = _repository.User.GetUserById(autorizedUserId);
            if (user.IsNull())
            {
                _logger.LogError(string.Format(Resource.LogErrorGetByIsNull, nameof(user), nameof(autorizedUserId), autorizedUserId));
                return NotFound();
            }

            _repository.User.DeleteUser(user);

            return NoContent();
        }
    }
}
