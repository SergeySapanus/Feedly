using Microsoft.AspNetCore.Mvc;
using MyFeedlyServer.Extensions;

namespace MyFeedlyServer.Controllers
{
    public abstract class BaseController : Controller
    {
        protected virtual int? AuthorizedUserId => this.GetAuthorizedUserId();
    }
}