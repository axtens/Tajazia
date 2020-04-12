using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuarterMaster.Scripting;
using Microsoft.VisualBasic;

namespace Tajazia
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void WBMain_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            Ruleset.JSE.AddHostObject("CSDocument", WBMain.Document);
            if (Program.keyValuePairs.ContainsKey("p1"))
            {
                var result = Program.ruleset.Run(File.ReadAllText(Program.keyValuePairs["p1"].ToString()), Program.config, Program.keyValuePairs);
                Form1.ActiveForm.Close();
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Ruleset.JSE.AddHostObject("CSForm", sender);
            Ruleset.JSE.AddHostObject("CSFormEventArgs", e);
            Ruleset.JSE.AddHostType("CSVBInteraction", typeof(Microsoft.VisualBasic.Interaction));
            Ruleset.JSE.AddHostObject("CSWebBrowser", WBMain);
            Ruleset.JSE.AddHostObject("CSRunButton", BRun);
            Ruleset.JSE.AddHostObject("CSListBoxScripts", LBScripts);
            Ruleset.JSE.AddHostObject("CSLStatus", LStatus);
            Ruleset.JSE.AddHostType("CSMessageBox", typeof(MessageBox));
            Ruleset.JSE.AddHostType("CSHtmlElementInsertionOrientation", typeof(HtmlElementInsertionOrientation));

            var html = "<HTML><HEAD></HEAD><BODY></BODY><?HTML>";
            var path = Path.Combine(Path.GetTempPath(),"Tajazia.html");
            if (!File.Exists(path))
            {
                File.WriteAllText(path, html);
            }

            WBMain.Url = new System.Uri("file://" + path.Replace("\\", "/")); //  C:/TMP/Tajazia.html");

            var sitesFile = Program.config.Retrieve("Sites", null) ?? ".\\Sites.txt";
            if (File.Exists(sitesFile))
            {
                LBScripts.DataSource = File.ReadAllLines(sitesFile);
            }
        }

        private void BRun_Click(object sender, EventArgs e)
        {
            Program.keyValuePairs["siteUrl"] = LBScripts.SelectedItem.ToString();
            var result = Program.ruleset.Run(File.ReadAllText(".\\Scripts\\Tajazia.js"), Program.config, Program.keyValuePairs);
            //MessageBox.Show(result.Item2.ToString());
        }

        private void BReloadList_Click(object sender, EventArgs e)
        {
            var sitesFile = Program.config.Retrieve("Sites", null) ?? ".\\Sites.txt";
            if (File.Exists(sitesFile))
            {
                LBScripts.DataSource = File.ReadAllLines(sitesFile);
            }
        }

        private void LBScripts_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
