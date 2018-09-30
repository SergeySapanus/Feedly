using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Contracts.Repositories;
using Entities;
using Entities.Extensions;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using MyFeedlyServer.Extensions;
using MyFeedlyServer.Resources;
using NewsModel = Entities.Models.NewsModel;

namespace MyFeedlyServer.Controllers
{
    [Route("api/collection")]
    public class CollectionController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;
        private readonly ISyndicationManager _syndicationManager;

        public CollectionController(ILoggerManager logger, IRepositoryWrapper repository, ISyndicationManager syndicationManager)
        {
            _logger = logger;
            _repository = repository;
            _syndicationManager = syndicationManager;
        }

        [HttpGet("{id}", Name = nameof(GetCollectionById))]
        public IActionResult GetCollectionById(int id)
        {
            try
            {
                var collection = new CollectionGetModel(_repository.Collection.GetCollectionById(id));

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

        [HttpGet("{id}/news", Name = nameof(GetNewsByCollectionId))]
        public IActionResult GetNewsByCollectionId(int id)
        {
            try
            {
                var collection = _repository.Collection.GetCollectionById(id);
                if (collection.IsNull())
                {
                    _logger.LogError(string.Format(Resource.LogErrorGetByIdIsNull, nameof(collection), id));
                    return NotFound();
                }

                var result = _repository.Feed.GetNewsByCollection(collection, _syndicationManager).Select(f => new NewsModel(f));

                _logger.LogInfo(string.Format(Resource.LogInfoGetById, nameof(collection), id));
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(Resource.LogErrorException, nameof(GetCollectionById), ex.InnerException?.Message ?? ex.Message));
                return StatusCode(500, Resource.Status500);
            }
        }

        [HttpPost]
        public IActionResult CreateCollection([FromBody]CollectionCreateOrUpdateModel collection)
        {
            try
            {
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

                _repository.Collection.CreateCollection(collection.GetEntity());

                return CreatedAtRoute(nameof(GetCollectionById), new { id = collection.Id }, new EntityModel<Collection>(collection.GetEntity()));
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(Resource.LogErrorException, nameof(CreateCollection), ex.InnerException?.Message ?? ex.Message));

                return StatusCode(500, Resource.Status500);
            }
        }
    }
}
