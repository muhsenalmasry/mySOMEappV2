using mySOMEappV2.Authenticators;
using mySOMEappV2.Models;

namespace mySOMEappV2.Helpers
{
    public class UserHelper
    {
        private static readonly someDBContext _context = new();

        public static User? GetUser(string userId)
        {
            return _context.Users.Where(u => u.Id.ToString() == userId).FirstOrDefault();
        }

        public static User? GetUser(string email, string password)
        {
            var user = _context.Users.Where(u => u.Email == email).FirstOrDefault();
            if (user != null)
            {
                if (BCrypt.Net.BCrypt.Verify(password, user.Password))
                {
                    return user;
                }
            }
            return null;
        }

        public static User? GetDetails(string name, string id)
        {
            var user = _context.Users.Where(u => u.FirstName == name || u.LastName == name || u.FirstName + " " + u.LastName == name).FirstOrDefault();
            if (user != null)
            {
                _context.Entry(user).Collection(u => u.Posts).Load();
                foreach (var post in user.Posts)
                {
                    _context.Entry(post).Collection(p => p.Comments).Load();
                    foreach (var comment in post.Comments)
                    {
                        _context.Entry(comment).Reference(c => c.User).Load();
                    }
                }

                if (user.Id.ToString() == id)
                {
                    _context.Entry(user).Collection(u => u.FollowFollowerNavigations).Load();
                    foreach (var f in user.FollowFollowerNavigations)
                    {
                        _context.Entry(f).Reference(f => f.FollowingNavigation).Load();
                    }
                }

                return user;
            }
            return null;
            
        }

        public static List<User>? GetUserMessages(User user)
        {

            List<Message> messages = new();
            if (user != null)
            {
                _context.Entry(user).Collection(u => u.MessageRecieverNavigations).Load();
                _context.Entry(user).Collection(u => u.MessageSenderNavigations).Load();
                foreach (var item in user.MessageRecieverNavigations)
                {
                    _context.Entry(item).Reference(m => m.SenderNavigation).Load();
                    _context.Entry(item).Reference(m => m.RecieverNavigation).Load();
                    messages.Add(item);
                }
                foreach (var item in user.MessageSenderNavigations)
                {
                    _context.Entry(item).Reference(m => m.SenderNavigation).Load();
                    _context.Entry(item).Reference(m => m.RecieverNavigation).Load();
                    messages.Add(item);
                }
                var messagesWith = messages.GroupBy(m => m.Sender == user.Id ? m.RecieverNavigation : m.SenderNavigation).Select(grp => grp.Key).ToList();
                return messagesWith;
            }
            return null;
            
        }

        public static List<Message> GetMessagesWith(string userId, string with)
        {
            var messages = _context.Messages.Where(m => (m.Sender.ToString() == userId && m.Reciever.ToString() == with) || (m.Sender.ToString() == with && m.Reciever.ToString() == userId)).ToList();
            foreach (var m in messages)
            {
                _context.Entry(m).Reference(m => m.SenderNavigation).Load();
                _context.Entry(m).Reference(m => m.RecieverNavigation).Load();
            }
            return messages;
        }

        public async static Task<bool> CreateUser(string FirstName, string LastName, string Email, string Password, DateTime Birthday, IFormFile ProfilePic, IFormFile ProfileCover)
        {
            var user = new User();
            user.FirstName = FirstName;
            user.LastName = LastName;
            user.Email = Email;
            user.Password = BCrypt.Net.BCrypt.HashPassword(Password);
            user.Birthday = Birthday;

            if (ProfilePic != null)
            {
                if (ProfilePic.Length > 0)
                {
                    MemoryStream ms = new();
                    ProfilePic.CopyTo(ms);
                    user.ProfilePic = ms.ToArray();
                    ms.Close();
                    ms.Dispose();
                }
            }
            if (ProfileCover != null)
            {
                if (ProfileCover.Length > 0)
                {
                    MemoryStream ms = new();
                    ProfileCover.CopyTo(ms);
                    user.ProfileCover = ms.ToArray();
                    ms.Close();
                    ms.Dispose();
                }
            }
            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {

                return false;
            }

        }

        public async static Task<bool> CreateMessage(string sender, string reciever, string msgContent)
        {
            Message message = new();
            message.Sender = Guid.Parse(sender);
            message.Reciever = Guid.Parse(reciever);
            message.Content = msgContent;

            try
            {
                _context.Messages.Add(message);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async static Task<bool> Follow(string name, string id)
        {
            var user = _context.Users.Where(u => (u.FirstName + " " + u.LastName).Contains(name)).FirstOrDefault();
            if (user != null)
            {
                Follow f = new();
                f.Follower = Guid.Parse(id);
                f.Following = user.Id;
                try
                {
                    _context.Follows.Add(f);
                    await _context.SaveChangesAsync();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return false;
        }
    }
}
