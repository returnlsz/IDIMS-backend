using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test.Service.Management.Notice
{
    public class UpdateDto
    {
        public int notice_id { get; set; }
        public string notice_name { get; set; }
        public string content { get; set; }
        public DateTime time { get; set; }
    }
}
