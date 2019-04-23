using MyHttpServer.HttpServer;
using MyHttpServer.HttpServer.HttpHandlers;
using MyHttpServer.Json;
using MyHttpServer.Sql;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MyHttpServer.HttpServer.HttpHandlers
{
    class HttpHandler_EnterMap : HttpRequestHandler
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

            string id = urlParams["id"];
            string tableName = "id_" + id.ToString() + "_entered_map";
            bool isTableExist = SqlWorker.MySqlIsExist("floorswaper", tableName);
            int mapTimes = 1;
            if (!isTableExist)
            {
                SqlWorker.MySqlCreateTable("floorswaper", tableName, new string[] { urlParams["map_name"] });
                SqlWorker.MySqlInsert("floorswaper", tableName, new string[] { urlParams["map_name"] }, new string[] { mapTimes.ToString() });
                return JsonWorker.MakeSampleReturnJson(null, new string[] { "normal", "OK", "200" }).jstr;
            }
            else
            {
                if(SqlWorker.MySqlIsExist("floorswaper", tableName, urlParams["map_name"]))
                {
                    MySqlConnection conn;
                    string outStr = "";
                    var reader = SqlWorker.MySqlQuery("floorswaper", tableName, new string[] { urlParams["map_name"] }, "id", id, out conn, out outStr);
                    reader.Read();
                    mapTimes = int.Parse(reader.GetString(urlParams["map_name"])) + 1;
                    SqlWorker.MySqlEdit("floorswaper", tableName, id, new string[] { urlParams["map_name"] }, new string[] { mapTimes.ToString() });
                    conn.Close();
                    conn = null;
                    return JsonWorker.MakeSampleReturnJson(null, new string[] { "normal", "OK", "200" }).jstr;
                }
                else
                {
                    SqlWorker.MySqlCreateColumn("floorswaper", tableName, urlParams["map_name"]);
                    SqlWorker.MySqlEdit("floorswaper", tableName, id, new string[] { urlParams["map_name"] }, new string[] { mapTimes.ToString() });
                    return JsonWorker.MakeSampleReturnJson(null, new string[] { "normal", "OK", "200" }).jstr;
                }
            }
            
            //return base.PostHandle(request, response);
        }
    }
}
