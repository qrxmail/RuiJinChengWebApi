using System;
using System.ComponentModel.DataAnnotations;

namespace CityGasWebApi.Models.Common
{
    public class Files
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>对应的表数据ID</summary>
		public Guid DataId { get; set; }

        /// <summary>对应表名</summary>
		public string TableName { get; set; }

        /// <summary>文件原名</summary>
        public string FileName { get; set; }

        /// <summary>存储路径</summary>
        public string Path { get; set; }

        /// <summary>文件存储名</summary>
        public string StoreName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUser { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        public DateTime LastUpdateTime { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public string LastUpdateUser { get; set; }
    }
}
