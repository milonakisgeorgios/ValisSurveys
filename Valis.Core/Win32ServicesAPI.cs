using System;
using System.Runtime.InteropServices;

namespace Valis.Core
{
    [StructLayout(LayoutKind.Sequential)]
    class QUERY_SERVICE_CONFIG
    {
        [MarshalAs(System.Runtime.InteropServices.UnmanagedType.U4)]
        public UInt32 dwServiceType;
        [MarshalAs(System.Runtime.InteropServices.UnmanagedType.U4)]
        public UInt32 dwStartType;
        [MarshalAs(System.Runtime.InteropServices.UnmanagedType.U4)]
        public UInt32 dwErrorControl;
        [MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)]
        public String lpBinaryPathName;
        [MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)]
        public String lpLoadOrderGroup;
        [MarshalAs(System.Runtime.InteropServices.UnmanagedType.U4)]
        public UInt32 dwTagID;
        [MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)]
        public String lpDependencies;
        [MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)]
        public String lpServiceStartName;
        [MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr)]
        public String lpDisplayName;
    };



    class ServicesAPI
    {
        [DllImport("advapi32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        public static extern Boolean QueryServiceConfig(IntPtr hService, IntPtr intPtrQueryConfig, UInt32 cbBufSize, out UInt32 pcbBytesNeeded);

        /*The CloseServiceHandle function closes a handle to a service control manager or service object.*/
        [DllImport("advapi32.dll")]
        public static extern bool CloseServiceHandle(IntPtr hSCObject);

        /*The OpenSCManager function establishes a connection to the service control manager on the specified 
         * computer and opens the specified service control manager database.*/
        [DllImport("advapi32.dll")]
        public static extern IntPtr OpenSCManager(string lpMachineName, string lpDatabaseName, SC_DESIRED_ACCESS dwDesiredAccess);


        //[StructLayout(LayoutKind.Sequential)]
        //public struct SERVICE_STATUS
        //{
        //    public int serviceType;
        //    public int currentState;
        //    public int controlsAccepted;
        //    public int win32ExitCode;
        //    public int serviceSpecificExitCode;
        //    public int checkPoint;
        //    public int waitHint;
        //}

        //public enum State
        //{
        //    SERVICE_STOPPED = 0x00000001,
        //    SERVICE_START_PENDING = 0x00000002,
        //    SERVICE_STOP_PENDING = 0x00000003,
        //    SERVICE_RUNNING = 0x00000004,
        //    SERVICE_CONTINUE_PENDING = 0x00000005,
        //    SERVICE_PAUSE_PENDING = 0x00000006,
        //    SERVICE_PAUSED = 0x00000007,
        //}
        //[DllImport("ADVAPI32.DLL", EntryPoint = "SetServiceStatus")]
        //public static extern bool SetServiceStatus(IntPtr hServiceStatus, SERVICE_STATUS lpServiceStatus);



        //
        // Service Control Manager object specific access types
        //
        public enum SC_DESIRED_ACCESS : int
        {
            SC_MANAGER_CONNECT = 0x0001,
            SC_MANAGER_CREATE_SERVICE = 0x0002,
            SC_MANAGER_ENUMERATE_SERVICE = 0x0004,
            SC_MANAGER_LOCK = 0x0008,
            SC_MANAGER_QUERY_LOCK_STATUS = 0x0010,
            SC_MANAGER_MODIFY_BOOT_CONFIG = 0x0020,
            STANDARD_RIGHTS_REQUIRED = 0x000F0000,
            SC_MANAGER_ALL_ACCESS = 0x000F0000 | 0x0001 | 0x0002 | 0x0004 | 0x0008 | 0x0010 | 0x0020
        }

        /*The OpenService function opens an existing service.*/
        [DllImport("advapi32.dll")]
        public static extern IntPtr OpenService(IntPtr hSCManager, string lpServiceName, SERVICE_DESIRED_ACCESS dwDesiredAccess);

        //
        // Service object specific access type
        //
        public enum SERVICE_DESIRED_ACCESS : int
        {
            SERVICE_QUERY_CONFIG = 0x0001,
            SERVICE_CHANGE_CONFIG = 0x0002,
            SERVICE_QUERY_STATUS = 0x0004,
            SERVICE_ENUMERATE_DEPENDENTS = 0x0008,
            SERVICE_START = 0x0010,
            SERVICE_STOP = 0x0020,
            SERVICE_PAUSE_CONTINUE = 0x0040,
            SERVICE_INTERROGATE = 0x0080,
            SERVICE_USER_DEFINED_CONTROL = 0x0100,
            STANDARD_RIGHTS_REQUIRED = 0x000F0000,
            SERVICE_ALL_ACCESS = 0x000F0000 | 0x0001 | 0x0002 | 0x0004 | 0x0008 | 0x0010 | 0x0020 | 0x0040 | 0x0080 | 0x0100
        }


        /*The CreateService function creates a service object and adds it to the specified service control manager database.*/
        [DllImport("advapi32.dll")]
        public static extern IntPtr CreateService(IntPtr SC_HANDLE, string lpSvcName, string lpDisplayName,
            SERVICE_DESIRED_ACCESS dwDesiredAccess, SERVICE_TYPE dwServiceType, START_TYPE dwStartType, ERROR_CONTROL_TYPE dwErrorControl, string lpPathName,
            string lpLoadOrderGroup, int lpdwTagId, string lpDependencies, string lpServiceStartName, string lpPassword);


        //
        // Service Types (Bit Mask)
        //
        public enum SERVICE_TYPE : int
        {
            SERVICE_KERNEL_DRIVER = 0x00000001,
            SERVICE_FILE_SYSTEM_DRIVER = 0x00000002,
            SERVICE_ADAPTER = 0x00000004,
            SERVICE_RECOGNIZER_DRIVER = 0x00000008,
            SERVICE_DRIVER = 0x00000001 | 0x00000002 | 0x00000008,
            SERVICE_WIN32_OWN_PROCESS = 0x00000010,
            SERVICE_WIN32_SHARE_PROCESS = 0x00000020,
            SERVICE_WIN32 = 0x00000010 | 0x00000020,
            SERVICE_INTERACTIVE_PROCESS = 0x00000100,
            SERVICE_TYPE_ALL = 0x00000010 | 0x00000020 | 0x00000004 | 0x00000001 | 0x00000002 | 0x00000008 | 0x00000100
        }

        //
        // Start Type
        //
        public enum START_TYPE : int
        {
            SERVICE_BOOT_START = 0x00000000,
            SERVICE_SYSTEM_START = 0x00000001,
            SERVICE_AUTO_START = 0x00000002,
            SERVICE_DEMAND_START = 0x00000003,
            SERVICE_DISABLED = 0x00000004
        }

        //
        // Error control type
        //
        public enum ERROR_CONTROL_TYPE : int
        {
            SERVICE_ERROR_IGNORE = 0x00000000,
            SERVICE_ERROR_NORMAL = 0x00000001,
            SERVICE_ERROR_SEVERE = 0x00000002,
            SERVICE_ERROR_CRITICAL = 0x00000003
        }



        /*The DeleteService function marks the specified service for deletion from the service control manager database.*/
        [DllImport("advapi32.dll")]
        public static extern bool DeleteService(IntPtr svHandle);
    }


    /// <summary>
    /// 
    /// </summary>
    public static class ValisServiceInstaller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ServiceName"></param>
        /// <returns></returns>
        public static bool IsServiceInstalled(string ServiceName)
        {
            bool retValue = false;
            IntPtr scm_handle = IntPtr.Zero, srv_handle = IntPtr.Zero;

            try
            {
                scm_handle = ServicesAPI.OpenSCManager(null, null, ServicesAPI.SC_DESIRED_ACCESS.SC_MANAGER_CREATE_SERVICE);
                if (scm_handle == IntPtr.Zero)
                    return false;

                srv_handle = ServicesAPI.OpenService(scm_handle, ServiceName, ServicesAPI.SERVICE_DESIRED_ACCESS.SERVICE_ALL_ACCESS);
                retValue = srv_handle != IntPtr.Zero;
            }
            catch (Exception ex)
            {
                Console.WriteLine("(_IsServiceInstalled) Exception: {0}", ex.Message);
                retValue = false;
            }
            finally
            {
                if (srv_handle != IntPtr.Zero)
                    ServicesAPI.CloseServiceHandle(srv_handle);
                if (scm_handle != IntPtr.Zero)
                    ServicesAPI.CloseServiceHandle(scm_handle);
            }
            return retValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ServiceName"></param>
        /// <param name="DisplayName"></param>
        /// <param name="PathName"></param>
        /// <returns></returns>
        public static bool InstallService(string ServiceName, string DisplayName, string PathName)
        {
            bool retValue = false;
            IntPtr scm_handle = IntPtr.Zero;
            IntPtr srv_handle = IntPtr.Zero;

            try
            {
                scm_handle = ServicesAPI.OpenSCManager(null, null, ServicesAPI.SC_DESIRED_ACCESS.SC_MANAGER_CREATE_SERVICE);
                if (scm_handle == IntPtr.Zero)
                    return false;

                srv_handle = ServicesAPI.CreateService(scm_handle, ServiceName, DisplayName, (ServicesAPI.SERVICE_DESIRED_ACCESS)0, ServicesAPI.SERVICE_TYPE.SERVICE_WIN32_OWN_PROCESS, ServicesAPI.START_TYPE.SERVICE_AUTO_START, ServicesAPI.ERROR_CONTROL_TYPE.SERVICE_ERROR_NORMAL, PathName, null, 0, null, null, null);
                retValue = srv_handle != IntPtr.Zero;
            }
            catch (Exception ex)
            {
                Console.WriteLine("(InstallService) Exception: {0}", ex.Message);
                retValue = false;
            }
            finally
            {
                if (srv_handle != IntPtr.Zero)
                    ServicesAPI.CloseServiceHandle(srv_handle);
                if (scm_handle != IntPtr.Zero)
                    ServicesAPI.CloseServiceHandle(scm_handle);
            }
            return retValue;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ServiceName"></param>
        /// <returns></returns>
        public static bool UninstallService(string ServiceName)
        {
            bool retValue = false;
            IntPtr scm_handle = IntPtr.Zero;
            IntPtr srv_handle = IntPtr.Zero;

            try
            {
                scm_handle = ServicesAPI.OpenSCManager(null, null, ServicesAPI.SC_DESIRED_ACCESS.SC_MANAGER_ALL_ACCESS);
                if (scm_handle == IntPtr.Zero)
                    return false;

                srv_handle = ServicesAPI.OpenService(scm_handle, ServiceName, ServicesAPI.SERVICE_DESIRED_ACCESS.SERVICE_ALL_ACCESS);
                if (srv_handle == IntPtr.Zero)
                    return false;

                return ServicesAPI.DeleteService(srv_handle) == true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("(UninstallService) Exception: {0}", ex.Message);
            }
            finally
            {
                if (srv_handle != IntPtr.Zero)
                    ServicesAPI.CloseServiceHandle(srv_handle);
                if (scm_handle != IntPtr.Zero)
                    ServicesAPI.CloseServiceHandle(scm_handle);
            }
            return retValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ServiceName"></param>
        /// <returns></returns>
        public static string GetServiceBinary(string ServiceName)
        {
            string retValue = null;
            IntPtr scm_handle = IntPtr.Zero;
            IntPtr srv_handle = IntPtr.Zero;

            try
            {
                scm_handle = ServicesAPI.OpenSCManager(null, null, ServicesAPI.SC_DESIRED_ACCESS.SC_MANAGER_ALL_ACCESS);
                if (scm_handle != IntPtr.Zero)
                {
                    srv_handle = ServicesAPI.OpenService(scm_handle, ServiceName, ServicesAPI.SERVICE_DESIRED_ACCESS.SERVICE_QUERY_CONFIG);
                    if (srv_handle != IntPtr.Zero)
                    {
                        UInt32 dwBytesNeeded = 0;

                        // Allocate memory for struct.
                        IntPtr ptr = Marshal.AllocHGlobal(4096);
                        try
                        {
                            bool success = ServicesAPI.QueryServiceConfig(srv_handle, ptr, 4096, out dwBytesNeeded);
                            if (success)
                            {
                                QUERY_SERVICE_CONFIG qsc = new QUERY_SERVICE_CONFIG();
                                // Copy 
                                Marshal.PtrToStructure(ptr, qsc);
                                retValue = qsc.lpBinaryPathName;
                            }
                        }
                        finally
                        {
                            // Free memory for struct.
                            Marshal.FreeHGlobal(ptr);
                        }
                    }
                }
            }
            finally
            {
                if (srv_handle != IntPtr.Zero)
                    ServicesAPI.CloseServiceHandle(srv_handle);
                if (scm_handle != IntPtr.Zero)
                    ServicesAPI.CloseServiceHandle(scm_handle);
            }

            return retValue;
        }
    }
}
