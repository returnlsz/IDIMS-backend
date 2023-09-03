using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace test.Common.Db
{
    //测试用，最后改成实际操作的
    /*    public class DbContext
        {
            public static SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = "Data Source=127.0.0.1:1521/orcl;Persist Security Info=True;User ID=hr;PassWord=123;",
                DbType = DbType.Oracle,
                IsAutoCloseConnection = true
            });
        }*/

    /// <summary>
    /// 我本地调试的数据库信息，后端另外的同学调试时注释即可
    /// </summary>
    public class DbContext
    {
        public static SqlSugarClient db = new SqlSugarClient(new ConnectionConfig()
        {
            ConnectionString = "Data Source=localhost:1521/orcl2;Persist Security Info=True;User ID='\"C##USER2\"';PassWord=lsz2151280;",
            //ConnectionString = "DATA SOURCE=localhost:1521/orcl2;TNS_ADMIN=C:\\Users\\LISONGZE\\Oracle\\network\\admin;PERSIST SECURITY INFO=True;USER ID='\"C##USER2\"'",
            //DATA SOURCE=localhost:1521/orcl2;TNS_ADMIN=C:\Users\LISONGZE\Oracle\network\admin;PERSIST SECURITY INFO=True;USER ID='"C##USER2"'
            DbType = DbType.Oracle,
            IsAutoCloseConnection = true
        });
    }
}
