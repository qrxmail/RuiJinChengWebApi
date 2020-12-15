using System;
using System.Text;
using System.Security.Cryptography;
using CityGasWebApi.Models.Work;


namespace CityGasWebApi.Services
{
	public class QRCodeEncoder
	{
		const string salt = "CosMosSalt";

		public static string Encoder(WorkTicket ticket)
		{
			var createTime = ticket.CreateTime.ToString("yyyyMMddHHmmss");
			var text = ticket.SerialNumber + createTime + salt;
			return nameof(WorkTicket) + ":" + ticket.SerialNumber + ":" + createTime + ":" + GetEncrptResult(text);
		}

		static string GetEncrptResult(string text)
		{
			var md5 = new MD5CryptoServiceProvider();
			byte[] palindata = Encoding.Default.GetBytes(text);//将要加密的字符串转换为字节数组
			byte[] encryptdata = md5.ComputeHash(palindata);//将字符串加密后也转换为字符数组
			return Convert.ToBase64String(encryptdata).Substring(0, 10);//将加密后的字节数组转换为加密字符串
		}
	}
}
