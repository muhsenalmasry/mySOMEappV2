using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using mySOMEappV2.Authenticators;
using mySOMEappV2.Helpers;
using mySOMEappV2.Models;

namespace mySOMEappV2.Controllers
{
    public class UserController : Controller
    {
        private readonly someDBContext _context = new();

        // GET: UserController
        [HttpGet]
        public ActionResult Login() => View();

        [HttpPost]
        public IActionResult Login([FromForm] string Email, string Password)
        {
            var user = UserHelper.GetUser(Email, Password);
            if (user != null)
            {
                HttpContext.Session.SetString("loggedUser", user.FirstName + " " + user.LastName);
                HttpContext.Session.SetString("loggedUserId", user.Id.ToString());
                return RedirectToAction("Index", "Home");
            }
            return View(user);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        // GET: UserController/Details/5
        public ActionResult Details(string name)
        {
            var userId = HttpContext.Session.GetString("loggedUserId");
            var user = UserHelper.GetDetails(name, userId);
            if (user != null)
            {
                ViewBag.UserId = user.Id.ToString();
                ViewBag.Messages = UserHelper.GetUserMessages(user);
                return View(user);
            }
            return View();
        }

        public ActionResult GetMessagesWith(string userId, string withUser)
        {
            var messages = UserHelper.GetMessagesWith(userId, withUser);
            ViewBag.UserId = userId;
            ViewBag.MsgRcvrId = withUser;
            return View(messages);
        }

        // GET: UserController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("FirstName, LastName, Email, Password, Birthday, ProfilePic, ProfileCover")] string FirstName, string LastName, string Username, string Email, string Password, DateTime Birthday, IFormFile ProfilePic, IFormFile ProfileCover)
        {
            var isValid = await UserHelper.CreateUser(FirstName, LastName, Email, Password, Birthday, ProfilePic, ProfileCover);

            if (isValid)
            {
                return RedirectToAction(nameof(Login));
            }
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> SendMessage(string senderId, string recieverId, string content)
        {
            await UserHelper.CreateMessage(senderId, recieverId, content);
            return RedirectToAction("GetMessagesWith", new { userId = senderId, withUser = recieverId });
        }


        public IActionResult AddFriend()
        {
            var users = _context.Users.ToList();
            return View(users);
        }


        [HttpPost]
        public async Task<IActionResult> AddFriend(string name)
        {
            var userId = HttpContext.Session.GetString("loggedUserId");
            var friendAdded = UserHelper.Follow(name, userId);
            if (friendAdded != null)
            {
                return RedirectToAction(nameof(AddFriend));
            }
            return RedirectToAction(nameof(AddFriend));
        }

        // GET: UserController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
