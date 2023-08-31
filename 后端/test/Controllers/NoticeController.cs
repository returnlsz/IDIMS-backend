using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using test.Common;
using test.Service.Management.Notice;
using test.Service.User;
using test.Service.Token;
using Newtonsoft.Json;

using test.Module.Entities;


namespace test.WebAPI.Controllers
{
    [Route("[controller]/[Action]")]
    [ApiController]
    public class NoticeController:ControllerBase
    {
        private readonly ICreateNotice _createNotice;
        private readonly IUpdateNotice _updateNotice;
        private readonly IUserService _userService;
        private readonly IJWTService _jwtService;
        public NoticeController(ICreateNotice createNotice,IUpdateNotice updateNotice,
                                IUserService userService,IJWTService jwtService)
        {
            _createNotice = createNotice;
            _updateNotice = updateNotice;
            _userService = userService;
            _jwtService = jwtService;
        }

        [HttpPost]
        [Authorize]
        public ActionResult<object> CreateNotice([FromBody] NoticeDto input)
        {
            if (input == null)
            {
                return BadRequest("NullInput");
            }
            var notice = new NoticeDto
            {
                content = input.content,
                name = input.name,
                time = input.time,

            };
            string token;
            token = HttpContext.Request.Headers["Authorization"].ToString().Split(" ")[1];
            //string id = _jwtService.DecodeToken(token);
            string id = token;
            var result = _createNotice.CreateNotice(notice,id);
            if(result==true)
                return Ok("success");
            else
                return BadRequest("fail to insert notice");
        }

        [HttpPost]
        [Authorize]
        public ActionResult<object> UpdateNotice([FromBody] UpdateDto input)
        {
            if (input == null)
            {
                return BadRequest("Null Input");
            }
            var update_notice = new UpdateDto
            {
                notice_id = input.notice_id,
                notice_name = input.notice_name,
                content = input.content,
                time = input.time,
            };
            var result = _updateNotice.UpdateNotice(update_notice);
            if (result == true)
                return Ok("Success");
            else
                return BadRequest("Fail to Update");
        }
    }
}
