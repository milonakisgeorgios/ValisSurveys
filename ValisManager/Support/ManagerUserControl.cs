using Valis.Core;

namespace ValisManager
{
    public class ManagerUserControl : System.Web.UI.UserControl
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


        protected string GetRequiredIcon()
        {
            return "<img class=\"required-image\" title=\"This field is required\" alt=\"required field\" src=\"/content/images/requiredIcon1.gif\">";
        }
    }
}