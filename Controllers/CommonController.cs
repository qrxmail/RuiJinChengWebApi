using System.Linq;
using System.Threading.Tasks;
using RuiJinChengWebApi.Models;
using RuiJinChengWebApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Quartz;
using RuiJinChengWebApi.Hubs;
using Microsoft.AspNetCore.SignalR;
using System.Threading;
using System;

namespace RuiJinChengWebApi.Controllers
{

    [Route("api/common")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly ILogger<CommonController> _logger;
        private readonly IHubContext<ChatHub> _hub;

        private readonly RuiJinChengWebContext _context;

        public CommonController(RuiJinChengWebContext context, ILogger<CommonController> logger,IHubContext<ChatHub> hub)
        {
            _context = context;
            _logger = logger;
            _hub = hub;
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
        /// websocekt测试（利用死循环实现定时发送socket消息）
        /// </summary>
        /// <returns></returns>
        [Route("webSocketTest")]
        [HttpGet]
        public async Task WebSocketTest()
        {
            while (true)
            {
                try
                {
                    Thread.Sleep(10000);
                    await _hub.Clients.All.SendAsync("ReceiveMessage", "job", "test");
                }
                catch (Exception)
                {
                    break;
                }
            }
        }

    }


}
