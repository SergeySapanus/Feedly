using Microsoft.AspNetCore.Mvc;
using MyFeedlyServer.Extensions;

namespace MyFeedlyServer.Controllers
{
    public abstract class BaseController : Controller
    {
        protected virtual int AuthorizedUserId
        {
            get
            {
                var authorizedUserId = this.GetAuthorizedUserId();
                if (authorizedUserId.HasValue)
                    return authorizedUserId.Value;

                return -1;
            }
        }

        protected string GetDataProtectionPurpose()
        {
            return typeof(BaseController).FullName;
        }
    }
}