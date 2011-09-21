using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace keystoreBrute
{
    public partial class frmBrute : Form
    {
        public frmBrute()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            string personalFolder = Environment.GetFolderPath(Environment.SpecialFolder.Personal);

            if (personalFolder.EndsWith("Documents"))
                personalFolder = Path.GetDirectoryName(personalFolder);

            personalFolder = Path.Combine(personalFolder, ".android");

            txtKeystoreFile.Text = Path.Combine(personalFolder, "debug.keyStore");
        }

        bool passwordLost;
        List<String> keyParts = new List<string>();

        int part1 = -1;
        int part2 = -1;
        int part3 = -1;
        int part4 = -1;
        int part5 = -1;
        public String GetPassword()
        {
            part1++;
            if (part1 >= keyParts.Count)
            {
                part1 = 0;
                part2++;
            }
            if (part2 >= keyParts.Count)
            {
                part2 = 0;
                part3++;
            }
            if (part3 >= keyParts.Count)
            {
                part3 = 0;
                part4++;
            }
            if (part3 >= keyParts.Count)
            {
                part3 = 0;
                part4++;
            }
            if (part4 >= keyParts.Count)
            {
                part4 = 0;
                part5++;
            }
            if (part5 >= keyParts.Count)
            {
                MessageBox.Show("Unable to Find Password");
                passwordLost = false;
                return "";
            }


            string result = keyParts[part1];
            if (part2 >= 0) result += keyParts[part2];
            if (part3 >= 0) result += keyParts[part3];
            if (part4 >= 0) result += keyParts[part4];
            if (part5 >= 0) result += keyParts[part5];
            return result;
        }

        public void ExecuteCommand()
        {
		    String password;
            password = GetPassword();
            lblPassword.Text = password;

            string output = Shell.Execute("keytool", "-list -keystore  " + txtKeystoreFile.Text + " -storepass " + password);

            if (output.Contains("fingerprint"))
            {
                MessageBox.Show(String.Format("The Password is {0}", password));
                passwordLost = false;
            }

            string result = "\nPassword: " + password + "\r\n\t";
            result += output;
            if (output.Trim() == "")
            {
                txtFailedPasswords.AppendText(password + "\r\n");
            }

            txtResults.AppendText(result);
            
            
        }

        private void btnBruteForce_Click(object sender, EventArgs e)
        {
            keyParts.Clear();
            keyParts.AddRange(txtKeyParts.Lines);
            txtResults.Text = "";
            txtFailedPasswords.Text = "";
            while (keyParts.Contains("")) keyParts.Remove("");

            txtKeyParts.Lines = keyParts.ToArray();

            passwordLost = true;

            while (passwordLost)
            {
                ExecuteCommand();
                Application.DoEvents();
            }
        }
    }
}
