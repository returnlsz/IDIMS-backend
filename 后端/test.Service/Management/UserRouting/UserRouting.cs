using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using test.Common.Db;
using test.Module.Entities;

namespace test.Service.Management.UserRouting
{
    public class UserRoutingService: IUserRoutingService
    {
        private readonly IMapper _mapper;
        public UserRoutingService(IMapper mapper)
        {
            _mapper = mapper;
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
        /// 获取用户行程
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<user_itinerary_dto> GetRouting(string id)
        {
            List<user_itinerary_dto> user_Itinerary_Dtos = DbContext.db.Queryable<user_itinerary>().Where(it => it.id == id).Select(x => new user_itinerary_dto()
            {
                pos_id = x.pos_id,
                time = x.time
            }).OrderByDescending(dto => dto.time).ToList();
            return user_Itinerary_Dtos;
        }
    }
}
