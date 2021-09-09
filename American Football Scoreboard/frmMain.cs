using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace American_Football_Scoreboard
{
    public partial class FrmMain : Form
    {
        const string awayTeamNameFile = "AwayTeamName.txt";
        const string awayTeamScoreFile = "AwayTeamScore.txt";
        const string awayTimeoutsRemainingFile = "AwayTimeoutsRemaining.txt";
        const string distanceFile = "Distance.txt";
        const string downFile = "Down.txt";
        const string gameClockFile = "GameClock.txt";
        const string homeTeamNameFile = "HomeTeamName.txt";
        const string homeTeamScoreFile = "HomeTeamScore.txt";
        const string homeTimeoutsRemainingFile = "HomeTimeoutsRemaining.txt";
        const string periodFile = "Period.txt";
        const string playClockFile = "PlayClock.txt";
        const string supplementalFile = "Supplemental.txt";

        const char padZero = '0';

        private bool periodClockRunning = false;
        private DateTime periodClockEnd = DateTime.UtcNow;
        private TimeSpan periodTimeRemaining = new TimeSpan(0, 0, 0);

        private bool playClockRunning = false;
        private DateTime playTimeEnd = DateTime.UtcNow;
        private TimeSpan playTimeRemaining = new TimeSpan(0, 0, 0);

        public FrmMain()
        {
            InitializeComponent();
            LoadSettings();
            InitializeUI();
            RegisterHotKeys();
        }

        /*
        private void FrmMain_KeyDown(object sender, KeyEventArgs e)
        {
            // Start/Stop Game Clock
            if (e.KeyCode.ToString() == Properties.Settings.Default.HotKeyStartStopGameClock
                && e.Alt == Properties.Settings.Default.HotKeyStartStopGameClockAlt
                && e.Control == Properties.Settings.Default.HotKeyStartStopGameClockCtrl
                && e.Shift == Properties.Settings.Default.HotKeyStartStopGameClockShift)
            {
                butStartStopGameClock.PerformClick();
            }
            // Start/Stop Play Clock
            else if (e.KeyCode.ToString() == Properties.Settings.Default.HotKeyStartStopPlayClock
                && e.Alt == Properties.Settings.Default.HotKeyStartStopPlayClockAlt
                && e.Control == Properties.Settings.Default.HotKeyStartStopPlayClockCtrl
                && e.Shift == Properties.Settings.Default.HotKeyStartStopPlayClockShift)
            {
                butStartStopPlayClock.PerformClick();
            }
            // New Play Clock
            else if (e.KeyCode.ToString() == Properties.Settings.Default.HotKeyNewPlayClock
                && e.Alt == Properties.Settings.Default.HotKeyNewPlayClockAlt
                && e.Control == Properties.Settings.Default.HotKeyNewPlayClockCtrl
                && e.Shift == Properties.Settings.Default.HotKeyNewPlayClockShift)
            {
                butNewPlayClock.PerformClick();
            }
            // Clear Clocks
            else if (e.KeyCode.ToString() == Properties.Settings.Default.HotKeyClearClocks
                && e.Alt == Properties.Settings.Default.HotKeyClearClocksAlt
                && e.Control == Properties.Settings.Default.HotKeyClearClocksCtrl
                && e.Shift == Properties.Settings.Default.HotKeyClearClocksShift)
            {
                butClearClocks.PerformClick();
            }
            // Next Down
            else if (e.KeyCode.ToString() == Properties.Settings.Default.HotKeyNextDown
                && e.Alt == Properties.Settings.Default.HotKeyNextDownAlt
                && e.Control == Properties.Settings.Default.HotKeyNextDownCtrl
                && e.Shift == Properties.Settings.Default.HotKeyNextDownShift)
            {
                NextDown();
            }
            // Next Period
            else if (e.KeyCode.ToString() == Properties.Settings.Default.HotKeyNextPeriod
                && e.Alt == Properties.Settings.Default.HotKeyNextPeriodAlt
                && e.Control == Properties.Settings.Default.HotKeyNextPeriodCtrl
                && e.Shift == Properties.Settings.Default.HotKeyNextPeriodShift)
            {
                NextPeriod();
            }
        }
        */

        /*
        Function to add a specified number of points to a specified control
        Called by all functions which alter a score
        */
        private void AddScore(TextBox control, int points)
        {
            if(int.TryParse(s: control.Text, out int oldScore))
                control.Text = (oldScore + points).ToString();
        }

        /*
        Function to increase/decrease the number of timeouts in a specified control
        Called by all functions which alter a number of timeouts
        */
        private void AddTimeout(TextBox control, int timeoutsToAdd)
        {
            if (int.TryParse(s: control.Text, out int currentTimeouts))
            {
                control.Text = (currentTimeouts + timeoutsToAdd).ToString();
            }
            else
            {
                control.Text = (timeoutsToAdd).ToString();
            }
        }

        private void ButAwayAddOne_Click(object sender, EventArgs e)
        {
            AddScore(txtAwayScore, 1);
        }

        private void ButAwayAddTwo_Click(object sender, EventArgs e)
        {
            AddScore(txtAwayScore, 2);
        }

        private void ButAwayAddThree_Click(object sender, EventArgs e)
        {
            AddScore(txtAwayScore, 3);
        }

        private void ButAwayAddSix_Click(object sender, EventArgs e)
        {
            AddScore(txtAwayScore, 6);
        }

        private void ButAwayTimeoutsAdd_Click(object sender, EventArgs e)
        {
            AddTimeout(txtAwayTimeouts, 1);
        }

        private void ButAwayTimeoutsSubtract_Click(object sender, EventArgs e)
        {
            AddTimeout(txtAwayTimeouts, -1);
        }

        private void ButClearAway_Click(object sender, EventArgs e)
        {
            txtAwayTimeouts.Text = Properties.Settings.Default.TimeoutsPerHalf;
            txtAwayScore.Text = "0";
            txtAwayTeam.Text = "";
        }

        private void ButClearClocks_Click(object sender, EventArgs e)
        {
            tmrClockRefresh.Enabled = false;
            playClockRunning = false;
            txtGameClock.Text = Properties.Settings.Default.DefaultPeriod;
            _ = WriteFileAsync(gameClockFile, txtGameClock.Text);
            txtPlayClock.Text = Properties.Settings.Default.DefaultPlay;
            _ = WriteFileAsync(playClockFile, txtPlayClock.Text);
        }

        private void ButClearHome_Click(object sender, EventArgs e)
        {
            txtHomeTimeouts.Text = Properties.Settings.Default.TimeoutsPerHalf;
            txtHomeScore.Text = "0";
            txtHomeTeam.Text = "";
        }

        private void ButDistanceGoal_Click(object sender, EventArgs e)
        {
            txtDistance.Text = Properties.Settings.Default["GoalText"].ToString();
        }

        private void ButDownClear_Click(object sender, EventArgs e)
        {
            rbDownOne.Checked = false;
            rbDownTwo.Checked = false;
            rbDownThree.Checked = false;
            rbDownFour.Checked = false;
            txtDistance.Text = "";
        }

        private void ButHomeAddOne_Click(object sender, EventArgs e)
        {
            AddScore(txtHomeScore, 1);
        }

        private void ButHomeAddTwo_Click(object sender, EventArgs e)
        {
            AddScore(txtHomeScore, 2);
        }

        private void ButHomeAddThree_Click(object sender, EventArgs e)
        {
            AddScore(txtHomeScore, 3);
        }

        private void ButHomeAddSix_Click(object sender, EventArgs e)
        {
            AddScore(txtHomeScore, 6);
        }

        private void ButHomeTimeoutsAdd_Click(object sender, EventArgs e)
        {
            AddTimeout(txtHomeTimeouts, 1);
        }

        private void ButHomeTimeoutsSubtract_Click(object sender, EventArgs e)
        {
            AddTimeout(txtHomeTimeouts, -1);
        }

        private void ButNewPlayClock_Click(object sender, EventArgs e)
        {
            txtPlayClock.Text = Properties.Settings.Default.DefaultPlay.ToString();
            playTimeRemaining = new TimeSpan(0, 0, int.Parse(txtPlayClock.Text));
            playTimeEnd = DateTime.UtcNow + playTimeRemaining;
            butStartStopPlayClock.Text = "Stop Play Clock"; 
            if (!tmrClockRefresh.Enabled)
            {
                tmrClockRefresh.Enabled = true;
            }
            playClockRunning = true;
        }

        private void ButOutputFolder_Click(object sender, EventArgs e)
        {
            DialogResult result = fbdOutput.ShowDialog();
            if (result == DialogResult.OK)
            {
                txtOutputFolder.Text = fbdOutput.SelectedPath;
            }
        }

        private void ButPeriodClear_Click(object sender, EventArgs e)
        {
            rbPeriodOne.Checked = false;
            rbPeriodTwo.Checked = false;
            rbPeriodThree.Checked = false;
            rbPeriodFour.Checked = false;
            rbPeriodOT.Checked = false;
        }

        private void ButSaveHotKey_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default["HotKeyStartStopGameClock"] = txtHotKeyStartStopGameClock.Text;
            Properties.Settings.Default["HotKeyStartStopPlayClock"] = txtHotKeyStartStopPlayClock.Text;
            Properties.Settings.Default["HotKeyNewPlayClock"] = txtHotKeyNewPlayClock.Text;
            Properties.Settings.Default["HotKeyClearClocks"] = txtHotKeyClearClocks.Text;
            Properties.Settings.Default["HotKeyNextDown"] = txtHotKeyNextDown.Text;
            Properties.Settings.Default["HotKeyNextPeriod"] = txtHotKeyNextPeriod.Text;
            Properties.Settings.Default["HotKeyAway1"] = txtHotKeyAway1.Text;
            Properties.Settings.Default["HotKeyAway2"] = txtHotKeyAway2.Text;
            Properties.Settings.Default["HotKeyAway3"] = txtHotKeyAway3.Text;
            Properties.Settings.Default["HotKeyAway6"] = txtHotKeyAway6.Text;
            Properties.Settings.Default["HotKeyHome1"] = txtHotKeyHome1.Text;
            Properties.Settings.Default["HotKeyHome2"] = txtHotKeyHome2.Text;
            Properties.Settings.Default["HotKeyHome3"] = txtHotKeyHome3.Text;
            Properties.Settings.Default["HotKeyHome6"] = txtHotKeyHome6.Text;
            Properties.Settings.Default["HotKeyPossession"] = txtHotKeyPossession.Text;
            Properties.Settings.Default.Save();

            DialogResult result = MessageBox.Show(text: "Please re-start the application for new Hot Keys to take effect. Restart Now?", caption: "AFS", buttons: MessageBoxButtons.YesNo, icon: MessageBoxIcon.Information);
            if (result == DialogResult.Yes)
            {
                Application.Restart();
                Environment.Exit(0);
            }
        }

        private void ButSaveSettings_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default["DefaultPeriod"] = txtPeriodDuration.Text;
            Properties.Settings.Default["DefaultPlay"] = txtPlayClockDuration.Text;
            Properties.Settings.Default["GoalText"] = txtGoalText.Text;
            if (!string.IsNullOrEmpty(txtOutputFolder.Text))
            {
                Properties.Settings.Default["OutputPath"] = txtOutputFolder.Text;
            }
            Properties.Settings.Default["TimeoutsPerHalf"] = txtTimeoutsPerHalf.Text;
            if (int.TryParse(s: txtRefreshInterval.Text, out int refreshInterval))
            {
                Properties.Settings.Default["RefreshInterval"] = refreshInterval;
            }
            else
            {
                Properties.Settings.Default["RefreshInterval"] = 400;
            }
            Properties.Settings.Default.Save();
            tmrClockRefresh.Interval = Properties.Settings.Default.RefreshInterval;
        }

        private void ButSendSupplemental_Click(object sender, EventArgs e)
        {
            _ = WriteFileAsync(supplementalFile, txtSupplemental.Text);
        }

        private void ButStartStopGameClock_Click(object sender, EventArgs e)
        {
            if (periodClockRunning)
            {
                periodClockRunning = false;
                if (!playClockRunning)
                {
                    tmrClockRefresh.Enabled = false;
                }
                butStartStopGameClock.Text = "Start Game Clock";
            }
            else
            {
                periodClockRunning = true;
                if (!tmrClockRefresh.Enabled)
                    tmrClockRefresh.Enabled = true;
                if (txtGameClock.Text.Trim().Length == 0)
                    txtGameClock.Text = Properties.Settings.Default.DefaultPeriod.ToString();
                periodTimeRemaining = new TimeSpan(0, int.Parse(txtGameClock.Text.Substring(0, 2)), int.Parse(txtGameClock.Text.Substring(3, 2)));
                periodClockEnd = DateTime.UtcNow + periodTimeRemaining;
                butStartStopGameClock.Text = "Stop Game Clock";
            }
        }

        private void ButStartPlayClock_Click(object sender, EventArgs e)
        {
            if (playClockRunning)
            {
                playClockRunning = false;
                if (!playClockRunning)
                {
                    tmrClockRefresh.Enabled = false;
                }
                butStartStopPlayClock.Text = "Start Play Clock";
            }
            else
            {
                playClockRunning = true;
                if (!tmrClockRefresh.Enabled)
                    tmrClockRefresh.Enabled = true;
                if (txtPlayClock.Text.Trim().Length == 0)
                    txtPlayClock.Text = Properties.Settings.Default.DefaultPlay.ToString();
                playTimeRemaining = new TimeSpan(0, 0, int.Parse(txtPlayClock.Text));
                playTimeEnd = DateTime.UtcNow + playTimeRemaining;
                butStartStopPlayClock.Text = "Stop Play Clock";
            }
        }

        private void ButStopPlayClock_Click(object sender, EventArgs e)
        {
            playClockRunning = false;
            if (!playClockRunning)
            {
                tmrClockRefresh.Enabled = false;
            }
        }

        private void ChkAwayPossession_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAwayPossession.Checked)
            {
                chkHomePossession.Checked = false;
                _ = CopyFileAsync(sourcePath: Path.Combine(Properties.Settings.Default.OutputPath, "Possession\\Possession.png"), destinationPath: Path.Combine(Properties.Settings.Default.OutputPath, "AwayPossession.png"));
            }
            else
            {
                _ = CopyFileAsync(sourcePath: Path.Combine(Properties.Settings.Default.OutputPath, "Possession\\NonPossession.png"), destinationPath: Path.Combine(Properties.Settings.Default.OutputPath, "AwayPossession.png"));
            }
        }

        private void ChkHomePossession_CheckedChanged(object sender, EventArgs e)
        {
            if (chkHomePossession.Checked)
            {
                chkAwayPossession.Checked = false;
                _ = CopyFileAsync(sourcePath: Path.Combine(Properties.Settings.Default.OutputPath, "Possession\\Possession.png"), destinationPath: Path.Combine(Properties.Settings.Default.OutputPath, "HomePossession.png"));
            }
            else
            {
                _ = CopyFileAsync(sourcePath: Path.Combine(Properties.Settings.Default.OutputPath, "Possession\\NonPossession.png"), destinationPath: Path.Combine(Properties.Settings.Default.OutputPath, "HomePossession.png"));
            }
        }

        private void TogglePossession()
        {
            if (!chkHomePossession.Checked)
            {
                chkHomePossession.Checked = true;
            }
            else
            {
                chkAwayPossession.Checked = true;
            }
        }

        /*
        Copy a file in the filesystem
        Used to update the timeouts remaining images
        */
        static async Task CopyFileAsync(string sourcePath, string destinationPath)
        {
            using (Stream source = File.Open(path: sourcePath, mode: FileMode.Open))
            {
                using (Stream destination = File.Create(path: destinationPath))
                {
                    await source.CopyToAsync(destination);
                }
            }
        }

        private void DecrementGameClock()
        {
            periodTimeRemaining = periodClockEnd - DateTime.UtcNow;
            txtGameClock.Text = periodTimeRemaining.Minutes.ToString().PadLeft(2, padZero) + ":" + periodTimeRemaining.Seconds.ToString().PadLeft(2, padZero);
            _ = WriteFileAsync(gameClockFile, txtGameClock.Text);
            if (txtGameClock.Text == "00:00")
                periodClockRunning = false;
        }

        private void DecementPlayClock()
        {
            playTimeRemaining = playTimeEnd - DateTime.UtcNow;
            txtPlayClock.Text = ((int)playTimeRemaining.TotalSeconds).ToString();
            _ = WriteFileAsync(playClockFile, txtPlayClock.Text);
            if (txtPlayClock.Text == "0")
            {
                playClockRunning = false;
            }
        }

        private void InitializeTextBox(TextBox textBox, string fileName)
        {
            string currentFile = Path.Combine(Properties.Settings.Default.OutputPath, fileName);
            if (!File.Exists(currentFile))
            {
                FileStream fs = File.Create(currentFile);
                fs.Close();
            }
            textBox.Text = File.ReadAllText(currentFile);
        }

        private void InitializeUI()
        {
            txtGameClock.Text = Properties.Settings.Default.DefaultPeriod;
            _ = WriteFileAsync(gameClockFile, txtGameClock.Text);
            txtPlayClock.Text = Properties.Settings.Default.DefaultPlay;
            txtHomeTimeouts.Text = Properties.Settings.Default.TimeoutsPerHalf;
            txtAwayTimeouts.Text = Properties.Settings.Default.TimeoutsPerHalf;
            InitializeTextBox(textBox: txtHomeTeam, fileName: homeTeamNameFile);
            InitializeTextBox(textBox: txtHomeScore, fileName: homeTeamScoreFile);
            InitializeTextBox(textBox: txtHomeTimeouts, fileName: homeTimeoutsRemainingFile);
            InitializeTextBox(textBox: txtAwayTeam, fileName: awayTeamNameFile);
            InitializeTextBox(textBox: txtAwayScore, fileName: awayTeamScoreFile);
            InitializeTextBox(textBox: txtAwayTimeouts, fileName: awayTimeoutsRemainingFile);
            InitializeTextBox(textBox: txtGameClock, fileName: gameClockFile);
            InitializeTextBox(textBox: txtPlayClock, fileName: playClockFile);
            InitializeTextBox(textBox: txtDistance, fileName: distanceFile);
            InitializeTextBox(textBox: txtSupplemental, fileName: supplementalFile);

            string currentFile = Path.Combine(Properties.Settings.Default.OutputPath, downFile);
            if (!File.Exists(currentFile))
            {
                FileStream fs = File.Create(currentFile);
                fs.Close();
            }
            string down = File.ReadAllText(Path.Combine(Properties.Settings.Default.OutputPath, downFile));
            switch (down)
            {
                case "2nd":
                    rbDownTwo.Checked = true;
                    break;
                case "3rd":
                    rbDownThree.Checked = true;
                    break;
                case "4th":
                    rbDownFour.Checked = true;
                    break;
                default:
                    rbDownOne.Checked = true;
                    break;
            }
            currentFile = Path.Combine(Properties.Settings.Default.OutputPath, periodFile);
            if (!File.Exists(currentFile))
            {
                FileStream fs = File.Create(currentFile);
                fs.Close();
            }

            string quarter = File.ReadAllText(Path.Combine(Properties.Settings.Default.OutputPath, periodFile));
            switch (quarter)
            {
                case "1":
                    rbPeriodOne.Checked = true;
                    break;
                case "2":
                    rbPeriodTwo.Checked = true;
                    break;
                case "3":
                    rbPeriodThree.Checked = true;
                    break;
                case "4":
                    rbPeriodFour.Checked = true;
                    break;
                default:
                    txtPeriodOT.Text = quarter;
                    rbPeriodOT.Checked = true;
                    break;
            }
        }

        private void LoadSettings()
        {
            txtPeriodDuration.Text = Properties.Settings.Default.DefaultPeriod;
            txtPlayClockDuration.Text = Properties.Settings.Default.DefaultPlay;
            txtGoalText.Text = Properties.Settings.Default.GoalText;
            txtOutputFolder.Text = Properties.Settings.Default.OutputPath;
            txtTimeoutsPerHalf.Text = Properties.Settings.Default.TimeoutsPerHalf;
            tmrClockRefresh.Interval = Properties.Settings.Default.RefreshInterval;
            txtHotKeyStartStopGameClock.Text = Properties.Settings.Default.HotKeyStartStopGameClock;
            txtHotKeyStartStopPlayClock.Text = Properties.Settings.Default.HotKeyStartStopPlayClock;
            txtHotKeyNewPlayClock.Text = Properties.Settings.Default.HotKeyNewPlayClock;
            txtHotKeyClearClocks.Text = Properties.Settings.Default.HotKeyClearClocks;
            txtHotKeyNextDown.Text = Properties.Settings.Default.HotKeyNextDown;
            txtHotKeyNextPeriod.Text = Properties.Settings.Default.HotKeyNextPeriod;
            txtRefreshInterval.Text = Properties.Settings.Default.RefreshInterval.ToString();
            txtHotKeyHome1.Text = Properties.Settings.Default.HotKeyHome1;
            txtHotKeyHome2.Text = Properties.Settings.Default.HotKeyHome2;
            txtHotKeyHome3.Text = Properties.Settings.Default.HotKeyHome3;
            txtHotKeyHome6.Text = Properties.Settings.Default.HotKeyHome6;
            txtHotKeyAway1.Text = Properties.Settings.Default.HotKeyAway1;
            txtHotKeyAway2.Text = Properties.Settings.Default.HotKeyAway2;
            txtHotKeyAway3.Text = Properties.Settings.Default.HotKeyAway3;
            txtHotKeyAway6.Text = Properties.Settings.Default.HotKeyAway6;
            txtHotKeyPossession.Text = Properties.Settings.Default.HotKeyPossession;
        }

        private void NextDown()
        {
            if (rbDownOne.Checked == true)
            {
                rbDownTwo.Checked = true;
            }
            else if (rbDownTwo.Checked == true)
            {
                rbDownThree.Checked = true;
            }
            else if (rbDownThree.Checked == true)
            {
                rbDownFour.Checked = true;
            }
            else
            {
                rbDownOne.Checked = true;
                txtDistance.Text = "10";
            }
        }

        private void NextPeriod()
        {
            if (rbPeriodOne.Checked == true)
            {
                rbPeriodTwo.Checked = true;
            }
            else if (rbPeriodTwo.Checked == true)
            {
                rbPeriodThree.Checked = true;
            }
            else if (rbPeriodThree.Checked == true)
            {
                rbPeriodFour.Checked = true;
            }
            else
            {
                rbPeriodOne.Checked = true;
                txtDistance.Text = "10";
            }
        }

        private void RbDownFour_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDownFour.Checked)
            {
                _ = WriteFileAsync(downFile, "4th");
            }
        }

        private void RbDownOne_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDownOne.Checked)
            {
                _ = WriteFileAsync(downFile, "1st");
                txtDistance.Text = "10";
            }
        }

        private void RbDownThree_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDownThree.Checked)
            {
                _ = WriteFileAsync(downFile, "3rd");
            }
        }

        private void RbDownTwo_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDownTwo.Checked)
            {
                _ = WriteFileAsync(downFile, "2nd");
            }
        }

        private void RbPeriodOne_CheckedChanged(object sender, EventArgs e)
        {
            _ = WriteFileAsync(periodFile, "1");
        }

        private void RbPeriodTwo_CheckedChanged(object sender, EventArgs e)
        {
            _ = WriteFileAsync(periodFile, "2");
        }

        private void RbPeriodThree_CheckedChanged(object sender, EventArgs e)
        {
            _ = WriteFileAsync(periodFile, "3");
        }

        private void RbPeriodFour_CheckedChanged(object sender, EventArgs e)
        {
            _ = WriteFileAsync(periodFile, "4");
        }

        private void RbPeriodOT_CheckedChanged(object sender, EventArgs e)
        {
            _ = WriteFileAsync(periodFile, txtPeriodOT.Text);
        }

        private void RegisterHotKeys()
        {
            String settingValue = Properties.Settings.Default.HotKeyStartStopGameClock;
            if (!string.IsNullOrEmpty(settingValue))
            {
                if (!GlobalHotKey.RegisterHotKey(settingValue, () => butStartStopGameClock.PerformClick()))
                {
                    MessageBox.Show(text: "Unable to register Hot Key for Start/Stop Game Clock!", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
                }
            }

            settingValue = Properties.Settings.Default.HotKeyStartStopPlayClock;
            if (!string.IsNullOrEmpty(settingValue))
            {
                if (!GlobalHotKey.RegisterHotKey(settingValue, () => butStartStopPlayClock.PerformClick()))
                {
                    MessageBox.Show(text: "Unable to register Hot Key for Start/Stop Play Clock!", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
                }
            }

            settingValue = Properties.Settings.Default.HotKeyNewPlayClock;
            if (!string.IsNullOrEmpty(settingValue))
            {
                if (!GlobalHotKey.RegisterHotKey(settingValue, () => butNewPlayClock.PerformClick()))
                {
                    MessageBox.Show(text: "Unable to register Hot Key for New Play Clock!", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
                }
            }

            settingValue = Properties.Settings.Default.HotKeyClearClocks;
            if (!string.IsNullOrEmpty(settingValue))
            {
                if (!GlobalHotKey.RegisterHotKey(settingValue, () => butClearClocks.PerformClick()))
                {
                    MessageBox.Show(text: "Unable to register Hot Key for Clear Clocks!", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
                }
            }

            settingValue = Properties.Settings.Default.HotKeyNextDown;
            if (!string.IsNullOrEmpty(settingValue))
            {
                if (!GlobalHotKey.RegisterHotKey(settingValue, () => NextDown()))
                {
                    MessageBox.Show(text: "Unable to register Hot Key for Next Down!", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
                }
            }

            settingValue = Properties.Settings.Default.HotKeyNextPeriod;
            if (!string.IsNullOrEmpty(settingValue))
            {
                if (!GlobalHotKey.RegisterHotKey(settingValue, () => NextPeriod()))
                {
                    MessageBox.Show(text: "Unable to register Hot Key for Next Period!", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
                }
            }

            settingValue = Properties.Settings.Default.HotKeyHome1;
            if (!string.IsNullOrEmpty(settingValue))
            {
                if (!GlobalHotKey.RegisterHotKey(settingValue, () => butHomeAddOne.PerformClick()))
                {
                    MessageBox.Show(text: "Unable to register Hot Key for Home Team 1 point!", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
                }
            }

            settingValue = Properties.Settings.Default.HotKeyHome2;
            if (!string.IsNullOrEmpty(settingValue))
            {
                if (!GlobalHotKey.RegisterHotKey(settingValue, () => butHomeAddTwo.PerformClick()))
                {
                    MessageBox.Show(text: "Unable to register Hot Key for Home Team 2 point2!", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
                }
            }

            settingValue = Properties.Settings.Default.HotKeyHome3;
            if (!string.IsNullOrEmpty(settingValue))
            {
                if (!GlobalHotKey.RegisterHotKey(settingValue, () => butHomeAddThree.PerformClick()))
                {
                    MessageBox.Show(text: "Unable to register Hot Key for Home Team 3 point!", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
                }
            }

            settingValue = Properties.Settings.Default.HotKeyHome6;
            if (!string.IsNullOrEmpty(settingValue))
            {
                if (!GlobalHotKey.RegisterHotKey(settingValue, () => butHomeAddSix.PerformClick()))
                {
                    MessageBox.Show(text: "Unable to register Hot Key for Home Team 6 point!", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
                }
            }

            settingValue = Properties.Settings.Default.HotKeyAway1;
            if (!string.IsNullOrEmpty(settingValue))
            {
                if (!GlobalHotKey.RegisterHotKey(settingValue, () => butAwayAddOne.PerformClick()))
                {
                    MessageBox.Show(text: "Unable to register Hot Key for Away Team 1 point!", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
                }
            }

            settingValue = Properties.Settings.Default.HotKeyAway2;
            if (!string.IsNullOrEmpty(settingValue))
            {
                if (!GlobalHotKey.RegisterHotKey(settingValue, () => butAwayAddTwo.PerformClick()))
                {
                    MessageBox.Show(text: "Unable to register Hot Key for Away Team 2 points!", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
                }
            }

            settingValue = Properties.Settings.Default.HotKeyAway3;
            if (!string.IsNullOrEmpty(settingValue))
            {
                if (!GlobalHotKey.RegisterHotKey(settingValue, () => butAwayAddThree.PerformClick()))
                {
                    MessageBox.Show(text: "Unable to register Hot Key for Away Team 3 points!", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
                }
            }

            settingValue = Properties.Settings.Default.HotKeyAway6;
            if (!string.IsNullOrEmpty(settingValue))
            {
                if (!GlobalHotKey.RegisterHotKey(settingValue, () => butAwayAddSix.PerformClick()))
                {
                    MessageBox.Show(text: "Unable to register Hot Key for Away Team 6 points!", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
                }
            }

            settingValue = Properties.Settings.Default.HotKeyPossession;
            if (!string.IsNullOrEmpty(settingValue))
            {
                if (!GlobalHotKey.RegisterHotKey(settingValue, () => TogglePossession()))
                {
                    MessageBox.Show(text: "Unable to register Hot Key for Possession!", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
                }
            }
        }

        private void TmrClockRefresh_Tick(object sender, EventArgs e)
        {
            if (periodClockRunning)
            {
                DecrementGameClock();
            }
            if (playClockRunning)
            {
                DecementPlayClock();
            }
        }

        private void ToolStripMenuItemAbout_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/hoga2443/AmericanFootballScoreboard/wiki");
        }

        private void ToolStripMenuItemReportIssue_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://github.com/hoga2443/AmericanFootballScoreboard/issues");
        }

        private void ToolStripMenuItemClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
            this.Close();
        }

        private void ToolStripMenuItemOpenOutputFolder_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
            {
                FileName = Properties.Settings.Default.OutputPath,
                UseShellExecute = true,
                Verb = "open"
            });
        }

        private void ToolStripMenuItemSaveHotKeys_Click(object sender, EventArgs e)
        {
            butSaveHotKey.PerformClick();
        }

        private void ToolStripMenuItemSaveSettings_Click(object sender, EventArgs e)
        {
            butSaveSettings.PerformClick();
        }

        private void TxtAwayScore_TextChanged(object sender, EventArgs e)
        {
            _ = WriteFileAsync(awayTeamScoreFile, txtAwayScore.Text);
        }

        private void TxtAwayTeam_Leave(object sender, EventArgs e)
        {
            _ = WriteFileAsync(awayTeamNameFile, txtAwayTeam.Text);
        }

        private void TxtAwayTimeouts_TextChanged(object sender, EventArgs e)
        {
            _ = WriteFileAsync(awayTimeoutsRemainingFile, txtAwayTimeouts.Text);
            switch (txtAwayTimeouts.Text)
            {
                case "0":
                    _ = CopyFileAsync(sourcePath: Path.Combine(Properties.Settings.Default.OutputPath, "AwayTimeouts\\AwayTimeouts.png"), destinationPath: Path.Combine(Properties.Settings.Default.OutputPath, "AwayTimeouts.png"));
                    break;
                case "1":
                    _ = CopyFileAsync(sourcePath: Path.Combine(Properties.Settings.Default.OutputPath, "AwayTimeouts\\AwayTimeouts.png"), destinationPath: Path.Combine(Properties.Settings.Default.OutputPath, "AwayTimeouts.png"));
                    break;
                case "2":
                    _ = CopyFileAsync(sourcePath: Path.Combine(Properties.Settings.Default.OutputPath, "AwayTimeouts\\AwayTimeouts.png"), destinationPath: Path.Combine(Properties.Settings.Default.OutputPath, "AwayTimeouts.png"));
                    break;
                case "3":
                    _ = CopyFileAsync(sourcePath: Path.Combine(Properties.Settings.Default.OutputPath, "AwayTimeouts\\AwayTimeouts.png"), destinationPath: Path.Combine(Properties.Settings.Default.OutputPath, "AwayTimeouts.png"));
                    break;
                default:
                    _ = CopyFileAsync(sourcePath: Path.Combine(Properties.Settings.Default.OutputPath, "AwayTimeouts\\AwayTimeouts.png"), destinationPath: Path.Combine(Properties.Settings.Default.OutputPath, "AwayTimeouts.png"));
                    break;
            }
        }

        private void TxtDistance_TextChanged(object sender, EventArgs e)
        {
            _ = WriteFileAsync(distanceFile, txtDistance.Text);
        }

        private void TxtHomeScore_TextChanged(object sender, EventArgs e)
        {
            _ = WriteFileAsync(homeTeamScoreFile, txtHomeScore.Text);
        }

        private void TxtHomeTeam_Leave(object sender, EventArgs e)
        {
            _ = WriteFileAsync(homeTeamNameFile, txtHomeTeam.Text);
        }

        private void TxtHomeTimeouts_TextChanged(object sender, EventArgs e)
        {
            _ = WriteFileAsync(homeTimeoutsRemainingFile, txtHomeTimeouts.Text);
            switch (txtHomeTimeouts.Text)
            {
                case "0":
                    _ = CopyFileAsync(sourcePath: Path.Combine(Properties.Settings.Default.OutputPath, "AwayTimeouts\\AwayTimeouts.png"), destinationPath: Path.Combine(Properties.Settings.Default.OutputPath, "AwayTimeouts.png"));
                    break;
                case "1":
                    _ = CopyFileAsync(sourcePath: Path.Combine(Properties.Settings.Default.OutputPath, "AwayTimeouts\\AwayTimeouts.png"), destinationPath: Path.Combine(Properties.Settings.Default.OutputPath, "AwayTimeouts.png"));
                    break;
                case "2":
                    _ = CopyFileAsync(sourcePath: Path.Combine(Properties.Settings.Default.OutputPath, "AwayTimeouts\\AwayTimeouts.png"), destinationPath: Path.Combine(Properties.Settings.Default.OutputPath, "AwayTimeouts.png"));
                    break;
                case "3":
                    _ = CopyFileAsync(sourcePath: Path.Combine(Properties.Settings.Default.OutputPath, "AwayTimeouts\\AwayTimeouts.png"), destinationPath: Path.Combine(Properties.Settings.Default.OutputPath, "AwayTimeouts.png"));
                    break;
                default:
                    _ = CopyFileAsync(sourcePath: Path.Combine(Properties.Settings.Default.OutputPath, "AwayTimeouts\\AwayTimeouts.png"), destinationPath: Path.Combine(Properties.Settings.Default.OutputPath, "AwayTimeouts.png"));
                    break;
            }
        }

        private void TxtPeriodOT_TextChanged(object sender, EventArgs e)
        {
            _ = WriteFileAsync(periodFile, txtPeriodOT.Text);
        }

        /*
        Write a specified value to a specified file.
        Values are only available to applications using the scoreboard after the file has been written.
        */
        static async Task WriteFileAsync(string file, string content)
        {
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(Properties.Settings.Default.OutputPath, file)))
            {
                await outputFile.WriteAsync(content);
            }
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            GlobalHotKey.DeRegisterHotKeys();
        }
    }
}
