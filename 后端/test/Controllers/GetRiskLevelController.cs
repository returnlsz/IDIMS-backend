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
    /// <summary>
    /// 获取用户所在地的风险等级，默认用户行程表中一个用户有多个行程
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class GetRiskLevelController : ControllerBase
    {
        public IUserService _userService;
        public IJWTService _jwtService;
        public GetRiskLevelController(IUserService userService, IJWTService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }
        [HttpGet]
        [Authorize]
        public string GetRiskLevel()
        {
            string token;
            string data;
            try
            {
                token = HttpContext.Request.Headers["Authorization"].ToString().Split(" ")[1];
            }
            catch (SecurityTokenExpiredException)
            {
                data = JsonConvert.SerializeObject(new { code = 400, ErrorMessage = "用户凭证已过期，请重新登陆" });
                return data;
            }
            catch (Exception)
            {
                data = JsonConvert.SerializeObject(new { code = 400, ErrorMessage = "无效令牌签名" });
                return data;
            }
            string id = _jwtService.DecodeToken(token);
            int dangerlevel = _userService.GetRiskLevel(id);
            if (dangerlevel == -1)
            {
                data = JsonConvert.SerializeObject(new
                {
                    code = 400,
                    message = "查不到用户行程信息"
                });
                return data;
            }
            data = JsonConvert.SerializeObject(new { code = 200, message = "success", dangerlevel = dangerlevel });
            return data;
        }
    }
}
