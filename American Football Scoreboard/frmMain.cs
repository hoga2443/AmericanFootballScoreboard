using EmbedIO;
using EmbedIO.Actions;
using EmbedIO.Routing;
using EmbedIO.WebApi;
using OBSWebsocketDotNet;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using MethodInvoker = System.Windows.Forms.MethodInvoker;
namespace American_Football_Scoreboard
{
    public partial class FrmMain : Form
    {
        protected OBSWebsocket obs;
        private static readonly HttpListener Listener = new();
        const string awayPenaltyType = "AwayPenaltyType.txt";
        const string awayPeriodScoreFirst = "AwayPeriodScoreFirst.txt";
        const string awayPeriodScoreSecond = "AwayPeriodScoreSecond.txt";
        const string awayPeriodScoreThird = "AwayPeriodScoreThird.txt";
        const string awayPeriodScoreFourth = "AwayPeriodScoreFourth.txt";
        const string awayPeriodScoreOT = "AwayPeriodScoreOT.txt";
        const string awayTeamNameFile = "AwayTeamName.txt";
        const string awayTeamScoreFile = "AwayTeamScore.txt";
        const string awayTimeoutsRemainingFile = "AwayTimeoutsRemaining.txt";
        private Period currentPeriod = Period.Unknown;
        const string distanceFile = "Distance.txt";
        const string downFile = "Down.txt";
        const string downDistanceFile = "DownDistance.txt";
        const string gameClockFile = "GameClock.txt";
        private bool gameClockRunning = false;
        const string homePenaltyType = "HomePenaltyType.txt";
        const string homePeriodScoreFirst = "HomePeriodScoreFirst.txt";
        const string homePeriodScoreSecond = "HomePeriodScoreSecond.txt";
        const string homePeriodScoreThird = "HomePeriodScoreThird.txt";
        const string homePeriodScoreFourth = "HomePeriodScoreFourth.txt";
        const string homePeriodScoreOT = "HomePeriodScoreOT.txt";
        const string homeTeamNameFile = "HomeTeamName.txt";
        const string homeTeamScoreFile = "HomeTeamScore.txt";
        const string homeTimeoutsRemainingFile = "HomeTimeoutsRemaining.txt";
        public class MyApiController : WebApiController
        {
            FrmMain frmMain = (FrmMain)Application.OpenForms["FrmMain"];
            // GET /api/AwayFieldGoal
            [Route(HttpVerbs.Get, "/AwayFieldGoal")]
            public string AwayFieldGoal()
            {
                frmMain.ButAwayFieldGoal_Event();
                return "AwayFieldGoal";
            }
            // GET /api/AwayPatKick
            [Route(HttpVerbs.Get, "/AwayPatKick")]
            public string AwayPatKick()
            {
                frmMain.ButAwayPatKick_Event();
                return "AwayPatKick";
            }
            // GET /api/AwayPatConversion
            [Route(HttpVerbs.Get, "/AwayPatConversion")]
            public string AwayPatConversion()
            {
                frmMain.ButAwayPatConversion_Event();
                return "AwayPatConversion";
            }
            // GET /api/AwayPossession
            [Route(HttpVerbs.Get, "/AwayPossession")]
            public string AwayPossession()
            {
                frmMain.AwayPossession_Event();
                return "AwayPossession";
            }
            // GET /api/AwaySafety
            [Route(HttpVerbs.Get, "/AwaySafety")]
            public string AwaySafety()
            {
                frmMain.ButAwaySafety_Event();
                return "AwaySafety";
            }
            // GET /api/AwayTouchdown
            [Route(HttpVerbs.Get, "/AwayTouchdown")]
            public string AwayTouchdown()
            {
                frmMain.ButAwayTouchdown_Event();
                return "AwayTouchdown";
            }
            // GET /api/AwayTimeoutAdd
            [Route(HttpVerbs.Get, "/AwayTimeoutAdd")]
            public string AwayTimeoutAdd()
            {
                frmMain.ButAwayTimeoutsAdd_Event();
                return "AwayTimeoutAdd";
            }
            // GET /api/AwayTimeoutSubtract
            [Route(HttpVerbs.Get, "/AwayTimeoutSubtract")]
            public string AwayTimeoutSubtract()
            {
                frmMain.ButAwayTimeoutsSubtract_Event();
                return "AwayTimeoutSubtract";
            }
            // GET /api/DownBlank
            [Route(HttpVerbs.Get, "/DownBlank")]
            public string DownBlank()
            {
                frmMain.RbDownBlank_Event();
                return "DownBlank";
            }
            // GET /api/DownOne
            [Route(HttpVerbs.Get, "/DownOne")]
            public string DownOne()
            {
                frmMain.RbDownOne_Event();
                return "DownOne";
            }
            // GET /api/DownTwo
            [Route(HttpVerbs.Get, "/DownTwo")]
            public string DownTwo()
            {
                frmMain.RbDownTwo_Event();
                return "DownTwo";
            }
            // GET /api/DownThree
            [Route(HttpVerbs.Get, "/DownThree")]
            public string DownThree()
            {
                frmMain.RbDownThree_Event();
                return "DownThree";
            }
            // GET /api/DownFour
            [Route(HttpVerbs.Get, "/DownFour")]
            public string DownFour()
            {
                frmMain.RbDownFour_Event();
                return "DownFour";
            }
            // GET /api/DownNext
            [Route(HttpVerbs.Get, "/DownNext")]
            public string DownNext()
            {
                frmMain.NextDown();
                return "DownNext";
            }
            // GET /api/Flag
            [Route(HttpVerbs.Get, "/Flag")]
            public string ChkFlagChecked_Event()
            {
                frmMain.NextDown();
                return "Flag";
            }
            // GET /api/HomeFieldGoal
            [Route(HttpVerbs.Get, "/HomeFieldGoal")]
            public string HomeFieldGoal()
            {
                frmMain.ButHomeFieldGoal_Event();
                return "HomeFieldGoal";
            }
            // GET /api/HomePatKick
            [Route(HttpVerbs.Get, "/HomePatKick")]
            public string HomePatKick()
            {
                frmMain.ButHomePatKick_Event();
                return "HomePatKick";
            }
            // GET /api/HomePatConversion
            [Route(HttpVerbs.Get, "/HomePatConversion")]
            public string HomePatConversion()
            {
                frmMain.ButHomePatConversion_Event();
                return "HomePatConversion";
            }
            // GET /api/HomePossession
            [Route(HttpVerbs.Get, "/HomePossession")]
            public string HomePossession()
            {
                frmMain.HomePossession_Event();
                return "HomePossession";
            }
            // GET /api/HomeSafety
            [Route(HttpVerbs.Get, "/HomeSafety")]
            public string HomeSafety()
            {
                frmMain.ButHomeSafety_Event();
                return "HomeSafety";
            }
            // GET /api/HomeTouchdown
            [Route(HttpVerbs.Get, "/HomeTouchdown")]
            public string HomeTouchdown()
            {
                frmMain.ButHomeTouchdown_Event();
                return "HomeTouchdown";
            }
            // GET /api/HomeTimeoutAdd
            [Route(HttpVerbs.Get, "/HomeTimeoutAdd")]
            public string HomeTimeoutAdd()
            {
                frmMain.ButHomeTimeoutsAdd_Event();
                return "HomeTimeoutAdd";
            }
            // GET /api/HomeTimeoutSubtract
            [Route(HttpVerbs.Get, "/HomeTimeoutSubtract")]
            public string HomeTimeoutSubtract()
            {
                frmMain.ButHomeTimeoutsSubtract_Event();
                return "HomeTimeoutSubtract";
            }
            // GET /api/PeriodOne
            [Route(HttpVerbs.Get, "/PeriodOne")]
            public string PeriodOne()
            {
                frmMain.RbPeriodOne_Event();
                return "PeriodOne";
            }
            // GET /api/PeriodTwo
            [Route(HttpVerbs.Get, "/PeriodTwo")]
            public string PeriodTwo()
            {
                frmMain.RbPeriodTwo_Event();
                return "PeriodTwo";
            }
            // GET /api/PeriodHalf
            [Route(HttpVerbs.Get, "/PeriodHalf")]
            public string PeriodHalf()
            {
                frmMain.RbPeriodHalf_Event();
                return "PeriodHalf";
            }
            // GET /api/PeriodThree
            [Route(HttpVerbs.Get, "/PeriodThree")]
            public string PeriodThree()
            {
                frmMain.RbPeriodThree_Event();
                return "PeriodThree";
            }
            // GET /api/PeriodFour
            [Route(HttpVerbs.Get, "/PeriodFour")]
            public string PeriodFour()
            {
                frmMain.RbPeriodFour_Event();
                return "PeriodFour";
            }
            // GET /api/PeriodOT
            [Route(HttpVerbs.Get, "/PeriodOT")]
            public string PeriodOT()
            {
                frmMain.RbPeriodOT_Event();
                return "PeriodOT";
            }
            // GET /api/PeriodFinal
            [Route(HttpVerbs.Get, "/PeriodFinal")]
            public string PeriodFinal()
            {
                frmMain.RbPeriodFinal_Event();
                return "PeriodFinal";
            }
            // GET /api/PeriodNext
            [Route(HttpVerbs.Get, "/PeriodNext")]
            public string PeriodNext()
            {
                frmMain.NextPeriod();
                return "PeriodNext";
            }
        }
        private readonly TimeSpan oneMinute = new(hours: 0, minutes: 1, seconds: 0);
        const char padZero = '0';
        const string penaltyType = "PenaltyType.txt";
        private enum Period { One, Two, Half, Three, Four, OT, Final, Unknown };
        const string periodFile = "Period.txt";
        const string playClockFile = "PlayClock.txt";
        const string scoreDescription = "ScoreDescription.txt";
        const string spotFile = "Spot.txt";
        const string supplementalFile = "Supplemental.txt";
        private DateTime periodClockEnd = DateTime.UtcNow;
        private TimeSpan periodTimeRemaining = new(hours: 0, minutes: 0, seconds: 0);
        private bool playClockRunning = false;
        private DateTime playTimeEnd = DateTime.UtcNow;
        private TimeSpan playTimeRemaining = new(hours: 0, minutes: 0, seconds: 0);
        private enum Score { FieldGoal, PatKick, PatConversion, Safety, Touchdown };
        public WebServer server;
        public FrmMain()
        {
            InitializeComponent();
            AddVersionNumber();
            LoadSettings();
            InitializeUI();
            InitializeDatabase();
            RegisterHotKeys();
            obs = new OBSWebsocket();
            obs.Connected += OnConnect;
            ObsConnect();
            this.WindowState = FormWindowState.Normal;
            PopulateImageButtonsAway();
            PopulateImageButtonsHome();
            if (!string.IsNullOrEmpty(Properties.Settings.Default.HttpListenerPort))
                StartHttpListener();
        }
        private void AddScore(bool home, TextBox textBox, int points, string message = "")
        {
            rbDownBlank.Checked = true;
            if (int.TryParse(s: textBox.Text, out int oldScore))
                textBox.Text = (oldScore + points).ToString();
            if (message != string.Empty)
            {
                _ = Common.WriteFileAsync(file: scoreDescription, content: message);
                tmrScore.Interval = Properties.Settings.Default.FlagDisplayDuration;
                tmrScore.Enabled = true;
            }
            if (home)
            {
                switch (currentPeriod)
                {
                    case Period.One:
                        UpdatePeriodScore(control: txtPeriodHomeFirst, points: points);
                        break;
                    case Period.Two:
                        UpdatePeriodScore(control: txtPeriodHomeSecond, points: points);
                        break;
                    case Period.Three:
                        UpdatePeriodScore(control: txtPeriodHomeThird, points: points);
                        break;
                    case Period.Four:
                        UpdatePeriodScore(control: txtPeriodHomeFourth, points: points);
                        break;
                    case Period.OT:
                        UpdatePeriodScore(control: txtPeriodHomeOT, points: points);
                        break;
                }
            }
            else
            {
                switch (currentPeriod)
                {
                    case Period.One:
                        UpdatePeriodScore(control: txtPeriodAwayFirst, points: points);
                        break;
                    case Period.Two:
                        UpdatePeriodScore(control: txtPeriodAwaySecond, points: points);
                        break;
                    case Period.Three:
                        UpdatePeriodScore(control: txtPeriodAwayThird, points: points);
                        break;
                    case Period.Four:
                        UpdatePeriodScore(control: txtPeriodAwayFourth, points: points);
                        break;
                    case Period.OT:
                        UpdatePeriodScore(control: txtPeriodAwayOT, points: points);
                        break;
                }
            }
        }
        private void AddGameTime(int seconds)
        {
            TimeSpan additionalSeconds = new(days: 0, hours: 0, minutes: 0, seconds: seconds, milliseconds: 0);
            if (gameClockRunning)
            {
                periodClockEnd += additionalSeconds;
            }
            else
            {
                periodTimeRemaining = TimeRemainingFromTextBox() + additionalSeconds;
                if (periodTimeRemaining < oneMinute && Properties.Settings.Default.SubSecond)
                    txtGameClock.Text = "0:" + periodTimeRemaining.Seconds.ToString().PadLeft(totalWidth: 2, paddingChar: padZero) + "." + periodTimeRemaining.Milliseconds.ToString()[..1];
                else
                    txtGameClock.Text = periodTimeRemaining.Minutes.ToString().PadLeft(totalWidth: 2, paddingChar: padZero) + ":" + periodTimeRemaining.Seconds.ToString().PadLeft(totalWidth: 2, paddingChar: padZero);
                _ = Common.WriteFileAsync(file: gameClockFile, content: txtGameClock.Text);
            }
        }
        private void AddTimeout(TextBox control, int timeoutsToAdd)
        {
            if (int.TryParse(s: control.Text, out int currentTimeouts))
            {
                if (currentTimeouts + timeoutsToAdd < 0)
                    control.Text = "0";
                else
                    control.Text = (currentTimeouts + timeoutsToAdd).ToString();
            }
            else
                control.Text = (timeoutsToAdd).ToString();
            if (timeoutsToAdd == -1)
            {
                gameClockRunning = false;
                butStartStopGameClock.Text = "Start Game Clock";
                if (playClockRunning == false)
                    tmrClockRefresh.Enabled = false;
                SetPlayClock(duration: Properties.Settings.Default.ShortPlayClock, start: false);
            }
        }
        private void AddVersionNumber()
        {
            /*
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(fileName: assembly.Location);
            this.Text += $" v.{versionInfo.FileVersion}";
            */
            this.Text += $" v.3.0.1";
        }
        private void AdvanceQuarter()
        {
            butStartStopGameClock.Text = "Start Game Clock";
            gameClockRunning = false;
            if (rbPeriodOne.Checked)
                rbPeriodTwo.Checked = true;
            else if (rbPeriodTwo.Checked)
                rbPeriodHalf.Checked = true;
            else if (rbPeriodHalf.Checked)
                rbPeriodThree.Checked = true;
            else if (rbPeriodThree.Checked)
                rbPeriodFour.Checked = true;
            else if (rbPeriodFour.Checked && txtAwayScore.Text == txtHomeScore.Text)
                rbPeriodOT.Checked = true;
            else if (rbPeriodFour.Checked && txtAwayScore.Text != txtHomeScore.Text)
                rbPeriodFinal.Checked = true;
            ClearClocks();
        }
        private void AwayTimeoutsSubtract()
        {
            AddTimeout(control: txtAwayTimeouts, timeoutsToAdd: -1);
        }
        private void ButAwayFieldGoal_Click(object sender, EventArgs e)
        {
            ButAwayFieldGoal_Event();
        }
        private void ButAwayFieldGoal_Event()
        {
            if (txtAwayScore.InvokeRequired)
            {
                txtAwayScore.Invoke(new MethodInvoker(delegate
                {
                    ButAwayFieldGoal_Event();
                }));
            }
            else
            {
                AddScore(home: false, textBox: txtAwayScore, points: Properties.Settings.Default.FieldGoal);
                WriteScore(text: "Field Goal");
                WriteScoreImage(score: Score.FieldGoal);
            }
        }
        public void ButAwayPatConversion_Click(object sender, EventArgs e)
        {
            ButAwayPatConversion_Event();
        }
        public void ButAwayPatConversion_Event()
        {
            if (txtAwayScore.InvokeRequired)
            {
                txtAwayScore.Invoke(new MethodInvoker(delegate
                {
                    ButAwayPatConversion_Event();
                }));
            }
            else
            {
                AddScore(home: false, textBox: txtAwayScore, points: Properties.Settings.Default.PatConversion);
                WriteScore(text: "Conversion");
                WriteScoreImage(score: Score.PatConversion);
            }
        }
        public void ButAwayPatKick_Click(object sender, EventArgs e)
        {
            ButAwayPatKick_Event();
        }
        public void ButAwayPatKick_Event()
        {
            if (txtAwayScore.InvokeRequired)
            {
                txtAwayScore.Invoke(new MethodInvoker(delegate
                {
                    ButAwayPatKick_Event();
                }));
            }
            else
            {
                AddScore(home: false, textBox: txtAwayScore, points: Properties.Settings.Default.PatKick);
                WriteScore(text: "PAT");
                WriteScoreImage(score: Score.PatKick);
            }
        }
        private void ButAwayPlayerShow_Click(object sender, EventArgs e)
        {
            Common.ShowHidePlayer(home: false, button: butAwayPlayerShow, textBox: txtAwayPlayerNumber, obs: obs);
            tmrPlayerAway.Interval = Properties.Settings.Default.FlagDisplayDuration;
            tmrPlayerAway.Enabled = true;
            tmrPlayerAway.Start();
        }
        private void ButAwaySafety_Click(object sender, EventArgs e)
        {
            ButAwaySafety_Event();
        }
        public void ButAwaySafety_Event()
        {
            if (txtAwayScore.InvokeRequired)
            {
                txtAwayScore.Invoke(new MethodInvoker(delegate
                {
                    ButAwaySafety_Event();
                }));
            }
            else
            {
                AddScore(home: false, textBox: txtAwayScore, points: Properties.Settings.Default.Safety);
                WriteScore(text: "Safety");
                WriteScoreImage(score: Score.Safety);
            }
        }
        private void ButAwayTimeoutsAdd_Click(object sender, EventArgs e)
        {
            ButAwayTimeoutsAdd_Event();
        }
        public void ButAwayTimeoutsAdd_Event()
        {
            if (txtAwayTimeouts.InvokeRequired)
            {
                txtAwayTimeouts.Invoke(new MethodInvoker(delegate
                {
                    ButAwayTimeoutsAdd_Event();
                }));
            }
            else
                AddTimeout(control: txtAwayTimeouts, timeoutsToAdd: 1);
        }
        private void ButAwayTimeoutsSubtract_Click(object sender, EventArgs e)
        {
            ButAwayTimeoutsSubtract_Event();
        }
        public void ButAwayTimeoutsSubtract_Event()
        {
            if (txtAwayTimeouts.InvokeRequired)
            {
                txtAwayTimeouts.Invoke(new MethodInvoker(delegate
                {
                    ButAwayTimeoutsSubtract_Event();
                }));
            }
            else
                AddTimeout(control: txtAwayTimeouts, timeoutsToAdd: -1);
        }
        private void ButAwayTouchdown_Click(object sender, EventArgs e)
        {
            ButAwayTouchdown_Event();
        }
        public void ButAwayTouchdown_Event()
        {
            if (txtAwayScore.InvokeRequired)
            {
                txtAwayScore.Invoke(new MethodInvoker(delegate
                {
                    ButAwayTouchdown_Event();
                }));
            }
            else
            {
                AddScore(home: false, textBox: txtAwayScore, points: Properties.Settings.Default.Touchdown);
                WriteScore(text: "Touchdown");
                WriteScoreImage(score: Score.Touchdown);
            }
        }
        public void ButClearAll_Click(object sender, EventArgs e)
        {
            ClearAway();
            ClearClocks();
            ClearDown();
            ClearHome();
            ClearPeriod();
        }
        public void ButClearAway_Click(object sender, EventArgs e)
        {
            ClearAway();
        }
        public void ButClearClocks_Click(object sender, EventArgs e)
        {
            ClearClocks();
        }
        public void ButClearDown_Click(object sender, EventArgs e)
        {
            ClearDown();
        }
        public void ButClearHome_Click(object sender, EventArgs e)
        {
            ClearHome();
        }
        public void ButClearPeriod_Click(object sender, EventArgs e)
        {
            ClearPeriod();
        }
        public void ButClearPlay_Click(object sender, EventArgs e)
        {
            playClockRunning = false;
            txtPlayClock.Text = string.Empty;
            _ = Common.WriteFileAsync(file: playClockFile, content: txtPlayClock.Text);
            butStartStopPlayClock.Text = "Start Play Clock";
        }
        public void ButDistanceGoal_Click(object sender, EventArgs e)
        {
            txtDistance.Text = Properties.Settings.Default["GoalText"].ToString();
        }
        public void ButGameAdd1_Click(object sender, EventArgs e)
        {
            AddGameTime(seconds: 1);
        }
        public void ButGameAdd10_Click(object sender, EventArgs e)
        {
            AddGameTime(seconds: 10);
        }
        public void ButGameSubtract1_Click(object sender, EventArgs e)
        {
            AddGameTime(seconds: -1);
        }
        public void ButGameSubtract10_Click(object sender, EventArgs e)
        {
            AddGameTime(seconds: -10);
        }
        public void ButHomeFieldGoal_Click(object sender, EventArgs e)
        {
            ButHomeFieldGoal_Event();
        }
        public void ButHomeFieldGoal_Event()
        {
            if (txtHomeScore.InvokeRequired)
            {
                txtHomeScore.Invoke(new MethodInvoker(delegate
                {
                    ButHomeFieldGoal_Event();
                }));
            }
            else
            {
                AddScore(home: true, textBox: txtHomeScore, points: Properties.Settings.Default.FieldGoal);
                WriteScore(text: "Field Goal");
                WriteScoreImage(score: Score.FieldGoal);
            }
        }
        public void ButHomePatConversion_Click(object sender, EventArgs e)
        {
            ButHomePatConversion_Event();
        }
        public void ButHomePatConversion_Event()
        {
            if (txtHomeScore.InvokeRequired)
            {
                txtHomeScore.Invoke(new MethodInvoker(delegate
                {
                    ButHomePatConversion_Event();
                }));
            }
            else
            {
                AddScore(home: true, textBox: txtHomeScore, points: Properties.Settings.Default.PatConversion);
                WriteScore(text: "Conversion");
                WriteScoreImage(score: Score.PatConversion);
            }
        }
        public void ButHomePatKick_Click(object sender, EventArgs e)
        {
            ButHomePatKick_Event();
        }
        public void ButHomePatKick_Event()
        {
            if (txtHomeScore.InvokeRequired)
            {
                txtHomeScore.Invoke(new MethodInvoker(delegate
                {
                    ButHomePatKick_Event();
                }));
            }
            else
            {
                AddScore(home: true, textBox: txtHomeScore, points: Properties.Settings.Default.PatKick);
                WriteScore(text: "PAT");
                WriteScoreImage(score: Score.PatKick);
            }
        }
        private void ButHomePlayerShow_Click(object sender, EventArgs e)
        {
            Common.ShowHidePlayer(home: true, button: butHomePlayerShow, textBox: txtHomePlayerNumber, obs: obs);
            tmrPlayerHome.Interval = Properties.Settings.Default.FlagDisplayDuration;
            tmrPlayerHome.Enabled = true;
            tmrPlayerHome.Start();
        }
        private void ButHomeSafety_Click(object sender, EventArgs e)
        {
            ButHomeSafety_Event();
        }
        public void ButHomeSafety_Event()
        {
            if (txtHomeScore.InvokeRequired)
            {
                txtHomeScore.Invoke(new MethodInvoker(delegate
                {
                    ButHomeSafety_Event();
                }));
            }
            else
            {
                AddScore(home: true, textBox: txtHomeScore, points: Properties.Settings.Default.Safety);
                WriteScore(text: "Safety");
                WriteScoreImage(score: Score.Safety);
            }
        }
        private void ButHomeTimeoutsAdd_Click(object sender, EventArgs e)
        {
            ButHomeTimeoutsAdd_Event();
        }
        public void ButHomeTimeoutsAdd_Event()
        {
            if (txtHomeTimeouts.InvokeRequired)
            {
                txtHomeTimeouts.Invoke(new MethodInvoker(delegate
                {
                    ButHomeTimeoutsAdd_Event();
                }));
            }
            else
            {
                AddTimeout(control: txtHomeTimeouts, timeoutsToAdd: 1);
            }
        }
        private void ButHomeTimeoutsSubtract_Click(object sender, EventArgs e)
        {
            ButHomeTimeoutsSubtract_Event();
        }
        public void ButHomeTimeoutsSubtract_Event()
        {
            if (txtHomeTimeouts.InvokeRequired)
            {
                txtHomeTimeouts.Invoke(new MethodInvoker(delegate
                {
                    ButHomeTimeoutsSubtract_Event();
                }));
            }
            else
            {
                AddTimeout(control: txtHomeTimeouts, timeoutsToAdd: -1);
            }
        }
        private void ButHomeTouchdown_Click(object sender, EventArgs e)
        {
            ButHomeTouchdown_Event();
        }
        public bool ButHomeTouchdown_Event()
        {
            if (txtHomeScore.InvokeRequired)
            {
                txtHomeScore.Invoke(new MethodInvoker(delegate
                {
                    ButHomeTouchdown_Event();
                }));
            }
            else
            {
                AddScore(home: true, textBox: txtHomeScore, points: Properties.Settings.Default.Touchdown);
                WriteScore(text: "Touchdown");
                WriteScoreImage(score: Score.Touchdown);
            }
            return true;
        }
        public void ButNewDefaultPlay_Click(object sender, EventArgs e)
        {
            SetPlayClock(duration: Properties.Settings.Default.DefaultPlayClock, start: true);
        }
        public void ButNewShortPlay_Click(object sender, EventArgs e)
        {
            SetPlayClock(duration: Properties.Settings.Default.ShortPlayClock, start: true);
        }
        public void ButOutputFolder_Click(object sender, EventArgs e)
        {
            DialogResult result = fbdOutput.ShowDialog();
            if (result == DialogResult.OK)
                txtOutputFolder.Text = fbdOutput.SelectedPath;
        }
        public void ButSaveHotKey_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default["HotKeyAwayFieldGoal"] = txtHotKeyAwayFieldGoal.Text;
            Properties.Settings.Default["HotKeyAwayPatConversion"] = txtHotKeyAwayPatConversion.Text;
            Properties.Settings.Default["HotKeyAwayPatKick"] = txtHotKeyAwayPatKick.Text;
            Properties.Settings.Default["HotKeyAwaySafety"] = txtHotKeyAwaySafety.Text;
            Properties.Settings.Default["HotKeyAwayTouchdown"] = txtHotKeyAwayTouchdown.Text;
            Properties.Settings.Default["HotKeyClearClocks"] = txtHotKeyClearClocks.Text;
            Properties.Settings.Default["HotKeyClearPlayClock"] = txtHotKeyClearPlayClock.Text;
            Properties.Settings.Default["HotKeyFirstDown"] = txtHotKeyFirstDown.Text;
            Properties.Settings.Default["HotKeyFlag"] = txtHotKeyFlag.Text;
            Properties.Settings.Default["HotKeyGameAdd10s"] = txtHotKeyGameAdd10s.Text;
            Properties.Settings.Default["HotKeyGameAdd1s"] = txtHotKeyGameAdd1s.Text;
            Properties.Settings.Default["HotKeyGameSubtract10s"] = txtHotKeyGameSubtract10s.Text;
            Properties.Settings.Default["HotKeyGameSubtract1s"] = txtHotKeyGameSubtract1s.Text;
            Properties.Settings.Default["HotKeyHomeFieldGoal"] = txtHotKeyHomeFieldGoal.Text;
            Properties.Settings.Default["HotKeyHomePatConversion"] = txtHotKeyHomePatConversion.Text;
            Properties.Settings.Default["HotKeyHomePatKick"] = txtHotKeyHomePatKick.Text;
            Properties.Settings.Default["HotKeyHomeSafety"] = txtHotKeyHomeSafety.Text;
            Properties.Settings.Default["HotKeyHomeTouchdown"] = txtHotKeyHomeTouchdown.Text;
            Properties.Settings.Default["HotKeyNewDefaultPlayClock"] = txtHotKeyNewDefaultPlayClock.Text;
            Properties.Settings.Default["HotKeyNewShortPlayClock"] = txtHotKeyNewShortPlayClock.Text;
            Properties.Settings.Default["HotKeyNextDown"] = txtHotKeyNextDown.Text;
            Properties.Settings.Default["HotKeyNextPeriod"] = txtHotKeyNextPeriod.Text;
            Properties.Settings.Default["HotKeyPossession"] = txtHotKeyPossession.Text;
            Properties.Settings.Default["HotKeyRedZone"] = txtHotKeyRedZone.Text;
            Properties.Settings.Default["HotKeyStartStopGameClock"] = txtHotKeyStartStopGameClock.Text;
            Properties.Settings.Default["HotKeyStartStopPlayClock"] = txtHotKeyStartStopPlayClock.Text;
            Properties.Settings.Default.Save();
            DialogResult result = MessageBox.Show(text: "Please re-start the application for new Hot Keys to take effect. Restart Now?", caption: "AFS", buttons: MessageBoxButtons.YesNo, icon: MessageBoxIcon.Information);
            if (result == DialogResult.Yes)
            {
                System.Windows.Forms.Application.Restart();
                Environment.Exit(exitCode: 0);
            }
        }
        public void ButSaveSettings_Click(object sender, EventArgs e)
        {
            string errorMessage = String.Empty;
            if (!ValidTime(txtPeriodDuration.Text))
                errorMessage += "Default Period Duration must be in format 00:00 or 0:00.0. ";
            if (!int.TryParse(s: txtDefaultPlayClock.Text, out _))
                errorMessage += "Default Play Clock must be an integer. ";
            if (string.IsNullOrEmpty(txtOutputFolder.Text))
                errorMessage += "Please specify an output folder. ";
            if (!int.TryParse(s: txtRefreshInterval.Text, out int refreshInterval))
                errorMessage += "Refresh Interval must be an integer. ";
            if (!int.TryParse(s: txtFlagDisplayDuration.Text, out int flagDisplayDuration))
                errorMessage += "Flag Display Duration must be an integer. ";
            if (!int.TryParse(s: txtSettingSafety.Text, out int safetyPoints))
                errorMessage += "Safety points must be an integer. ";
            if (!int.TryParse(s: txtSettingPatKick.Text, out int patKickPoints))
                errorMessage += "PAT - Kick points must be an integer. ";
            if (!int.TryParse(s: txtSettingPatConversion.Text, out int patConversionPoints))
                errorMessage += "PAT - Conv points must be an integer. ";
            if (!int.TryParse(s: txtSettingFieldGoal.Text, out int patFieldGoalPoints))
                errorMessage += "Field Goal points must be an integer. ";
            if (!int.TryParse(s: txtSettingTouchdown.Text, out int patTouchdownPoints))
                errorMessage += "Touchdown points must be an integer. ";
            if (string.IsNullOrEmpty(value: errorMessage))
            {
                Properties.Settings.Default["AdvanceQuarter"] = chkAdvanceQuarter.Checked;
                Properties.Settings.Default["AlwaysOnTop"] = chkTop.Checked;
                Properties.Settings.Default["DefaultPeriod"] = txtPeriodDuration.Text;
                Properties.Settings.Default["DefaultPlayClock"] = txtDefaultPlayClock.Text;
                Properties.Settings.Default["Down1"] = txtSettingDown1.Text;
                Properties.Settings.Default["Down2"] = txtSettingDown2.Text;
                Properties.Settings.Default["Down3"] = txtSettingDown3.Text;
                Properties.Settings.Default["Down4"] = txtSettingDown4.Text;
                Properties.Settings.Default["Down4"] = txtSettingDown4.Text;
                Properties.Settings.Default["FieldGoal"] = patFieldGoalPoints;
                Properties.Settings.Default["FlagDisplayDuration"] = flagDisplayDuration;
                Properties.Settings.Default["GoalText"] = txtGoalText.Text;
                Properties.Settings.Default["OutputPath"] = txtOutputFolder.Text;
                Properties.Settings.Default["PatConversion"] = patConversionPoints;
                Properties.Settings.Default["PatKick"] = patKickPoints;
                Properties.Settings.Default["Period1"] = txtSettingPeriod1.Text;
                Properties.Settings.Default["Period2"] = txtSettingPeriod2.Text;
                Properties.Settings.Default["Period3"] = txtSettingPeriod3.Text;
                Properties.Settings.Default["Period4"] = txtSettingPeriod4.Text;
                Properties.Settings.Default["PeriodHalf"] = txtSettingPeriodHalf.Text;
                Properties.Settings.Default["PeriodFinal"] = txtSettingPeriodFinal.Text;
                Properties.Settings.Default["PlayerImageFileType"] = lstPlayerImageFileType.Text;
                Properties.Settings.Default["PossessionChangeFirstDown"] = chkSettingFirstDown.Checked;
                Properties.Settings.Default["RefreshInterval"] = refreshInterval;
                Properties.Settings.Default["Safety"] = safetyPoints;
                Properties.Settings.Default["ShortPlayClock"] = txtShortPlayClock.Text;
                Properties.Settings.Default["SubSecond"] = chkSubSecond.Checked;
                Properties.Settings.Default["TimeoutsPerHalf"] = txtTimeoutsPerHalf.Text;
                Properties.Settings.Default["Touchdown"] = patTouchdownPoints;
                Properties.Settings.Default["ObsWebSocketPort"] = txtWebSocketPort.Text;
                Properties.Settings.Default["ObsWebSocketPassword"] = txtWebSocketPassword.Text;
                Properties.Settings.Default["ObsWebSocketServer"] = txtWebSocketServer.Text;
                Properties.Settings.Default["HttpListenerPort"] = txtHttpListenerPort.Text;
                Properties.Settings.Default.Save();
                tmrFlag.Interval = Properties.Settings.Default.FlagDisplayDuration;
                tmrClockRefresh.Interval = Properties.Settings.Default.RefreshInterval;
                this.TopMost = Properties.Settings.Default.AlwaysOnTop;
                PopulateImageButtonsAway();
                PopulateImageButtonsHome();
                MessageBox.Show(text: "Settings Saved Successfully", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
            }
            else
                MessageBox.Show(text: errorMessage.Trim(), caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Warning);
            InitializeDatabase();
        }
        public void ButSendSupplemental_Click(object sender, EventArgs e)
        {
            rbMessageClear.Checked = false;
            SendSupplemental();
        }
        public void ButStartStopGameClock_Click(object sender, EventArgs e)
        {
            if (gameClockRunning)
            {
                if (!playClockRunning)
                    tmrClockRefresh.Enabled = false;
                butStartStopGameClock.Text = "Start Game Clock";
            }
            else
            {
                if (txtGameClock.Text.Trim() == string.Empty)
                    txtGameClock.Text = Properties.Settings.Default.DefaultPeriod.ToString();

                periodTimeRemaining = TimeRemainingFromTextBox();
                periodClockEnd = DateTime.UtcNow + periodTimeRemaining;
                butStartStopGameClock.Text = "Stop Game Clock";
            }
            gameClockRunning = !gameClockRunning;
            tmrClockRefresh.Enabled = gameClockRunning;
        }
        public void ButStartStopPlayClock_Click(object sender, EventArgs e)
        {
            if (playClockRunning)
            {
                if (!playClockRunning)
                    tmrClockRefresh.Enabled = false;
                butStartStopPlayClock.Text = "Start Play Clock";
            }
            else
            {
                if (!tmrClockRefresh.Enabled)
                    tmrClockRefresh.Enabled = true;
                if (txtPlayClock.Text.Trim().Length == 0)
                    txtPlayClock.Text = Properties.Settings.Default.DefaultPlayClock.ToString();
                playTimeRemaining = new TimeSpan(hours: 0, minutes: 0, seconds: int.Parse(s: txtPlayClock.Text));
                playTimeEnd = DateTime.UtcNow + playTimeRemaining;
                butStartStopPlayClock.Text = "Stop Play Clock";
            }
            playClockRunning = !playClockRunning;
        }
        private void ButSeparatePlayerImageForm_Click(object sender, EventArgs e)
        {
            FrmPlayerImages frmPlayerImages = new();
            frmPlayerImages.Show();
        }
        private static async Task CheckForUpdates()
        {
            /*
            using (var manager = await UpdateManager.GitHubUpdateManager(repoUrl: @"https://github.com/hoga2443/AmericanFootballScoreboard"))
            {
                try
                {
                    var updateInfo = await manager.CheckForUpdate();

                    if (updateInfo.ReleasesToApply.Count > 0)
                    {
                        var versionCount = updateInfo.ReleasesToApply.Count;

                        string versionWord = versionCount > 1 ? "releases" : "release";
                        string message = new StringBuilder().AppendLine($"Your installation is {versionCount} {versionWord} behind.").
                                                          AppendLine("If you choose to update, changes won't take affect until AFS is restarted.").
                                                          AppendLine("Would you like to download and install the update?").
                                                          ToString();

                        DialogResult result = MessageBox.Show(text: message, caption: "AFS", buttons: MessageBoxButtons.YesNo, icon: MessageBoxIcon.Question);
                        if (result != DialogResult.Yes)
                            return;

                        var updateResult = await manager.UpdateApp();
                        result = MessageBox.Show(text: $"New version {updateResult.Version} has been installed and will take effect when AFS is restarted.", 
                            caption: "AFS", 
                            buttons: MessageBoxButtons.OK, 
                            icon: MessageBoxIcon.Information);
                    }
                    else
                        MessageBox.Show(text: "You have the latest version, no update is available!", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
                }
                catch
                {
                    MessageBox.Show(text: "Error checking for updates!", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
                }
            }
            */
        }
        private void ChkAwayPossession_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAwayPossession.Checked)
                AwayPossession_Event();
            else
                _ = CopyFileAsync(sourcePath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "Possession\\NonPossession.png"), destinationPath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "AwayPossession.png"));
        }
        public void AwayPossession_Event()
        {
            if (chkAwayPossession.InvokeRequired)
            {
                chkAwayPossession.Invoke(new MethodInvoker(delegate
                {
                    AwayPossession_Event();
                }));
            }
            else
            {
                if (!chkAwayPossession.Checked)
                    chkAwayPossession.Checked = true;
                chkHomePossession.Checked = false;
                _ = CopyFileAsync(sourcePath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "Possession\\AwayPossession.png"), destinationPath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "AwayPossession.png"));
                if (Properties.Settings.Default.PossessionChangeFirstDown)
                    rbDownOne.Checked = true;
            }
        }
        private void ChkFlag_CheckedChanged(object sender, EventArgs e)
        {
            if (chkFlag.Checked)
                ChkFlagChecked_Event();
            else
                ChkFlagUnchecked_Event();
        }
        public void ChkFlagChecked_Event()
        {
            if (chkFlag.InvokeRequired)
            {
                chkFlag.Invoke(new MethodInvoker(delegate
                {
                    ChkFlagChecked_Event();
                }));
            }
            else
            {
                if (!chkFlag.Checked)
                    chkFlag.Checked = true;
                _ = CopyFileAsync(sourcePath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "Flag\\Flag.png"), destinationPath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "Flag.png"));
                tmrFlag.Interval = Properties.Settings.Default.FlagDisplayDuration;
                tmrFlag.Enabled = true;
                tmrFlag.Start();
            }
        }
        public void ChkFlagUnchecked_Event()
        {
            if (chkFlag.InvokeRequired)
            {
                chkFlag.Invoke(new MethodInvoker(delegate
                {
                    ChkFlagUnchecked_Event();
                }));
            }
            else
            {
                if (chkFlag.Checked)
                    chkFlag.Checked = false;
                _ = CopyFileAsync(sourcePath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "Flag\\NoFlag.png"), destinationPath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "Flag.png"));
                tmrFlag.Stop();
                tmrFlag.Enabled = false;
            }
        }
        private void ChkHomePossession_CheckedChanged(object sender, EventArgs e)
        {
            if (chkHomePossession.Checked)
                HomePossession_Event();
            else
                _ = CopyFileAsync(sourcePath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "Possession\\NonPossession.png"), destinationPath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "HomePossession.png"));
        }
        public void HomePossession_Event()
        {
            if (chkHomePossession.InvokeRequired)
            {
                chkHomePossession.Invoke(new MethodInvoker(delegate
                {
                    HomePossession_Event();
                }));
            }
            else
            {
                if (!chkHomePossession.Checked)
                    chkHomePossession.Checked = true;
                chkAwayPossession.Checked = false;
                _ = CopyFileAsync(sourcePath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "Possession\\HomePossession.png"), destinationPath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "HomePossession.png"));
                if (Properties.Settings.Default.PossessionChangeFirstDown)
                    rbDownOne.Checked = true;
            }
        }
        private void ChkRed_CheckedChanged(object sender, EventArgs e)
        {
            if (chkRed.Checked)
            {
                _ = CopyFileAsync(sourcePath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "RedZone\\RedZone.png"), destinationPath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "RedZone.png"));
                tmrRed.Interval = Properties.Settings.Default.FlagDisplayDuration;
                tmrRed.Enabled = true;
                tmrRed.Start();
            }
            else
            {
                _ = CopyFileAsync(sourcePath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "RedZone\\NoRedZone.png"), destinationPath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "RedZone.png"));
                tmrRed.Stop();
                tmrRed.Enabled = false;
            }
        }
        private void ClearAway()
        {
            txtAwayTimeouts.Text = Properties.Settings.Default.TimeoutsPerHalf;
            txtAwayScore.Text = "0";
            txtAwayTeam.Text = string.Empty;
            txtPeriodAwayFirst.Text = string.Empty;
            txtPeriodAwaySecond.Text = string.Empty;
            txtPeriodAwayThird.Text = string.Empty;
            txtPeriodAwayFourth.Text = string.Empty;
            txtPeriodAwayOT.Text = string.Empty;
            chkAwayPossession.Checked = false;
            _ = Common.WriteFileAsync(file: awayTeamNameFile, content: txtAwayTeam.Text);
        }
        private void ClearClocks()
        {
            tmrClockRefresh.Enabled = false;
            playClockRunning = false;
            txtGameClock.Text = Properties.Settings.Default.DefaultPeriod;
            _ = Common.WriteFileAsync(file: gameClockFile, content: txtGameClock.Text);
            txtPlayClock.Text = Properties.Settings.Default.DefaultPlayClock;
            _ = Common.WriteFileAsync(file: playClockFile, content: txtPlayClock.Text);
        }
        private void ClearDown()
        {
            rbDownBlank.Checked = true;
            rbDownOne.Checked = false;
            rbDownTwo.Checked = false;
            rbDownThree.Checked = false;
            rbDownFour.Checked = false;
            txtDistance.Text = String.Empty;
            txtSpot.Text = String.Empty;
        }
        private void ClearHome()
        {
            txtHomeTimeouts.Text = Properties.Settings.Default.TimeoutsPerHalf;
            txtHomeScore.Text = "0";
            txtHomeTeam.Text = string.Empty;
            txtPeriodHomeFirst.Text = string.Empty;
            txtPeriodHomeSecond.Text = string.Empty;
            txtPeriodHomeThird.Text = string.Empty;
            txtPeriodHomeFourth.Text = string.Empty;
            txtPeriodHomeOT.Text = string.Empty;
            chkHomePossession.Checked = false;
            _ = Common.WriteFileAsync(file: homeTeamNameFile, content: txtHomeTeam.Text);
        }
        private void ClearPeriod()
        {
            rbPeriodFinal.Checked = false;
            rbPeriodFour.Checked = false;
            rbPeriodHalf.Checked = false;
            rbPeriodOne.Checked = false;
            rbPeriodOT.Checked = false;
            rbPeriodThree.Checked = false;
            rbPeriodTwo.Checked = false;
            _ = Common.WriteFileAsync(file: periodFile, content: string.Empty);
            txtPeriodAwayFirst.Text = string.Empty;
            txtPeriodAwaySecond.Text = string.Empty;
            txtPeriodAwayThird.Text = string.Empty;
            txtPeriodAwayFourth.Text = string.Empty;
            txtPeriodAwayOT.Text = string.Empty;
            txtPeriodHomeFirst.Text = string.Empty;
            txtPeriodHomeSecond.Text = string.Empty;
            txtPeriodHomeThird.Text = string.Empty;
            txtPeriodHomeFourth.Text = string.Empty;
            txtPeriodHomeOT.Text = string.Empty;
            currentPeriod = Period.Unknown;
        }
        static async Task CopyFileAsync(string sourcePath, string destinationPath)
        {
            using Stream source = File.Open(path: sourcePath, mode: FileMode.Open);
            using Stream destination = File.Create(path: destinationPath);
            await source.CopyToAsync(destination: destination);
        }
        private static WebServer CreateWebServer(string url)
        {
            return new WebServer(o => o
                .WithUrlPrefix(url)
                .WithMode(HttpListenerMode.EmbedIO))
                .WithWebApi("/api", m => m.WithController<MyApiController>())
                .WithModule(new ActionModule("/", HttpVerbs.Get, ctx =>
                {
                    ctx.Response.ContentType = "text/plain";
                    return ctx.SendStringAsync("Root endpoint", "text/plain", System.Text.Encoding.UTF8);
                }));
        }
        private void DecrementGameClock()
        {
            periodTimeRemaining = periodClockEnd - DateTime.UtcNow;
            if (periodTimeRemaining < oneMinute && Properties.Settings.Default.SubSecond)
                txtGameClock.Text = "0:" + periodTimeRemaining.Seconds.ToString().PadLeft(totalWidth: 2, paddingChar: padZero) + "." + periodTimeRemaining.Milliseconds.ToString()[..1];
            else
                txtGameClock.Text = periodTimeRemaining.Minutes.ToString().PadLeft(totalWidth: 2, paddingChar: padZero) + ":" + periodTimeRemaining.Seconds.ToString().PadLeft(totalWidth: 2, paddingChar: padZero);
            _ = Common.WriteFileAsync(file: gameClockFile, content: txtGameClock.Text);
            if (DateTime.Compare(t1: periodClockEnd, t2: DateTime.UtcNow) <= 0)
                txtGameClock.Text = "0:00.0";
            if (txtGameClock.Text == "0:00.0" || txtGameClock.Text == "00:00")
            {
                gameClockRunning = false;
                butStartStopGameClock.Text = "Start Game Clock";
                if (Properties.Settings.Default.AdvanceQuarter)
                    AdvanceQuarter();
                if (!playClockRunning)
                    tmrClockRefresh.Enabled = false;
            }
        }
        private void DecrementPlayClock()
        {
            playTimeRemaining = playTimeEnd - DateTime.UtcNow;
            txtPlayClock.Text = ((int)playTimeRemaining.TotalSeconds).ToString();
            _ = Common.WriteFileAsync(file: playClockFile, content: txtPlayClock.Text);
            if (DateTime.Compare(t1: playTimeEnd, t2: DateTime.UtcNow) <= 0)
                txtPlayClock.Text = "0";
            if (txtPlayClock.Text == "0")
            {
                playClockRunning = false;
                butStartStopPlayClock.Text = "Start Play Clock";
                if (!gameClockRunning)
                    tmrClockRefresh.Enabled = false;
            }
        }
        private void FirstDown()
        {
            if (rbDownOne.Checked != true)
            {
                rbDownOne.Checked = true;
                txtDistance.Text = "10";
            }
        }
        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            GlobalHotKey.DeRegisterHotKeys();
            obs.Disconnect();
        }
        private void FrmMain_Load(object sender, EventArgs e)
        {
            txtDistance.Click += TxtDistance_OnFocus;
            txtDistance.GotFocus += TxtDistance_OnFocus;
            txtSpot.Click += TxtSpot_OnFocus;
            txtSpot.GotFocus += TxtSpot_OnFocus;
        }
        private async Task HttpListenAsync()
        {
            while (true)
            {
                try
                {
                    // Wait for an incoming request
                    var context = await Listener.GetContextAsync();
                    Console.WriteLine("Client connected");
                    // Process the request in a separate task
                    _ = Task.Run(() => ProcessHttpRequest(context));
                }
                catch (HttpListenerException ex) when (ex.ErrorCode == 995)
                {
                    // Handle listener shutdown gracefully
                    // Console.WriteLine("Listener stopped.");
                    break;
                }
            }
        }
        private void HomeTimeoutsSubtract()
        {
            AddTimeout(control: txtHomeTimeouts, timeoutsToAdd: -1);
        }
        private void InitializeDown()
        {
            string currentFile = Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: downFile);
            if (!File.Exists(path: currentFile))
            {
                FileStream fs = File.Create(path: currentFile);
                fs.Close();
            }
            string down = File.ReadAllText(path: currentFile);
            if (down == Properties.Settings.Default.Down1)
                rbDownOne.Checked = true;
            else if (down == Properties.Settings.Default.Down2)
                rbDownTwo.Checked = true;
            else if (down == Properties.Settings.Default.Down3)
                rbDownThree.Checked = true;
            else if (down == Properties.Settings.Default.Down4)
                rbDownFour.Checked = true;
            else
                rbDownBlank.Checked = true;
        }
        private void InitializeQuarter()
        {
            string currentFile = Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: periodFile);
            if (!File.Exists(path: currentFile))
            {
                FileStream fs = File.Create(path: currentFile);
                fs.Close();
            }
            string quarter = File.ReadAllText(path: currentFile);
            if (quarter == Properties.Settings.Default.Period1)
                rbPeriodOne.Checked = true;
            else if (quarter == Properties.Settings.Default.Period2)
                rbPeriodTwo.Checked = true;
            else if (quarter == Properties.Settings.Default.PeriodHalf)
                rbPeriodHalf.Checked = true;
            else if (quarter == Properties.Settings.Default.Period3)
                rbPeriodThree.Checked = true;
            else if (quarter == Properties.Settings.Default.Period4)
                rbPeriodFour.Checked = true;
            else if (quarter == Properties.Settings.Default.PeriodFinal)
                rbPeriodFinal.Checked = true;
            /*
            else
            {
                rbPeriodOT.Checked = true;
                txtPeriodOT.Text = quarter;
            }
            */
        }
        private static void InitializeTextBox(TextBox textBox, string fileName)
        {
            string currentFile = Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: fileName);
            if (!File.Exists(path: currentFile))
            {
                FileStream fs = File.Create(path: currentFile);
                fs.Close();
            }
            textBox.Text = File.ReadAllText(path: currentFile);
        }
        private static void InitializeTextBox(TextBox textBox, string fileName, string defaultValue)
        {
            string currentFile = Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: fileName);
            if (!File.Exists(path: currentFile))
            {
                FileStream fs = File.Create(path: currentFile);
                fs.Close();
                textBox.Text = defaultValue;
            }
            else
                textBox.Text = File.ReadAllText(path: currentFile);
        }
        private void InitializeDatabase()
        {
            string sqLiteDatabase = Path.Combine(txtOutputFolder.Text, "AmericanFootballScoreboard.sqlite3");
            if (!File.Exists(sqLiteDatabase))
            {
                SQLiteConnection.CreateFile(sqLiteDatabase);
                using SQLiteConnection sqLiteConnection = new(@"Data Source=" + sqLiteDatabase + ";Version=3;");
                sqLiteConnection.Open();
                string sql = "SELECT name FROM sqlite_master WHERE name='players'";
                SQLiteCommand sQLiteCommand = new(sql, sqLiteConnection);
                var name = sQLiteCommand.ExecuteScalar();
                if (name != null && name.ToString() == "players")
                {
                    sql = "CREATE TABLE \"players\" ( \"Home\" INTEGER NOT NULL DEFAULT 0, \"Number\" INTEGER, \"Name\" TEXT, \"Position\" TEXT, \"Height\" TEXT, \"Weight\" INTEGER, \"Year\" TEXT, \"Hometown\" TEXT);";
                    sQLiteCommand = new(sql, sqLiteConnection);
                    sQLiteCommand.ExecuteNonQuery();
                }
            }
        }
        private void InitializeUI()
        {
            InitializeTextBox(textBox: txtHomeTeam, fileName: homeTeamNameFile);
            InitializeTextBox(textBox: txtHomeScore, fileName: homeTeamScoreFile, defaultValue: "0");
            InitializeTextBox(textBox: txtHomeTimeouts, fileName: homeTimeoutsRemainingFile);
            InitializeTextBox(textBox: txtAwayTeam, fileName: awayTeamNameFile);
            InitializeTextBox(textBox: txtAwayScore, fileName: awayTeamScoreFile, defaultValue: "0");
            InitializeTextBox(textBox: txtAwayTimeouts, fileName: awayTimeoutsRemainingFile);
            InitializeTextBox(textBox: txtGameClock, fileName: gameClockFile, defaultValue: "00:00");
            InitializeTextBox(textBox: txtPlayClock, fileName: playClockFile, defaultValue: "0");
            InitializeTextBox(textBox: txtDistance, fileName: distanceFile);
            InitializeTextBox(textBox: txtSpot, fileName: spotFile);
            InitializeTextBox(textBox: txtPeriodAwayFirst, fileName: awayPeriodScoreFirst);
            InitializeTextBox(textBox: txtPeriodAwaySecond, fileName: awayPeriodScoreSecond);
            InitializeTextBox(textBox: txtPeriodAwayThird, fileName: awayPeriodScoreThird);
            InitializeTextBox(textBox: txtPeriodAwayFourth, fileName: awayPeriodScoreFourth);
            InitializeTextBox(textBox: txtPeriodAwayOT, fileName: awayPeriodScoreOT);
            InitializeTextBox(textBox: txtPeriodHomeFirst, fileName: homePeriodScoreFirst);
            InitializeTextBox(textBox: txtPeriodHomeSecond, fileName: homePeriodScoreSecond);
            InitializeTextBox(textBox: txtPeriodHomeThird, fileName: homePeriodScoreThird);
            InitializeTextBox(textBox: txtPeriodHomeFourth, fileName: homePeriodScoreFourth);
            InitializeTextBox(textBox: txtPeriodHomeOT, fileName: homePeriodScoreOT);

            // chkHomePossession.Checked = true;
            InitializeDown();
            InitializeQuarter();
        }
        private void LoadApplicationSettings()
        {
            txtPeriodDuration.Text = Properties.Settings.Default.DefaultPeriod;
            txtDefaultPlayClock.Text = Properties.Settings.Default.DefaultPlayClock;
            txtGoalText.Text = Properties.Settings.Default.GoalText;
            txtOutputFolder.Text = Properties.Settings.Default.OutputPath;
            txtShortPlayClock.Text = Properties.Settings.Default.ShortPlayClock;
            txtTimeoutsPerHalf.Text = Properties.Settings.Default.TimeoutsPerHalf;
            tmrClockRefresh.Interval = Properties.Settings.Default.RefreshInterval;
            tmrFlag.Interval = Properties.Settings.Default.FlagDisplayDuration;
            chkAdvanceQuarter.Checked = Properties.Settings.Default.AdvanceQuarter;
            txtRefreshInterval.Text = Properties.Settings.Default.RefreshInterval.ToString();
            txtFlagDisplayDuration.Text = Properties.Settings.Default.FlagDisplayDuration.ToString();
            txtSettingPeriod1.Text = Properties.Settings.Default.Period1;
            txtSettingPeriod2.Text = Properties.Settings.Default.Period2;
            txtSettingPeriod3.Text = Properties.Settings.Default.Period3;
            txtSettingPeriod4.Text = Properties.Settings.Default.Period4;
            txtSettingPeriodHalf.Text = Properties.Settings.Default.PeriodHalf;
            txtSettingPeriodFinal.Text = Properties.Settings.Default.PeriodFinal;
            txtSettingDown1.Text = Properties.Settings.Default.Down1;
            txtSettingDown2.Text = Properties.Settings.Default.Down2;
            txtSettingDown3.Text = Properties.Settings.Default.Down3;
            txtSettingDown4.Text = Properties.Settings.Default.Down4;
            chkTop.Checked = Properties.Settings.Default.AlwaysOnTop;
            chkSubSecond.Checked = Properties.Settings.Default.SubSecond;
            txtSettingSafety.Text = Properties.Settings.Default.Safety.ToString();
            txtSettingPatKick.Text = Properties.Settings.Default.PatKick.ToString();
            txtSettingPatConversion.Text = Properties.Settings.Default.PatConversion.ToString();
            txtSettingFieldGoal.Text = Properties.Settings.Default.FieldGoal.ToString();
            txtSettingTouchdown.Text = Properties.Settings.Default.Touchdown.ToString();
            lstPlayerImageFileType.Text = Properties.Settings.Default.PlayerImageFileType.ToString();
            chkSettingFirstDown.Checked = Properties.Settings.Default.PossessionChangeFirstDown;
            txtWebSocketPort.Text = Properties.Settings.Default.ObsWebSocketPort;
            txtWebSocketPassword.Text = Properties.Settings.Default.ObsWebSocketPassword;
            txtWebSocketServer.Text = Properties.Settings.Default.ObsWebSocketServer;
            txtHttpListenerPort.Text = Properties.Settings.Default.HttpListenerPort;
            this.TopMost = Properties.Settings.Default.AlwaysOnTop;
        }
        private void LoadHotKeySettings()
        {
            txtHotKeyAwayFieldGoal.Text = Properties.Settings.Default.HotKeyAwayFieldGoal;
            txtHotKeyAwayPatConversion.Text = Properties.Settings.Default.HotKeyAwayPatConversion;
            txtHotKeyAwayPatKick.Text = Properties.Settings.Default.HotKeyAwayPatKick;
            txtHotKeyAwaySafety.Text = Properties.Settings.Default.HotKeyAwaySafety;
            txtHotKeyAwayTimeout.Text = Properties.Settings.Default.HotKeyAwayTimeout;
            txtHotKeyAwayTouchdown.Text = Properties.Settings.Default.HotKeyAwayTouchdown;
            txtHotKeyClearClocks.Text = Properties.Settings.Default.HotKeyClearClocks;
            txtHotKeyClearPlayClock.Text = Properties.Settings.Default.HotKeyClearPlayClock;
            txtHotKeyFirstDown.Text = Properties.Settings.Default.HotKeyFirstDown;
            txtHotKeyFlag.Text = Properties.Settings.Default.HotKeyFlag;
            txtHotKeyGameAdd10s.Text = Properties.Settings.Default.HotKeyGameAdd10s;
            txtHotKeyGameAdd1s.Text = Properties.Settings.Default.HotKeyGameAdd1s;
            txtHotKeyGameSubtract10s.Text = Properties.Settings.Default.HotKeyGameSubtract10s;
            txtHotKeyGameSubtract1s.Text = Properties.Settings.Default.HotKeyGameSubtract1s;
            txtHotKeyHomeFieldGoal.Text = Properties.Settings.Default.HotKeyHomeFieldGoal;
            txtHotKeyHomePatConversion.Text = Properties.Settings.Default.HotKeyHomePatConversion;
            txtHotKeyHomePatKick.Text = Properties.Settings.Default.HotKeyHomePatKick;
            txtHotKeyHomeTouchdown.Text = Properties.Settings.Default.HotKeyHomeTouchdown;
            txtHotKeyHomeSafety.Text = Properties.Settings.Default.HotKeyHomeSafety;
            txtHotKeyHomeTimeout.Text = Properties.Settings.Default.HotKeyHomeTimeout;
            txtHotKeyNewDefaultPlayClock.Text = Properties.Settings.Default.HotKeyNewDefaultPlayClock;
            txtHotKeyNewShortPlayClock.Text = Properties.Settings.Default.HotKeyNewShortPlayClock;
            txtHotKeyNextDown.Text = Properties.Settings.Default.HotKeyNextDown;
            txtHotKeyNextPeriod.Text = Properties.Settings.Default.HotKeyNextPeriod;
            txtHotKeyPossession.Text = Properties.Settings.Default.HotKeyPossession;
            txtHotKeyRedZone.Text = Properties.Settings.Default.HotKeyRedZone;
            txtHotKeyStartStopGameClock.Text = Properties.Settings.Default.HotKeyStartStopGameClock;
            txtHotKeyStartStopPlayClock.Text = Properties.Settings.Default.HotKeyStartStopPlayClock;
        }
        private void LoadSettings()
        {
            LoadApplicationSettings();
            LoadHotKeySettings();
        }
        public void NextDown()
        {
            if (rbDownOne.InvokeRequired)
            {
                rbDownOne.Invoke(new MethodInvoker(delegate
                {
                    NextDown();
                }));
                return;
            }
            else
            {
                if (rbDownOne.Checked == true)
                    rbDownTwo.Checked = true;
                else if (rbDownTwo.Checked == true)
                    rbDownThree.Checked = true;
                else if (rbDownThree.Checked == true)
                    rbDownFour.Checked = true;
                else
                {
                    rbDownOne.Checked = true;
                    txtDistance.Text = "10";
                }
            }
        }
        public void NextPeriod()
        {
            if (rbPeriodOne.InvokeRequired)
            {
                rbPeriodOne.Invoke(new MethodInvoker(delegate
                {
                    NextPeriod();
                }));
                return;
            }
            else
            {
                if (rbPeriodOne.Checked == true)
                    rbPeriodTwo.Checked = true;
                else if (rbPeriodTwo.Checked == true)
                    rbPeriodHalf.Checked = true;
                else if (rbPeriodHalf.Checked == true)
                    rbPeriodThree.Checked = true;
                else if (rbPeriodThree.Checked == true)
                    rbPeriodFour.Checked = true;
                else if (rbPeriodFour.Checked == true)
                    rbPeriodFinal.Checked = true;
                else if (rbPeriodOT.Checked == true)
                    rbPeriodFinal.Checked = true;
                else
                {
                    rbPeriodOne.Checked = true;
                    txtDistance.Text = "10";
                }
            }
        }
        private void ObsConnect()
        {
            if (!obs.IsConnected)
            {
                System.Threading.Tasks.Task.Run(() =>
                {
                    try
                    {
                        obs.ConnectAsync("ws://127.0.0.1:" + Properties.Settings.Default.ObsWebSocketPort, Properties.Settings.Default.ObsWebSocketPassword);
                    }
                    catch (Exception ex)
                    {
                        BeginInvoke((MethodInvoker)delegate
                        {
                            MessageBox.Show("Connect failed : " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        });
                    }
                });
            }
            else
            {
                obs.Disconnect();
            }
        }
        private void OnConnect(object sender, EventArgs e)
        {
            _ = BeginInvoke((MethodInvoker)(() =>
            {
                //
            }));
        }
        public void PopulateImageButtonsAway()
        {
            PopulateImageButtons("AwayPlayers", gbAwayPlayers, "butAway", ShowAwayPlayer);
        }
        public void PopulateImageButtonsHome()
        {
            PopulateImageButtons("HomePlayers", gbHomePlayers, "butHome", ShowHomePlayer);
        }
        private static void PopulateImageButtons(string path, GroupBox groupBox, string buttonPrefix, EventHandler eventName)
        {
            string sqLiteDatabase = Path.Combine(Properties.Settings.Default["OutputPath"].ToString(), "AmericanFootballScoreboard.sqlite3");
            string playerImageFileType = Properties.Settings.Default["PlayerImageFileType"].ToString().ToUpper();
            groupBox.Controls.Clear();
            string sourcePath = Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: path);
            if (Directory.Exists(sourcePath))
            {
                string[] directoryContents = Directory.GetFiles(sourcePath);
                List<int> numbers = [];
                foreach (string file in directoryContents)
                {
                    if (file.ToUpper().EndsWith(playerImageFileType))
                    {
                        string playerNumber = file.Substring(file.LastIndexOf('\\') + 1, file.IndexOf('.') - file.LastIndexOf('\\') - 1);
                        if (int.TryParse(playerNumber, out int number))
                        {
                            numbers.Add(number);
                        }
                    }
                }
                using SQLiteConnection sqLiteConnection = new(@"Data Source=" + sqLiteDatabase + ";Version=3;");
                sqLiteConnection.Open();
                string sql;
                if (buttonPrefix == "butHome")
                    sql = "SELECT Number, Name FROM players WHERE Home = 1";
                else
                    sql = "SELECT Number, Name FROM players WHERE Home = 0";
                using (SQLiteCommand sQLiteCommand = new(sql, sqLiteConnection))
                {
                    using SQLiteDataReader reader = sQLiteCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        int number = reader.GetInt32(0);
                        if (!numbers.Contains(number))
                            numbers.Add(number);
                    }
                }
                numbers.Sort((x, y) => x.CompareTo(y));
                int buttonRow = 0;
                int buttonColumn = 0;
                foreach (var number in numbers)
                {
                    if (buttonRow % 18 == 0 && buttonRow > 0)
                    {
                        buttonRow = 0;
                        buttonColumn++;
                    }
                    Button button = new()
                    {
                        Name = buttonPrefix + number.ToString(),
                        Text = number.ToString(),
                        Top = 20 + buttonRow * 22,
                        Left = 10 + buttonColumn * 80,
                        Visible = true
                    };
                    button.Click += eventName;
                    groupBox.Controls.Add(button);
                    buttonRow++;
                }
            }
        }
        private async Task ProcessHttpRequest(HttpListenerContext context)
        {
            try
            {
                // Get the data from the HTTP stream
                var body = await new StreamReader(context.Request.InputStream).ReadToEndAsync();
                HttpListenerRequest request = context.Request;
                if (request.RawUrl.StartsWith("/AwayFieldgoal"))
                    ButAwayFieldGoal_Event();
                else if (request.RawUrl.StartsWith("/AwayPatKick"))
                    ButAwayPatKick_Event();
                else if (request.RawUrl.StartsWith("/AwayPatConversion"))
                    ButAwayPatConversion_Event();
                else if (request.RawUrl.StartsWith("/AwayPossession"))
                    AwayPossession_Event();
                else if (request.RawUrl.StartsWith("/AwaySafety"))
                    ButAwaySafety_Event();
                else if (request.RawUrl.StartsWith("/AwayTouchdown"))
                    ButAwayTouchdown_Event();
                else if (request.RawUrl.StartsWith("/AwayTimeoutAdd"))
                    ButAwayTimeoutsAdd_Event();
                else if (request.RawUrl.StartsWith("/AwayTimeoutSubtract"))
                    ButAwayTimeoutsSubtract_Event();
                else if (request.RawUrl.StartsWith("/DownBlank"))
                    RbDownBlank_Event();
                else if (request.RawUrl.StartsWith("/DownOne"))
                    RbDownOne_Event();
                else if (request.RawUrl.StartsWith("/DownTwo"))
                    RbDownTwo_Event();
                else if (request.RawUrl.StartsWith("/DownThree"))
                    RbDownThree_Event();
                else if (request.RawUrl.StartsWith("/DownFour"))
                    RbDownFour_Event();
                else if (request.RawUrl.StartsWith("/DownNext"))
                    NextDown();
                else if (request.RawUrl.StartsWith("/Flag"))
                    ChkFlagChecked_Event();
                else if (request.RawUrl.StartsWith("/HomeFieldgoal"))
                    ButHomeFieldGoal_Event();
                else if (request.RawUrl.StartsWith("/HomePatKick"))
                    ButHomePatKick_Event();
                else if (request.RawUrl.StartsWith("/HomePatConversion"))
                    ButHomePatConversion_Event();
                else if (request.RawUrl.StartsWith("/HomePossession"))
                    HomePossession_Event();
                else if (request.RawUrl.StartsWith("/HomeSafety"))
                    ButHomeSafety_Event();
                else if (request.RawUrl.StartsWith("/HomeTouchdown"))
                    ButHomeTouchdown_Event();
                else if (request.RawUrl.StartsWith("/HomeTimeoutAdd"))
                    ButHomeTimeoutsAdd_Event();
                else if (request.RawUrl.StartsWith("/HomeTimeoutSubtract"))
                    ButHomeTimeoutsSubtract_Event();
                else if (request.RawUrl.StartsWith("/PeriodOne"))
                    RbPeriodOne_Event();
                else if (request.RawUrl.StartsWith("/PeriodTwo"))
                    RbPeriodTwo_Event();
                else if (request.RawUrl.StartsWith("/PeriodHalf"))
                    RbPeriodHalf_Event();
                else if (request.RawUrl.StartsWith("/PeriodThree"))
                    RbPeriodThree_Event();
                else if (request.RawUrl.StartsWith("/PeriodFour"))
                    RbPeriodFour_Event();
                else if (request.RawUrl.StartsWith("/PeriodOT"))
                    RbPeriodOT_Event();
                else if (request.RawUrl.StartsWith("/PeriodFinal"))
                    RbPeriodFinal_Event();
                else if (request.RawUrl.StartsWith("/PeriodNext"))
                    NextPeriod();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing request: {ex.Message}");
            }
            finally
            {
                // Ensure the response is closed
                context.Response.OutputStream.Close();
                context.Response.Close();
            }
        }
        private void RbDownBlank_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDownBlank.Checked)
                RbDownBlank_Event();
        }
        public void RbDownBlank_Event()
        {
            if (rbDownBlank.InvokeRequired)
            {
                rbDownBlank.Invoke(new MethodInvoker(delegate
                {
                    RbDownBlank_Event();
                }));
            }
            else
            {
                _ = Common.WriteFileAsync(file: downFile, content: string.Empty);
                txtDistance.Text = string.Empty;
            }
        }
        private void RbDownFour_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDownFour.Checked)
                RbDownFour_Event();
        }
        public void RbDownFour_Event()
        {
            if (txtDistance.InvokeRequired)
            {
                txtDistance.Invoke(new MethodInvoker(delegate
                {
                    RbDownFour_Event();
                }));
            }
            else
            {
                if (!rbDownFour.Checked)
                    rbDownFour.Checked = true;
                _ = Common.WriteFileAsync(file: downFile, content: Properties.Settings.Default.Down4);
            }
            UpdateDownAndDistance();
        }
        private void RbDownOne_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDownOne.Checked)
            {
                RbDownOne_Event();
            }
        }
        public void RbDownOne_Event()
        {
            if (txtDistance.InvokeRequired)
            {
                txtDistance.Invoke(new MethodInvoker(delegate
                {
                    RbDownOne_Event();
                }));
            }
            else
            {
                if (!rbDownOne.Checked)
                    rbDownOne.Checked = true;
                _ = Common.WriteFileAsync(file: downFile, content: Properties.Settings.Default.Down1);
                txtDistance.Text = "10";
            }
            UpdateDownAndDistance();
        }
        private void RbDownThree_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDownThree.Checked)
                RbDownThree_Event();
        }
        public void RbDownThree_Event()
        {
            if (txtDistance.InvokeRequired)
            {
                txtDistance.Invoke(new MethodInvoker(delegate
                {
                    RbDownThree_Event();
                }));
            }
            else
            {
                if (!rbDownThree.Checked)
                    rbDownThree.Checked = true;
                _ = Common.WriteFileAsync(file: downFile, content: Properties.Settings.Default.Down3);
            }
            UpdateDownAndDistance();
        }
        private void RbDownTwo_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDownTwo.Checked)
                RbDownTwo_Event();
        }
        public void RbDownTwo_Event()
        {
            if (txtDistance.InvokeRequired)
            {
                txtDistance.Invoke(new MethodInvoker(delegate
                {
                    RbDownTwo_Event();
                }));
            }
            else
            {
                if (!rbDownTwo.Checked)
                    rbDownTwo.Checked = true;
                _ = Common.WriteFileAsync(file: downFile, content: Properties.Settings.Default.Down2);
            }
            UpdateDownAndDistance();
        }
        private void RbMessageClear_CheckedChanged(object sender, EventArgs e)
        {
            if (rbMessageClear.Checked)
            {
                txtSupplemental.Text = string.Empty;
                SendSupplemental();
            }
        }
        private void RbMessageInjury_CheckedChanged(object sender, EventArgs e)
        {
            if (rbMessageInjury.Checked)
            {
                txtSupplemental.Text = rbMessageInjury.Text;
                SendSupplemental();
            }
        }
        private void RbMessageWeather_CheckedChanged(object sender, EventArgs e)
        {
            if (rbMessageWeather.Checked)
            {
                txtSupplemental.Text = rbMessageWeather.Text;
                SendSupplemental();
            }
        }
        private void RbPenalty_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            if (rb.Checked)
            {
                WritePenaltyToFile(rb);
                tmrFlag.Enabled = true;
                tmrFlag.Start();
            }
        }
        private void RbPeriodFinal_CheckedChanged(object sender, EventArgs e)
        {
            if (rbPeriodFinal.Checked)
                RbPeriodFinal_Event();
        }
        public void RbPeriodFinal_Event()
        {
            if (rbPeriodFinal.InvokeRequired)
            {
                rbPeriodFinal.Invoke(new MethodInvoker(delegate
                {
                    RbPeriodFinal_Event();
                }));
            }
            else
            {
                if (!rbPeriodFinal.Checked)
                    rbPeriodFinal.Checked = true;
                rbDownBlank.Checked = true;
                txtAwayTimeouts.Text = "0";
                txtHomeTimeouts.Text = "0";
                txtDistance.Text = string.Empty;
                txtSpot.Text = string.Empty;
                chkAwayPossession.Checked = false;
                chkHomePossession.Checked = false;
                _ = Common.WriteFileAsync(file: periodFile, content: Properties.Settings.Default.PeriodFinal);
                currentPeriod = Period.Final;
                if (string.IsNullOrEmpty(txtPeriodAwayFourth.Text))
                    txtPeriodAwayFourth.Text = "0";
                if (string.IsNullOrEmpty(txtPeriodHomeFourth.Text))
                    txtPeriodHomeFourth.Text = "0";
                if (string.IsNullOrEmpty(txtPeriodAwayOT.Text))
                    txtPeriodAwayOT.Text = "0";
                if (string.IsNullOrEmpty(txtPeriodHomeOT.Text))
                    txtPeriodHomeOT.Text = "0";
                txtGameClock.Text = string.Empty;
                tmrClockRefresh.Enabled = false;
                playClockRunning = false;
                gameClockRunning = false;
            }
        }
        private void RbPeriodFour_CheckedChanged(object sender, EventArgs e)
        {
            if (rbPeriodFour.Checked)
                RbPeriodFour_Event();
        }
        public void RbPeriodFour_Event()
        {
            if (rbPeriodFour.InvokeRequired)
            {
                rbPeriodFour.Invoke(new MethodInvoker(delegate
                {
                    RbPeriodFour_Event();
                }));
            }
            else
            {
                if (!rbPeriodFour.Checked)
                    rbPeriodFour.Checked = true;
                _ = Common.WriteFileAsync(file: periodFile, content: Properties.Settings.Default.Period4);
                currentPeriod = Period.Four;
                if (string.IsNullOrEmpty(txtPeriodAwayThird.Text))
                    txtPeriodAwayThird.Text = "0";
                if (string.IsNullOrEmpty(txtPeriodHomeThird.Text))
                    txtPeriodHomeThird.Text = "0";
                txtGameClock.Text = Properties.Settings.Default.DefaultPeriod;
                tmrClockRefresh.Enabled = false;
                playClockRunning = false;
                gameClockRunning = false;
            }
        }
        private void RbPeriodHalf_CheckedChanged(object sender, EventArgs e)
        {
            if (rbPeriodHalf.Checked)
                RbPeriodHalf_Event();
        }
        public void RbPeriodHalf_Event()
        {
            if (rbPeriodHalf.InvokeRequired)
            {
                rbPeriodHalf.Invoke(new MethodInvoker(delegate
                {
                    RbPeriodHalf_Event();
                }));
            }
            else
            {
                if (!rbPeriodHalf.Checked)
                    rbPeriodHalf.Checked = true;
                txtHomeTimeouts.Text = "0";
                txtAwayTimeouts.Text = "0";
                rbDownBlank.Checked = true;
                txtDistance.Text = string.Empty;
                txtSpot.Text = string.Empty;
                chkAwayPossession.Checked = false;
                chkHomePossession.Checked = false;
                _ = Common.WriteFileAsync(file: periodFile, content: Properties.Settings.Default.PeriodHalf);
                currentPeriod = Period.Half;
                if (string.IsNullOrEmpty(txtPeriodAwaySecond.Text))
                    txtPeriodAwaySecond.Text = "0";
                if (string.IsNullOrEmpty(txtPeriodHomeSecond.Text))
                    txtPeriodHomeSecond.Text = "0";
                txtGameClock.Text = string.Empty;
                tmrClockRefresh.Enabled = false;
                playClockRunning = false;
                gameClockRunning = false;
            }
        }
        private void RbPeriodOne_CheckedChanged(object sender, EventArgs e)
        {
            if (rbPeriodOne.Checked)
                RbPeriodOne_Event();
        }
        public void RbPeriodOne_Event()
        {
            if (rbPeriodOne.InvokeRequired)
            {
                rbPeriodOne.Invoke(new MethodInvoker(delegate
                {
                    RbPeriodOne_Event();
                }));
            }
            else
            {
                if (!rbPeriodOne.Checked)
                    rbPeriodOne.Checked = true;
                txtAwayTimeouts.Text = Properties.Settings.Default.TimeoutsPerHalf;
                txtHomeTimeouts.Text = Properties.Settings.Default.TimeoutsPerHalf;
                _ = Common.WriteFileAsync(file: periodFile, content: Properties.Settings.Default.Period1);
                currentPeriod = Period.One;
                txtGameClock.Text = Properties.Settings.Default.DefaultPeriod;
                tmrClockRefresh.Enabled = false;
                playClockRunning = false;
                gameClockRunning = false;
            }
        }
        private void RbPeriodOT_CheckedChanged(object sender, EventArgs e)
        {
            if (rbPeriodFour.Checked)
            {
                if (string.IsNullOrEmpty(txtPeriodAwayThird.Text))
                    txtPeriodAwayFirst.Text = "0";
                if (string.IsNullOrEmpty(txtPeriodHomeThird.Text))
                    txtPeriodHomeFirst.Text = "0";
            }
            if (rbPeriodOT.Checked)
            {
                RbPeriodOT_Event();
            }
        }
        public void RbPeriodOT_Event()
        {
            if (rbPeriodOT.InvokeRequired)
            {
                rbPeriodOT.Invoke(new MethodInvoker(delegate
                {
                    RbPeriodOT_Event();
                }));
            }
            else
            {
                if (!rbPeriodOT.Checked)
                    rbPeriodOT.Checked = true;
                if (string.IsNullOrEmpty(txtPeriodAwayFourth.Text))
                    txtPeriodAwayFourth.Text = "0";
                if (string.IsNullOrEmpty(txtPeriodHomeFourth.Text))
                    txtPeriodHomeFourth.Text = "0";
                chkAwayPossession.Checked = false;
                chkHomePossession.Checked = false;
                txtAwayTimeouts.Text = "1";
                txtHomeTimeouts.Text = "1";
                _ = Common.WriteFileAsync(file: periodFile, content: txtPeriodOT.Text);
                currentPeriod = Period.OT;
                if (string.IsNullOrEmpty(txtPeriodAwayFourth.Text))
                    txtPeriodAwayFirst.Text = "0";
                if (string.IsNullOrEmpty(txtPeriodHomeFourth.Text))
                    txtPeriodHomeFirst.Text = "0";
                txtGameClock.Text = Properties.Settings.Default.DefaultPeriod;
                tmrClockRefresh.Enabled = false;
                playClockRunning = false;
                gameClockRunning = false;

            }
        }
        private void RbPeriodThree_CheckedChanged(object sender, EventArgs e)
        {
            if (rbPeriodThree.Checked)
                RbPeriodThree_Event();
        }
        public void RbPeriodThree_Event()
        {
            if (rbPeriodThree.InvokeRequired)
            {
                rbPeriodThree.Invoke(new MethodInvoker(delegate
                {
                    RbPeriodThree_Event();
                }));
            }
            else
            {
                if (!rbPeriodThree.Checked)
                    rbPeriodThree.Checked = true;
                txtAwayTimeouts.Text = Properties.Settings.Default.TimeoutsPerHalf;
                txtHomeTimeouts.Text = Properties.Settings.Default.TimeoutsPerHalf;
                chkAwayPossession.Checked = false;
                chkHomePossession.Checked = false;
                _ = Common.WriteFileAsync(file: periodFile, content: Properties.Settings.Default.Period3);
                currentPeriod = Period.Three;
                if (string.IsNullOrEmpty(txtPeriodAwaySecond.Text))
                    txtPeriodAwaySecond.Text = "0";
                if (string.IsNullOrEmpty(txtPeriodHomeSecond.Text))
                    txtPeriodHomeSecond.Text = "0";
                txtGameClock.Text = Properties.Settings.Default.DefaultPeriod;
                tmrClockRefresh.Enabled = false;
                playClockRunning = false;
                gameClockRunning = false;
            }
        }
        private void RbPeriodTwo_CheckedChanged(object sender, EventArgs e)
        {
            if (rbPeriodTwo.Checked)
                RbPeriodTwo_Event();
        }
        public void RbPeriodTwo_Event()
        {
            if (rbPeriodTwo.InvokeRequired)
            {
                rbPeriodTwo.Invoke(new MethodInvoker(delegate
                {
                    RbPeriodTwo_Event();
                }));
            }
            else
            {
                if (!rbPeriodTwo.Checked)
                    rbPeriodTwo.Checked = true;
                _ = Common.WriteFileAsync(file: periodFile, content: Properties.Settings.Default.Period2);
                currentPeriod = Period.Two;
                if (string.IsNullOrEmpty(txtPeriodAwayFirst.Text))
                    txtPeriodAwayFirst.Text = "0";
                if (string.IsNullOrEmpty(txtPeriodHomeFirst.Text))
                    txtPeriodHomeFirst.Text = "0";
                txtGameClock.Text = Properties.Settings.Default.DefaultPeriod;
            }
        }
        private void RegisterHotKeys()
        {
            if (!GlobalHotKey.RegisterHotKey(aKeyGestureString: Properties.Settings.Default.HotKeyAwayTimeout, aAction: () => AwayTimeoutsSubtract()))
                MessageBox.Show(text: "Unable to register Hot Key for Away Timeout!", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
            if (!GlobalHotKey.RegisterHotKey(aKeyGestureString: Properties.Settings.Default.HotKeyStartStopGameClock, aAction: () => butStartStopGameClock.PerformClick()))
                MessageBox.Show(text: "Unable to register Hot Key for Start/Stop Game Clock!", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
            if (!GlobalHotKey.RegisterHotKey(aKeyGestureString: Properties.Settings.Default.HotKeyStartStopPlayClock, aAction: () => butStartStopPlayClock.PerformClick()))
                MessageBox.Show(text: "Unable to register Hot Key for Start/Stop Play Clock!", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
            if (!GlobalHotKey.RegisterHotKey(aKeyGestureString: Properties.Settings.Default.HotKeyNewDefaultPlayClock, aAction: () => butNewDefaultPlay.PerformClick()))
                MessageBox.Show(text: "Unable to register Hot Key for New Default Play Clock!", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
            if (!GlobalHotKey.RegisterHotKey(aKeyGestureString: Properties.Settings.Default.HotKeyNewShortPlayClock, aAction: () => butNewShortPlay.PerformClick()))
                MessageBox.Show(text: "Unable to register Hot Key for New Short Play Clock!", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
            if (!GlobalHotKey.RegisterHotKey(aKeyGestureString: Properties.Settings.Default.HotKeyClearClocks, aAction: () => butClearClocks.PerformClick()))
                MessageBox.Show(text: "Unable to register Hot Key for Clear Clocks!", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
            if (!GlobalHotKey.RegisterHotKey(aKeyGestureString: Properties.Settings.Default.HotKeyFirstDown, aAction: () => FirstDown()))
                MessageBox.Show(text: "Unable to register Hot Key for First Down!", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
            if (!GlobalHotKey.RegisterHotKey(aKeyGestureString: Properties.Settings.Default.HotKeyFlag, aAction: () => ToggleFlag()))
                MessageBox.Show(text: "Unable to register Hot Key for Flag!", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
            if (!GlobalHotKey.RegisterHotKey(aKeyGestureString: Properties.Settings.Default.HotKeyHomeTimeout, aAction: () => HomeTimeoutsSubtract()))
                MessageBox.Show(text: "Unable to register Hot Key for Home Timeout!", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
            if (!GlobalHotKey.RegisterHotKey(aKeyGestureString: Properties.Settings.Default.HotKeyNextDown, aAction: () => NextDown()))
                MessageBox.Show(text: "Unable to register Hot Key for Next Down!", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
            if (!GlobalHotKey.RegisterHotKey(aKeyGestureString: Properties.Settings.Default.HotKeyNextPeriod, aAction: () => NextPeriod()))
                MessageBox.Show(text: "Unable to register Hot Key for Next Period!", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
            if (!GlobalHotKey.RegisterHotKey(aKeyGestureString: Properties.Settings.Default.HotKeyHomeSafety, aAction: () => butHomeSafety.PerformClick()))
                MessageBox.Show(text: "Unable to register Hot Key for Home Safety!", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
            if (!GlobalHotKey.RegisterHotKey(aKeyGestureString: Properties.Settings.Default.HotKeyHomePatKick, aAction: () => butHomePatKick.PerformClick()))
                MessageBox.Show(text: "Unable to register Hot Key for Home PAT - Kick!", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
            if (!GlobalHotKey.RegisterHotKey(aKeyGestureString: Properties.Settings.Default.HotKeyHomePatConversion, aAction: () => butHomePatConversion.PerformClick()))
                MessageBox.Show(text: "Unable to register Hot Key for Home PAT - Conversion!", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
            if (!GlobalHotKey.RegisterHotKey(aKeyGestureString: Properties.Settings.Default.HotKeyHomeFieldGoal, aAction: () => butHomeFieldGoal.PerformClick()))
                MessageBox.Show(text: "Unable to register Hot Key for Home Field Goal!", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
            if (!GlobalHotKey.RegisterHotKey(aKeyGestureString: Properties.Settings.Default.HotKeyHomeTouchdown, aAction: () => butHomeTouchdown.PerformClick()))
                MessageBox.Show(text: "Unable to register Hot Key for Home Touchdown!", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
            if (!GlobalHotKey.RegisterHotKey(aKeyGestureString: Properties.Settings.Default.HotKeyAwaySafety, aAction: () => butAwaySafety.PerformClick()))
                MessageBox.Show(text: "Unable to register Hot Key for Away Safety!", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
            if (!GlobalHotKey.RegisterHotKey(aKeyGestureString: Properties.Settings.Default.HotKeyAwayPatKick, aAction: () => butAwayPatKick.PerformClick()))
                MessageBox.Show(text: "Unable to register Hot Key for Away PAT - Kick!", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
            if (!GlobalHotKey.RegisterHotKey(aKeyGestureString: Properties.Settings.Default.HotKeyAwayPatConversion, aAction: () => butAwayPatConversion.PerformClick()))
                MessageBox.Show(text: "Unable to register Hot Key for Away PAT - Conversion!", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
            if (!GlobalHotKey.RegisterHotKey(aKeyGestureString: Properties.Settings.Default.HotKeyAwayFieldGoal, aAction: () => butAwayFieldGoal.PerformClick()))
                MessageBox.Show(text: "Unable to register Hot Key for Away Field Goal!", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
            if (!GlobalHotKey.RegisterHotKey(aKeyGestureString: Properties.Settings.Default.HotKeyAwayTouchdown, aAction: () => butAwayTouchdown.PerformClick()))
                MessageBox.Show(text: "Unable to register Hot Key for Away Touchdown!", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
            if (!GlobalHotKey.RegisterHotKey(aKeyGestureString: Properties.Settings.Default.HotKeyPossession, aAction: () => TogglePossession()))
                MessageBox.Show(text: "Unable to register Hot Key for Possession!", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
        }
        private void SendSupplemental()
        {
            _ = Common.WriteFileAsync(file: supplementalFile, content: txtSupplemental.Text);
        }
        private void SetPlayClock(string duration, bool start)
        {
            txtPlayClock.Text = duration;
            if (start)
            {
                playTimeRemaining = new TimeSpan(hours: 0, minutes: 0, seconds: int.Parse(txtPlayClock.Text));
                playTimeEnd = DateTime.UtcNow + playTimeRemaining;
                butStartStopPlayClock.Text = "Stop Play Clock";
                if (!tmrClockRefresh.Enabled)
                    tmrClockRefresh.Enabled = true;
                playClockRunning = true;
            }
        }
        private void ShowAwayPlayer(object sender, EventArgs e)
        {
            Common.ShowPlayer(false, (sender as Button).Text, obs: obs);
            tmrPlayerAway.Interval = Properties.Settings.Default.FlagDisplayDuration;
            tmrPlayerAway.Enabled = true;
            tmrPlayerAway.Start();
        }
        private void ShowHomePlayer(object sender, EventArgs e)
        {
            Common.ShowPlayer(true, (sender as Button).Text, obs: obs);
            tmrPlayerHome.Interval = Properties.Settings.Default.FlagDisplayDuration;
            tmrPlayerHome.Enabled = true;
            tmrPlayerHome.Start();
        }
        private void StartHttpListener()
        {
            if (!Listener.IsListening)
            {
                try
                {
                    server = CreateWebServer("http://" + Properties.Settings.Default.HttpListenerPort + "/");
                    server.Start();
                    txtListenerState.Text = "Listening";
                    txtListenerState.BackColor = System.Drawing.Color.LightGreen;

                }
                catch (HttpListenerException ex)
                {
                    MessageBox.Show($"Error starting HTTP listener: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtListenerState.Text = "Error";
                    txtListenerState.BackColor = System.Drawing.Color.Red;
                }
            }
        }
        private TimeSpan TimeRemainingFromTextBox()
        {
            if (txtGameClock.Text.Contains('.'))
            {
                periodTimeRemaining = new TimeSpan(days: 0, hours: 0, minutes: int.Parse(txtGameClock.Text[..1]), seconds: int.Parse(txtGameClock.Text.Substring(startIndex: 2, length: 2)), milliseconds: int.Parse(txtGameClock.Text.Substring(startIndex: 5, length: 1)) * 100);
            }
            else
            {
                periodTimeRemaining = new TimeSpan(hours: 0, minutes: int.Parse(txtGameClock.Text[..2]), seconds: int.Parse(txtGameClock.Text.Substring(startIndex: 3, length: 2)));
            }
            return periodTimeRemaining;
        }
        private void TmrClockRefresh_Tick(object sender, EventArgs e)
        {
            if (gameClockRunning)
                DecrementGameClock();
            if (playClockRunning)
                DecrementPlayClock();
        }
        private void TmrFlag_Tick(object sender, EventArgs e)
        {
            tmrFlag.Stop();
            tmrFlag.Enabled = false;
            chkFlag.Checked = false;
            _ = Common.WriteFileAsync(file: awayPenaltyType, content: string.Empty);
            _ = Common.WriteFileAsync(file: homePenaltyType, content: string.Empty);
            _ = Common.WriteFileAsync(file: penaltyType, content: string.Empty);
            foreach (Control control in this.gbPenalties.Controls)
            {
                if (control is RadioButton)
                {
                    RadioButton radio = control as RadioButton;
                    if (radio.Checked)
                    {
                        radio.Checked = false;
                    }
                }
            }
            rbAwayPenalty.Checked = false;
            rbHomePenalty.Checked = false;
        }
        private void TmrPlayerAway_Tick(object sender, EventArgs e)
        {
            butAwayPlayerShow.PerformClick();
            tmrPlayerAway.Enabled = false;
            Common.HidePlayer(home: false);
        }
        private void TmrPlayerHome_Tick(object sender, EventArgs e)
        {
            butHomePlayerShow.PerformClick();
            tmrPlayerHome.Enabled = false;
            Common.HidePlayer(home: true);
        }
        private void TmrRed_Tick(object sender, EventArgs e)
        {
            chkRed.Checked = false;
        }
        private void TmrScore_Tick(object sender, EventArgs e)
        {
            _ = Common.WriteFileAsync(file: scoreDescription, content: string.Empty);
            _ = CopyFileAsync(sourcePath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "Score\\NoScore.png"), destinationPath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "Score.png"));
            tmrScore.Enabled = false;
        }
        private void ToggleFlag()
        {
            if (chkFlag.Checked)
                chkFlag.Checked = false;
            else
                chkFlag.Checked = true;
        }
        private void TogglePossession()
        {
            if (!chkHomePossession.Checked)
                chkHomePossession.Checked = true;
            else
                chkAwayPossession.Checked = true;
            if (Properties.Settings.Default.PossessionChangeFirstDown)
                rbDownOne.Checked = true;
        }
        private void ToolStripMenuItemAbout_Click(object sender, EventArgs e)
        {
            Process.Start(fileName: "https://github.com/hoga2443/AmericanFootballScoreboard/wiki");
        }
        private void ToolStripMenuItemCheckForUpdate_Click(object sender, EventArgs e)
        {
            _ = CheckForUpdates();
        }
        private void ToolStripMenuItemClose_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
            this.Close();
        }
        private void ToolStripMenuItemOpenOutputFolder_Click(object sender, EventArgs e)
        {
            Process.Start(new ProcessStartInfo()
            {
                FileName = Properties.Settings.Default.OutputPath,
                UseShellExecute = true,
                Verb = "open"
            });
        }
        private void ToolStripMenuItemReportIssue_Click(object sender, EventArgs e)
        {
            Process.Start(fileName: "https://github.com/hoga2443/AmericanFootballScoreboard/issues");
        }
        private void ToolStripMenuItemSaveHotKeys_Click(object sender, EventArgs e)
        {
            butSaveHotKey.PerformClick();
        }
        private void ToolStripMenuItemSaveSettings_Click(object sender, EventArgs e)
        {
            butSaveSettings.PerformClick();
        }
        private void TxtAwayPlayerNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                butAwayPlayerShow.PerformClick();
            }
        }
        private void TxtAwayScore_TextChanged(object sender, EventArgs e)
        {
            if (!int.TryParse(s: txtAwayScore.Text, result: out _) && txtAwayScore.Text != string.Empty)
                MessageBox.Show(text: "Please enter an integer for away score.", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Exclamation);
            else
                _ = Common.WriteFileAsync(file: awayTeamScoreFile, content: txtAwayScore.Text);
        }
        private void TxtAwayTeam_Leave(object sender, EventArgs e)
        {
            _ = Common.WriteFileAsync(file: awayTeamNameFile, content: txtAwayTeam.Text);
        }
        private void TxtAwayTimeouts_TextChanged(object sender, EventArgs e)
        {
            _ = Common.WriteFileAsync(file: awayTimeoutsRemainingFile, content: txtAwayTimeouts.Text);
            _ = txtAwayTimeouts.Text switch
            {
                "0" => CopyFileAsync(sourcePath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "AwayTimeouts\\0Timeouts.png"), destinationPath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "AwayTimeouts.png")),
                "1" => CopyFileAsync(sourcePath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "AwayTimeouts\\1Timeouts.png"), destinationPath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "AwayTimeouts.png")),
                "2" => CopyFileAsync(sourcePath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "AwayTimeouts\\2Timeouts.png"), destinationPath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "AwayTimeouts.png")),
                _ => CopyFileAsync(sourcePath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "AwayTimeouts\\3Timeouts.png"), destinationPath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "AwayTimeouts.png")),
            };
        }
        private void TxtDistance_OnFocus(object sender, EventArgs e)
        {
            txtDistance.SelectAll();
        }
        private void TxtDistance_TextChanged(object sender, EventArgs e)
        {
            _ = Common.WriteFileAsync(file: distanceFile, content: txtDistance.Text);
            UpdateDownAndDistance();
        }
        private void TxtGameClock_Leave(object sender, EventArgs e)
        {
            if (!ValidTime(time: txtGameClock.Text))
                MessageBox.Show(text: "Please enter a valid time mm:ss.", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Exclamation);
            else
                _ = Common.WriteFileAsync(file: gameClockFile, content: txtGameClock.Text);
        }
        private void TxtHomePlayerNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                butHomePlayerShow.PerformClick();
        }
        private void TxtHomeScore_TextChanged(object sender, EventArgs e)
        {
            if (!int.TryParse(s: txtHomeScore.Text, result: out _) && txtHomeScore.Text != string.Empty)
                MessageBox.Show(text: "Please enter an integer for home score.", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Exclamation);
            else
                _ = Common.WriteFileAsync(file: homeTeamScoreFile, content: txtHomeScore.Text);
        }
        private void TxtHomeTeam_Leave(object sender, EventArgs e)
        {
            _ = Common.WriteFileAsync(file: homeTeamNameFile, content: txtHomeTeam.Text);
        }
        private void TxtHomeTimeouts_TextChanged(object sender, EventArgs e)
        {
            _ = Common.WriteFileAsync(file: homeTimeoutsRemainingFile, content: txtHomeTimeouts.Text);
            _ = txtHomeTimeouts.Text switch
            {
                "0" => CopyFileAsync(sourcePath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "HomeTimeouts\\0Timeouts.png"), destinationPath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "HomeTimeouts.png")),
                "1" => CopyFileAsync(sourcePath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "HomeTimeouts\\1Timeouts.png"), destinationPath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "HomeTimeouts.png")),
                "2" => CopyFileAsync(sourcePath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "HomeTimeouts\\2Timeouts.png"), destinationPath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "HomeTimeouts.png")),
                _ => CopyFileAsync(sourcePath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "HomeTimeouts\\3Timeouts.png"), destinationPath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "HomeTimeouts.png")),
            };
        }
        private void TxtPeriodAwayFirst_TextChanged(object sender, EventArgs e)
        {
            _ = Common.WriteFileAsync(file: awayPeriodScoreFirst, content: txtPeriodAwayFirst.Text);
        }
        private void TxtPeriodAwayFourth_TextChanged(object sender, EventArgs e)
        {
            _ = Common.WriteFileAsync(file: awayPeriodScoreFourth, content: txtPeriodAwayFourth.Text);
        }
        private void TxtPeriodAwayOT_TextChanged(object sender, EventArgs e)
        {
            _ = Common.WriteFileAsync(file: awayPeriodScoreOT, content: txtPeriodAwayOT.Text);
        }
        private void TxtPeriodAwaySecond_TextChanged(object sender, EventArgs e)
        {
            _ = Common.WriteFileAsync(file: awayPeriodScoreSecond, content: txtPeriodAwaySecond.Text);
        }
        private void TxtPeriodAwayThird_TextChanged(object sender, EventArgs e)
        {
            _ = Common.WriteFileAsync(file: awayPeriodScoreThird, content: txtPeriodAwayThird.Text);
        }
        private void TxtPeriodHomeFirst_TextChanged(object sender, EventArgs e)
        {
            _ = Common.WriteFileAsync(file: homePeriodScoreFirst, content: txtPeriodHomeFirst.Text);
        }
        private void TxtPeriodHomeFourth_TextChanged(object sender, EventArgs e)
        {
            _ = Common.WriteFileAsync(file: homePeriodScoreFourth, content: txtPeriodHomeFourth.Text);
        }
        private void TxtPeriodHomeOT_TextChanged(object sender, EventArgs e)
        {
            _ = Common.WriteFileAsync(file: homePeriodScoreOT, content: txtPeriodHomeOT.Text);
        }
        private void TxtPeriodHomeSecond_TextChanged(object sender, EventArgs e)
        {
            _ = Common.WriteFileAsync(file: homePeriodScoreSecond, content: txtPeriodHomeSecond.Text);
        }
        private void TxtPeriodHomeThird_TextChanged(object sender, EventArgs e)
        {
            _ = Common.WriteFileAsync(file: homePeriodScoreThird, content: txtPeriodHomeThird.Text);
        }
        private void TxtPeriodOT_TextChanged(object sender, EventArgs e)
        {
            _ = Common.WriteFileAsync(file: periodFile, content: txtPeriodOT.Text);
        }
        private void TxtPlayClock_Leave(object sender, EventArgs e)
        {
            if (!int.TryParse(s: txtPlayClock.Text, result: out _) && txtPlayClock.Text != string.Empty)
                MessageBox.Show(text: "Please enter an integer for play clock.", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Exclamation);
            else
                _ = Common.WriteFileAsync(file: playClockFile, content: txtPlayClock.Text);
        }
        private void TxtSpot_OnFocus(object sender, EventArgs e)
        {
            txtSpot.SelectAll();
        }
        private void TxtSpot_TextChanged(object sender, EventArgs e)
        {
            _ = Common.WriteFileAsync(file: spotFile, content: txtSpot.Text);
        }
        private void UpdateDownAndDistance()
        {
            if (rbDownOne.Checked)
                _ = Common.WriteFileAsync(file: downDistanceFile, content: Properties.Settings.Default.Down1 + " & " + txtDistance.Text);
            if (rbDownTwo.Checked)
                _ = Common.WriteFileAsync(file: downDistanceFile, content: Properties.Settings.Default.Down2 + " & " + txtDistance.Text);
            if (rbDownThree.Checked)
                _ = Common.WriteFileAsync(file: downDistanceFile, content: Properties.Settings.Default.Down3 + " & " + txtDistance.Text);
            if (rbDownFour.Checked)
                _ = Common.WriteFileAsync(file: downDistanceFile, content: Properties.Settings.Default.Down4 + " & " + txtDistance.Text);
            if (rbDownBlank.Checked)
                _ = Common.WriteFileAsync(file: downDistanceFile, content: string.Empty);
        }
        private static void UpdatePeriodScore(TextBox control, int points)
        {
            try
            {
                if (string.IsNullOrEmpty(control.Text))
                    control.Text = "0";
                control.Text = (int.Parse(s: control.Text) + points).ToString();
            }
            catch { }
        }
        /*
        Validate time in in a valid mm:ss format
        */
        private static bool ValidTime(string time)
        {
            if (string.IsNullOrEmpty(time))
                return true;
            if (time.Contains(value: '.'))
            {
                if (time.Length > 6)
                    return false;
                if (time[..1] != "0")
                    return false;
                if (time.Substring(startIndex: 1, length: 1) != ":")
                    return false;
                if (!int.TryParse(time.AsSpan(start: 2, length: 2), out int seconds))
                    return false;
                if (seconds > 59)
                    return false;
                if (time.Substring(startIndex: 4, length: 1) != ".")
                    return false;
                if (!int.TryParse(time.AsSpan(start: 5, length: 1), out _))
                    return false;
            }
            else
            {
                if (time.Length != 5)
                    return false;
                if (!int.TryParse(time[..2], out _))
                    return false;
                if (time.Substring(startIndex: 2, length: 1) != ":")
                    return false;
                if (!int.TryParse(time.AsSpan(start: 3, length: 2), out int seconds))
                    return false;
                if (seconds > 59)
                    return false;
            }
            return true;
        }
        private void WritePenaltyToFile(RadioButton radioButton)
        {
            if (rbHomePenalty.Checked)
                _ = Common.WriteFileAsync(file: homePenaltyType, content: " " + radioButton.Text + " ");
            else if (rbAwayPenalty.Checked)
                _ = Common.WriteFileAsync(file: awayPenaltyType, content: " " + radioButton.Text + " ");
            else
                _ = Common.WriteFileAsync(file: penaltyType, content: " " + radioButton.Text + " ");
            tmrFlag.Interval = Properties.Settings.Default.FlagDisplayDuration;
            tmrFlag.Enabled = true;
            tmrFlag.Start();
        }
        private void WriteScore(string text)
        {
            _ = Common.WriteFileAsync(file: scoreDescription, content: text);
            tmrScore.Interval = Properties.Settings.Default.FlagDisplayDuration;
            tmrScore.Enabled = true;
            tmrScore.Start();
        }
        private bool WriteScoreImage(Score score)
        {
            bool result = false;
            string sourcePath = string.Empty;
            switch (score)
            {
                case Score.Touchdown:
                    sourcePath = Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "Score\\Touchdown.png");
                    break;
                case Score.FieldGoal:
                    sourcePath = Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "Score\\FieldGoal.png");
                    break;
                case Score.PatConversion:
                    sourcePath = Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "Score\\PatConversion.png");
                    break;
                case Score.PatKick:
                    sourcePath = Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "Score\\PatKick.png");
                    break;
                case Score.Safety:
                    sourcePath = Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "Score\\Safety.png");
                    break;
            }

            if (File.Exists(sourcePath))
            {
                _ = CopyFileAsync(sourcePath: sourcePath, destinationPath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "Score.png"));
                tmrScore.Enabled = true;
                tmrScore.Start();
            }
            return result;
        }
    }
}