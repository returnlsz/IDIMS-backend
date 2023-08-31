using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test.Service.Management.WorkArrange
{
    public interface IWAInterface
    {
        bool GovSetArrangement(string manager_id,string staff_id, string pos_id, string ddl_time);

        bool StaffPostWork(string worker_id,string manager_id,string pos_id,string ddl_time,string done_time);
        List<StaffWorkStatus> StaffGetWork(string staff_id);
    }
}
