using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace test.Module.Entities
{
    /// <summary>
    /// 预约信息表
    /// </summary>
    public class appointment_info
    {
        [SugarColumn(IsPrimaryKey = true)]
        public int id { get; set; }
        public string? user_id { get; set; }
        public string? hos_id { get; set; }
        public string? time { get; set; }
        public int type { get; set; }
    }
}
