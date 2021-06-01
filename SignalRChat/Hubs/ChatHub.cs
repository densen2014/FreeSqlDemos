using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static SignalRChat.Startup;

namespace SignalRChat.Hubs
{
    //ChatHub 类继承自 SignalR Hub 类。 Hub 类管理连接、组和消息。
    //可通过已连接客户端调用 SendMessage，以向所有客户端发送消息

    public class ChatHub : Hub
    {
        IFreeSql _fsql { get; set; }
        public ChatHub(IFreeSql fsql)
        {
            _fsql = fsql;
        }

        public async Task InitMessage()
        {
            var x = new Random(1000);
            var items = new List<Item>();
            for (int i = 0; i < 1000; i++)
            {
                items.Add(new Item() { Text = x.Next (1,1000).ToString (), Description = "message " + x.Next(20, 600).ToString() });
            }
            _fsql.Insert<Item>().AppendData(items).ExecuteIdentity();
            await Clients.All.SendAsync("ReceiveMessage", items[0].Text, items[0].Description , DateTime.Now.ToString("hh:mm:ss"));
        }

        public async Task SendMessage(string user, string message)
        {
            var id = _fsql.Insert<Item>().AppendData(new Item() { Text = user, Description = message }).ExecuteIdentity();
            await Clients.All.SendAsync("ReceiveMessage", user, message + "  [" + id + "]", DateTime.Now.ToString("hh:mm:ss"));
        }


        public async Task LoadMessage()
        {
            var ItemList = _fsql.Select<Item>().ToList();
            foreach (var item in ItemList)
            {
                await Clients.All.SendAsync("ReceiveMessage", item.Text, item.Description, item.Date.ToString("yyyy-MM-dd hh:mm:ss"));
            }
        }

        public async Task LoadAllMessage()
        {
            var ItemList = _fsql.Select<Item>().ToList();
            await Clients.All.SendAsync("ReceiveAllMessage", ItemList);
        }

        public async Task ResetMessage()
        {
            var res = _fsql.Delete<Item>().Where(a => 1 == 1).ExecuteAffrows();

            await Clients.All.SendAsync("ReceiveMessage", "root", $"Reset db, delete {res} rows.", DateTime.Now.ToString("hh:mm:ss"), true);
        }


    }
}