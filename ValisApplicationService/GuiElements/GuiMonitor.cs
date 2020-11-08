using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace ValisApplicationService.GuiElements
{
    class GuiMonitor : IMonitor
    {
        private ListView traceListView;
        bool ShowErrors { get; set; }
        bool ShowWarnings { get; set; }
        bool ShowInfos { get; set; }
        bool ShowDebug { get; set; }


        public static GuiMonitor Instance = new GuiMonitor();
        private GuiMonitor()
        {
            this.traceListView = MainWindow.Instance.traceListView;
            this.traceListView.DoubleClick += new EventHandler(OnViewDoubleClick);

            MainWindow.Instance.chkError.Checked = ShowErrors = true;
            MainWindow.Instance.chkWarning.Checked = ShowWarnings = true;
            MainWindow.Instance.chkInfo.Checked = ShowInfos = true;
            MainWindow.Instance.chkDebug.Checked = ShowDebug = true;

            MainWindow.Instance.chkDebug.CheckedChanged += delegate { ShowDebug = MainWindow.Instance.chkDebug.Checked; };
            MainWindow.Instance.chkInfo.CheckedChanged += delegate { ShowInfos = MainWindow.Instance.chkInfo.Checked; };
            MainWindow.Instance.chkWarning.CheckedChanged += delegate { ShowWarnings = MainWindow.Instance.chkWarning.Checked; };
            MainWindow.Instance.chkError.CheckedChanged += delegate { ShowErrors = MainWindow.Instance.chkError.Checked; };
        }

        void OnViewDoubleClick(object sender, EventArgs e)
        {
            ListViewItem lvitem = traceListView.SelectedItems[0];
            if (lvitem == null)
                return;


            if (lvitem.Tag == null)
            {
                //Απλό text message
                TraceLevel level = (TraceLevel)Enum.Parse(typeof(TraceLevel), lvitem.SubItems[2].Text);
                switch (level)
                {
                    case TraceLevel.Error:
                        MessageBox.Show(lvitem.SubItems[2].Text, "ValisApplicationService - Error Message:", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    case TraceLevel.Warning:
                        MessageBox.Show(lvitem.SubItems[2].Text, "ValisApplicationService - Warning Message:", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        break;
                    case TraceLevel.Info:
                        MessageBox.Show(lvitem.SubItems[2].Text, "ValisApplicationService - Info Message:", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    case TraceLevel.Verbose:
                        MessageBox.Show(lvitem.SubItems[2].Text, "ValisApplicationService - Verbose Message:", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                }
            }
            else
            {
                //Εχουμε Exception
                ShowExceptionForm form = new ShowExceptionForm();
                form.LoggedException = (Exception)lvitem.Tag;
                form.ShowDialog();
            }
        }




        public void ShowMessage(DateTime date, int thread, System.Diagnostics.TraceLevel level, string logger, string message, Exception exception)
        {
            if (level == TraceLevel.Error && !ShowErrors)
                return;
            if (level == TraceLevel.Warning && !ShowWarnings)
                return;
            if (level == TraceLevel.Info && !ShowInfos)
                return;
            if (level == TraceLevel.Verbose && !ShowDebug)
                return;

            traceListView.Invoke(new Action<DateTime, int, System.Diagnostics.TraceLevel, string, string, Exception>(ShowMessageImpl), new object[] { date, thread, level, logger, message, exception });
        }

        void ShowMessageImpl(DateTime date, int thread, System.Diagnostics.TraceLevel level, string logger, string message, Exception exception)
        {
            int rowIndex = GetRowIndex();

            ListViewItem lvitem = new ListViewItem(date.ToString());
            lvitem.Tag = exception;
            lvitem.SubItems.Add(thread.ToString());
            lvitem.SubItems.Add(level.ToString());
            lvitem.SubItems.Add(logger.ToString());
            lvitem.SubItems.Add(message.ToString());
            if (exception != null)
            {
                lvitem.SubItems.Add(exception.Message);
            }

            traceListView.Items.Add(lvitem);
            traceListView.EnsureVisible(rowIndex);

            ColorizeListViewItem(level, lvitem);
        }


        int GetRowIndex()
        {
            int rowIndex = traceListView.Items.Count;
            if (rowIndex > 800)
            {
                traceListView.Items.Clear();
                rowIndex = 0;
            }
            return rowIndex;
        }
        void ColorizeListViewItem(TraceLevel level, ListViewItem lvitem)
        {
            switch (level)
            {
                case TraceLevel.Error:
                    lvitem.ForeColor = Color.Red;
                    lvitem.Font = new Font(lvitem.Font, FontStyle.Bold);
                    break;
                case TraceLevel.Warning:
                    lvitem.ForeColor = Color.Coral;
                    lvitem.Font = new Font(lvitem.Font, FontStyle.Bold);
                    break;
                case TraceLevel.Info:
                    lvitem.ForeColor = Color.Black;
                    break;
                case TraceLevel.Verbose:
                    lvitem.ForeColor = Color.DimGray;
                    break;
            }
        }


    }
}
