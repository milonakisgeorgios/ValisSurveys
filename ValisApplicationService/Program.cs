using System;
using System.ServiceProcess;
using System.Windows.Forms;
using Valis.Core;
using ValisApplicationService.GuiElements;

namespace ValisApplicationService
{
    class Program
    {
        static void Main(string[] args)
        {
            bool startGui = false;

            if (args.Length > 0)
            {
                #region
                foreach (var arg in args)
                {
                    string _arg = arg.ToLowerInvariant();

                    if (String.Compare(_arg, "-install", StringComparison.Ordinal) == 0)
                    {
                        if (args.Length > 1)
                            InstallService(args[1]);
                        else
                            InstallService(string.Empty);
                        return;
                    }
                    else if (String.Compare(_arg, "-remove", StringComparison.Ordinal) == 0)
                    {
                        RemoveService();
                        return;
                    }
                    else if (String.Compare(_arg, "-help", StringComparison.Ordinal) == 0 || String.Compare(arg, "?", StringComparison.Ordinal) == 0)
                    {
                        ShowHelp();
                        return;
                    }
                    else if (String.Compare("-gui", _arg, StringComparison.Ordinal) == 0)
                    {
                        startGui = true;
                    }
                }
                #endregion
            }


            if (startGui == true)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                System.Threading.Thread.CurrentThread.Name = "ValisDaemon (GUI)";
                Application.Run(new MainWindow());
            }
            else
            {
                Console.WriteLine("ServicesToRun");
                ServiceBase[] ServicesToRun = new ServiceBase[] { new ValisDaemonService() };
                ServiceBase.Run(ServicesToRun);
            }
        }



        static void RemoveService()
        {
            ShowHeader("Service Remover Invoked");

            if (Utility.IsUserAdministrator() == false)
            {
                Console.WriteLine("\n\nIn order to remove the service, you must be a local administrator!\n\n");

                ShowFooter();
                return;
            }

            if (ValisServiceInstaller.IsServiceInstalled(Globals.ServiceName) == true)
            {
                Console.WriteLine();

                if (ValisServiceInstaller.UninstallService(Globals.ServiceName) == true)
                {
                    Console.WriteLine("* Service '{0}' Removed OK!", Globals.ServiceName);
                }
                else
                {
                    Console.WriteLine("* Service '{0}' did not removed!", Globals.ServiceName);
                }
            }
            else
            {
                Console.WriteLine("* The service '{0}' is not installed!", Globals.ServiceName);
            }
            ShowFooter();
        }
        static void InstallService(string imageName)
        {
            ShowHeader("Service Installer Invoked");

            if (Utility.IsUserAdministrator() == false)
            {
                Console.WriteLine("\n\nIn order to install the service, you must be a local administrator!\n\n");

                ShowFooter();
                return;
            }

            if (ValisServiceInstaller.IsServiceInstalled(Globals.ServiceName) == false)
            {
                string fileImagePath = AppDomain.CurrentDomain.BaseDirectory + System.AppDomain.CurrentDomain.FriendlyName;
                if (!string.IsNullOrEmpty(imageName))
                {
                    fileImagePath = AppDomain.CurrentDomain.BaseDirectory + imageName;
                }

                Console.WriteLine();
                Console.WriteLine("* Service Name = '{0}'", Globals.ServiceName);
                Console.WriteLine("* Service Path = '{0}'", fileImagePath);

                ValisServiceInstaller.InstallService(Globals.ServiceName, Globals.ServiceName, fileImagePath);
            }
            else
            {
                Console.WriteLine("* The service '{0}' is already installed!", Globals.ServiceName);
            }

            ShowFooter();
        }

        static void ShowHelp()
        {
            ShowHeader("Help Page");
            Console.WriteLine("* Command line parameters:");
            Console.WriteLine("*		-install	: installs the service on local machine.");
            Console.WriteLine("*		-remove		: remove the service from local machine");
            Console.WriteLine("*		-gui	    : service runs as gui application");
            Console.WriteLine("*");
            Console.WriteLine("* In order to remove the service, it must be stopped.");
            Console.WriteLine("*	Use 'net stop {0}' to stop the service.", Globals.ServiceName);
            Console.WriteLine("* After installation you should start the service.");
            Console.WriteLine("*	Use 'net start {0}' to start the service.", Globals.ServiceName);
            Console.WriteLine("*");
            Console.WriteLine("* Also be sure that {0}.exe.config file, contains the correct settings!", System.AppDomain.CurrentDomain.FriendlyName);
            ShowFooter();
        }
        static void ShowHeader(string message)
        {
            Console.WriteLine("\n\n\n**************************************************************\n*");
            Console.WriteLine("* ValisDaemon Copyright© 2014 GeorgeMilonakis");
            Console.WriteLine("* {0}...", message);
        }
        static void ShowFooter()
        {
            Console.WriteLine("*");
            Console.WriteLine("**************************************************************\n\n");
        }
    }
}
