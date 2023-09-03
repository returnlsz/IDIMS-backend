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
using test.Service.Management.UserRouting;
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
        /// 查出谁是密接的服务，返回密接者的id列表
        /// </summary>
        /// <param name="id"></param>
        /// <param name="sick_time"></param>
        /// <returns></returns>
        public List<string> ContactManagement(string id,DateTime sick_time)
        {
            List<user_itinerary_dto> user_Itinerary_Dtos = new List<user_itinerary_dto>();
            ///获取用户成为阳性前两个小时的所有pos_id和time
            user_Itinerary_Dtos = DbContext.db.Queryable<user_itinerary>()
              .Where(it => it.id == id &&
                          it.time > sick_time.AddHours(-2) &&
                          it.time < sick_time)
              .Select(x => new user_itinerary_dto()
              {
                  pos_id = x.pos_id,
                  time = x.time
              })
              .ToList();

            // 1. sick用户行程记录
            var sickItineraries = user_Itinerary_Dtos;

            // 2. 记录所有密接用户id
            List<string> contactIds = new List<string>();

            foreach (var itinerary in sickItineraries)
            {
                // pos_id和时间范围
                var posId = itinerary.pos_id;
                var time = itinerary.time;

                // 3. 查询其他用户
                var others = DbContext.db.Queryable<user_itinerary>()
                  .Where(u => u.id != id && // 不包括sick用户自己
                              u.pos_id == posId &&
                              u.time > time.AddHours(-1) &&
                              u.time < time.AddHours(1))
                  .Select(u => u.id)
                  .ToList();

                // 4. 添加到结果集
                contactIds.AddRange(others);
            }
            // 去重 contactIds
            contactIds = contactIds.Distinct().ToList();
            //这时候可能包含一些阳性用户被判定为密接，排除这些人
            // 去除已确诊阳性用户
            List<string> filteredContactIds = new List<string>();
            foreach (var contactId in contactIds)
            {
                var userHealth = DbContext.db.Queryable<user_health_state>()
                  .Where(u => u.id == contactId)
                  .Select(u => u.current_status)
                  .First();

                if (userHealth != "阳性")
                {
                    filteredContactIds.Add(contactId);
                }
            }
            return filteredContactIds;
        }
    }
}
