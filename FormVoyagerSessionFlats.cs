using SessionFlats.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SessionFlats
{
    public partial class FormVoyagerSessionFlats : Form
    {
        const string userName = "rrskybox";
        const string userPassword = "darkstar4ME";

        const string seqFlatFile = "FlatSequence_LRGB_NoRotation.s2f";

        const string RemoteActionEvent = "RemoteActionResult";
        const string GetArrayEvent = "ArrayElementData";

        public static LogEvent StatusReportEvent;
        public static PollEvent PollVoyagerEvent;

        List<int> paList = new List<int>();

        VoyManager voyMan;

        public FormVoyagerSessionFlats(string[] args)
        {

            InitializeComponent();
            //Add log event generator
            PollVoyagerEvent = new PollEvent();
            PollVoyagerEvent.PollEventHandler += PollEvent_Handler;

            //Add poll event generator
            StatusReportEvent = new LogEvent();
            StatusReportEvent.LogEventHandler += LogReportUpdate_Handler;

            //Set Session Start and End
            SessionStartDT.Value = DateTime.Now - TimeSpan.FromDays(1);
            SessionEndDT.Value = SessionStartDT.Value + TimeSpan.FromDays(1);

            //Connect to Voyager
            voyMan = new(userName, userPassword);

            //var pollTest = voyMan.CmdResults("Polling");

            //VoyagerImageFolder.Text = "D:\\Voyager";
            VoyagerImageFolder.Text = Settings.Default.FitsFilePath;

            ////Put one entry in the pa list for debug purposes
            //paList.Add(0);

            if (args.Length > 0  && Directory.Exists(args[0]))
            {
                Settings.Default.FitsFilePath = args[0];
                VoyagerImageFolder.Text = args[0];
                MakeFitsList();
                TakeFlats();
            }
        }

        public void LogReportUpdate_Handler(object sender, LogEvent.LogEventArgs e)
        {
            TrafficTextBox.AppendText(e.LogEntry + "\r\n\r\n");
            this.Show();
            return;
        }

        public void PollEvent_Handler(object sender, PollEvent.PollEventArgs e)
        {
            voyMan.HeartBeat();
        }

        private void IncrementPM_Click(object sender, EventArgs e)
        {
            if (SessionStartDT.Value > SessionEndDT.Value + TimeSpan.FromDays(1))
                SessionStartDT.Value = SessionStartDT.Value + TimeSpan.FromDays(1);
        }

        private void DecrementPM_Click(object sender, EventArgs e)
        {
            if (SessionStartDT.Value > SessionEndDT.Value - TimeSpan.FromDays(1))
                SessionStartDT.Value = SessionStartDT.Value - TimeSpan.FromDays(1);
        }

        private void IncrementAM_Click(object sender, EventArgs e)
        {
            if (SessionEndDT.Value < SessionEndDT.Value + TimeSpan.FromDays(1))
                SessionEndDT.Value = SessionEndDT.Value + TimeSpan.FromDays(1);
        }

        private void DecrementAM_Click(object sender, EventArgs e)
        {
            if (SessionEndDT.Value < SessionEndDT.Value - TimeSpan.FromDays(1))
                SessionEndDT.Value = SessionEndDT.Value - TimeSpan.FromDays(1);
        }

        private void SessionStartDT_ValueChanged(object sender, EventArgs e)
        {
            //Use picked time and set session end (AM) to the next day
            if (SessionEndDT.Value <= SessionStartDT.Value)
                SessionEndDT.Value = SessionStartDT.Value + TimeSpan.FromDays(1);
        }

        private void SessionEndDT_ValueChanged(object sender, EventArgs e)
        {
            //Use picked time and set session start (PM) to the previous day
            if (SessionEndDT.Value <= SessionStartDT.Value)
                SessionStartDT.Value = SessionEndDT.Value - TimeSpan.FromDays(1);
        }

        private void ListFlatsButton_Click(object sender, EventArgs e)
        {
            MakeFitsList();
        }

        private void MakeFitsList()
        {
            //Compile list of need flats from search of light fits files in target directory
            IEnumerable<string> allFitsFiles = Directory.GetFiles(VoyagerImageFolder.Text, "*.fit", SearchOption.AllDirectories)
                .ToList()
                .Where(filePath => !filePath.Contains("Flats"));
            IEnumerable<string> sessionFitsFiles = allFitsFiles.Where
                (ses => File.GetCreationTime(ses) >= SessionStartDT.Value && File.GetCreationTime(ses) <= SessionEndDT.Value);
            //Create list of PA's with 180 inversons
            //Maybe we'll make this smarter later
            paList = new List<int>();
            foreach (string fits in sessionFitsFiles)
            {
                FitsFile ff = new FitsFile(fits, false);  // do not read in image data
                if (ff.ReadKey("ROTATANG") != null)
                {
                    int pa = (int)Convert.ToDouble(ff.ReadKey("ROTATANG"));
                    paList.Add(pa);
                    paList.Add((pa + 180) % 360);
                }
            }
            paList = paList.Distinct().ToList();
            TrafficTextBox.Clear();
            foreach (int pa in paList)
                TrafficTextBox.AppendText(pa.ToString() + "\r\n");
        }

        private void TakeFlatsButton_Click(object sender, EventArgs e)
        {
            TakeFlats();
        }

        private void TakeFlats()
        {
            //Execute the LRGB generic flat for each rotation in the paList

            //Set up path the flat sequence file
            string seqFlatPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile) +
               "\\Documents\\Voyager\\ConfigSequence\\" +
               seqFlatFile;

            //Use the Voyage Flat Sequence to take flats of at each pa in the paList
            foreach (int pa in paList)
            {
                //Unpark mount
                voyMan.MountAction(VoyCommand.MountAction.Unpark);
                //Rotate
                voyMan.Rotate(pa);
                //Image
                voyMan.ImageFlat(seqFlatPath);
            }
        }

        private void ChooseButton_Click(object sender, EventArgs e)
        {
           
            if (FitsFileFolderDialog.ShowDialog() == DialogResult.OK);
            Settings.Default.FitsFilePath = FitsFileFolderDialog.SelectedPath;
            VoyagerImageFolder.Text = Settings.Default.FitsFilePath;
        }

        //Get Profiles
        //json = voyCmd.GetArrayElementData();
        //    lg.LogIt(String.Format("GetArrayElementsData Send: {0}", json));
        //    VoyWeb.WriteWebIO(json);
        //    cmdRsp = CmdResults(GetArrayEvent);
        //lg.LogIt(String.Format("Rotator Connected = {0} PA = {1}", cmdRsp.ROTCONN, cmdRsp.PAROT));

    }
}
