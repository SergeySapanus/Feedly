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
    [Route("api/collection")]
    public class CollectionController : BaseController
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

        [Authorize]
        [HttpGet("{id}", Name = nameof(GetCollectionById))]
        public IActionResult GetCollectionById(int id)
        {
            var autorizedUserId = AuthorizedUserId;

            if (!autorizedUserId.HasValue)
            {
                _logger.LogError(Resource.LogErrorUserIsNotAutorized);
                return Unauthorized();
            }

            var collection = new CollectionGetModel(_repository.Collection.GetCollectionByIdAndUserId(id, autorizedUserId.Value));
            if (collection.IsNull())
            {
                _logger.LogError(string.Format(Resource.LogErrorGetByIsNull, nameof(collection), nameof(id), id));
                return NotFound();
            }

            _logger.LogInfo(string.Format(Resource.LogInfoGetById, nameof(collection), id));
            return Ok(collection);
        }

        [Authorize]
        [HttpGet("{id}/news", Name = nameof(GetNewsByCollectionId))]
        public IActionResult GetNewsByCollectionId(int id)
        {
            var autorizedUserId = AuthorizedUserId;

            if (!autorizedUserId.HasValue)
            {
                _logger.LogError(Resource.LogErrorUserIsNotAutorized);
                return Unauthorized();
            }

            var collection = _repository.Collection.GetCollectionByIdAndUserId(id, autorizedUserId.Value);
            if (collection.IsNull())
            {
                _logger.LogError(string.Format(Resource.LogErrorGetByIsNull, nameof(collection), nameof(id), id));
                return NotFound();
            }

            var result = _repository.Feed.GetNewsByCollection(collection, _syndicationManager).Select(f => new NewsModel(f));

            _logger.LogInfo(string.Format(Resource.LogInfoGetById, nameof(collection), id));
            return Ok(result);
        }

        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [Authorize]
        [HttpPost]
        public IActionResult CreateCollection([FromBody]CollectionCreateOrUpdateModel collection)
        {
            var autorizedUserId = AuthorizedUserId;

            if (!autorizedUserId.HasValue || !autorizedUserId.Value.Equals(collection.UserId))
            {
                _logger.LogError(Resource.LogErrorUserIsNotAutorized);
                return Unauthorized();
            }

            var user = _repository.User.GetUserById(collection.UserId);
            if (user.IsNull())
            {
                _logger.LogError(string.Format(Resource.LogErrorGetByIsNull, nameof(user), nameof(collection.UserId), collection.UserId));
                return NotFound();
            }

            _repository.Collection.CreateCollection(collection.GetEntity());

            return CreatedAtRoute(nameof(GetCollectionById), new { id = collection.Id }, new EntityModel<Collection>(collection.GetEntity()));
        }
    }
}
