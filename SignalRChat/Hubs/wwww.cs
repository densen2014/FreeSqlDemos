using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace SignalRChat.Hubs
{
    //ChatHub 类继承自 SignalR Hub 类。 Hub 类管理连接、组和消息。
    //可通过已连接客户端调用 SendMessage，以向所有客户端发送消息

    public class hub : Hub
    {
        public async Task SendMessage(string user, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", user, message);
        }
    }
}