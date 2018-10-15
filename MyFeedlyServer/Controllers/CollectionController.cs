using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Authorization;
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
    [SwaggerTag("Create, read Collections")]
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

        [SwaggerOperation(
            Summary = "Get collection by id",
            Description = "Get collection by id",
            OperationId = "GetCollectionById"
        )]
        [SwaggerResponse((int)HttpStatusCode.OK, "Info about collection", typeof(CollectionGetModel))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Collection for authorized user hasn't been found in db")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [Authorize]
        [HttpGet("{id}", Name = nameof(GetCollectionById))]
        public IActionResult GetCollectionById([SwaggerParameter("Collection identifier", Required = true)]int id)
        {
            var autorizedUserId = AuthorizedUserId;

            var collection = new CollectionGetModel(_repository.Collection.GetCollectionByIdAndUserId(id, autorizedUserId));
            if (collection.IsNull())
            {
                _logger.LogError(string.Format(Resource.LogErrorGetByIsNull, nameof(collection), nameof(id), id));
                return NotFound();
            }

            _logger.LogInfo(string.Format(Resource.LogInfoGetById, nameof(collection), id));
            return Ok(collection);
        }

        [SwaggerOperation(
            Summary = "Get all news for a collection",
            Description = "Get all news for a collection",
            OperationId = "GetNewsByCollectionId"
        )]
        [SwaggerResponse((int)HttpStatusCode.OK, "List collection news", typeof(NewsGetModel))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Collection for authorized user hasn't been found in db")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [Authorize]
        [HttpGet("{id}/news", Name = nameof(GetNewsByCollectionId))]
        public IActionResult GetNewsByCollectionId([SwaggerParameter("Collection identifier", Required = true)]int id)
        {
            var autorizedUserId = AuthorizedUserId;

            var collection = _repository.Collection.GetCollectionByIdAndUserId(id, autorizedUserId);
            if (collection.IsNull())
            {
                _logger.LogError(string.Format(Resource.LogErrorGetByIsNull, nameof(collection), nameof(id), id));
                return NotFound();
            }

            var result = _repository.Feed.GetNewsByCollection(collection, _syndicationManager).Select(f => new NewsGetModel(f));

            _logger.LogInfo(string.Format(Resource.LogInfoGetById, nameof(collection), id));
            return Ok(result);
        }

        [SwaggerOperation(
            Summary = "Create collection",
            Description = "Create collection",
            OperationId = "CreateCollection"
        )]
        [SwaggerResponse((int)HttpStatusCode.Created, "Create created successfully", typeof(EntityGetModel<IEntity>))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Collection for authorized user hasn't been found in db")]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, "User hasn't been authorized")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [Authorize]
        [HttpPost]
        public IActionResult CreateCollection([FromBody]CollectionCreateOrUpdateModel collection)
        {
            if (!AuthorizedUserId.Equals(collection.UserId))
            {
                _logger.LogError(Resource.LogErrorUserIsNotAutorized);
                return NotFound();
            }

            _repository.Collection.CreateCollection(collection.GetEntity());

            return CreatedAtRoute(nameof(GetCollectionById), new { id = collection.Id }, new EntityGetModel<IEntity>(collection.GetEntity()));
        }
    }
}
