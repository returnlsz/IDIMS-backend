using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace test.Service.Management.Reservevation
{
    /// <summary>
    /// 预约信息dto
    /// </summary>
    public class appointment_info_dto
    {
        public string? user_id { get; set; }
        public DateTime? time { get; set; }
        public int state { get; set; }
    }
}