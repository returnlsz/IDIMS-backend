using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace test.Service.Management.WorkArrange
{
    public class StaffWorkStatus
    {
        public string manager_id { get; set; }
        public string pos_id { get; set; }
        public string ddl_time { get; set; }
        public string status { get; set; }
        public string donetime { get; set; }
    }
    public class SetWork
    {
        public string staff_id { get; set; }
        public string pos_id { get; set; }
        public string ddl_time { get; set; }
    }
}
