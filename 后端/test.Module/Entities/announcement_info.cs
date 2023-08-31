using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace test.Module.Entities
{
    /// <summary>
    /// 公告
    /// </summary>
    public class announcement_info
    {
        [SugarColumn(IsPrimaryKey = true)]
        public int notice_id { get; set; }
        public string? time { get; set; }
        public string? name { get; set; }
        public string? content { get; set; }
        public string? super_id { get; set; }
    }
}
