using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test.Service.Management.RiskLevel
{
    public interface IRiskLevelService
    {
        //获取用户所在地的风险等级
        int GetRiskLevel(string id);
        //超级用户修改地区风险等级
        int ModifyDangerLevel(string province, string city, string area, int dangerlevel);
    }
}
