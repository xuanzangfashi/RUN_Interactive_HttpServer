using System;
using MyHttpServer.Statics;
using MyHttpServer.Sql;
using MyHttpServer.HttpServer;
using MyHttpServer.HttpServer.HttpHandlers;


namespace MyHttpServer
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!StaticObjects.InitObjects())
            {
                Debuger.PrintStr("Init StaticObjects faild!", EPRINT_TYPE.ERROR);
                Debuger.ExitProgram();
                return;
            }
            else
            {
                Debuger.PrintStr("Init StaticObjects done!", EPRINT_TYPE.NORMAL);
            }
            if (!SqlWorker.MySqlInit())
            {
                Debuger.PrintStr("Init SqlWorker faild!", EPRINT_TYPE.ERROR);
                Debuger.ExitProgram();
                return;
            }
            else
            {
                Debuger.PrintStr("Init SqlWorker done!", EPRINT_TYPE.NORMAL);
            }
            if (!HttpListenerManager.Instance.Init())
            {
                Debuger.PrintStr("Init HttpListenerManager faild!", EPRINT_TYPE.ERROR);
                Debuger.ExitProgram();
                return;
            }
            else
            {
                Debuger.PrintStr("Init HttpListenerManager done!", EPRINT_TYPE.NORMAL);
            }
            Debuger.PrintStr($"Waiting for client request,time:{DateTime.Now.ToString()}", EPRINT_TYPE.NORMAL);

            if (StaticObjects.IsForceGC)
            {
                Debuger.StartForceGC(StaticObjects.ForceGCInterval);
            }

            HttpRequestHandler.CreateHttpRequestHandler<HttpHandler_Login>("login");
            HttpRequestHandler.CreateHttpRequestHandler<HttpHandler_Logout>("logout");
            HttpRequestHandler.CreateHttpRequestHandler<HttpHandler_EnterMap>("enter_map");


            while (true)
            {
                
                switch(Debuger.InputCommand())
                {
                    case ECOMMAND_TYPE.EXIT:
                        return;
                        break;
                    case ECOMMAND_TYPE.NORMAL:
                        break;
                }
            }

        }
    }
}
