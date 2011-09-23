namespace keystoreBrute
{
    partial class frmBrute
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
            this.btnBruteForce = new System.Windows.Forms.Button();
            this.txtResults = new System.Windows.Forms.TextBox();
            this.txtKeyParts = new System.Windows.Forms.TextBox();
            this.txtFailedPasswords = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtKeystoreFile = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtMaxPassSegments = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnBruteForce
            // 
            this.btnBruteForce.Location = new System.Drawing.Point(239, 6);
            this.btnBruteForce.Name = "btnBruteForce";
            this.btnBruteForce.Size = new System.Drawing.Size(75, 48);
            this.btnBruteForce.TabIndex = 0;
            this.btnBruteForce.Text = "Execute Brute Force";
            this.btnBruteForce.UseVisualStyleBackColor = true;
            this.btnBruteForce.Click += new System.EventHandler(this.btnBruteForce_Click);
            // 
            // txtResults
            // 
            this.txtResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtResults.Location = new System.Drawing.Point(0, 227);
            this.txtResults.Multiline = true;
            this.txtResults.Name = "txtResults";
            this.txtResults.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtResults.Size = new System.Drawing.Size(565, 208);
            this.txtResults.TabIndex = 1;
            // 
            // txtKeyParts
            // 
            this.txtKeyParts.Location = new System.Drawing.Point(0, 25);
            this.txtKeyParts.Multiline = true;
            this.txtKeyParts.Name = "txtKeyParts";
            this.txtKeyParts.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtKeyParts.Size = new System.Drawing.Size(223, 178);
            this.txtKeyParts.TabIndex = 2;
            this.txtKeyParts.Text = "an\r\ndr\r\noid\r\n";
            // 
            // txtFailedPasswords
            // 
            this.txtFailedPasswords.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFailedPasswords.Location = new System.Drawing.Point(350, 25);
            this.txtFailedPasswords.Multiline = true;
            this.txtFailedPasswords.Name = "txtFailedPasswords";
            this.txtFailedPasswords.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtFailedPasswords.Size = new System.Drawing.Size(215, 165);
            this.txtFailedPasswords.TabIndex = 3;
            // 
            // lblPassword
            // 
            this.lblPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPassword.Location = new System.Drawing.Point(236, 190);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(329, 16);
            this.lblPassword.TabIndex = 4;
            this.lblPassword.Text = "lblPassword";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(229, 167);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Current Password:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(-3, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(170, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Password Segments: (one per line)";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(347, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Failed Attempts";
            // 
            // txtKeystoreFile
            // 
            this.txtKeystoreFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtKeystoreFile.Location = new System.Drawing.Point(57, 205);
            this.txtKeystoreFile.Name = "txtKeystoreFile";
            this.txtKeystoreFile.Size = new System.Drawing.Size(508, 20);
            this.txtKeystoreFile.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(-2, 208);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "keystore";
            // 
            // txtMaxPassSegments
            // 
            this.txtMaxPassSegments.Location = new System.Drawing.Point(271, 112);
            this.txtMaxPassSegments.Name = "txtMaxPassSegments";
            this.txtMaxPassSegments.Size = new System.Drawing.Size(68, 20);
            this.txtMaxPassSegments.TabIndex = 10;
            this.txtMaxPassSegments.Text = "1000";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(229, 87);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(106, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Max Pass Segments:";
            // 
            // frmBrute
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(567, 436);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtMaxPassSegments);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtKeystoreFile);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtFailedPasswords);
            this.Controls.Add(this.txtKeyParts);
            this.Controls.Add(this.txtResults);
            this.Controls.Add(this.btnBruteForce);
            this.Name = "frmBrute";
            this.Text = "frmBrute";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnBruteForce;
        private System.Windows.Forms.TextBox txtResults;
        private System.Windows.Forms.TextBox txtKeyParts;
        private System.Windows.Forms.TextBox txtFailedPasswords;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtKeystoreFile;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtMaxPassSegments;
        private System.Windows.Forms.Label label5;
    }
}