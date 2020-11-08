using log4net;
using System;
using System.Globalization;
using System.ServiceProcess;

namespace ValisApplicationService
{
    partial class ValisDaemonService : ServiceBase
    {
        protected static ILog Logger = LogManager.GetLogger(typeof(ValisDaemonService));

        public ValisDaemonService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                if (!TheController.Instance.Start())
                {
                    Logger.InfoFormat("{0} service DID NOT start!", Globals.ServiceName);
                }
                else
                {
                    Logger.InfoFormat("{0} service STARTED!", Globals.ServiceName);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }

        protected override void OnStop()
        {
            try
            {
                if (TheController.Instance.IsAlive)
                {
                    TheController.Instance.Stop();
                    TheController.Instance.Quit();

                    Logger.InfoFormat("{0} service stopped!", Globals.ServiceName);
                }
                else
                {
                    Logger.InfoFormat("{0} service DID NOT stop!", Globals.ServiceName);
                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex);
            }
        }
    }
}
