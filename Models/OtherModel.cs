using System;
using System.Collections.Generic;

namespace CityGasWebApi.Models
{
    /// <summary>
    /// 前端列表数据的通用参数
    /// </summary>
    public class TableData
    {
        public int Total { get; set; }
        public bool Success { get; set; }
        public int PageSize { get; set; }
        public int Current { get; set; }
        public dynamic Data { get; set; }
    }

    /// <summary>
    /// 用来接收删除参数
    /// </summary>
    public class DelObj
    {
        public List<Guid> Id { get; set; }
    }

    public class ResultObj
    {
        public bool IsSuccess { get; set; }
        public string ErrMsg { get; set; }
    }
}
