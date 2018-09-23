using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Contracts;
using Entities;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

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
            var domesticCollections = _repoWrapper.Collection.FindByCondition(x => x.User.Name.Equals("Domestic"));

            var collectionNames = domesticCollections as Collection[] ?? domesticCollections.ToArray();
            if (collectionNames.Any())
                return collectionNames.Select(c => c.Name);

            var users = _repoWrapper.User.FindAll();

            var userNames = users as User[] ?? users.ToArray();
            if (userNames.Any())
                return userNames.Select(u => u.Name);

            return new[] { "Empty" };
        }

        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    _logger.LogInfo("Here is info message from our values controller.");
        //    _logger.LogDebug("Here is debug message from our values controller.");
        //    _logger.LogWarn("Here is warn message from our values controller.");
        //    _logger.LogError("Here is error message from our values controller.");

        //    return new[] { "value1", "value2" };
        //}

        //// GET api/values
        //[HttpGet]
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/values/5
        //[HttpGet("{id}")]
        //public string Get(int id)
        //{
        //    return "value";
        //}

        //// POST api/values
        //[HttpPost]
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/values/5
        //[HttpPut("{id}")]
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/values/5
        //[HttpDelete("{id}")]
        //public void Delete(int id)
        //{
        //}
    }
}
