using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Quartz;
using RuiJinChengWebApi.Hubs;
using RuiJinChengWebApi.Services;
using System.Threading.Tasks;

namespace RuiJinChengWebApi.Controllers.Quartz
{
    [Route("api")]
    [ApiController]
    public class QuartzController : ControllerBase
    {
        private ILog log = LogManager.GetLogger(Startup.repository.Name, typeof(QuartzController));

        private readonly ISchedulerFactory _schedulerFactory;
        private IScheduler _scheduler;
        private readonly IHubContext<ChatHub> _hub;

        public QuartzController(ISchedulerFactory schedulerFactory, IHubContext<ChatHub> hub)
        {
            _schedulerFactory = schedulerFactory;
            _hub = hub;
        }

        [HttpGet]
        [Route("QuartzTask")]
        public void QuartzTask(int type)
        {
            JobKey jobKey = new JobKey("workTicket", "work");
            switch (type)
            {
                //添加任务
                case 1:
                    var trigger = TriggerBuilder.Create()
                            .WithDescription("workTicket")
                            .WithIdentity("admin")
                            //.WithSchedule(CronScheduleBuilder.CronSchedule("0 0/30 * * * ? *").WithMisfireHandlingInstructionDoNothing())
                            .WithSimpleSchedule(x => x.WithIntervalInSeconds(5).RepeatForever().WithMisfireHandlingInstructionIgnoreMisfires())
                            .Build();
                    Jobs._hub = _hub;
                    _ = QuartzUtil.Add(typeof(Jobs), jobKey, trigger);
                    break;
                //暂停任务
                case 2:
                    _ = QuartzUtil.Stop(jobKey);
                    break;
                //恢复任务
                case 3:
                    _ = QuartzUtil.Resume(jobKey);
                    break;
                //删除任务
                case 4:
                    _ = QuartzUtil.Delete(jobKey);
                    break;
            }
        }

        /// <summary>
        /// 定时任务测试
        /// </summary>
        /// <returns></returns>
        [Route("taskTest")]
        [HttpGet]
        public async Task TaskTest()
        {
            log.Info("定时任务测试开始");

            //通过工场类获得调度器
            _scheduler = await _schedulerFactory.GetScheduler();
            //开启调度器
            await _scheduler.Start();
            //创建触发器(也叫时间策略)
            var trigger = TriggerBuilder.Create()
                            .WithSimpleSchedule(x => x.WithIntervalInSeconds(10).RepeatForever())//每10秒执行一次
                            .Build();
            //实例化Jobs用到的对象（singalr）
            Jobs._hub = _hub;
            //创建作业实例
            //Jobs即我们需要执行的作业
            var jobDetail = JobBuilder.Create<Jobs>()
                            .WithIdentity("Myjob", "group")//我们给这个作业取了个“Myjob”的名字，并取了个组名为“group”
                            .Build();
            //将触发器和作业任务绑定到调度器中
            await _scheduler.ScheduleJob(jobDetail, trigger);

            log.Info("定时任务测试结束");
        }
    }

}
