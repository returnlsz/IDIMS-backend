using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace test.Module.Entities
{
    /// <summary>
    /// 工作人员健康状况
    /// </summary>
    public class worker_health_state
    {
        [SugarColumn(IsPrimaryKey = true)]
        public string? worker_id { get; set; }
        public string? time { get;set; }
        public string? state { get; set; }
    }
}
