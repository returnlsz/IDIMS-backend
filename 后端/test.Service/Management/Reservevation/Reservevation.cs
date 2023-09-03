using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using test.Common.Db;
using test.Module.Entities;

namespace test.Service.Management.Reservevation
{
    public class ReservevationService: IReservevationService
    {
        private readonly IMapper _mapper;
        public ReservevationService(IMapper mapper)
        {
            _mapper = mapper;
        }
        /// <summary>
        /// 用户预约的数据库操作
        /// </summary>
        /// <param name="user_id"></param>
        /// <param name="hos_name"></param>
        /// <param name="type"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        public int Reservevation(string user_id, string hos_name, int type, DateTime time)
        {
            ///先获取预约表最后一行id的值（最新值）
            List<string> newest = (List<string>)DbContext.db.Queryable<appointment_info>().Select(it => it.id).ToList();
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
            //根据hos_name找到hos_id
            List<string> hos_ids = DbContext.db.Queryable<hospital_info>().Where(it => it.name == hos_name).Select(it => it.id).ToList();
            //如果找不到
            if (hos_ids.Count == 0)
            {
                return -1;
            }
            string hos_id = hos_ids[0];
            appointment_info Appointment_info = new appointment_info()
            {
                id = max,
                user_id = user_id,
                hos_id = hos_id,
                time = time,
                type = type,
                state = 0
            };
            ///返回受影响条数
            return DbContext.db.Insertable(Appointment_info).ExecuteCommand();
        }
        /// <summary>
        /// 获取预约信息
        /// </summary>
        /// <param name="hos_id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<appointment_info_dto> GetUserReservation(string hos_id, int type)
        {
            List<appointment_info_dto> appointment_Info_Dtos = new List<appointment_info_dto>();
            appointment_Info_Dtos = DbContext.db.Queryable<appointment_info>().Where(it => it.hos_id == hos_id && it.type == type && it.state == 0).Select(x => new appointment_info_dto()
            {
                user_id = x.user_id,
                time = x.time,
                state = x.state
            }).ToList();
            return appointment_Info_Dtos;
        }
        /// <summary>
        /// 修改预约信息
        /// </summary>
        /// <param name="hos_id"></param>
        /// <param name="time"></param>
        /// <param name="user_id"></param>
        /// <param name="type"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public int ModifyReservation(string hos_id, DateTime time, string user_id, int type, int state)
        {
            List<appointment_info> appointment_Infos = new List<appointment_info>();
            appointment_Infos = DbContext.db.Queryable<appointment_info>().Where(it => it.hos_id == hos_id
            && it.type == type && it.user_id == user_id && it.time == time).ToList();
            if (appointment_Infos.Count > 0)
            {
                appointment_info appointment_Info = appointment_Infos[0];
                DbContext.db.Tracking(appointment_Info);//创建跟踪
                appointment_Info.state = state;
                int result = DbContext.db.Updateable(appointment_Info).ExecuteCommand();
                return result;
            }
            else
            {
                return -1;
            }
        }
    }
}
