using System.Linq;
using CityGasWebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityGasWebApi.Controllers
{
    [Route("api/common")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        private readonly RuiJinChengWebContext _context;

        public CommonController(RuiJinChengWebContext context)
        {
            _context = context;
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

    }


}
