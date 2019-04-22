using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using RunInteractiveHttpServer.Json;

namespace RunInteractiveHttpServer.HttpServer.HttpHandlers
{
    public class HttpHandler_UploadFile : HttpRequestHandler
    {
        protected override bool OnCreate()
        {
            return base.OnCreate();
        }

        protected override string GetHandle(HttpListenerRequest request, HttpListenerResponse response)
        {
            return JsonWorker.MakeSampleReturnJson(null, new string[] { "Warning", "Not response for get method in current subdir!", "404" }).jstr;
        }

        protected override string PostHandle(HttpListenerRequest request, HttpListenerResponse response)
        {
            var httpStreamWorker = HttpStreamWorker.CreateHttpStreamWorker(request.RemoteEndPoint.Address.ToString());
            Dictionary<string, string> urlParams = null;
            HttpListenerManager.Instance.GetUrlParams(request.Url.Query, out urlParams);
            if (urlParams != null)
            {
                if (urlParams.ContainsKey("fileName") && urlParams.ContainsKey("dataContinue"))
                {
                    return httpStreamWorker.ReadInputStream(request.InputStream, urlParams["fileName"], urlParams["dataContinue"]);
                }
                else
                {
                    return JsonWorker.MakeSampleReturnJson(null, new string[] { "error", "params incomplete!", "200" }).jstr;
                } 
            }
            else
            {
                return JsonWorker.MakeSampleReturnJson(null, new string[] { "error", "params incomplete!", "200" }).jstr;
            }
        }
    }
}
