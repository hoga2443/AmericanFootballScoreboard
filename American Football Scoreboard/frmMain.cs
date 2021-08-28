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

        private void LoadSettings()
        {
            txtPeriodDuration.Text = Properties.Settings.Default.DefaultPeriod;
            txtPlayClockDuration.Text = Properties.Settings.Default.DefaultPlay;
            txtGoalText.Text = Properties.Settings.Default.GoalText;
            txtOutputFolder.Text = Properties.Settings.Default.OutputPath;
            txtTimeoutsPerHalf.Text = Properties.Settings.Default.TimeoutsPerHalf;
            txtHotKeyStartStopGameClock.Text = Properties.Settings.Default.HotKeyStartStopGameClock;
            txtHotKeyStartStopPlayClock.Text = Properties.Settings.Default.HotKeyStartStopPlayClock;
            txtHotKeyNewPlayClock.Text = Properties.Settings.Default.HotKeyNewPlayClock;
            txtHotKeyClearClocks.Text = Properties.Settings.Default.HotKeyClearClocks;
            txtHotKeyNextDown.Text = Properties.Settings.Default.HotKeyNextDown;
            txtHotKeyNextPeriod.Text = Properties.Settings.Default.HotKeyNextPeriod;
        }

        private void InitializeUI()
        {
            txtGameClock.Text = Properties.Settings.Default.DefaultPeriod;
            _ = WriteFileAsync(gameClockFile, txtGameClock.Text);
            txtPlayClock.Text = Properties.Settings.Default.DefaultPlay;
            txtHomeTimeouts.Text = Properties.Settings.Default.TimeoutsPerHalf;
            txtAwayTimeouts.Text = Properties.Settings.Default.TimeoutsPerHalf;
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
            Properties.Settings.Default.Save();

            MessageBox.Show(text: "Restart Application to Re-load HotKeys", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Warning);
        }

        private void ButSaveSettings_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default["DefaultPeriod"] = txtPeriodDuration.Text;
            Properties.Settings.Default["DefaultPlay"] = txtPlayClockDuration.Text;
            Properties.Settings.Default["GoalText"] = txtGoalText.Text;
            Properties.Settings.Default["OutputPath"] = txtOutputFolder.Text;
            Properties.Settings.Default["TimeoutsPerHalf"] = txtTimeoutsPerHalf.Text;
            Properties.Settings.Default.Save();
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
            GlobalHotKey.RegisterHotKey(Properties.Settings.Default.HotKeyStartStopGameClock.ToString(), () => butStartStopGameClock.PerformClick());
            GlobalHotKey.RegisterHotKey(Properties.Settings.Default.HotKeyStartStopPlayClock.ToString(), () => butStartStopPlayClock.PerformClick());
            GlobalHotKey.RegisterHotKey(Properties.Settings.Default.HotKeyNewPlayClock.ToString(), () => butNewPlayClock.PerformClick());
            GlobalHotKey.RegisterHotKey(Properties.Settings.Default.HotKeyClearClocks.ToString(), () => butClearClocks.PerformClick());
            GlobalHotKey.RegisterHotKey(Properties.Settings.Default.HotKeyNextDown.ToString(), () => NextDown());
            GlobalHotKey.RegisterHotKey(Properties.Settings.Default.HotKeyNextPeriod.ToString(), () => NextPeriod());
            GlobalHotKey.RegisterHotKey(Properties.Settings.Default.HotKeyHome1.ToString(), () => butHomeAddOne.PerformClick());
            GlobalHotKey.RegisterHotKey(Properties.Settings.Default.HotKeyHome2.ToString(), () => butHomeAddTwo.PerformClick());
            GlobalHotKey.RegisterHotKey(Properties.Settings.Default.HotKeyHome3.ToString(), () => butHomeAddThree.PerformClick());
            GlobalHotKey.RegisterHotKey(Properties.Settings.Default.HotKeyHome6.ToString(), () => butHomeAddSix.PerformClick());
            GlobalHotKey.RegisterHotKey(Properties.Settings.Default.HotKeyAway1.ToString(), () => butAwayAddOne.PerformClick());
            GlobalHotKey.RegisterHotKey(Properties.Settings.Default.HotKeyAway2.ToString(), () => butAwayAddTwo.PerformClick());
            GlobalHotKey.RegisterHotKey(Properties.Settings.Default.HotKeyAway3.ToString(), () => butAwayAddThree.PerformClick());
            GlobalHotKey.RegisterHotKey(Properties.Settings.Default.HotKeyAway6.ToString(), () => butAwayAddSix.PerformClick());
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

        private void ToolStripMenuItemSaveSettings_Click(object sender, EventArgs e)
        {
            butSaveHotKey.PerformClick();
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
                    _ = CopyFileAsync(sourcePath: Path.Combine(Properties.Settings.Default.OutputPath, "Timeouts2\\0Timeouts.png"), destinationPath: Path.Combine(Properties.Settings.Default.OutputPath, "Timeouts2.png"));
                    break;
                case "1":
                    _ = CopyFileAsync(sourcePath: Path.Combine(Properties.Settings.Default.OutputPath, "Timeouts2\\1Timeouts.png"), destinationPath: Path.Combine(Properties.Settings.Default.OutputPath, "Timeouts2.png"));
                    break;
                case "2":
                    _ = CopyFileAsync(sourcePath: Path.Combine(Properties.Settings.Default.OutputPath, "Timeouts2\\2Timeouts.png"), destinationPath: Path.Combine(Properties.Settings.Default.OutputPath, "Timeouts2.png"));
                    break;
                case "3":
                    _ = CopyFileAsync(sourcePath: Path.Combine(Properties.Settings.Default.OutputPath, "Timeouts2\\3Timeouts.png"), destinationPath: Path.Combine(Properties.Settings.Default.OutputPath, "Timeouts2.png"));
                    break;
                default:
                    _ = CopyFileAsync(sourcePath: Path.Combine(Properties.Settings.Default.OutputPath, "Timeouts2\\0Timeouts.png"), destinationPath: Path.Combine(Properties.Settings.Default.OutputPath, "Timeouts2.png"));
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
                    _ = CopyFileAsync(sourcePath: Path.Combine(Properties.Settings.Default.OutputPath, "Timeouts1\\0Timeouts.png"), destinationPath: Path.Combine(Properties.Settings.Default.OutputPath, "Timeouts1.png"));
                    break;
                case "1":
                    _ = CopyFileAsync(sourcePath: Path.Combine(Properties.Settings.Default.OutputPath, "Timeouts1\\1Timeouts.png"), destinationPath: Path.Combine(Properties.Settings.Default.OutputPath, "Timeouts1.png"));
                    break;
                case "2":
                    _ = CopyFileAsync(sourcePath: Path.Combine(Properties.Settings.Default.OutputPath, "Timeouts1\\2Timeouts.png"), destinationPath: Path.Combine(Properties.Settings.Default.OutputPath, "Timeouts1.png"));
                    break;
                case "3":
                    _ = CopyFileAsync(sourcePath: Path.Combine(Properties.Settings.Default.OutputPath, "Timeouts1\\3Timeouts.png"), destinationPath: Path.Combine(Properties.Settings.Default.OutputPath, "Timeouts1.png"));
                    break;
                default:
                    _ = CopyFileAsync(sourcePath: Path.Combine(Properties.Settings.Default.OutputPath, "Timeouts1\\0Timeouts.png"), destinationPath: Path.Combine(Properties.Settings.Default.OutputPath, "Timeouts1.png"));
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
    }
}
