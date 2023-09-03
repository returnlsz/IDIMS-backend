using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test.Service.Management.Reservevation
{
    public interface IReservevationService
    {
        //用户预约的数据库操作
        int Reservevation(string user_id, string hos_name, int type, DateTime time);
        //获取预约信息
        List<appointment_info_dto> GetUserReservation(string hos_id, int type);
        //修改预约信息
        int ModifyReservation(string hos_id, DateTime time, string user_id, int type, int state);
    }
}
