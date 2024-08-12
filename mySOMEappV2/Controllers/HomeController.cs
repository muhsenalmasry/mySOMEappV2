using Microsoft.AspNetCore.Mvc;
using mySOMEappV2.Authenticators;
using mySOMEappV2.Helpers;
using mySOMEappV2.Models;
using System.Diagnostics;

namespace mySOMEappV2.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IJwtTokenManager _tokenManager;

        public HomeController(ILogger<HomeController> logger, IJwtTokenManager tokenManager)
        {
            _logger = logger;
            _tokenManager = tokenManager;
        }

        public IActionResult Index()
        {
            var userId = HttpContext.Session.GetString("loggedUserId");
            var userName = HttpContext.Session.GetString("loggedUser");
            if (userId != null)
            {
                var posts = HomeHelper.GetPosts(userId);
                ViewBag.UserId = userId;
                ViewBag.UserName = userName;
                return View(posts);
            }
            return RedirectToAction("Login", "User");
        }

        [HttpPost]
        public async Task<IActionResult> Index([Bind("postContent, file")] string postContent, IFormFile file)
        {
            var userId = HttpContext.Session.GetString("loggedUserId");
            await HomeHelper.AddPost(userId, postContent, (IFormFile)file);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> LikePost(string postId)
        {
            HomeHelper.LikePost(postId);
            return RedirectToAction(nameof(Index));
        }

        public static IEnumerable<Comment> GetComments(string postId)
        {
            var comments = HomeHelper.GetComments(postId);
            return comments;
        }

        public async Task<IActionResult> AddComment(string content, string postId, string userId)
        {
            await HomeHelper.AddComment(content, postId, userId);
            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}