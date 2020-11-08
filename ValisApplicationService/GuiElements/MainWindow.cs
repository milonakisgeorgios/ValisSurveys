using System;
using System.Windows.Forms;

namespace ValisApplicationService.GuiElements
{
    public partial class MainWindow : Form
    {
        public static MainWindow Instance;


        public MainWindow()
        {
            if (MainWindow.Instance != null)
            {
                throw new Exception("MainWindow already exists!");
            }

            InitializeComponent();

            MainWindow.Instance = this;
        }

        private void MainWindow_Load(object sender, EventArgs e)
        {
            Globals.IsGuiPresent = true;
            this.Text = "Valis Application Service Controler";
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (TheController.Instance.IsAlive)
                {
                    TheController.Instance.Stop();
                    TheController.Instance.Quit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Globals.ServiceName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (ex.InnerException != null)
                {
                    MessageBox.Show(ex.InnerException.Message, Globals.ServiceName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void OnBtnStart(object sender, EventArgs e)
        {
            try
            {
                this.btnStart.Enabled = false;
                this.btnStop.Enabled = false;
                this.manualTimerEventToolStripMenuItem.Enabled = false;

                if (!TheController.Instance.Start())
                {
                    throw new Exception("Service could not started!");
                }

                this.btnStop.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Globals.ServiceName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (ex.InnerException != null)
                {
                    MessageBox.Show(ex.InnerException.Message, Globals.ServiceName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                this.btnStart.Enabled = true;
                this.btnStop.Enabled = false;
            }
        }

        private void OnBtnStop(object sender, EventArgs e)
        {
            try
            {
                TheController.Instance.Stop();

                this.btnStart.Enabled = true;
                this.btnStop.Enabled = false;
                this.manualTimerEventToolStripMenuItem.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Globals.ServiceName, MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (ex.InnerException != null)
                {
                    MessageBox.Show(ex.InnerException.Message, Globals.ServiceName, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                this.btnStart.Enabled = false;
                this.btnStop.Enabled = true;
            }
        }

        private void OnManualTimerEventCheckedChanged(object sender, EventArgs e)
        {
            if (manualTimerEventToolStripMenuItem.Checked)
            {
                this.btnEmulateTimerPulse.Enabled = true;
                TheController.Instance.UseRealHeartBeat = false;
            }
            else
            {
                this.btnEmulateTimerPulse.Enabled = false;
                TheController.Instance.UseRealHeartBeat = true;
            }
        }

        private void btnEmulateTimerPulse_Click(object sender, EventArgs e)
        {
            if(TheController.Instance.IsAlive && TheController.Instance.UseRealHeartBeat == false)
            {
                TheController.Instance.EmulateHeartBeat();
            }
        }

        private void OnbtnClear(object sender, EventArgs e)
        {
            traceListView.Items.Clear();
        }

    }
}
