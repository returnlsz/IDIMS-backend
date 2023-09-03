using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using test.Module.Entities;
using test.Service.User.Dto;

namespace test.Service.User
{
    public interface IUserService
    {
        //登录
        Task<Users> CheckLogin(LoginDto login);
        //注册
        UserDto AddUser(InputUserDto input);
        //根据pos_id获取地区信息
        List<regioninfo> PosIdGetLocation(string pos_id);
        //获取用户健康状态
        List<user_health_state> GetUserHealthInfo(string id);
        //修改用户信息
        int ModifyUserInfo(string id, string name, string gender, string password, string phone_number, string user_address);
        //获取用户信息
        string GetUserInfo(string id);
        //密接管理
        List<string> ContactManagement(string id, DateTime sick_time);
    }
}
