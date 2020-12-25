using log4net;
using Microsoft.AspNetCore.SignalR;
using Quartz;
using RuiJinChengWebApi.Hubs;
using System;
using System.Threading.Tasks;

namespace RuiJinChengWebApi.Services
{
    public class Jobs : IJob
    {
        private ILog log = LogManager.GetLogger(Startup.repository.Name, typeof(Jobs));

        // 通过静态变量实例化
        public static IHubContext<ChatHub> _hub { get; set; }

        // 定时任务执行方法
        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(() =>
            {
                // 定时发送websocket消息
                _hub.Clients.All.SendAsync("ReceiveMessage", "job"+ DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), "test11111111");
                log.Info("定时任务执行！！！！");
            });
        }
    }
}
