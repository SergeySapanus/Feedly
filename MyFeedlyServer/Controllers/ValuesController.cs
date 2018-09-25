using System.Collections.Generic;
using System.Linq;
using Contracts;
using Microsoft.AspNetCore.Mvc;

namespace MyFeedlyServer.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly IRepositoryWrapper _repoWrapper;
        private readonly ILoggerManager _logger;

        public ValuesController(IRepositoryWrapper repoWrapper, ILoggerManager logger)
        {
            _repoWrapper = repoWrapper;
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<string> Get()
        {
            //return _repoWrapper.Collection
            //    .FindByCondition(x => x.User.Name.Equals("Domestic"))
            //    .SelectMany(c => c.CollectionsFeeds)
            //    .Select(cf => cf.Feed)
            //    .Distinct()
            //    .Select(f => f.Name);
            //.SelectMany(f => f.News)
            //.Select(n => n.Content);

            return _repoWrapper.CollectionFeed
                .FindByCondition(cf => cf.Collection.User.Name == "Name").Select(cf => cf.Id.ToString());

            //return _repoWrapper.Collection.
            //    FindByCondition(x => x.User.Name.Equals("Domestic")).Select(c => c.Name);

            //return _repoWrapper.User.FindAll().Select(u => u.Name);

            //return new[] { "Empty" };
        }
    }
}
