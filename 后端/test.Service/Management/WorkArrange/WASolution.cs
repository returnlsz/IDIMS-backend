using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using test.Module.Entities;
using test.Common.Db;

namespace test.Service.Management.WorkArrange
{
    public class WASolution : IWAInterface
    {
        public bool GovSetArrangement(string manager_id,string staff_id, string pos_id, string ddl_time)
        {
            work_arrangement inputdata = new work_arrangement
            {
                manager_id = manager_id,
                worker_id = staff_id,
                pos_id = pos_id,
                ddl_time = ddl_time,
                done_time = null,
                type = "未完成"
            };
            var result=DbContext.db.Insertable(inputdata).ExecuteCommand();
            return result > 0;
        }

        public List<StaffWorkStatus> StaffGetWork(string staff_id)
        {
            List<StaffWorkStatus>? dataresult=DbContext.db.Queryable<work_arrangement>()
                .Where(x => x.worker_id == staff_id)
                .Select(x => new StaffWorkStatus
                {
                    manager_id = x.manager_id,
                    pos_id=x.pos_id,
                    ddl_time=x.ddl_time,
                    status=x.type
                    

                }).ToList();
            return dataresult;
        }

        public bool StaffPostWork(string worker_id,string manager_id, string pos_id, string ddl_time, string done_time)
        {
            var result = DbContext.db.Updateable<work_arrangement>()
                .SetColumns(it => new work_arrangement
                {
                    manager_id = it.manager_id,
                    worker_id = it.worker_id,
                    pos_id = it.pos_id,
                    ddl_time = it.ddl_time,
                    done_time = it.done_time,
                    type = "已完成"
                })
                .Where(x => x.worker_id == worker_id && x.manager_id == manager_id && x.ddl_time == ddl_time)
                .ExecuteCommand();

            return result > 0;

        }
    }
}
