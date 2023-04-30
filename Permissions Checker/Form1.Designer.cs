namespace Permissions_Checker
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnAnalyze = new System.Windows.Forms.Button();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtStatusReport = new System.Windows.Forms.TextBox();
            this.txtUserTesting = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnAnalyze
            // 
            this.btnAnalyze.Location = new System.Drawing.Point(525, 97);
            this.btnAnalyze.Name = "btnAnalyze";
            this.btnAnalyze.Size = new System.Drawing.Size(160, 51);
            this.btnAnalyze.TabIndex = 0;
            this.btnAnalyze.Text = "Analyze";
            this.btnAnalyze.UseVisualStyleBackColor = true;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(75, 54);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(126, 64);
            this.btnBrowse.TabIndex = 1;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            // 
            // txtStatusReport
            // 
            this.txtStatusReport.Location = new System.Drawing.Point(428, 157);
            this.txtStatusReport.Multiline = true;
            this.txtStatusReport.Name = "txtStatusReport";
            this.txtStatusReport.Size = new System.Drawing.Size(363, 285);
            this.txtStatusReport.TabIndex = 2;
            // 
            // txtUserTesting
            // 
            this.txtUserTesting.Location = new System.Drawing.Point(12, 157);
            this.txtUserTesting.Multiline = true;
            this.txtUserTesting.Name = "txtUserTesting";
            this.txtUserTesting.Size = new System.Drawing.Size(369, 294);
            this.txtUserTesting.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.txtUserTesting);
            this.Controls.Add(this.txtStatusReport);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.btnAnalyze);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnAnalyze;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox txtStatusReport;
        private System.Windows.Forms.TextBox txtUserTesting;
    }
}

