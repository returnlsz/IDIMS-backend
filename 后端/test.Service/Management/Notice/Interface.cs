using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace test.Service.Management.Notice
{
    public interface ICreateNotice
    {
        bool CreateNotice(NoticeDto noticeDto,string id);
    }
    public interface IUpdateNotice
    {
        bool UpdateNotice(UpdateDto noticeDto);
    }
    public interface IGetAllNotice
    {
        List<Tuple<string, string, int>> GetAllNotice();
    }
    
}
