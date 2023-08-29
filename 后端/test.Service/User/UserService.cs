using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using AutoMapper;
using Newtonsoft.Json;
using SqlSugar;
using test.Common.Db;
using test.Module.Entities;
using test.Service.User.Dto;

namespace test.Service.User
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        public UserService(IMapper mapper)
        {
            _mapper = mapper;
        }
        public async Task<Users> CheckLogin(LoginDto login)
        {
            return await DbContext.db.Queryable<Users>().FirstAsync(m => m.id.Equals(login.id) && m.pwd.Equals(login.pwd));
        }
        public UserDto AddUser(InputUserDto input)
        {
            Users user = TransInputDto(input);
            if (!DbContext.db.Queryable<Users>().Any(m => m.id.Equals(input.id)))
            {
                DbContext.db.Insertable(user).ExecuteCommand();
                return _mapper.Map<UserDto>(user);
            }
            else
                throw new Exception("id已存在");
        }
        private Users TransInputDto(InputUserDto input)
        {
            var user = _mapper.Map<Users>(input);
            return user;
        }
        /// <summary>
        /// 用户上传抗原的数据库操作
        /// </summary>
        /// <param name="id"></param>
        /// <param name="test_time"></param>
        /// <param name="test_result"></param>
        /// <returns></returns>
        public int CheckAndUploadantigen(string id, DateTime test_time, string test_result)
        {
            ///数据库写入操作
            ///
            antigen_record antigen_Record = new antigen_record()
            {
                user_id = id,
                test_time = test_time,
                test_result = test_result
            };
            ///返回受影响条数
            return DbContext.db.Insertable(antigen_Record).ExecuteCommand();
        }
        /// <summary>
        /// 用户举报的数据库操作
        /// </summary>
        /// <returns></returns>
        public int UserPutReport(DateTime time, string id, string content)
        {
            ///先获取report表最后一行message_id的值（最新值）
            List<string> newest = (List<string>)DbContext.db.Queryable<report_info>().Select(it => it.message_id).ToList();
            string max = newest.OrderByDescending(x => x).First();
            int maxint = int.Parse(max);
            maxint++;
            max = maxint.ToString();
            report_info report_Info = new report_info()
            {
                time = time,
                user_id = id,
                state = 0,
                message_id = max,
                content = content
            };
            ///返回受影响条数
            return DbContext.db.Insertable(report_Info).ExecuteCommand();
        }
        /// <summary>
        /// 用户上传行程的数据库操作
        /// </summary>
        /// <returns></returns>
        public int UserUploadRouting(string id, string pos_id, DateTime time)
        {
            user_itinerary user_Itinerary = new user_itinerary()
            {
                id = id,
                pos_id = pos_id,
                time = time
            };

            ///返回受影响条数
            return DbContext.db.Insertable(user_Itinerary).ExecuteCommand();
        }
        /// <summary>
        /// 根据pos_id返回地区信息
        /// </summary>
        /// <param name="pos_id"></param>
        /// <returns></returns>
        public List<regioninfo> PosIdGetLocation(string pos_id)
        {
            List<regioninfo> Regioninfo = DbContext.db.Queryable<regioninfo>()
                        .Where(it => it.pos_id == pos_id).ToList();
            return Regioninfo;
        }
        /// <summary>
        /// 返回用户的健康信息表的数据库查询操作
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<user_health_state> GetUserHealthInfo(string id)
        {
            List<user_health_state> user_Health_state = DbContext.db.Queryable<user_health_state>()
                        .Where(it => it.id == id).ToList();
            return user_Health_state;
        }
        /// <summary>
        /// 查询用户所在地的风险等级的数据库操作
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public int GetRiskLevel(string id)
        {
            List<user_itinerary> user_Itineraries = new List<user_itinerary>();
            //查询行程
            user_Itineraries = DbContext.db.Queryable<user_itinerary>().Where(it => it.id == id).ToList();
            //如果查不到用户行程
            if (user_Itineraries.Count() == 0)
            {
                return -1;
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
            //根据该pos_id查询对应区域
            List<regioninfo> regioninfos = new List<regioninfo>();
            regioninfos = DbContext.db.Queryable<regioninfo>().Where(it => it.pos_id == pos_id).ToList();
            //根据区域信息差风险等级表
            List<dangerlevelinfo> dangerlevelinfos = new List<dangerlevelinfo>();
            dangerlevelinfos = DbContext.db.Queryable<dangerlevelinfo>().Where(it => it.province == regioninfos[0].province
            && it.city == regioninfos[0].city
            && it.area == regioninfos[0].area).ToList();

            return dangerlevelinfos[0].DangerLevel;
        }
        /// <summary>
        /// 修改用户信息的数据库操作
        /// </summary>
        /// <returns></returns>
        public int ModifyUserInfo(string id, string name, string gender, string password, string phone_number, string user_address)
        {
            ///默认一定能查询到用户信息
            List<userinfo> Userinfos = DbContext.db.Queryable<userinfo>().Where(it => it.id == id).ToList();
            if (Userinfos.Count == 0)
            {
                return -1;
            }
            userinfo Userinfo = Userinfos[0];
            DbContext.db.Tracking(Userinfo);//创建跟踪
            Userinfo.name = name;
            Userinfo.gender = gender;
            Userinfo.password = password;
            Userinfo.phone_number = phone_number;
            Userinfo.user_address = user_address;
            int result = DbContext.db.Updateable(Userinfo).ExecuteCommand();
            return result;
        }
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public string GetUserInfo(string id)
        {
            List<userinfo> Userinfos = DbContext.db.Queryable<userinfo>().Where(it => it.id == id).ToList();
            if (Userinfos.Count == 0)
            {
                return "";
            }
            userinfo Userinfo = Userinfos[0];
            string data;
            data = JsonConvert.SerializeObject(new
            {
                name = Userinfo.name,
                gender = Userinfo.gender,
                password = Userinfo.password,
                phone_number = Userinfo.phone_number,
                user_address = Userinfo.user_address,
                user_id = Userinfo.id,
                code = 200,
                message = "success"
            });
            return data;
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
    }
}
