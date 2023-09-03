using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using test.Module.Entities;

namespace test.Service.Management.HosInfo
{
    public interface IHosInfoService
    {
        //查询指定地点的医疗点信息
        List<hospital_info> SearchHosInfo(string province, string city, string area, string street);
        //查询用户所在地的医疗点信息的数据库操作
        string GetHosInfo(string id);
    }
}
