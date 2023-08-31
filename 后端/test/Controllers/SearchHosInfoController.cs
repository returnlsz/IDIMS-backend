using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using test.Module.Entities;
using test.Service.Token;
using test.Service.User;

namespace test.WebAPI.Controllers
{
    /// <summary>
    /// 查询对应地区的医疗点信息
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class SearchHosInfoController : ControllerBase
    {
        public IUserService _userService;
        public IJWTService _jwtService;
        public SearchHosInfoController(IUserService userService, IJWTService jwtService)
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
    }
}
