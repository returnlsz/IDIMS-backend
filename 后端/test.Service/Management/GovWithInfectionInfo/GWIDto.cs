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
        public string pos_name { get; set; }
        public int number { get; set; }
    }
    public class DayInfectNumber
    {
        public string Province { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public int number { get; set; }
    }
}
