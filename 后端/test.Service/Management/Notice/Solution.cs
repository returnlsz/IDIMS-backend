using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using test.Service.Management.Notice;
using SqlSugar;
using test.Module.Entities;
using test.Common.Db;
namespace test.Service.Management
{
    public class Solution : ICreateNotice, IUpdateNotice
    {
        public bool CreateNotice(NoticeDto noticeDto,string id)
        {
            try
            {
                int count = DbContext.db.Queryable<notice_info>().Count();
                int set_notice_id;
                if (count == 0)
                {
                    set_notice_id = 0;
                }
                else
                {
                    int max_count = DbContext.db.Queryable<notice_info>().Max(it => it.notice_id);
                    set_notice_id = max_count + 1;
                }
                notice_info notice = new notice_info
                {
                    notice_id = set_notice_id,
                    super_id = id,
                    time = noticeDto.time,
                    content = noticeDto.content,
                    name = noticeDto.name

                };
                var result = DbContext.db.Insertable<notice_info>(notice).ExecuteCommand();
                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error:"+ex.Message);
                return false;
            }
        }

        public bool UpdateNotice(UpdateDto noticeDto)
        {
            try
            {
                notice_info notice = new notice_info
                {
                    notice_id = noticeDto.notice_id,
                    super_id = "",
                    time = noticeDto.time,
                    content = noticeDto.content,
                    name = noticeDto.notice_name,

                };
                var result = DbContext.db.Updateable<notice_info>(notice).UpdateColumns(it=>new {it.name,it.content,it.time}).ExecuteCommand();
                return result > 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error:" + ex.Message);
                return false;
            }
        }
    }
}
