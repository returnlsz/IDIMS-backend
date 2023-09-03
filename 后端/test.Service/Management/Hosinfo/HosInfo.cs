using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using AutoMapper;
using Newtonsoft.Json;
using SqlSugar;
using test.Common.Db;
using test.Module.Entities;


namespace test.Service.Management.HosInfo
{
    public class HosInfoService : IHosInfoService
    {
        private readonly IMapper _mapper;
        public HosInfoService(IMapper mapper)
        {
            _mapper = mapper;
        }
        /// <summary>
        /// 查询对应地区的医疗点信息
        /// </summary>
        /// <param name="province"></param>
        /// <param name="city"></param>
        /// <param name="area"></param>
        /// <param name="street"></param>
        /// <returns></returns>
        public List<hospital_info> SearchHosInfo(string province, string city, string area, string street)
        {
            ///根据地区信息找到pos_id
            List<regioninfo> Regioninfo = DbContext.db.Queryable<regioninfo>().Where(it => it.province == province
            && it.city == city && it.area == area && it.Street == street).ToList();
            if (Regioninfo.Count == 0)
            {
                return new List<hospital_info>();
            }
            string pos_id = Regioninfo[0].pos_id;
            ///根据pos_id找到医疗点
            List<hospital_info> hospital_Info = DbContext.db.Queryable<hospital_info>().Where(it => it.pos_id == pos_id).ToList();
            return hospital_Info;
        }
        /// <summary>
        /// 查询用户所在地的医疗点信息的数据库操作
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetHosInfo(string id)
        {
            List<user_itinerary> user_Itineraries = new List<user_itinerary>();
            //查询行程
            user_Itineraries = DbContext.db.Queryable<user_itinerary>().Where(it => it.id == id).ToList();
            //如果查不到用户行程
            if (user_Itineraries.Count() == 0)
            {
                return "";
            }
            // 1. 得到最大日期字符串
            var maxDateStr = user_Itineraries
              .Select(x => x.time)
              .Max();
            // 2. 根据最大日期字符串查找对象
            var maxItinerary = user_Itineraries
              .First(x => x.time == maxDateStr);
            //获取用户最新经过地区的pos_id
            string pos_id = maxItinerary.pos_id;
            //根据pos_id查医疗点
            var mid = DbContext.db.Queryable<hospital_info>().Where(it => it.pos_id == pos_id).Select(x => new
            {
                x.name,
                x.max_num_of_people,
                x.current_num_of_people,
                x.pos_id
            }).ToList();
            if (mid.Count == 0)
            {
                return "";
            }
            return JsonConvert.SerializeObject(new { Hospital_info = mid });
        }
    }
}
