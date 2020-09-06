using Microsoft.Extensions.Configuration;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlyGetter.Helper
{
    public class DbHelper
    {
        private static SqlSugarClient db
        {
            get
            {
                return new SqlSugarClient(new ConnectionConfig()
                {
                    ConnectionString = Startup.Configuration.GetConnectionString("SQLConnection"),//必填, 数据库连接字符串
                    DbType = DbType.MySql,         //必填, 数据库类型
                    IsAutoCloseConnection = true,       //默认false, 时候知道关闭数据库连接, 设置为true无需使用using或者Close操作
                    InitKeyType = InitKeyType.Attribute    //默认SystemTable, 字段信息读取, 如：该属性是不是主键，是不是标识列等等信息
                });
            }
        }

        public static List<string> Test()
        {
            try
            {
                var info = db.DbMaintenance.GetDataBaseList(db);
                return info;
            }
            catch (Exception exp)
            {
                return null;
            }
        }
    }
}
