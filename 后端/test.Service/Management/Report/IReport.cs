using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using test.Module.Entities;

namespace test.Service.Management.Report
{
    public interface IReportService
    {
        //将举报信息修改为已处理（单条）
        string FinishReport(string message_id);
        //超级用户获取所有用户举报信息（未处理的）
        List<report_info> GetReport();
        //用户获取自己的举报信息
        List<report_info> GetReport(string id);
        //用户举报
        int UserPutReport(DateTime time, string id, string content);
    }
}
