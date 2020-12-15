using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CityGasWebApi.Models;
using CityGasWebApi.Models.Common;
using CityGasWebApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace CityGasWebApi.Controllers.BaseInfo
{
    [Route("api/files")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly RuiJinChengWebContext _context;

        public FilesController(RuiJinChengWebContext context)
        {
            _context = context;
        }

        // 带多个查询条件的查询
        [Route("query")]
        [HttpGet]
        public ActionResult<TableData> Query(string queryStr)
        {
            JObject jObject = new JObject();
            if (string.IsNullOrEmpty(queryStr) == false)
            {
                jObject = JsonConvert.DeserializeObject<JObject>(queryStr);
            }

            int current = jObject.Value<int>("current") == 0 ? 1 : jObject.Value<int>("current");
            int pageSize = jObject.Value<int>("pageSize") == 0 ? 20 : jObject.Value<int>("pageSize");
            string dataId = jObject.Value<string>("dataId");
            string tableName = jObject.Value<string>("tableName");

            //防止查询条件都不满足，先生成一个空的查询
            var where = _context.Files.Where(p => true);

            if (string.IsNullOrEmpty(dataId) == false)
            {
                where = where.Where(p => p.DataId.ToString().Equals(dataId));
            }
            if (string.IsNullOrEmpty(dataId) == false)
            {
                where = where.Where(p => p.TableName.Equals(tableName));
            }

            //统计总记录数
            int total = where.Count();

            // 解析排序规则
            string sorterKey = "";
            string sortRule = "";
            JObject sorterObj = jObject.Value<JObject>("sorter");
            IEnumerable<JProperty> properties = sorterObj.Properties();
            foreach (JProperty item in properties)
            {
                sorterKey = item.Name;
                sortRule = item.Value.ToString();
            }
            if (string.IsNullOrEmpty(sorterKey) == false && string.IsNullOrEmpty(sortRule) == false)
            {
                // 按照最后更新时间排序
                if (sorterKey.Equals("lastUpdateTime") && sortRule.Equals("descend"))
                {
                    where = where.OrderByDescending(p => p.LastUpdateTime);
                }
                else if (sorterKey.Equals("lastUpdateTime") && sortRule.Equals("ascend"))
                {
                    where = where.OrderBy(p => p.LastUpdateTime);
                }
            }
            else
            {
                //结果按照最后修改时间倒序排序
                where = where.OrderByDescending(p => p.LastUpdateTime);
            }

            //跳过翻页的数量
            where = where.Skip(pageSize * (current - 1));
            //获取结果
            List<Files> dataList = where.Take(pageSize).ToList();

            TableData resultObj = new TableData();
            resultObj.Data = dataList;
            resultObj.Current = current;
            resultObj.Success = true;
            resultObj.PageSize = pageSize;
            resultObj.Total = total;

            return resultObj;
        }

        // 更新文件
        [Route("batchUpdate")]
        public bool UpdateAllAsync(List<Files> fileList)
        {
            // 获取当前登录用户名
            string _currentUserName = CommonService.GetCurrentUser(HttpContext).UserName;

            bool result = true;
            if (fileList != null)
            {
                // 查询已有的文件
                List<Files> files = _context.Files.Where(p => p.DataId == fileList[0].DataId && p.TableName == fileList[0].TableName).ToList();

                // 删除已有的文件
                _context.Files.RemoveRange(files);

                // 新增现有的文件
                List<Files> fileListUpload = new List<Files>();
                for (int i = 0; i < fileList.Count; i++)
                {
                    // 文件名不为空时，才上传
                    if (string.IsNullOrEmpty(fileList[i].FileName) == false)
                    {
                        fileListUpload.Add(fileList[i]);

                        fileList[i].CreateUser = _currentUserName;
                        fileList[i].CreateTime = DateTime.Now;
                        fileList[i].LastUpdateUser = _currentUserName;
                        fileList[i].LastUpdateTime = DateTime.Now;
                    }
                    _context.Files.AddRange(fileListUpload);
                }
            }
            return result;
        }

        // 文件上传
        [Route("uploadFiles")]
        public List<Files> UploadFiles([FromForm] IFormCollection formCollection)
        {
            List<Files> fileNameList = new List<Files>();
            // 文件服务器地址
            string storePath = "D:/Apache24/htdocs/file/";
            string storeName = DateTime.Now.ToString("yyyyMMddHHmmssffff");

            try
            {
                for (int i = 0; i < formCollection.Files.Count; i++)
                {
                    // 上传的文件
                    IFormFile uploadFile = formCollection.Files[index: i];
                    string storeFile = storePath + storeName + uploadFile.FileName;
                    using (FileStream fs = new FileStream(storeFile, FileMode.Create))
                    {
                        // 将上传的文件保存到服务器
                        uploadFile.CopyTo(fs);
                        fs.Flush();
                        Files dto = new Files();
                        dto.FileName = uploadFile.FileName;
                        dto.Path = storePath;
                        dto.StoreName = storeName + uploadFile.FileName;
                        fileNameList.Add(dto);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return fileNameList;
            }
            return fileNameList;
        }



    }

}
