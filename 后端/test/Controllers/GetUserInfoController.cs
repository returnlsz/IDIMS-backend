using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using test.Service.Token;
using test.Service.User;

namespace test.WebAPI.Controllers
{
    /// <summary>
    /// 获取用户个人信息
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class GetUserInfoController : ControllerBase
    {
        public IUserService _userService;
        public IJWTService _jwtService;
        public GetUserInfoController(IUserService userService, IJWTService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }
        [HttpGet]
        [Authorize]
        public string GetUserInfo()
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
            data= _userService.GetUserInfo(id);
            if (data == "")
            {
                data = JsonConvert.SerializeObject(new { code = 400, message = "找不到用户信息" });
                return data;
            }
            else
                return data;
        }
    }
}
