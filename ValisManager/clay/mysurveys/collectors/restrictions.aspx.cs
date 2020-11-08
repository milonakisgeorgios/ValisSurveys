using System;
using System.Globalization;
using System.Web.UI.WebControls;

namespace ValisManager.clay.mysurveys.collectors
{
    public partial class restrictions : CollectorsPage
    {


        public string TimeZoneNotice
        {
            get
            {
                var tzi = TimeZoneInfo.FindSystemTimeZoneById(Globals.UserToken.TimeZoneId);
                return string.Format("(Time based on {0})", tzi.DisplayName);
            }
        }



        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            if(this.IsPostBack == false)
            {
                this.ddlHour.Items.Clear();
                this.ddlHour.Items.Add(new ListItem("01", "1"));
                this.ddlHour.Items.Add(new ListItem("02", "2"));
                this.ddlHour.Items.Add(new ListItem("03", "3"));
                this.ddlHour.Items.Add(new ListItem("04", "4"));
                this.ddlHour.Items.Add(new ListItem("05", "5"));
                this.ddlHour.Items.Add(new ListItem("06", "6"));
                this.ddlHour.Items.Add(new ListItem("07", "7"));
                this.ddlHour.Items.Add(new ListItem("08", "8"));
                this.ddlHour.Items.Add(new ListItem("09", "9"));
                this.ddlHour.Items.Add(new ListItem("10", "10"));
                this.ddlHour.Items.Add(new ListItem("11", "11"));
                this.ddlHour.Items.Add(new ListItem("12", "12"));
                this.ddlHour.Items.Add(new ListItem("13", "13"));
                this.ddlHour.Items.Add(new ListItem("14", "14"));
                this.ddlHour.Items.Add(new ListItem("15", "15"));
                this.ddlHour.Items.Add(new ListItem("16", "16"));
                this.ddlHour.Items.Add(new ListItem("17", "17"));
                this.ddlHour.Items.Add(new ListItem("18", "18"));
                this.ddlHour.Items.Add(new ListItem("19", "19"));
                this.ddlHour.Items.Add(new ListItem("20", "20"));
                this.ddlHour.Items.Add(new ListItem("21", "21"));
                this.ddlHour.Items.Add(new ListItem("22", "22"));
                this.ddlHour.Items.Add(new ListItem("23", "23"));

                this.ddlMinute.Items.Clear();
                this.ddlMinute.Items.Add(new ListItem("00", "0"));
                this.ddlMinute.Items.Add(new ListItem("15", "15"));
                this.ddlMinute.Items.Add(new ListItem("30", "30"));
                this.ddlMinute.Items.Add(new ListItem("45", "45"));

            }
        }



        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (this.IsPostBack == false)
            {
                //StopCollectorDT
                if (this.SelectedCollector.EnableStopCollectorDT && this.SelectedCollector.StopCollectorDT.HasValue)
                {
                    this.chkCutoffDate.Checked = true;

                    this.txtCutdate.Text = this.SelectedCollector.StopCollectorDT.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                    this.ddlHour.SelectedValue = this.SelectedCollector.StopCollectorDT.Value.Hour.ToString(CultureInfo.InvariantCulture);
                    this.ddlMinute.SelectedValue = this.SelectedCollector.StopCollectorDT.Value.Minute.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    this.chkCutoffDate.Checked = false;
                }

                //panMaxResponse
                if (this.SelectedCollector.EnableMaxResponseCount && this.SelectedCollector.MaxResponses.HasValue)
                {
                    this.chkMaxResponse.Checked = true;
                    this.txtMaxResponse.Text = this.SelectedCollector.MaxResponses.Value.ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    this.chkMaxResponse.Checked = false;
                }
            }
        }



        protected void saveRestrictions_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.chkCutoffDate.Checked)
                {
                    if (string.IsNullOrEmpty(this.txtCutdate.Text))
                    {
                        throw new ArgumentException("'Cutoff date & time', must have a value!");
                    }
                    var stopDate = DateTime.ParseExact(this.txtCutdate.Text, "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    var scheduleDateTime = new DateTime(stopDate.Year, stopDate.Month, stopDate.Day, Int32.Parse(ddlHour.SelectedValue), Int32.Parse(ddlMinute.SelectedValue), 0);


                    this.SelectedCollector.EnableStopCollectorDT = true;
                    this.SelectedCollector.StopCollectorDT = scheduleDateTime;
                }
                else
                {
                    this.SelectedCollector.EnableStopCollectorDT = false;
                    this.SelectedCollector.StopCollectorDT = null;
                }

                if (this.chkMaxResponse.Checked)
                {
                    if (string.IsNullOrEmpty(this.txtCutdate.Text))
                    {
                        throw new ArgumentException("'Max Response Count', must have a value!");
                    }
                    this.SelectedCollector.EnableMaxResponseCount = true;
                    this.SelectedCollector.MaxResponses = Int32.Parse(this.txtMaxResponse.Text);
                }
                else
                {
                    this.SelectedCollector.EnableMaxResponseCount = false;
                    this.SelectedCollector.MaxResponses = null;
                }


                this.SelectedCollector = SurveyManager.UpdateCollector(this.SelectedCollector);
                this.InfoMessage = "Restrictions saved succesfully!";
            }
            catch (Exception ex)
            {
                this.ErrorMessage = ex.Message;
            }
        }
    }
}