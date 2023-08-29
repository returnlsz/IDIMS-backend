using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace test.Module.Entities
{
    /// <summary>
    /// 风险等级
    /// </summary>
    public class danger_level
    {
        [SugarColumn(IsPrimaryKey = true)]
        public string? provice { get; set; }
        [SugarColumn(IsPrimaryKey = true)]
        public string? city { get; set; }
        [SugarColumn(IsPrimaryKey = true)]
        public string? area { get; set; }
        public string? level { get; set; }
    }
}
