using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Security.AccessControl;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Sockets;
using System.Net.Http;

namespace PermissionsChecker
{
    public partial class MainForm : Form
    {
        private const string ApiKey = "LN9ZcLH6R3wxVY9D";

        private Button btnAnalyze;
        private Button btnBrowse;
        private RichTextBox txtStatusReport;
        private RichTextBox txtUserTesting;

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

        private string[] webUrls = new string[]
        {
            "http://www.projectmatrix.com",
            "https://www.projectmatrix.com",
            "https://login.projectmatrix.com",
            "https://data.projectmatrix.com/",
            "https://api.projectmatrix.com/datawarehouse/getEnterprises.php",
            "https://api.projectmatrix.com/datawarehouse/getCatalogs.php"
        };

        private string[] webUrlNames = new string[]
        {
            "ProjectMatrix Website (HTTP)",
            "ProjectMatrix Website (HTTPS)",
            "ProjectMatrix Login",
            "Data Download Location",
            "API GetEnterprises",
            "API GetCatalogs"
        };

        private int[] ports = new int[]
        {
            60294,
            8730,
            8731
        };

        private string[] portNames = new string[]
        {
            "License Server/Login Check (60294)",
            "ProjectNotify Server (8730)",
            "ProjectNotify Client (8731)"
        };

        public MainForm()
        {
            InitializeComponent();
            this.BackgroundImage = Image.FromFile(@"C:\CETDEV\PMX Tool Image.png");
            this.BackgroundImageLayout = ImageLayout.Stretch;
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
            this.btnAnalyze.Location = new System.Drawing.Point(12, 12);
            this.btnAnalyze.Name = "btnAnalyze";
            this.btnAnalyze.Size = new System.Drawing.Size(75, 23);
            this.btnAnalyze.TabIndex = 0;
            this.btnAnalyze.Text = "Analyze";
            this.btnAnalyze.UseVisualStyleBackColor = true;
            this.btnAnalyze.Click += new System.EventHandler(this.btnAnalyze_Click);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(12, 41);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(75, 23);
            this.btnBrowse.TabIndex = 1;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtStatusReport
            // 
            this.txtStatusReport.Location = new System.Drawing.Point(93, 12);
            this.txtStatusReport.Multiline = true;
            this.txtStatusReport.Name = "txtStatusReport";
            this.txtStatusReport.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.txtStatusReport.Size = new System.Drawing.Size(400, 150);
            this.txtStatusReport.TabIndex = 2;
            // 
            // txtUserTesting
            // 
            this.txtUserTesting.Location = new System.Drawing.Point(12, 70);
            this.txtUserTesting.Multiline = true;
            this.txtUserTesting.Name = "txtUserTesting";
            this.txtUserTesting.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            this.txtUserTesting.Size = new System.Drawing.Size(400, 150);
            this.txtUserTesting.TabIndex = 3;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.txtUserTesting);
            this.Controls.Add(this.txtStatusReport);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.btnAnalyze);
            this.Name = "MainForm";
            this.Text = "Permissions Checker";
            this.ResumeLayout(false);
            this.PerformLayout();

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

        private async void AnalyzePermissions()
        {
            txtStatusReport.Clear();

            for (int i = 0; i < folders.Length; i++)
            {
                string folderPath = Environment.ExpandEnvironmentVariables(folders[i]);
                string folderName = folderNames[i];

                if (Directory.Exists(folderPath))
                {
                    AppendTextWithColor(txtStatusReport, $"{folderName} - ", Color.Black);
                    CheckPermissions(folderPath);
                }
            }

            for (int i = 0; i < webUrls.Length; i++)
            {
                string url = webUrls[i];
                string urlName = webUrlNames[i];

                AppendTextWithColor(txtStatusReport, $"{urlName} - ", Color.Black);
                await CheckWebUrl(url);
            }

            for (int i = 0; i < ports.Length; i++)
            {
                int port = ports[i];
                string portName = portNames[i];

                AppendTextWithColor(txtStatusReport, $"{portName} - ", Color.Black);
                CheckPort(port);
            }

            private async void AnalyzeCustomFolder(string folderPath)
            {
                txtUserTesting.Clear();
                AppendTextWithColor(txtUserTesting, $"{Path.GetFileName(folderPath)} - ", Color.Black);
                CheckPermissions(folderPath);
            }

            private void CheckPermissions(string folderPath)
            {
                try
                {
                    DirectoryInfo di = new DirectoryInfo(folderPath);
                    DirectorySecurity dSecurity = di.GetAccessControl();

                    AuthorizationRuleCollection acl = dSecurity.GetAccessRules(true, true, typeof(System.Security.Principal.NTAccount));
                    bool isWritable = false;

                    foreach (FileSystemAccessRule rule in acl)
                    {
                        if ((FileSystemRights.Write & rule.FileSystemRights) == FileSystemRights.Write)
                        {
                            isWritable = true;
                            break;
                        }
                    }

                    if (isWritable)
                    {
                        AppendTextWithColor(txtStatusReport, "PASS\n", Color.Green);
                    }
                    else
                    {
                        AppendTextWithColor(txtStatusReport, "FAIL\n", Color.Red);
                    }
                }
                catch (Exception ex)
                {
                    AppendTextWithColor(txtStatusReport, "FAIL\n", Color.Red);
                }
            }

            private async System.Threading.Tasks.Task CheckWebUrl(string url)
            {
                try
                {
                    using (HttpClient client = new HttpClient())
                    {
                        HttpResponseMessage response = await client.GetAsync(url);

                        if (response.IsSuccessStatusCode)
                        {
                            AppendTextWithColor(txtStatusReport, "PASS\n", Color.Green);
                        }
                        else
                        {
                            AppendTextWithColor(txtStatusReport, "FAIL\n", Color.Red);
                        }
                    }
                }
                catch (Exception ex)
                {
                    AppendTextWithColor(txtStatusReport, "FAIL\n", Color.Red);
                }
            }

            private void CheckPort(int port)
            {
                try
                {
                    using (TcpClient tcpClient = new TcpClient())
                    {
                        tcpClient.Connect("127.0.0.1", port);
                        AppendTextWithColor(txtStatusReport, "PASS\n", Color.Green);
                    }
                }
                catch (Exception ex)
                {
                    AppendTextWithColor(txtStatusReport, "FAIL\n", Color.Red);
                }
            }

            private void AppendTextWithColor(RichTextBox box, string text, Color color)
            {
                int start = box.TextLength;
                box.AppendText(text);
                int end = box.TextLength;

                box.Select(start, end - start);
                {
                    box.SelectionColor = color;
                }
                box.SelectionLength = 0;
            }
