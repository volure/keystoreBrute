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
        int maxPassSegments;
        int currentSegmentCount;
        List<string> combinations = new List<string>();
        Dictionary<int, List<string>> countedCombinations = new Dictionary<int, List<string>>();
        
        
        int index = 0;

        public string GetPassword ()
        {
            if (index < combinations.Count)
                return combinations[index++];

            if (SetNextCombinationList())
            {
                return GetPassword();
            }


            MessageBox.Show ("Unable to Find Password");
            passwordLost = false;
            return "";
        }

        public void ExecuteCommand ()
        {
            String password = GetPassword ();
            lblPassword.Text = password;

            Process output = Cmd.Execute ("keytool", "-list -keystore \"" + txtKeystoreFile.Text + "\" -storepass " + password.Replace ("\"", "\\\""));

            if (output.ExitCode == 0) {
                MessageBox.Show (String.Format ("The Password is {0}", password));
                passwordLost = false;
            }

            string result = "\nPassword: " + password + "\r\n\t";
            result += output.StandardOutput.ReadToEnd().Trim();

            txtResults.AppendText (result + "\r\n");

        }

        private void btnBruteForce_Click (object sender, EventArgs e)
        {
            List<string> parts = new List<string> ();
            parts.AddRange (txtKeyParts.Lines);
            txtResults.Text = "";
            txtFailedPasswords.Text = "";
            maxPassSegments = 9999;
            int.TryParse(txtMaxPassSegments.Text, out maxPassSegments);

            currentSegmentCount = maxPassSegments;

            while (parts.Contains (""))
                parts.Remove ("");

            txtKeyParts.Lines = parts.ToArray ();

            passwordLost = true;

            combinations.Clear(); 
            countedCombinations.Clear();
            List<List<string>> permutations;
            permutations = PermuteUtils.Permute<string> (parts, parts.Count).Select (permutation => permutation.ToList ()).ToList ();

            while (permutations.First().Count > 0) {
                Console.WriteLine ("permutations " + permutations.Count);
                combinations = permutations.Select (seq => String.Join ("", seq.ToArray ())).Distinct ().ToList ();
                countedCombinations.Add(permutations[0].Count, combinations);
                permutations = permutations.Select (seq => seq.Take (seq.Count - 1).ToList ()).ToList ();
            }

            SetNextCombinationList();


            while (passwordLost) {
                ExecuteCommand();
                Application.DoEvents();
            }
        }

        private bool SetNextCombinationList()
        {
            combinations = null;
            while (!countedCombinations.Keys.Contains(currentSegmentCount)) 
            {
                currentSegmentCount--;
                if (currentSegmentCount == 0) return false;
            }

            combinations = countedCombinations[currentSegmentCount];
            countedCombinations.Remove(currentSegmentCount);
            index = 0;
            return true;
        }
    }
}
