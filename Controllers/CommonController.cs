using System.Linq;
using System.Threading.Tasks;
using CityGasWebApi.Models;
using CityGasWebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Quartz;
using RuiJinChengWebApi.Services;

namespace CityGasWebApi.Controllers
{
    [Route("api/common")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly ILogger<CommonController> _logger;
        private readonly ISchedulerFactory _schedulerFactory;
        private IScheduler _scheduler;

        private readonly RuiJinChengWebContext _context;

        public CommonController(RuiJinChengWebContext context, ILogger<CommonController> logger, ISchedulerFactory schedulerFactory)
        {
            _context = context;
            _logger = logger;
            _schedulerFactory = schedulerFactory;
        }

        // 获取站点数据（下拉选框数据：所属单位Branch、管理区District、站名Name）
        [Route("getOilStation")]
        public dynamic GetOilStation()
        {
            var stationData = _context.OilStation.ToList();

            var queryGroup = from a in stationData.GroupBy(t => new { t.Branch })
                             select new
                             {
                                 Label = a.Key.Branch,
                                 Value = a.Key.Branch,
                                 Children = from b in stationData.Where(p => p.Branch.Equals(a.Key.Branch)).GroupBy(t => new { t.District })
                                            select new
                                            {
                                                Label = b.Key.District,
                                                Value = b.Key.District,
                                                Children = from c in stationData.Where(p => p.District.Equals(b.Key.District)).GroupBy(t => new { t.Name, t.PK })
                                                           select new
                                                           {
                                                               Label = c.Key.Name,
                                                               Value = c.Key.PK,
                                                           }
                                            }
                             };

            return queryGroup.ToList();
        }

        // 获取司机数据（下拉选框数据）
        [Route("getDriver")]
        public dynamic GetDriver()
        {
            //var data = _context.Driver.ToList();

            //var query = from a in data
            //            select new
            //                 {
            //                     Text = a.Name,
            //                     Value = a.PK,
            //                 };
            var query = from a in _context.WorkTicket.GroupBy(t => new { t.Driver })
                        select new
                        {
                            Text = a.Key.Driver,
                            Value = a.Key.Driver,
                        };

            return query.ToList();
        }

        // 获取车辆数据（下拉选框数据）
        [Route("getTruck")]
        public dynamic GetTruck()
        {
            //var data = _context.Truck.ToList();

            //var query = from a in data
            //            select new
            //            {
            //                Text = a.Number,
            //                Value = a.PK,
            //            };
            var query = from a in _context.WorkTicket.GroupBy(t => new { t.CarNumber })
                        select new
                        {
                            Text = a.Key.CarNumber,
                            Value = a.Key.CarNumber,
                        };
            return query.ToList();
        }

        /// <summary>
        /// rpc客户端请求测试
        /// </summary>
        /// <returns></returns>
        [Route("rpcClientTest")]
        [HttpGet]
        public async Task<ActionResult<string>> rpcClientTestAsync()
        {
            return await CommonService.RpcClient();
        }

        /// <summary>
        /// 定时任务测试
        /// </summary>
        /// <returns></returns>
        [Route("taskTest")]
        [HttpGet]
        public async Task TaskTest()
        {
            //通过工场类获得调度器
            _scheduler = await _schedulerFactory.GetScheduler();
            //开启调度器
            await _scheduler.Start();
            //创建触发器(也叫时间策略)
            var trigger = TriggerBuilder.Create()
                            .WithSimpleSchedule(x => x.WithIntervalInSeconds(10).RepeatForever())//每10秒执行一次
                            .Build();
            //创建作业实例
            //Jobs即我们需要执行的作业
            var jobDetail = JobBuilder.Create<Jobs>()
                            .WithIdentity("Myjob", "group")//我们给这个作业取了个“Myjob”的名字，并取了个组名为“group”
                            .Build();
            //将触发器和作业任务绑定到调度器中
            await _scheduler.ScheduleJob(jobDetail, trigger);
          
        }


    }


}
