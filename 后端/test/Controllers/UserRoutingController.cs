using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using test.Service.Token;
using test.Service.User;
using test.Service.User.Dto;
using test.Service.Management.UserRouting;

namespace test.WebAPI.Controllers
{
    /// <summary>
    /// 用户上传行程
    /// </summary>
    [Route("[controller]/[Action]")]
    [ApiController]
    public class UserRoutingController : ControllerBase
    {
        public IUserRoutingService _userService;
        public IJWTService _jwtService;
        public UserRoutingController(IUserRoutingService userService, IJWTService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }
        [HttpPost]
        [Authorize]
        public string UploadRouting(DateTime time,string pos_id)
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
            int result = _userService.UserUploadRouting(id,pos_id,time);
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
        /// 获取用户行程
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public string GetRouting()
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
            List<user_itinerary_dto> user_Itinerary_Dtos = _userService.GetRouting(id);
            if(user_Itinerary_Dtos.Count > 0)
            {
                data = JsonConvert.SerializeObject(new { code = 200, message = "success", user_Itinerary_Dtos= user_Itinerary_Dtos });
                return data;
            }
            else
            {
                data = JsonConvert.SerializeObject(new { code = 400, message = "该用户无行程信息" });
                return data;
            }
        }
/*        [HttpGet]

        public List<string> mijie()
        {
            DateTime sickTime = new DateTime(2023, 9, 1, 8, 0, 0);
            return _userService.ContactManagement("sfsf", sickTime);
        }*/
    }
}
