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
    [Route("api/feed")]
    public class FeedController : Controller
    {
        private readonly ILoggerManager _logger;
        private readonly IRepositoryWrapper _repository;

        public FeedController(ILoggerManager logger, IRepositoryWrapper repository)
        {
            _logger = logger;
            _repository = repository;
        }

        [HttpGet]
        public IActionResult GetAllFeeds()
        {
            var feeds = _repository.Feed.GetAllFeeds().Select(f => new FeedGetModel(f));

            _logger.LogInfo(Resource.LogInfoGetAllFeeds);
            return Ok(feeds);
        }

        [HttpGet("{id}", Name = nameof(GetFeedById))]
        public IActionResult GetFeedById(int id)
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

        [Authorize]
        [HttpPost]
        public IActionResult CreateFeed([FromBody]FeedCreateOrUpdateModel feedModel)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError(string.Format(Resource.LogErrorInvalidModel, nameof(feedModel), ModelState.GetAllErrors()));
                return BadRequest(Resource.Status400BadRequestInvalidModel);
            }

            var autorizedUserId = this.GetAutorizedUserId();

            if (!autorizedUserId.HasValue)
            {
                _logger.LogError(Resource.LogErrorUserIsNotAutorized);
                return Unauthorized();
            }

            var collection = _repository.Collection.GetCollectionByIdAndUserId(feedModel.CollectionId, autorizedUserId.Value);
            if (collection.IsNull())
            {
                _logger.LogError(string.Format(Resource.LogErrorGetByIsNull, nameof(collection), nameof(feedModel.CollectionId), feedModel.CollectionId));
                return NotFound();
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

            return CreatedAtRoute(nameof(GetFeedById), new { id = feedModel.Id }, new EntityModel<Feed>(feed));
        }
    }
}
