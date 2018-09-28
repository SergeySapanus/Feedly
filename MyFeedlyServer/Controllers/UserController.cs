using System;
using System.Linq;
using Contracts;
using Entities.Models;
using Entities.Extensions;
using Microsoft.AspNetCore.Mvc;
using MyFeedlyServer.Extensions;
using MyFeedlyServer.Models;
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
                var users = _repository.User.GetAllUsers().Select(u => new UserModel(u));

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
                var user = new UserModel(_repository.User.GetUserById(id));

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
                var user = new UserWithCollectionsModel(_repository.User.GetUserById(id));

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
        public IActionResult CreateUser([FromBody]User user)
        {
            try
            {
                if (user.IsNull())
                {
                    _logger.LogError(string.Format(Resource.LogErrorObjectIsNull, nameof(user)));
                    return BadRequest(string.Format(Resource.Status400BadRequestObjectIsNull, nameof(user)));
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError(string.Format(Resource.LogErrorInvalidModel, nameof(user), ModelState.GetAllErrors()));
                    return BadRequest(Resource.Status400BadRequestInvalidModel);
                }

                _repository.User.CreateUser(user);

                return CreatedAtRoute(nameof(GetUserById), new { id = user.Id }, new EntityModel<User>(user));
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(Resource.LogErrorException, nameof(CreateUser), ex.InnerException?.Message ?? ex.Message));

                return StatusCode(500, Resource.Status500);
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, [FromBody]User user)
        {
            try
            {
                if (user.IsNull())
                {
                    _logger.LogError(string.Format(Resource.LogErrorObjectIsNull, nameof(user)));
                    return BadRequest(string.Format(Resource.Status400BadRequestObjectIsNull, nameof(user)));
                }

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

                _repository.User.UpdateUser(dbUser, user);

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
