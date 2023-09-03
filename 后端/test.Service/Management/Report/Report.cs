using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using test.Common.Db;
using test.Module.Entities;

namespace test.Service.Management.Report
{
    public class ReportService: IReportService
    {
        private readonly IMapper _mapper;
        public ReportService(IMapper mapper)
        {
            _mapper = mapper;
        }
        /// <summary>
        /// 用户举报的数据库操作
        /// </summary>
        /// <returns></returns>
        public int UserPutReport(DateTime time, string id, string content)
        {
            ///先获取report表最后一行message_id的值（最新值）
            List<string> newest = (List<string>)DbContext.db.Queryable<report_info>().Select(it => it.message_id).ToList();
            string max;
            if (newest.Count == 0)
            {
                max = "1";
            }
            else
            {
                max = newest.OrderByDescending(x => x).First();
                int maxint = int.Parse(max);
                maxint++;
                max = maxint.ToString();
            }
            report_info report_Info = new report_info()
            {
                time = time,
                user_id = id,
                state = 0,
                message_id = max,
                content = content
            };
            ///返回受影响条数
            return DbContext.db.Insertable(report_Info).ExecuteCommand();
        }
        /// <summary>
        /// 超级用户获取所有用户举报信息（未处理的）
        /// </summary>
        /// <returns></returns>
        public List<report_info> GetReport()
        {
            List<report_info> report_Infos = new List<report_info>();
            report_Infos = DbContext.db.Queryable<report_info>().Where(it => it.state == 0).ToList();
            return report_Infos;
        }
        /// <summary>
        /// 将举报信息修改为已处理（单条）
        /// </summary>
        /// <param name="message_id"></param>
        /// <returns></returns>
        public string FinishReport(string message_id)
        {
            List<report_info> report_Infos = new List<report_info>();
            report_Infos = DbContext.db.Queryable<report_info>().Where(it => it.message_id == message_id).ToList();
            if (report_Infos.Count == 0)
            {
                return "";
            }
            report_info report_Info = report_Infos[0];
            DbContext.db.Tracking(report_Info);//创建跟踪
            report_Info.state = 1;

            int result = DbContext.db.Updateable(report_Info).ExecuteCommand();
            if (result == 0)
            {
                return "";
            }
            else
            {
                return JsonConvert.SerializeObject(new
                {
                    time = report_Info.time,
                    user_id = report_Info.user_id,
                    state = 1,
                    message_id = report_Info.message_id,
                    content = report_Info.content
                });
            }
        }
        /// <summary>
        /// 用户获取自己的举报信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<report_info> GetReport(string id)
        {
            List<report_info> report_Infos = new List<report_info>();
            report_Infos = DbContext.db.Queryable<report_info>().Where(it => it.user_id == id).ToList();
            return report_Infos;
        }
    }
}
