using Valis.Core;

namespace ValisManager
{
    public class ManagerMasterPage : System.Web.UI.MasterPage
    {
        /// <summary>
        /// 
        /// </summary>
        protected VLSystemManager SystemManager
        {
            get
            {
                if (this.Context.Items["VLSystemManager"] == null)
                {
                    this.Context.Items["VLSystemManager"] = VLSystemManager.GetAnInstance(Globals.UserToken);
                }
                return (VLSystemManager)this.Context.Items["VLSystemManager"];
            }
        }



        /// <summary>
        /// 
        /// </summary>
        protected VLSurveyManager SurveyManager
        {
            get
            {
                if (this.Context.Items["VLSurveyManager"] == null)
                {
                    this.Context.Items["VLSurveyManager"] = VLSurveyManager.GetAnInstance(Globals.UserToken);
                }
                return (VLSurveyManager)this.Context.Items["VLSurveyManager"];
            }
        }









    }
}