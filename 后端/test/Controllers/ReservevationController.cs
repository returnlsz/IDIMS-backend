using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using test.Service.Token;
using test.Service.User;
using test.Service.Management.Reservevation;

namespace test.WebAPI.Controllers
{
    /// <summary>
    /// 和预约有关的
    /// </summary>
    [Route("[controller]/[Action]")]
    [ApiController]
    public class ReservevationController : ControllerBase
    {
        public IReservevationService _userService;
        public IJWTService _jwtService;
        public ReservevationController(IReservevationService userService, IJWTService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }
        /// <summary>
        /// 用户预约
        /// </summary>
        /// <param name="hos_name"></param>
        /// <param name="type"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public string Reservevation(string hos_name,int type,DateTime time)
        {
            string token;
            string data;
            try
            {
                token = HttpContext.Request.Headers["Authorization"].ToString().Split(" ")[1];
            }
            catch (SecurityTokenExpiredException)
            {
                data = JsonConvert.SerializeObject(new { code = 400, message = "用户凭证已过期，请重新登陆" });
                return data;
            }
            catch (Exception)
            {
                data = JsonConvert.SerializeObject(new { code = 400, message = "无效令牌签名" });
                return data;
            }
            string id = _jwtService.DecodeToken(token);
            int result=_userService.Reservevation(id, hos_name, type, time);
            if (result > 0)
            {
                data = JsonConvert.SerializeObject(new { code = 200, message = "success" });
                return data;
            }
            else
            {
                data = JsonConvert.SerializeObject(new { code = 400, message = "数据库原因，预约失败" });
                return data;
            }
        }
        /// <summary>
        /// 获取用户预约信息
        /// </summary>
        /// <param name="hos_id"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet]
        public string GetUserReservation(string hos_id,int type) {
            string data;
            List<appointment_info_dto> Appointment_info_dto = _userService.GetUserReservation(hos_id,type);
            if(Appointment_info_dto.Count()==0) {
                data = JsonConvert.SerializeObject(new { code = 400, message = "无预约信息" });
                return data;
            }
            else
            {
                data = JsonConvert.SerializeObject(new { code = 200, message = "success", ReservationList= Appointment_info_dto });
                return data;
            }
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
        [HttpPost]
        public string ModifyReservation(string hos_id,DateTime time,string user_id,int type,int state)
        {
            string data;
            int result = _userService.ModifyReservation(hos_id, time, user_id, type, state);
            if (result > 0)
            {
                data = JsonConvert.SerializeObject(new { code = 200, message = "success" });
                return data;
            }
            else
            {
                data = JsonConvert.SerializeObject(new { code = 400, message = "数据库原因，修改失败" });
                return data;
            }
        }
    }
}
