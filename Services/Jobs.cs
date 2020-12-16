using Quartz;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace RuiJinChengWebApi.Services
{
    public class Jobs : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            return Task.Run(() =>
            {
                using (StreamWriter sw = new StreamWriter(@"D:\message.log", true, Encoding.UTF8))
                {
                    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                }
            });
        }
    }
}
