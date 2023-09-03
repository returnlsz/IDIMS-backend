using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test.Service.Management.MedicalStaff
{
    public interface IMedicalStaffService
    {
        //获取医疗点所有工作人员的信息
        List<workerinfo_dto> GetMedicalAllStaff(string hos_id);
        //获取工作人员所在的医疗点id
        List<workerinfo_dto2> GetStaffMedicalPoint(string id);
    }
}
