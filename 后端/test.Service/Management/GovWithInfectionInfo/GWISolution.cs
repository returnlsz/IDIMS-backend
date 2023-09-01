using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using test.Common.Db;
using test.Module.Entities;
using SqlSugar;
namespace test.Service.Management.GovWithInfectionInfo
{
    public class GWISolution : IGWIInterface
    {
        public List<DayInfectNumber> GetDayInfectNumbers( DateTime time)
        {
            var result = DbContext.db.Queryable<regioninfo>()
                .LeftJoin<infection_info>((t1, t2) => t1.pos_id == t2.pos_id)
                .Where((t1, t2) => t2.time.Date == time.Date)
                .GroupBy((t1, t2) => t1.pos_id)
                .Select((t1, t2) => new DayInfectNumber
                {
                    Province = t1.province,
                    City = t1.city,
                    Region = t1.area,
                    number = SqlFunc.AggregateMin(t2.infected_number)

                })
                .ToList();
            return result;
        }

        public List<InfectPeopleNumber> GetInfectPeopleNumbers(CheckType checkType,DateTime time)
        {
            if(checkType == CheckType.Province)
            {
                var result = DbContext.db.Queryable<regioninfo>()
                    .LeftJoin<infection_info>((t1, t2) => t1.pos_id == t2.pos_id)
                    .Where((t1, t2) =>t2.time.Date == time.Date)
                    .GroupBy((t1, t2) => t1.province)
                    .Select((t1, t2) => new InfectPeopleNumber
                    {
                        pos_name = t1.province,
                        number =t2.infected_number
                    })
                    .ToList();
                return result;
            }
            else if (checkType == CheckType.City)
            {
                var result = DbContext.db.Queryable<regioninfo>()
                    .LeftJoin<infection_info>((t1, t2) => t1.pos_id == t2.pos_id)
                    .Where((t1, t2) => t2.time.Date == time.Date)
                    .GroupBy((t1, t2) => t1.city)
                    .Select((t1, t2) => new InfectPeopleNumber
                    {
                        pos_name = t1.province,
                        number = SqlFunc.AggregateSum(t2.infected_number)
                    })
                    .ToList();
                return result;
            }
            else if (checkType == CheckType.Region)
            {
                var result = DbContext.db.Queryable<regioninfo>()
                    .LeftJoin<infection_info>((t1, t2) => t1.pos_id == t2.pos_id)
                    .Where((t1, t2) => t2.time.Date == time.Date)
                    .GroupBy((t1, t2) => t1.area)
                    .Select((t1, t2) => new InfectPeopleNumber
                    {
                        pos_name = t1.province,
                        number = SqlFunc.AggregateSum(t2.infected_number)
                    })
                    .ToList();
                return result;
            }
            return null;
        }
    }
}
