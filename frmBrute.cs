using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace keystoreBrute
{
    public partial class frmBrute : Form
    {
        public frmBrute ()
        {
            InitializeComponent ();
        }

        protected override void OnLoad (EventArgs e)
        {
            base.OnLoad (e);
            string personalFolder = Environment.GetFolderPath (Environment.SpecialFolder.Personal);
            
            if (personalFolder.EndsWith ("Documents"))
                personalFolder = Path.GetDirectoryName (personalFolder);

            personalFolder = Path.Combine (personalFolder, ".android");

            txtKeystoreFile.Text = Path.Combine (personalFolder, "debug.keystore");
        }

        bool passwordLost;
        List<string> combinations = new List<string>();
        int index = 0;

        public string GetPassword ()
        {
            if (index < combinations.Count)
                return combinations[index++];

            MessageBox.Show ("Unable to Find Password");
            passwordLost = false;
            return "";
        }

        public void ExecuteCommand ()
        {
            String password = GetPassword ();
            lblPassword.Text = password;

            Process output = Cmd.Execute ("keytool", "-list -keystore  " + txtKeystoreFile.Text + " -storepass " + password.Replace ("\"", "\\\""));

            if (output.ExitCode == 0) {
                MessageBox.Show (String.Format ("The Password is {0}", password));
                passwordLost = false;
            }

            string result = "\nPassword: " + password + "\r\n\t";
            result += output.StandardOutput.ReadToEnd ().Trim ();

            txtResults.AppendText (result);


        }

        private void btnBruteForce_Click (object sender, EventArgs e)
        {
            index = 0;
            List<string> parts = new List<string> ();
            parts.AddRange (txtKeyParts.Lines);
            txtResults.Text = "";
            txtFailedPasswords.Text = "";
            while (parts.Contains (""))
                parts.Remove ("");

            txtKeyParts.Lines = parts.ToArray ();

            passwordLost = true;

            combinations.Clear ();
            List<List<string>> permutations;
            permutations = PermuteUtils.Permute<string> (parts, parts.Count).Select (permutation => permutation.ToList ()).ToList ();

            while (permutations.First().Count > 0) {
                Console.WriteLine ("permutations " + permutations.Count);
                combinations = combinations.Concat (permutations.Select (seq => String.Join ("", seq.ToArray ()))).Distinct ().ToList ();
                permutations = permutations.Select (seq => seq.Take (seq.Count - 1).ToList ()).ToList ();
            }

            while (passwordLost) {
                ExecuteCommand ();
                Application.DoEvents ();
            }
        }
    }
}
