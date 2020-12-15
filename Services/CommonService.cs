using CityGasWebApi.Models;
using Grpc.Net.Client;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using RJCGrpcService;
using System.Text;
using System.Threading.Tasks;

namespace CityGasWebApi.Services
{
    public class CommonService
    {
        public static User GetCurrentUser(HttpContext HttpContext)
        {
            byte[] currentUserByte = HttpContext.Session.Get("currentUser");
            string currentUserStr = currentUserByte == null ? null : Encoding.UTF8.GetString(currentUserByte);
            if (string.IsNullOrEmpty(currentUserStr) == false)
            {
                User currentUser = JsonConvert.DeserializeObject<User>(currentUserStr);
                return currentUser;
            }
            else
            {
                User currentUser = new User();
                currentUser.Status = "error";
                currentUser.CurrentAuthority = "guest";
                return currentUser;
            }
        }

        public static async Task<string> RpcClient()
        {
            using var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var client = new Greeter.GreeterClient(channel);
            var response = await client.SayHelloAsync(new HelloRequest { Name = "测试Rpc" });
            var response1 = await client.GetStationFieldDataAsync(new StationName { Name = "站点名称" });
            var response2 = await client.WriteStationAsync(new ControlModel { Action = "指令",Target="目标" });
            return "rpc应答数据\nSayHelloAsync：" + response.Message + "\nGetStationFieldDataAsync：" + response1.Data + "\nWriteStationAsync：" + response2.Result;
        }
    }
}
