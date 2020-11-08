using System;
using System.Web.UI;
using Valis.Core;

namespace ValisManager.clay.mysurveys.analysis
{
    public partial class report : AnalysisBasePage
    {
        VLView m_selectedView;


        public Guid ViewId
        {
            get
            {
                Object _obj = this.ViewState["ViewId"];
                if (_obj == null) return default(Guid);
                return (Guid)_obj;
            }
            set
            {
                this.ViewState["ViewId"] = value;
            }
        }

        public VLView SelectedView
        {
            get
            {
                if (m_selectedView == null)
                {
                    m_selectedView = SurveyManager.GetViewById(this.ViewId);
                }
                return m_selectedView;
            }
        }



        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (this.IsPostBack == false)
            {
                if (Request.Params["viewId"] != null)
                    this.ViewId = Guid.Parse(Request.Params["viewId"]);
                else
                {
                    this.m_selectedView = SurveyManager.GetDefaultView(this.Surveyid);
                    this.ViewId = this.SelectedView.ViewId;
                }
            }

            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "JsSummaryArray", GetJsSummaryArray(this.ViewId), true);
        }
    }
}