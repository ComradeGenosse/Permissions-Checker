using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Security.AccessControl;
using Newtonsoft.Json.Linq;
using System.Net;

namespace PermissionsChecker
{
    public partial class MainForm : Form
    {
        private const string ApiKey = "LN9ZcLH6R3wxVY9D";

        private Button btnAnalyze;
        private Button btnBrowse;
        private RichTextBox txtStatusReport; // Updated
        private RichTextBox txtUserTesting; // Updated

        public MainForm()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            this.btnAnalyze = new System.Windows.Forms.Button();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtStatusReport = new System.Windows.Forms.RichTextBox();
            this.txtUserTesting = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // btnAnalyze
            // 
            this.btnAnalyze.Location = new System.Drawing.Point(750, 234);
            this.btnAnalyze.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnAnalyze.Name = "btnAnalyze";
            this.btnAnalyze.Size = new System.Drawing.Size(112, 35);
            this.btnAnalyze.TabIndex = 0;
            this.btnAnalyze.Text = "Analyze";
            this.btnAnalyze.UseVisualStyleBackColor = true;
            this.btnAnalyze.Click += new System.EventHandler(this.btnAnalyze_Click);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(39, 14);
            this.btnBrowse.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(112, 35);
            this.btnBrowse.TabIndex = 1;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtStatusReport
            // 
            this.txtStatusReport.Location = new System.Drawing.Point(357, 309);
            this.txtStatusReport.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtStatusReport.Name = "txtStatusReport";
            this.txtStatusReport.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.txtStatusReport.Size = new System.Drawing.Size(818, 369);
            this.txtStatusReport.TabIndex = 2;
            this.txtStatusReport.Text = "";
            // 
            // txtUserTesting
            // 
            this.txtUserTesting.Location = new System.Drawing.Point(24, 59);
            this.txtUserTesting.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtUserTesting.Name = "txtUserTesting";
            this.txtUserTesting.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.txtUserTesting.Size = new System.Drawing.Size(575, 51);
            this.txtUserTesting.TabIndex = 3;
            this.txtUserTesting.Text = "";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 692);
            this.Controls.Add(this.txtUserTesting);
            this.Controls.Add(this.txtStatusReport);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.btnAnalyze);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "MainForm";
            this.Text = "Permissions Checker";
            this.ResumeLayout(false);

        }

        private string[] folders = new string[]
        {
            @"C:\Users\Public\ProjectMatrix",
            @"%LOCALAPPDATA%\ProjectSpec",
            @"%LOCALAPPDATA%\ProjectSymbolsforautocad",
            @"C:\Program Files\ProjectMatrix\ProjectMatrix Data Server",
            @"C:\Program Files (x86)\ProjectMatrix",
            @"%LOCALAPPDATA%\ProjectMatrix",
            @"%APPDATA%\ProjectMatrix",
            @"%LOCALAPPDATA%\CET Data",
            @"%LOCALAPPDATA%\ProjectNotify",
            @"C:\ProgramData\ProjectMatrix"
        };
        private string[] folderNames = new string[]
        {
        "Data Directory",
        "Spec Folder",
        "CIL for ACAD folder",
        "Data Warehouse folder",
        "CIL Apps folder",
        "CIL Config folder",
        "CIL User Files folder",
        "CET Folder",
        "Notify Folder",
        "CIL Legacy data folder"
        };

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Initialize UI elements
        }

        private void btnAnalyze_Click(object sender, EventArgs e)
        {
            AnalyzePermissions();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog fbd = new FolderBrowserDialog())
            {
                DialogResult result = fbd.ShowDialog();

                if (result == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    AnalyzeCustomFolder(fbd.SelectedPath);
                }
            }
        }

        private void AnalyzePermissions()
        {
            txtStatusReport.Clear();

            for (int i = 0; i < folders.Length; i++)
            {
                string folderPath = Environment.ExpandEnvironmentVariables(folders[i]);
                string folderName = folderNames[i];

                if (Directory.Exists(folderPath))
                {
                    AppendColoredText(txtStatusReport, $"{folderName} - ", Color.Black);
                    AppendColoredText(txtStatusReport, $"{CheckPermissions(folderPath)}\r\n", CheckPermissions(folderPath).Contains("PASS") ? Color.Green : Color.Red);
                }
            }

            bool awsServerStatus = PingMySQL();
            AppendColoredText(txtStatusReport, $"AWS Server: ", Color.Black);
            AppendColoredText(txtStatusReport, $"{(awsServerStatus ? "PASS" : "FAIL")}", awsServerStatus ? Color.Green : Color.Red);
        }

        private bool PingMySQL(bool isRemote = false)
        {
            try
            {
                string json = new WebClient().DownloadString("https://api.projectmatrix.com/datawarehouse/pingDatabase.php?key=" + ApiKey);
                return JObject.Parse(json)["ping_successful"].ToObject<bool>();
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        private void AnalyzeCustomFolder(string folderPath)
        {
            txtUserTesting.Clear();
            AppendColoredText(txtUserTesting, $"{Path.GetFileName(folderPath)} - ", Color.Black);
            AppendColoredText(txtUserTesting, $"{CheckPermissions(folderPath)}\r\n", CheckPermissions(folderPath).Contains("PASS") ? Color.Green : Color.Red);
        }

        private string CheckPermissions(string folderPath)
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(folderPath);
                DirectorySecurity ds = di.GetAccessControl();

                AuthorizationRuleCollection rules = ds.GetAccessRules(true, true, typeof(System.Security.Principal.NTAccount));

                bool canRead = false;
                bool canWrite = false;
                bool canExecute = false;

                foreach (FileSystemAccessRule rule in rules)
                {
                    if ((rule.FileSystemRights & FileSystemRights.Read) == FileSystemRights.Read)
                    {
                        canRead = true;
                    }
                    if ((rule.FileSystemRights & FileSystemRights.Write) == FileSystemRights.Write)
                    {
                        canWrite = true;
                    }
                    if ((rule.FileSystemRights & FileSystemRights.ExecuteFile) == FileSystemRights.ExecuteFile)
                    {
                        canExecute = true;
                    }
                }

                string result = $"Read: {(canRead ? "PASS" : "FAIL")}, Write: {(canWrite ? "PASS" : "FAIL")}, Execute: {(canExecute ? "PASS" : "FAIL")}";

                return result;
            }
            catch (Exception ex)
            {
                return "FAIL";
            }
        }

        private void AppendColoredText(RichTextBox rtb, string text, Color color)
        {
            rtb.SelectionStart = rtb.TextLength;
            rtb.SelectionLength = 0;

            rtb.SelectionColor = color;
            rtb.AppendText(text);
            rtb.SelectionColor = rtb.ForeColor;
        }
    }
}


