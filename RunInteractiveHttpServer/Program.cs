using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RunInteractiveHttpServer.Statics;
using RunInteractiveHttpServer.Sql;
using RunInteractiveHttpServer.HttpServer;

namespace RunInteractiveHttpServer
{
    class Program
    {
        static void Main(string[] args)
        {
            if(!StaticObjects.InitObjects())
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
