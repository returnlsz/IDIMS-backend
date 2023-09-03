using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test.Service.Management.Antigen
{
    public interface IAntigenService
    {
        //上传抗原并返回是否成功
        int CheckAndUploadantigen(string id, DateTime test_time, string test_result);
        //用户获取自己的抗原检测记录
        string GetUserAntigRecords(string id);
    }
}
