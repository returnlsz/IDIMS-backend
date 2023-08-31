using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test.Service.Management.GovWithInfectionInfo
{
    public enum  CheckType { Province = 0, City = 1, Region = 2, };
    public class InfectPeopleNumber
    {
        string? Province { get; set; }
        string? City { get; set; }
        string? Region { get; set; }
        int number=0;
    }
}
