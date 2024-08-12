using mySOMEappV2.Models;

namespace mySOMEappV2.Helpers
{
    public class HomeHelper
    {

        private static readonly someDBContext _context = new();
        public static List<Post>? GetPosts(string id)
        {
            var user = _context.Users.Where(u => u.Id.ToString() == id).FirstOrDefault();
            if (user != null)
            {
                var friendsIds = _context.Follows.Where(f => f.Follower == user.Id).Select(f => f.Following).ToList();
                var posts = _context.Posts.Where(p => p.UserId == user.Id || friendsIds.Contains(p.UserId)).OrderByDescending(p => p.Time).ToList();
                foreach (var p in posts)
                {
                    _context.Entry(p).Reference(p => p.User).Load();
                    _context.Entry(p).Collection(p => p.Comments).Load();
                    foreach (var c in p.Comments)
                    {
                        _context.Entry(c).Reference(c => c.User);
                    }
                }
                return posts;
            }
            return null;
        }

        public static async Task<bool> AddPost(string id, string postContent, IFormFile file)
        {
            Post post = new();
            post.UserId = Guid.Parse(id);
            post.Content = postContent;
            post.Likes = 0;
            if (file != null)
            {
                if (file.Length > 0)
                {
                    MemoryStream ms = new();
                    file.CopyTo(ms);
                    post.Media = ms.ToArray();
                    ms.Close();
                    ms.Dispose();
                }
            }

            try
            {
                _context.Posts.Add(post);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static void LikePost(string postId)
        {
            var post = _context.Posts.Where(p => p.Id.ToString() == postId).FirstOrDefault();
            if (post != null)
            {
                post.Likes += 1;
                _context.SaveChanges();
            }
        }

        public static List<Comment> GetComments(string postId)
        {
            var comments = _context.Comments.Where(c => c.PostId.ToString() == postId).ToList();
            foreach (var item in comments)
            {
                _context.Entry(item).Reference(i => i.User).Load();
            }

            return comments;
        }

        public async static Task<bool> AddComment(string content, string postId, string userId)
        {
            var comment = new Comment();
            comment.Content = content;
            comment.PostId = Guid.Parse(postId);
            comment.Time = DateTime.Now;
            comment.UserId = Guid.Parse(userId);
            try
            {
                _context.Comments.Add(comment);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
    }
}

