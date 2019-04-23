using MyHttpServer.HttpServer.HttpHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MyHttpServer.Json;
using MyHttpServer.HttpServer;
using MyHttpServer.Sql;

namespace MyHttpServer.HttpServer.HttpHandlers
{
    class HttpHandler_Logout : HttpRequestHandler
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
            SqlWorker.MySqlEdit("floorswaper", "id_" + id + "_Sign_time", new string[] { "sign_out_time" }, new string[] { DateTime.Now.ToString() }, 1, "DESC");

            return JsonWorker.MakeSampleReturnJson(null, new string[] { "normal", "OK", "200" }).jstr;
            //return base.PostHandle(request, response);
        }
    }
}
