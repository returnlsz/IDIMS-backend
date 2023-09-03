using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using test.Module.Entities;
using test.Service.Token;
using test.Service.User;
using test.Service.Management.HosInfo;

namespace test.WebAPI.Controllers
{
    /// <summary>
    /// 查询对应地区的医疗点信息
    /// </summary>
    [Route("[controller]/[Action]")]
    [ApiController]
    public class HosInfoController : ControllerBase
    {
        public IHosInfoService _userService;
        public IJWTService _jwtService;
        public HosInfoController(IHosInfoService userService, IJWTService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }
        [HttpGet]
        public string SearchHosInfo(string province,string city,string area,string street)
        {
            string data;
            List<hospital_info> hospital_Info=_userService.SearchHosInfo(province, city, area, street);
            if(hospital_Info.Count > 0)
            {
                data = JsonConvert.SerializeObject(new { code = 200, message = "success",
                    hospital_Info=hospital_Info
                });
                return data;
            }
            else
            {
                data = JsonConvert.SerializeObject(new { code = 400, message = "找不到该地区的医疗点信息" });
                return data;
            }
        }
        /// <summary>
        /// 获取用户所在地的医疗点信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public string GetHosInfo()
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
            string mid = _userService.GetHosInfo(id);
            if(mid == "")
            {
                data = JsonConvert.SerializeObject(new { code = 400, message = "找不到对应的医疗点信息" });
                return data;
            }
            else
            {
                data = JsonConvert.SerializeObject(new { code = 200, message = "success", MedicalPointList=mid });
                return data;
            }
        }
    }
}
