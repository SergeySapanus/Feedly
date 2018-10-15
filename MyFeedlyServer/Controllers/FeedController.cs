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
    [SwaggerTag("Add, read Feeds")]
    [Route("api/feed")]
    public class FeedController : BaseController
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;

        public FeedController(ILoggerManager logger, IRepositoryWrapper repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [SwaggerOperation(
            Summary = "Get all feeds",
            Description = "Get all feeds",
            OperationId = "GetAllFeeds"
        )]
        [SwaggerResponse((int)HttpStatusCode.OK, "List of feeds", typeof(FeedGetModel))]
        [HttpGet]
        public IActionResult GetAllFeeds()
        {
            var feeds = _repository.Feed.GetAllFeeds().Select(f => new FeedGetModel(f));

            _logger.LogInfo(Resource.LogInfoGetAllFeeds);
            return Ok(feeds);
        }

        [SwaggerOperation(
            Summary = "Get feed by id",
            Description = "Get feed by id",
            OperationId = "GetFeedById"
        )]
        [SwaggerResponse((int)HttpStatusCode.OK, "Info about feed", typeof(FeedGetModel))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Feed hasn't been found in db")]
        [HttpGet("{id}", Name = nameof(GetFeedById))]
        public IActionResult GetFeedById([SwaggerParameter("Feed identifier", Required = true)]int id)
        {
            var feed = new FeedGetModel(_repository.Feed.GetFeedById(id));

            if (feed.IsNull())
            {
                _logger.LogError(string.Format(Resource.LogErrorGetByIsNull, nameof(feed), nameof(id), id));
                return NotFound();
            }

            _logger.LogInfo(string.Format(Resource.LogInfoGetById, nameof(feed), id));
            return Ok(feed);
        }

        [SwaggerOperation(
            Summary = "Add feed to a collection",
            Description = "Add feed to a collection",
            OperationId = "CreateFeed"
        )]
        [SwaggerResponse((int)HttpStatusCode.Created, "Feed added successfully", typeof(EntityGetModel<IEntity>))]
        [SwaggerResponse((int)HttpStatusCode.NotFound, "Collection for authorized user hasn't been found in db", typeof(EntityGetModel<IEntity>))]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized, "User hasn't been authorized")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [Authorize]
        [HttpPost]
        public IActionResult CreateFeed([FromBody]FeedCreateOrUpdateModel feedModel)
        {
            var autorizedUserId = AuthorizedUserId;

            var collection = _repository.Collection.GetCollectionByIdAndUserId(feedModel.CollectionId, autorizedUserId);
            if (collection.IsNull())
            {
                _logger.LogError(string.Format(Resource.LogErrorGetByIsNull, nameof(collection), nameof(feedModel.CollectionId), feedModel.CollectionId));
                return NotFound(new EntityGetModel<IEntity>(feedModel.CollectionId));
            }

            var feed = feedModel.GetEntity();

            var feedByHash = _repository.Feed.GetFeedByHash(feed.GetHashCode());
            if (feedByHash.IsNull())
            {
                _repository.Feed.CreateFeed(collection, feed);
            }
            else
            {
                _repository.CollectionFeed.CreateCollectionFeed(collection, feed = feedByHash);
            }

            return CreatedAtRoute(nameof(GetFeedById), new { id = feedModel.Id }, new EntityGetModel<IEntity>(feed));
        }
    }
}
