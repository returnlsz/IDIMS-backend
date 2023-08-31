using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using test.Common.Db;
using test.Module.Entities;
using test.Service.Token;
using test.Service.User;


namespace test.WebAPI.Controllers
{
    /// <summary>
    /// 获取全国所有地区的风险等级
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class SearchRiskLevelController : ControllerBase
    {
        public IUserService _userService;
        public IJWTService _jwtService;
        public SearchRiskLevelController(IUserService userService, IJWTService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }
        [HttpGet]
        public string SearchRiskLevel()
        {
            List<string> province = new List<string>() {
                "黑龙江省","吉林省","辽宁省","河北省","甘肃省","青海省","陕西省","河南省","山东省","山西省",
                "安徽省","湖北省","湖南省","江苏省","四川省","贵州省","云南省","浙江省","江西省","广东省",
                "福建省","台湾省","海南省","新疆维吾尔自治区","内蒙古自治区","宁夏回族自治区","广西壮族自治区",
                "西藏自治区","北京市","上海市","天津市","重庆市","香港","澳门"
            };
            //表名暂定dangerlevelinfo，后续可以更改
            List<dangerlevelinfo> Dangerlevelinfo = new List<dangerlevelinfo>();
            Dangerlevelinfo= DbContext.db.Queryable<dangerlevelinfo>().ToList();
            //条数
            int nums = Dangerlevelinfo.Count();
            string data;
            data = JsonConvert.SerializeObject(new { code = 200, message = "success", DangerLevelInfo= Dangerlevelinfo });
            return data;
        }
    }
}
