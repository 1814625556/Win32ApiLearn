using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace SearchBar
{
    public class DateBaseHelper
    {
        public const string conStr = @"Data Source=C:\Program Files (x86)\开票软件\440002999999493.0\Bin\cc3268.dll;Pooling=true;Password=LoveR1314;";


        //读取上传实体
        public static dynamic GetHzscResult(string sqdh= "661543812468190619150832")
        {
            var redNotificationNo = "";
            var date = "";
            using (var con = new SQLiteConnection(conStr))
            {
                con.Open();
                var cmd = new SQLiteCommand($"select XXBBH,TKRQ from HZFP_SQD where SQDH='{sqdh}'", con) {CommandType = CommandType.Text};

                using (var reader = cmd.ExecuteReader())
                {
                    reader.Read();
                    redNotificationNo = reader.IsDBNull(0) ? null : reader.GetString(0);
                    date = reader.IsDBNull(1) ? null : reader.GetString(1);
                }
            }
            return new {redNotificationNo, date};
        }

        public static DataTable GetHzscResult2(string sqdh = "661543812468190619150832")
        {
            var table =  new DataTable();
            using (var con = new SQLiteConnection(conStr))
            {
                con.Open();
                using (var adapt = new SQLiteDataAdapter($"select XXBBH,TKRQ from HZFP_SQD where SQDH='{sqdh}'", con))
                {
                    adapt.Fill(table);
                }
            }
            return table;
        }



    }
}
