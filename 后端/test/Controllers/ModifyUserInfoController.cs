using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using test.Service.Token;
using test.Service.User;

namespace test.WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ModifyUserInfoController : ControllerBase
    {
        public IUserService _userService;
        public IJWTService _jwtService;
        public ModifyUserInfoController(IUserService userService, IJWTService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }
        /// <summary>
        /// 修改用户信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="gender"></param>
        /// <param name="password"></param>
        /// <param name="phone_number"></param>
        /// <param name="user_address"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public string ModifyUserInfo(string name,string gender,string password,string phone_number,string user_address)
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
            ///返回受影响的行数
            int result = _userService.ModifyUserInfo(id, name,gender,password,phone_number,user_address);
            if (result > 0)
            {
                data = JsonConvert.SerializeObject(new { code = 200, message = "success" });
                return data;
            }
            else
            {
                data = JsonConvert.SerializeObject(new { code = 400, message = "数据库原因，更新失败" });
                return data;
            }
        }
    }
}
