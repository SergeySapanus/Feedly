using System;
using System.Linq;
using Contracts;
using Entities.Concrete;
using Entities.Extensions;
using Entities.Model;
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
            try
            {
                var users = _repository.User.GetAllUsers().Select(u => new UserGetModel(u));

                _logger.LogInfo(Resource.LogInfoGetAllUsers);

                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(Resource.LogErrorException,
                    nameof(GetAllUsers),
                    ex.InnerException?.Message ?? ex.Message));

                return StatusCode(500, Resource.Status500);
            }
        }

        [HttpGet("{id}", Name = nameof(GetUserById))]
        public IActionResult GetUserById(int id)
        {
            try
            {
                var user = new UserGetModel(_repository.User.GetUserById(id));

                if (user.IsNull())
                {
                    _logger.LogError(string.Format(Resource.LogErrorGetByIdIsNull, nameof(user), id));
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo(string.Format(Resource.LogInfoGetById, nameof(user), id));
                    return Ok(user);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(Resource.LogErrorException, nameof(GetUserById), ex.InnerException?.Message ?? ex.Message));
                return StatusCode(500, Resource.Status500);
            }
        }

        [HttpGet("{id}/collection")]
        public IActionResult GetUserWithCollections(int id)
        {
            try
            {
                var user = new UserWithCollectionsGetModel(_repository.User.GetUserById(id));

                if (user.IsNull())
                {
                    _logger.LogError(string.Format(Resource.LogErrorGetByIdIsNull, nameof(user), id));
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo(string.Format(Resource.LogInfoGetById, nameof(user), id));
                    return Ok(user);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(Resource.LogErrorException, nameof(GetUserWithCollections), ex.InnerException?.Message ?? ex.Message));

                return StatusCode(500, Resource.Status500);
            }
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody]UserCreateOrUpdateModel user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError(string.Format(Resource.LogErrorInvalidModel, nameof(user), ModelState.GetAllErrors()));
                    return BadRequest(Resource.Status400BadRequestInvalidModel);
                }

                _repository.User.CreateUser(user.GetEntity());

                return CreatedAtRoute(nameof(GetUserById), new { id = user.Id }, new EntityModel<User>(user.GetEntity()));
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(Resource.LogErrorException, nameof(CreateUser), ex.InnerException?.Message ?? ex.Message));

                return StatusCode(500, Resource.Status500);
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody]UserCreateOrUpdateModel user)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError(string.Format(Resource.LogErrorInvalidModel, nameof(user), ModelState.GetAllErrors()));
                    return BadRequest(Resource.Status400BadRequestInvalidModel);
                }

                var dbUser = _repository.User.GetUserById(id);
                if (dbUser.IsNull())
                {
                    _logger.LogError(string.Format(Resource.LogErrorGetByIdIsNull, nameof(user), id));
                    return NotFound();
                }

                _repository.User.UpdateUser(dbUser, user.GetEntity());

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(Resource.LogErrorException, nameof(CreateUser), ex.InnerException?.Message ?? ex.Message));

                return StatusCode(500, Resource.Status500);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            try
            {
                var user = _repository.User.GetUserById(id);
                if (user.IsNull())
                {
                    _logger.LogError(string.Format(Resource.LogErrorGetByIdIsNull, nameof(user), id));
                    return NotFound();
                }

                _repository.User.DeleteUser(user);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(Resource.LogErrorException, nameof(CreateUser), ex.InnerException?.Message ?? ex.Message));

                return StatusCode(500, Resource.Status500);
            }
        }
    }
}
