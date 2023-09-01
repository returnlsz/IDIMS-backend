using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test.Service.Management.GovWithInfectionInfo
{
    public interface IGWIInterface
    {
        List<InfectPeopleNumber> GetInfectPeopleNumbers(CheckType checkType,DateTime time);
        List<DayInfectNumber> GetDayInfectNumbers(DateTime time);
    }
}
