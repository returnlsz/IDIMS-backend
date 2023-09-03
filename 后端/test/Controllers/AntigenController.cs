using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using test.Module.Entities;
using test.Service.Token;
using test.Service.User;
using test.Service.Management.Antigen;

namespace test.WebAPI.Controllers
{
    /// <summary>
    /// 用户上传抗原
    /// </summary>
    [Route("[controller]/[Action]")]
    [ApiController]
    public class AntigenController : ControllerBase
    {
        public IAntigenService _userService;
        public IJWTService _jwtService;
        public AntigenController(IAntigenService userService, IJWTService jwtService) {
            _userService = userService;
            _jwtService = jwtService;
        }
        [HttpPost]
        [Authorize]
        public string UploadAntigen(DateTime test_time,string test_result)
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
            int result= _userService.CheckAndUploadantigen(id, test_time, test_result);
            if (result > 0)
            {
                data = JsonConvert.SerializeObject(new { code = 200, message = "success" });
                return data;
            }
            else
            {
                data = JsonConvert.SerializeObject(new { code = 400, message = "数据库原因，抗原上传失败" });
                return data;
            }
        }
        /// <summary>
        /// 获取用户抗原记录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public string GetUserAntigRecords()
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
            string result = _userService.GetUserAntigRecords(id);
            if(result!="")
            {
                data = JsonConvert.SerializeObject(new { code = 200, message = "success", AntigenRecords= result });
                return data;
            }
            else
            {
                data = JsonConvert.SerializeObject(new { code = 400, message = "找不到该用户的抗原信息" });
                return data;
            }
        }
    }
}
