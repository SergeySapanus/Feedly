using System;
using Contracts;
using Entities.Concrete;
using Entities.Extensions;
using Entities.Model;
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

        [HttpGet("{id}", Name = nameof(GetFeedById))]
        public IActionResult GetFeedById(int id)
        {
            try
            {
                var feed = new FeedGetModel(_repository.Feed.GetFeedById(id));

                if (feed.IsNull())
                {
                    _logger.LogError(string.Format(Resource.LogErrorGetByIdIsNull, nameof(feed), id));
                    return NotFound();
                }
                else
                {
                    _logger.LogInfo(string.Format(Resource.LogInfoGetById, nameof(feed), id));
                    return Ok(feed);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(string.Format(Resource.LogErrorException, nameof(GetFeedById), ex.InnerException?.Message ?? ex.Message));
                return StatusCode(500, Resource.Status500);
            }
        }

        [HttpPost]
        public IActionResult CreateFeed([FromBody]FeedCreateOrUpdateModel feedModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogError(string.Format(Resource.LogErrorInvalidModel, nameof(feedModel), ModelState.GetAllErrors()));
                    return BadRequest(Resource.Status400BadRequestInvalidModel);
                }

                var collection = _repository.Collection.GetCollectionById(feedModel.CollectionId);
                if (collection.IsNull())
                {
                    _logger.LogError(string.Format(Resource.LogErrorGetByIdIsNull, nameof(collection), feedModel.CollectionId));
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
            catch (Exception ex)
            {
                _logger.LogError(string.Format(Resource.LogErrorException, nameof(CreateFeed), ex.InnerException?.Message ?? ex.Message));

                return StatusCode(500, Resource.Status500);
            }
        }
    }
}
