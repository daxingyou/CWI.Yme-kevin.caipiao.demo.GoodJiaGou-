using System;
using System.Windows.Forms;
using Yme.Mcp.BLL.SystemManage;
using Yme.Mcp.WinServ.Busy;
using Yme.Mcp.WinServ.Common;
using Yme.Util;
using Yme.Util.Log;

namespace Yme.Mcp.WinServ
{
    public partial class StartForm : Form
    {
        public StartForm()
        {
            InitializeComponent();
        }

        private void TestForm_Load(object sender, EventArgs e)
        {
            DateTime dbNow = SingleInstance<SystemBLL>.Instance.GetDbTime();
            SysDateTime.InitDateTime(dbNow);

            BusyMain.Instance.Start();
          
            label1.Text = "服务已启动";
        }

        private void HandleError(Exception ex)
        {
            LogUtil.Error(ex.Message, ex);
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            BusyMain.Instance.Start();
            label1.Text = "服务已启动";
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            BusyMain.Instance.Stop();
            label1.Text = "服务已停止";
        }
    }
}
