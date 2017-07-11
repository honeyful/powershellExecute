using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace PowershellExecute
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
        }

        private void txtPosh_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                try
                {
                    rtbOutput.Clear();
                    rtbOutput.AppendText(runScript(txtPosh.Text));
                }
                catch (Exception ex)
                {
                    rtbOutput.AppendText(ex.Data + Environment.NewLine);
                }
                finally
                {
                    txtPosh.Clear();
                }
            }
        }

        private string runScript(string s)
        {
            Runspace run = RunspaceFactory.CreateRunspace();

            run.Open();

            Pipeline pipe = run.CreatePipeline();
            pipe.Commands.AddScript(s);

            pipe.Commands.Add("Out-String");

            Collection<PSObject> results = pipe.Invoke();

            run.Close();

            StringBuilder stringBuilder = new StringBuilder();
            foreach (PSObject obj in results)
            {
                stringBuilder.AppendLine(obj.ToString());
            }

            return stringBuilder.ToString();
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            rtbOutput.ReadOnly = true;
            txtPosh.SelectAll();
        }
    }
}
