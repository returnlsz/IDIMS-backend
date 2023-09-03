using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using test.Module.Entities;
using test.Service.Management.Report;
using test.Service.Token;
using test.Service.User;

namespace test.WebAPI.Controllers
{
    [Route("[controller]/[Action]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        public IReportService _userService;
        public IJWTService _jwtService;
        public ReportController(IReportService userService, IJWTService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }
        /// <summary>
        /// 超级用户处理举报信息
        /// </summary>
        /// <param name="message_id"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public string FinishReport(string message_id)
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
            ///超级用户的id
            string id = _jwtService.DecodeToken(token);
            string result=_userService.FinishReport(message_id);
            if (result =="")
            {
                data = JsonConvert.SerializeObject(new { code = 400, message = "数据库原因，修改失败" });
                return data;
            }
            else
            {
                data = JsonConvert.SerializeObject(new { code = 200, message = "success",report_info=result});
                return data;
            }
        }
        /// <summary>
        /// 管理员获取用户举报信息（所有未处理的）
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public string GetReport()
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
            ///超级用户的id
            string id = _jwtService.DecodeToken(token);
            List<report_info> report_Infos = new List<report_info>();
            report_Infos = _userService.GetReport();
            if (report_Infos.Count == 0)
            {
                data = JsonConvert.SerializeObject(new { code = 400, message = "无未处理的举报信息" });
                return data;
            }
            else
            {
                data = JsonConvert.SerializeObject(new { code = 200, message = "success", report_Infos = report_Infos });
                return data;
            }
        }
        /// <summary>
        /// 用户上传举报信息
        /// </summary>
        /// <param name="content"></param>
        /// <param name="time"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        public string Report(string content, DateTime time)
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
            int result = _userService.UserPutReport(time, id, content);
            if (result > 0)
            {
                data = JsonConvert.SerializeObject(new { code = 200, message = "success" });
                return data;
            }
            else
            {
                data = JsonConvert.SerializeObject(new { code = 400, message = "数据库原因，举报信息上传失败" });
                return data;
            }
        }
        /// <summary>
        /// 用户查看自己的举报信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Authorize]
        public string GetSelfReport()
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
            List<report_info> report_Infos = new List<report_info>();
            report_Infos = _userService.GetReport(id);
            if (report_Infos.Count == 0)
            {
                data = JsonConvert.SerializeObject(new { code = 400, message = "此用户无举报信息" });
                return data;
            }
            else
            {
                data = JsonConvert.SerializeObject(new { code = 200, message = "success", Reports= report_Infos });
                return data;
            }
        }
    }
}
