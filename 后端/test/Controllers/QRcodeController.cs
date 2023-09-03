using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using test.Common.Db;
using test.Module.Entities;
using test.Service.Token;
using test.Service.User;

namespace test.WebAPI.Controllers
{
    [Route("[controller]/[Action]")]
    [ApiController]
    public class QRcodeController : ControllerBase
    {
        public IUserService _userService;
        public IJWTService _jwtService;
        public QRcodeController(IUserService userService, IJWTService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }
        /// <summary>
        /// 用户预约生成二维码功能
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public string AppointmentQRcode()
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
            ///查预约表看是什么type，默认一定有记录
            List<appointment_info> appointment_Infos = new List<appointment_info>();
            appointment_Infos = DbContext.db.Queryable<appointment_info>().Where(it => it.user_id == id).OrderByDescending(a => a.time).Take(1).ToList();
            //取最新记录
            ///筛选最新的记录
            appointment_info appointment_Info = appointment_Infos[0];
            data = JsonConvert.SerializeObject(new { code = 200, message = "success", user_id=id, type= appointment_Info.type});
            return data;

        }
    }
}
