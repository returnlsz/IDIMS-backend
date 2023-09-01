using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace test.Module.Entities
{
    /// <summary>
    /// 工作安排
    /// </summary>
    public class work_arrangement
    {
        [SugarColumn(IsPrimaryKey = true)]
        public string worker_id { get; set; }
        [SugarColumn(IsPrimaryKey = true)]
        public string manager_id { get; set; }


        public string? pos_id { get; set; }
        [SugarColumn(IsPrimaryKey = true)]
        public string ddl_time { get; set; }
        public string? done_time { get; set; }
        public string? type { get; set; }
    }
}
