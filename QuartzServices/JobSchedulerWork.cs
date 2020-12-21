using Quartz;
using Quartz.Impl;

namespace RuiJinChengWebApi.Services
{
    public class JobSchedulerWork
    {
        public static async void Work()
        {
            //调度器工厂
            ISchedulerFactory schedulerFactory = new StdSchedulerFactory();
            //调度器
            IScheduler scheduler = await schedulerFactory.GetScheduler();
            await scheduler.GetJobGroupNames();

            /*-------------计划任务代码实现------------------*/
            //创建任务
            IJobDetail jobDetail = JobBuilder.Create<Jobs>()
                .WithIdentity("Myjob", "group")//我们给这个作业取了个“Myjob”的名字，并取了个组名为“group”
                .Build();
            ////创建触发器 2点一分时被执行
            //ITrigger trigger9 = TriggerBuilder.Create()
            //    .WithCronSchedule("0 01 02 * * ?")
            //    .WithIdentity("TimeTriggerddd", "TimeGroupdd")
            //    .Build();
            //每隔多久执行一次  这个是每隔多久执行一遍
            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("Myjob", "group")
                .WithSimpleSchedule(t => t.WithIntervalInSeconds(1).RepeatForever())
                .Build();
            //添加任务及触发器至调度器中
            await scheduler.ScheduleJob(jobDetail, trigger);
            /*-------------计划任务代码实现------------------*/

            //启动
            await scheduler.Start();
        }
    }
}
