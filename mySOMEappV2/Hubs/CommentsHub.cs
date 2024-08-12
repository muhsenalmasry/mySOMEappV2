using Microsoft.AspNetCore.SignalR;

namespace mySOMEappV2.Hubs
{
    public class CommentsHub : Hub
    {
        public async Task<IAsyncResult> BroadcastComment(string userName, string comment)
        {
            var userId = Convert.ToInt32(userName.Split("*+*")[0]);
            var name = userName.Split("*+*")[1];
            var postId = Convert.ToInt32(userName.Split("*+*")[2]);
            //await HomeHelper.AddComment(comment, postId, userId);
            return Clients.All.SendAsync("broadcastComment", name, comment);
        }
    }
}
