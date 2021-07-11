using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace American_Football_Scoreboard
{
    public partial class frmMain : Form
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

        public frmMain()
        {
            InitializeComponent();
            LoadSettings();
            InitializeUI();
        }

        private void LoadSettings()
        {
            txtPeriodDuration.Text = Properties.Settings.Default.DefaultPeriod;
            txtPlayClockDuration.Text = Properties.Settings.Default.DefaultPlay;
            txtGoalText.Text = Properties.Settings.Default.GoalText;
            txtOutputFolder.Text = Properties.Settings.Default.OutputPath;
            txtTimeoutsPerHalf.Text = Properties.Settings.Default.TimeoutsPerHalf;
        }

        private void InitializeUI()
        {
            txtGameClock.Text = Properties.Settings.Default.DefaultPeriod;
            Task asyncTask = WriteFileAsync(gameClockFile, txtGameClock.Text);
            txtPlayClock.Text = Properties.Settings.Default.DefaultPlay;
            txtHomeTimeouts.Text = Properties.Settings.Default.TimeoutsPerHalf;
            txtAwayTimeouts.Text = Properties.Settings.Default.TimeoutsPerHalf;
        }

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
            txtGameClock.Text = Properties.Settings.Default.DefaultPeriod;
            txtPlayClock.Text = Properties.Settings.Default.DefaultPlay;
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
            rbPeriod1.Checked = false;
            rbPeriod2.Checked = false;
            rbPeriod3.Checked = false;
            rbPeriod4.Checked = false;
            rbPeriodOT.Checked = false;
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
            Task asyncTask = WriteFileAsync(supplementalFile, txtSupplemental.Text);
        }

        private void ButStartGameClock_Click(object sender, EventArgs e)
        {
            if (!tmrClockRefresh.Enabled)
                tmrClockRefresh.Enabled = true;
            if (txtGameClock.Text.Trim().Length == 0)
                txtGameClock.Text = Properties.Settings.Default.DefaultPeriod.ToString();
            periodTimeRemaining = new TimeSpan(0, int.Parse(txtGameClock.Text.Substring(0, 2)), int.Parse(txtGameClock.Text.Substring(3, 2)));
            periodClockEnd = DateTime.UtcNow + periodTimeRemaining;
            periodClockRunning = true;
        }

        private void ButStartPlayClock_Click(object sender, EventArgs e)
        {
            if (!tmrClockRefresh.Enabled)
                tmrClockRefresh.Enabled = true;
            if (txtPlayClock.Text.Trim().Length == 0)
                txtPlayClock.Text = Properties.Settings.Default.DefaultPlay.ToString();
            playTimeRemaining = new TimeSpan(0, 0, int.Parse(txtPlayClock.Text));
            playTimeEnd = DateTime.UtcNow + playTimeRemaining;
            playClockRunning = true;
        }

        private void ButStopGameClock_Click(object sender, EventArgs e)
        {
            periodClockRunning = false;
            if (!playClockRunning)
            {
                tmrClockRefresh.Enabled = false;
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
            Task asyncTask = WriteFileAsync(gameClockFile, txtGameClock.Text);
            if (txtGameClock.Text == "00:00")
                periodClockRunning = false;
        }

        private void DecementPlayClock()
        {
            playTimeRemaining = playTimeEnd - DateTime.UtcNow;
            txtPlayClock.Text = (int.Parse(txtPlayClock.Text) - 1).ToString();
            Task asyncTask = WriteFileAsync(playClockFile, txtPlayClock.Text);
            if (txtPlayClock.Text == "0")
            {
                playClockRunning = false;
            }
        }

        private void RbDownFour_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDownFour.Checked)
            {
                Task asyncTask = WriteFileAsync(downFile, "4th");
            }
        }

        private void RbDownOne_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDownOne.Checked)
            {
                Task asyncTask = WriteFileAsync(downFile, "1st");
                txtDistance.Text = "10";
            }
        }

        private void RbDownThree_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDownThree.Checked)
            {
                Task asyncTask = WriteFileAsync(downFile, "3rd");
            }
        }

        private void RbDownTwo_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDownTwo.Checked)
            {
                Task asyncTask = WriteFileAsync(downFile, "2nd");
            }
        }

        private void RbPeriod1_CheckedChanged(object sender, EventArgs e)
        {
            Task asyncTask = WriteFileAsync(periodFile, "1");
        }

        private void RbPeriod2_CheckedChanged(object sender, EventArgs e)
        {
            Task asyncTask = WriteFileAsync(periodFile, "2");
        }

        private void RbPeriod3_CheckedChanged(object sender, EventArgs e)
        {
            Task asyncTask = WriteFileAsync(periodFile, "3");
        }

        private void RbPeriod4_CheckedChanged(object sender, EventArgs e)
        {
            Task asyncTask = WriteFileAsync(periodFile, "4");
        }

        private void RbPeriodOT_CheckedChanged(object sender, EventArgs e)
        {
            Task asyncTask = WriteFileAsync(periodFile, txtPeriodOT.Text);
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

        private void TxtAwayScore_TextChanged(object sender, EventArgs e)
        {
            Task asyncTask = WriteFileAsync(awayTeamScoreFile, txtAwayScore.Text);
        }

        private void TxtAwayTeam_Leave(object sender, EventArgs e)
        {
            Task asyncTask = WriteFileAsync(awayTeamNameFile, txtAwayTeam.Text);
        }

        private void TxtAwayTimeouts_TextChanged(object sender, EventArgs e)
        {
            Task asyncTask = WriteFileAsync(awayTimeoutsRemainingFile, txtAwayTimeouts.Text);
            switch (txtAwayTimeouts.Text)
            {
                case "0":
                    var task0 = CopyFileAsync(sourcePath: Path.Combine(Properties.Settings.Default.OutputPath, "Timeouts2\\0Timeouts.png"), destinationPath: Path.Combine(Properties.Settings.Default.OutputPath, "Timeouts2.png"));
                    break;
                case "1":
                    var task1 = CopyFileAsync(sourcePath: Path.Combine(Properties.Settings.Default.OutputPath, "Timeouts2\\1Timeouts.png"), destinationPath: Path.Combine(Properties.Settings.Default.OutputPath, "Timeouts2.png"));
                    break;
                case "2":
                    var task2 = CopyFileAsync(sourcePath: Path.Combine(Properties.Settings.Default.OutputPath, "Timeouts2\\2Timeouts.png"), destinationPath: Path.Combine(Properties.Settings.Default.OutputPath, "Timeouts2.png"));
                    break;
                case "3":
                    var task3 = CopyFileAsync(sourcePath: Path.Combine(Properties.Settings.Default.OutputPath, "Timeouts2\\3Timeouts.png"), destinationPath: Path.Combine(Properties.Settings.Default.OutputPath, "Timeouts2.png"));
                    break;
                default:
                    var task4 = CopyFileAsync(sourcePath: Path.Combine(Properties.Settings.Default.OutputPath, "Timeouts2\\0Timeouts.png"), destinationPath: Path.Combine(Properties.Settings.Default.OutputPath, "Timeouts2.png"));
                    break;
            }
        }

        private void TxtDistance_TextChanged(object sender, EventArgs e)
        {
            Task asyncTask = WriteFileAsync(distanceFile, txtDistance.Text);
        }

        private void TxtHomeScore_TextChanged(object sender, EventArgs e)
        {
            Task asyncTask = WriteFileAsync(homeTeamScoreFile, txtHomeScore.Text);
        }

        private void TxtHomeTeam_Leave(object sender, EventArgs e)
        {
            Task asyncTask = WriteFileAsync(homeTeamNameFile, txtHomeTeam.Text);
        }

        private void TxtHomeTimeouts_TextChanged(object sender, EventArgs e)
        {
            Task asyncTask = WriteFileAsync(homeTimeoutsRemainingFile, txtHomeTimeouts.Text);
            switch (txtHomeTimeouts.Text)
            {
                case "0":
                    var task0 = CopyFileAsync(sourcePath: Path.Combine(Properties.Settings.Default.OutputPath, "Timeouts1\\0Timeouts.png"), destinationPath: Path.Combine(Properties.Settings.Default.OutputPath, "Timeouts1.png"));
                    break;
                case "1":
                    var task1 = CopyFileAsync(sourcePath: Path.Combine(Properties.Settings.Default.OutputPath, "Timeouts1\\1Timeouts.png"), destinationPath: Path.Combine(Properties.Settings.Default.OutputPath, "Timeouts1.png"));
                    break;
                case "2":
                    var task2 = CopyFileAsync(sourcePath: Path.Combine(Properties.Settings.Default.OutputPath, "Timeouts1\\2Timeouts.png"), destinationPath: Path.Combine(Properties.Settings.Default.OutputPath, "Timeouts1.png"));
                    break;
                case "3":
                    var task3 = CopyFileAsync(sourcePath: Path.Combine(Properties.Settings.Default.OutputPath, "Timeouts1\\3Timeouts.png"), destinationPath: Path.Combine(Properties.Settings.Default.OutputPath, "Timeouts1.png"));
                    break;
                default:
                    var task4 = CopyFileAsync(sourcePath: Path.Combine(Properties.Settings.Default.OutputPath, "Timeouts1\\0Timeouts.png"), destinationPath: Path.Combine(Properties.Settings.Default.OutputPath, "Timeouts1.png"));
                    break;
            }
        }

        private void TxtPeriodOT_TextChanged(object sender, EventArgs e)
        {
            Task asyncTask = WriteFileAsync(periodFile, txtPeriodOT.Text);
        }

        static async Task WriteFileAsync(string file, string content)
        {
            using (StreamWriter outputFile = new StreamWriter(Path.Combine(Properties.Settings.Default.OutputPath, file)))
            {
                await outputFile.WriteAsync(content);
            }
        }
    }
}
