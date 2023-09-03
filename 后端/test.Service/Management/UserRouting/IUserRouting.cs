using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test.Service.Management.UserRouting
{
    public interface IUserRoutingService
    {
        //获取用户行程
        List<user_itinerary_dto> GetRouting(string id);
        //用户上传行程
        int UserUploadRouting(string id, string pos_id, DateTime time);
    }
}
