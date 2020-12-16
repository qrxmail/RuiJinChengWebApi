using Microsoft.AspNetCore.SignalR;
using Quartz;
using RuiJinChengWebApi.Hubs;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace RuiJinChengWebApi.Services
{
    public class Jobs : IJob
    {
        // 通过静态变量实例化
        public static IHubContext<ChatHub> _hub { get; set; }

        // 定时任务执行方法
        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(() =>
            {
                // 定时发送websocket消息
                _hub.Clients.All.SendAsync("ReceiveMessage", "job", "test11111111");

                // 写日志
                using (StreamWriter sw = new StreamWriter(@"D:\message.log", true, Encoding.UTF8))
                {
                    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                }
            });
        }
    }
}
