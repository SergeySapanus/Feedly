﻿using System;
using Contracts;
using Entities.Extensions;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using MyFeedlyServer.Extensions;
using MyFeedlyServer.Models;
using MyFeedlyServer.Resources;

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

        [HttpGet("{id}", Name = nameof(GetCollectionById))]
        public IActionResult GetCollectionById(int id)
        {
            try
            {
                var collection = new CollectionModel(_repository.Collection.GetCollectionById(id));

                if (collection.IsNull())
                {
                    _logger.LogError(string.Format(Resource.LogErrorGetByIdIsNull, nameof(collection), id));
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo(string.Format(Resource.LogInfoGetById, nameof(collection), id));
                    return Ok(collection);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(Resource.LogErrorException, nameof(GetCollectionById), ex.InnerException?.Message ?? ex.Message));
                return StatusCode(500, Resource.Status500);
            }
        }

        [HttpPost]
        public IActionResult CreateCollection([FromBody]Collection collection)
        {
            try
            {
                if (collection.IsNull())
                {
                    _logger.LogError(string.Format(Resource.LogErrorObjectIsNull, nameof(collection)));
                    return BadRequest(string.Format(Resource.Status400BadRequestObjectIsNull, nameof(collection)));
                }

                var user = _repository.User.GetUserById(collection.UserId);
                if (user.IsNull())
                {
                    _logger.LogError(string.Format(Resource.LogErrorGetByIdIsNull, nameof(user), collection.UserId));
                    return NotFound();
                }

                if (!ModelState.IsValid)
                {
                    _logger.LogError(string.Format(Resource.LogErrorInvalidModel, nameof(collection), ModelState.GetAllErrors()));
                    return BadRequest(Resource.Status400BadRequestInvalidModel);
                }

                _repository.Collection.CreateCollection(collection);

                return CreatedAtRoute(nameof(GetCollectionById), new { id = collection.Id }, new EntityModel<Collection>(collection));
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(Resource.LogErrorException, nameof(CreateCollection), ex.InnerException?.Message ?? ex.Message));

                return StatusCode(500, Resource.Status500);
            }
        }
    }
}
