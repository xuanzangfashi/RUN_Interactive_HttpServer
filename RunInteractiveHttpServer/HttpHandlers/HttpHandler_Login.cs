using MySql.Data.MySqlClient;
using MyHttpServer.HttpServer;
using MyHttpServer.HttpServer.HttpHandlers;
using MyHttpServer.Json;
using MyHttpServer.Sql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MyHttpServer.HttpServer.HttpHandlers
{
    class HttpHandler_Login : HttpRequestHandler
    {
        protected override bool OnCreate()
        {
            return base.OnCreate();
        }

        protected override string GetHandle(HttpListenerRequest request, HttpListenerResponse response)
        {
            return JsonWorker.MakeSampleReturnJson(null, new string[] { "Warning", "No response for get method in current subdir!", "404" }).jstr;
        }

        protected override string PostHandle(HttpListenerRequest request, HttpListenerResponse response)
        {
            Dictionary<string, string> urlParams = null;
            HttpListenerManager.Instance.GetUrlParams(request.Url.Query, out urlParams);
            string reStr = "";
            try
            { 
                bool isExist = SqlWorker.MySqlIsExist("floorswaper", "users", new string[] { "user_name", "password" }, new string[] { urlParams["user_name"], urlParams["password"] });
                if (isExist)
                {
                    MySqlConnection conn;
                    string re;
                    var reader = SqlWorker.MySqlQuery("floorswaper", "users", new string[] { "*" }, "user_name", urlParams["user_name"], out conn, out re);
                    string id = "";
                    while (reader.Read())
                    {
                        id = reader.GetString("id");
                    }
                    conn.Close();
                    conn = null;

                    string SignTimeTableName = "id_" + id + "_Sign_time";
                    bool isSignTimeTableExist = SqlWorker.MySqlIsExist("floorswaper", SignTimeTableName);
                    if (!isSignTimeTableExist)
                    {
                        SqlWorker.MySqlCreateTable("floorswaper", SignTimeTableName, new string[] { "sign_in_time", "sign_out_time" });
                    }
                    SqlWorker.MySqlInsert("floorswaper", SignTimeTableName, new string[] { "sign_in_time" }, new string[] { DateTime.Now.ToString() });
                    reStr = JsonWorker.MakeSampleReturnJson(new string[] { "id" }, new string[] { "normal", "OK", "200", id }).jstr;
                }
                else
                {
                    reStr = JsonWorker.MakeSampleReturnJson(null, new string[] { "Warning", "Invaild User", "200" }).jstr;
                }
            }
            catch (Exception e)
            {
                reStr = JsonWorker.MakeSampleReturnJson(null, new string[] { "error", "error", "300" }).jstr;
            }
            return reStr;

            //return base.PostHandle(request, response);
        }
    }
}
