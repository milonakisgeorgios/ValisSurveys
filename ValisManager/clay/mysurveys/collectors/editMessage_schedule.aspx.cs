using System;
using System.Globalization;
using System.Threading;
using System.Web.UI.WebControls;
using Valis.Core;

namespace ValisManager.clay.mysurveys.collectors
{
    /// <summary>
    /// ΕΙΝΑΙ ΑΠΑΡΑΙΤΗΤΟ ΝΑ ΚΛΗΘΕΙ ΜΕ MessageID!!!
    /// </summary>
    public partial class editMessage_schedule : CollectorsPage
    {
        #region Grab MessageId from Url
        public Int32 MessageId
        {
            get
            {
                Object _obj = this.ViewState["MessageId"];
                if (_obj == null) return -1;
                return (Int32)_obj;
            }
            set
            {
                this.ViewState["MessageId"] = value;
            }
        }

        protected VLMessage SelectedMessage
        {
            get
            {
                if (this.Context.Items["SelectedMessage"] == null)
                {
                    this.Context.Items["SelectedMessage"] = SurveyManager.GetMessageById(this.MessageId);
                }
                return (VLMessage)this.Context.Items["SelectedMessage"];
            }
        }

        protected override void OnPreLoad(EventArgs e)
        {
            base.OnPreLoad(e);
            try
            {
                if (this.IsPostBack == false)
                {
                    if (string.IsNullOrEmpty(Request.Params["messageId"]))
                        throw new ArgumentNullException("messageId");
                    this.MessageId = Int32.Parse(Request.Params["messageId"]);
                }
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message;
            }
        }
        #endregion


        public string TimeZoneNotice
        {
            get
            {
                var tzi = TimeZoneInfo.FindSystemTimeZoneById(Globals.UserToken.TimeZoneId);
                return string.Format("(Time based on {0})", tzi.DisplayName);
            }
        }

        protected override void OnInitComplete(EventArgs e)
        {
            base.OnInitComplete(e);
            if (this.IsPostBack == false)
            {
                this.ddlHours.Items.Clear();
                this.ddlHours.Items.Add(new ListItem("00", "0"));
                this.ddlHours.Items.Add(new ListItem("01", "1"));
                this.ddlHours.Items.Add(new ListItem("02", "2"));
                this.ddlHours.Items.Add(new ListItem("03", "3"));
                this.ddlHours.Items.Add(new ListItem("04", "4"));
                this.ddlHours.Items.Add(new ListItem("05", "5"));
                this.ddlHours.Items.Add(new ListItem("06", "6"));
                this.ddlHours.Items.Add(new ListItem("07", "7"));
                this.ddlHours.Items.Add(new ListItem("08", "8"));
                this.ddlHours.Items.Add(new ListItem("09", "9"));
                this.ddlHours.Items.Add(new ListItem("10", "10"));
                this.ddlHours.Items.Add(new ListItem("11", "11"));
                this.ddlHours.Items.Add(new ListItem("12", "12"));
                this.ddlHours.Items.Add(new ListItem("13", "13"));
                this.ddlHours.Items.Add(new ListItem("14", "14"));
                this.ddlHours.Items.Add(new ListItem("15", "15"));
                this.ddlHours.Items.Add(new ListItem("16", "16"));
                this.ddlHours.Items.Add(new ListItem("17", "17"));
                this.ddlHours.Items.Add(new ListItem("18", "18"));
                this.ddlHours.Items.Add(new ListItem("19", "19"));
                this.ddlHours.Items.Add(new ListItem("20", "20"));
                this.ddlHours.Items.Add(new ListItem("21", "21"));
                this.ddlHours.Items.Add(new ListItem("22", "22"));
                this.ddlHours.Items.Add(new ListItem("23", "23"));


                this.ddlMinutes.Items.Clear();
                this.ddlMinutes.Items.Add(new ListItem("00", "0"));
                this.ddlMinutes.Items.Add(new ListItem("05", "5"));
                this.ddlMinutes.Items.Add(new ListItem("10", "10"));
                this.ddlMinutes.Items.Add(new ListItem("15", "15"));
                this.ddlMinutes.Items.Add(new ListItem("20", "20"));
                this.ddlMinutes.Items.Add(new ListItem("25", "25"));
                this.ddlMinutes.Items.Add(new ListItem("30", "30"));
                this.ddlMinutes.Items.Add(new ListItem("35", "35"));
                this.ddlMinutes.Items.Add(new ListItem("40", "40"));
                this.ddlMinutes.Items.Add(new ListItem("45", "45"));
                this.ddlMinutes.Items.Add(new ListItem("50", "50"));
                this.ddlMinutes.Items.Add(new ListItem("55", "55"));
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (this.IsPostBack == false)
            {
                this.rdbtnSchedule.Checked = true;
            }
        }

        protected void btnSaveSchedule_Click(object sender, EventArgs e)
        {
            try
            {
                var scd = DateTime.ParseExact(this.txtDate.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                var scheduleDateTime = new DateTime(scd.Year, scd.Month, scd.Day, Int32.Parse(ddlHours.SelectedValue), Int32.Parse(ddlMinutes.SelectedValue), 0);

                var updatedMessage = SurveyManager.ScheduleMessage(this.SelectedMessage.MessageId, scheduleDateTime);

                Response.Redirect(string.Format("messages.aspx?surveyid={0}&collectorId={1}&textslanguage={2}", this.Surveyid, this.CollectorId, this.TextsLanguage), false);
                this.Context.ApplicationInstance.CompleteRequest();
            }
            catch (ThreadAbortException)
            {
                //
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message;
            }
        }

        protected void btnSendImmediately_Click(object sender, EventArgs e)
        {
            try
            {
                var updatedMessage = SurveyManager.ScheduleMessageImmediately(this.SelectedMessage.MessageId);

                Response.Redirect(string.Format("messages.aspx?surveyid={0}&collectorId={1}&textslanguage={2}", this.Surveyid, this.CollectorId, this.TextsLanguage), false);
                this.Context.ApplicationInstance.CompleteRequest();
            }
            catch (ThreadAbortException)
            {
                //
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message;
            }
        }
    }
}