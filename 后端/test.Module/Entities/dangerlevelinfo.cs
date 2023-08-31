using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test.Module.Entities
{
    public class dangerlevelinfo
    {
        [SugarColumn(IsPrimaryKey = true)]
        public string province { get; set; }
        [SugarColumn(IsPrimaryKey = true)]
        public string city { get; set; }
        [SugarColumn(IsPrimaryKey = true)]
        public string area { get; set; }
        public int DangerLevel { get; set; }
    }
}
