using Microsoft.AspNetCore.Mvc;
using mySOMEappV2.Helpers;

namespace mySOMEappV2.ViewComponents
{
    public class PostCommentsViewComponent : ViewComponent
    {

        public PostCommentsViewComponent()
        {
        }

        public async Task<IViewComponentResult> InvokeAsync(string postId, string userId, string userName)
        {
            var comments = HomeHelper.GetComments(postId);
            ViewBag.PostId = postId;
            ViewBag.UserId = userId;
            ViewBag.UserName = userName;
            return View(comments);
        }

    }
}
