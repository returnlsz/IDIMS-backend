using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace test.Module.Entities
{
    /// <summary>
    /// 公告信息
    /// </summary>
    public class notice_info
    {
        public DateTime? time { get; set; }
        public string? name { get; set; }
        public string? content { get; set; }
        public string? super_id { get; set; }

        [SugarColumn(IsPrimaryKey = true)]
        public int notice_id { get; set; }
    }
}
