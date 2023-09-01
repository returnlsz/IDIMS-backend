using Microsoft.AspNetCore.Mvc;
using test.Service.Management.GovWithInfectionInfo;
using test.Service.Token;
using test.Module.Entities;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace test.WebAPI.Controllers
{
    [Route("[controller]/[Action]")]
    [ApiController]
    public class GetInfectNumberController : Controller
    {
        private readonly IGWIInterface _igwiinterface;

        public GetInfectNumberController(IGWIInterface igwiInterface)
        {
            _igwiinterface=igwiInterface;
        }
        [HttpGet]
        [Authorize]
        public ActionResult<object> GetProvinceInfector(DateTime time)
        {
            var result = _igwiinterface.GetInfectPeopleNumbers(CheckType.Province,time);
            if(result == null)
            {
                var Data = JsonConvert.SerializeObject(new
                {
                    message = "null result",
                    City = result
                });
                return Ok(Data);
            }
            else
            {
                var Data = JsonConvert.SerializeObject(new
                {
                    message = "success",
                    City = result
                });
                return Ok(Data);
            }
          
        }
        [HttpGet]
        [Authorize]
        public ActionResult<object> GetCityInfector(DateTime time)
        {
            var result = _igwiinterface.GetInfectPeopleNumbers(CheckType.City, time);
            if (result == null)
            {
                var Data = JsonConvert.SerializeObject(new
                {
                    message = "null result",
                    DistrictInfector = result
                });
                return Ok(Data);
            }
            else
            {
                var Data = JsonConvert.SerializeObject(new
                {
                    message = "success",
                    DistrictInfector = result
                });
                return Ok(Data);
            }

        }
        [HttpGet]
        [Authorize]
        public ActionResult<object> GetDistrictInfector(DateTime time)
        {
            var result = _igwiinterface.GetInfectPeopleNumbers(CheckType.Region, time);
            if (result == null)
            {
                var Data = JsonConvert.SerializeObject(new
                {
                    message = "null result",
                    ProvinceInfector = result
                });
                return Ok(Data);
            }
            else
            {
                var Data = JsonConvert.SerializeObject(new
                {
                    message = "success",
                    ProvinceInfector = result
                });
                return Ok(Data);
            }

        }
        [HttpGet]
        [Authorize]

        public ActionResult<object> GetDistrictInfectorAndTime(DateTime pos_id)
        {
            var result = _igwiinterface.GetDayInfectNumbers(pos_id);
            if (result == null)
            {
                var Data = JsonConvert.SerializeObject(new
                {
                    message = "null result",
                    ProvinceInfector = result
                });
                return Ok(Data);
            }
            else
            {
                var Data = JsonConvert.SerializeObject(new
                {
                    message = "success",
                    ProvinceInfector = result
                });
                return Ok(Data);
            }
        }
    }
}
