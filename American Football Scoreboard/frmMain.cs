using Squirrel;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;
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

        private bool gameClockRunning = false;
        private DateTime periodClockEnd = DateTime.UtcNow;
        private TimeSpan periodTimeRemaining = new TimeSpan(0, 0, 0);

        private bool playClockRunning = false;
        private DateTime playTimeEnd = DateTime.UtcNow;
        private TimeSpan playTimeRemaining = new TimeSpan(0, 0, 0);

        private bool advanceQuarter = false;

        public FrmMain()
        {
            InitializeComponent();
            AddVersionNumber();
            LoadSettings();
            InitializeUI();
            RegisterHotKeys();
        }

        private void AddScore(TextBox control, int points)
        {
            if(int.TryParse(s: control.Text, out int oldScore))
                control.Text = (oldScore + points).ToString();
        }

        /*
        Method to increase/decrease the number of timeouts in a specified control
        Called by all functions which alter a number of timeouts
        */
        private void AddTimeout(TextBox control, int timeoutsToAdd)
        {
            if (int.TryParse(s: control.Text, out int currentTimeouts))
                control.Text = (currentTimeouts + timeoutsToAdd).ToString();
            else
                control.Text = (timeoutsToAdd).ToString();
        }

        private void AddVersionNumber()
        {
            System.Reflection.Assembly assembly = System.Reflection.Assembly.GetExecutingAssembly();
            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(assembly.Location);
            this.Text += $" v.{versionInfo.FileVersion }";
        }

        private void AdvanceQuarter()
        {
            butStartStopGameClock.Text = "Start Game Clock";
            gameClockRunning = false;
            if (rbPeriodOne.Checked)
            {
                rbPeriodTwo.Checked = true;
                ClearClocks();
            }
            else if (rbPeriodTwo.Checked)
            {
                rbPeriodThree.Checked = true;
                ClearClocks();
            }
            else if (rbPeriodThree.Checked)
            {
                rbPeriodFour.Checked = true;
                ClearClocks();
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
            ClearClocks();
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
            rbDownBlank.Checked = true;
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
                tmrClockRefresh.Enabled = true;
            playClockRunning = true;
        }

        private void ButOutputFolder_Click(object sender, EventArgs e)
        {
            DialogResult result = fbdOutput.ShowDialog();
            if (result == DialogResult.OK)
                txtOutputFolder.Text = fbdOutput.SelectedPath;
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
            string errorMessage = "";
            if (txtPeriodDuration.Text.Length != 5)
                errorMessage += "Default Period Duration must be in format 00:00.  ";
            if (!int.TryParse(s: txtPlayClockDuration.Text, out int playClock))
                errorMessage += "Default Play Clock must be an integer.  ";
            if (string.IsNullOrEmpty(txtOutputFolder.Text))
                errorMessage += "Please specify an output folder.  ";
            if (!int.TryParse(s: txtRefreshInterval.Text, out int refreshInterval))
                errorMessage += "Refresh Interval must be an integer.  ";
            if (string.IsNullOrEmpty(errorMessage))
            {
                Properties.Settings.Default["DefaultPeriod"] = txtPeriodDuration.Text;
                Properties.Settings.Default["DefaultPlay"] = txtPlayClockDuration.Text;
                Properties.Settings.Default["GoalText"] = txtGoalText.Text;
                Properties.Settings.Default["OutputPath"] = txtOutputFolder.Text;
                Properties.Settings.Default["TimeoutsPerHalf"] = txtTimeoutsPerHalf.Text;
                Properties.Settings.Default["RefreshInterval"] = refreshInterval;
                tmrClockRefresh.Interval = Properties.Settings.Default.RefreshInterval;
                Properties.Settings.Default["AdvanceQuarter"] = chkAdvanceQuarter.Checked;
                Properties.Settings.Default["Down1"] = txtSettingDown1.Text;
                Properties.Settings.Default["Down2"] = txtSettingDown2.Text;
                Properties.Settings.Default["Down3"] = txtSettingDown3.Text;
                Properties.Settings.Default["Down4"] = txtSettingDown4.Text;
                Properties.Settings.Default["Period1"] = txtSettingPeriod1.Text;
                Properties.Settings.Default["Period2"] = txtSettingPeriod2.Text;
                Properties.Settings.Default["Period3"] = txtSettingPeriod3.Text;
                Properties.Settings.Default["Period4"] = txtSettingPeriod4.Text;
                Properties.Settings.Default.Save();
                MessageBox.Show(text: "Settings Saved Successfully", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
            }
            else
                MessageBox.Show(text: errorMessage.Trim(), caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Warning);
        }

        private void ButSendSupplemental_Click(object sender, EventArgs e)
        {
            _ = WriteFileAsync(supplementalFile, txtSupplemental.Text);
        }

        private void ButStartStopGameClock_Click(object sender, EventArgs e)
        {
            if (gameClockRunning)
            {
                if (!playClockRunning)
                    tmrClockRefresh.Enabled = false;
                butStartStopGameClock.Text = "Start Game Clock";
            }
            else
            {
                if (!tmrClockRefresh.Enabled)
                    tmrClockRefresh.Enabled = true;
                if (txtGameClock.Text.Trim().Length == 0)
                    txtGameClock.Text = Properties.Settings.Default.DefaultPeriod.ToString();
                else if (txtGameClock.Text.Trim().Length < 5)
                    txtGameClock.Text = ("00000" + txtGameClock.Text).Substring(txtGameClock.Text.Length);
                periodTimeRemaining = new TimeSpan(0, int.Parse(txtGameClock.Text.Substring(0, 2)), int.Parse(txtGameClock.Text.Substring(3, 2)));
                periodClockEnd = DateTime.UtcNow + periodTimeRemaining;
                butStartStopGameClock.Text = "Stop Game Clock";
            }
            gameClockRunning = !gameClockRunning;
        }

        private void ButStartStopPlayClock_Click(object sender, EventArgs e)
        {
            if (playClockRunning)
            {
                playClockRunning = false;
                if (!playClockRunning)
                    tmrClockRefresh.Enabled = false;
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

        private async Task CheckForUpdates()
        {
            using (var manager = await UpdateManager.GitHubUpdateManager(@"https://github.com/hoga2443/AmericanFootballScoreboard"))
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
                        result = MessageBox.Show(text: $"New version {updateResult.Version} has been installed and will take effect when AFS is restarted. Restart Now?", 
                            caption: "AFS", 
                            buttons: MessageBoxButtons.YesNo, 
                            icon: MessageBoxIcon.Question);
                        if (result == DialogResult.Yes)
                        {
                            Application.Restart();
                            Environment.Exit(0);
                        }
                    }
                    else
                        MessageBox.Show(text: "You have the latest version, no update is available!", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
                }
                catch
                {
                    MessageBox.Show(text: "Error checking for updates!", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
                }
            }
        }

        private void ChkAwayPossession_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAwayPossession.Checked)
            {
                chkHomePossession.Checked = false;
                _ = CopyFileAsync(sourcePath: Path.Combine(Properties.Settings.Default.OutputPath, "Possession\\Possession.png"), destinationPath: Path.Combine(Properties.Settings.Default.OutputPath, "AwayPossession.png"));
                rbDownOne.Checked = true;
            }
            else
                _ = CopyFileAsync(sourcePath: Path.Combine(Properties.Settings.Default.OutputPath, "Possession\\NonPossession.png"), destinationPath: Path.Combine(Properties.Settings.Default.OutputPath, "AwayPossession.png"));
        }

        private void ChkHomePossession_CheckedChanged(object sender, EventArgs e)
        {
            if (chkHomePossession.Checked)
            {
                chkAwayPossession.Checked = false;
                _ = CopyFileAsync(sourcePath: Path.Combine(Properties.Settings.Default.OutputPath, "Possession\\Possession.png"), destinationPath: Path.Combine(Properties.Settings.Default.OutputPath, "HomePossession.png"));
                rbDownOne.Checked = true;
            }
            else
                _ = CopyFileAsync(sourcePath: Path.Combine(Properties.Settings.Default.OutputPath, "Possession\\NonPossession.png"), destinationPath: Path.Combine(Properties.Settings.Default.OutputPath, "HomePossession.png"));
        }

        private void ClearClocks()
        {
            tmrClockRefresh.Enabled = false;
            playClockRunning = false;
            txtGameClock.Text = Properties.Settings.Default.DefaultPeriod;
            _ = WriteFileAsync(gameClockFile, txtGameClock.Text);
            txtPlayClock.Text = Properties.Settings.Default.DefaultPlay;
            _ = WriteFileAsync(playClockFile, txtPlayClock.Text);
        }

        /*
        Copy a file in the filesystem
        Used to update the timeouts remaining and possession images
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
            if (DateTime.Compare(periodClockEnd, DateTime.UtcNow) <= 0)
                txtGameClock.Text = "00:00";
            if (txtGameClock.Text == "00:00")
            {
                gameClockRunning = false;
                butStartStopGameClock.Text = "Start Game Clock";
                if (advanceQuarter)
                    AdvanceQuarter();
                if (!playClockRunning)
                    tmrClockRefresh.Enabled = false;
            }
        }

        private void DecementPlayClock()
        {
            playTimeRemaining = playTimeEnd - DateTime.UtcNow;
            txtPlayClock.Text = ((int)playTimeRemaining.TotalSeconds).ToString();
            _ = WriteFileAsync(playClockFile, txtPlayClock.Text);
            if (DateTime.Compare(playTimeEnd, DateTime.UtcNow) <= 0)
                txtPlayClock.Text = "0";
            if (txtPlayClock.Text == "0")
            {
                playClockRunning = false;
                butStartStopPlayClock.Text = "Start Play Clock";
                if (!gameClockRunning)
                    tmrClockRefresh.Enabled = false;
            }
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            GlobalHotKey.DeRegisterHotKeys();
        }

        private void InitializeDown()
        {
            string currentFile = Path.Combine(Properties.Settings.Default.OutputPath, downFile);
            if (!File.Exists(currentFile))
            {
                FileStream fs = File.Create(currentFile);
                fs.Close();
            }
            string down = File.ReadAllText(currentFile);
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
            string currentFile = Path.Combine(Properties.Settings.Default.OutputPath, periodFile);
            if (!File.Exists(currentFile))
            {
                FileStream fs = File.Create(currentFile);
                fs.Close();
            }

            string quarter = File.ReadAllText(currentFile);
            if (quarter == Properties.Settings.Default.Period1)
                rbPeriodOne.Checked = true;
            else if (quarter == Properties.Settings.Default.Period2)
                rbPeriodTwo.Checked = true;
            else if (quarter == Properties.Settings.Default.Period3)
                rbPeriodThree.Checked = true;
            else if (quarter == Properties.Settings.Default.Period4)
                rbPeriodFour.Checked = true;
            else
            {
                rbPeriodOT.Checked = true;
                txtPeriodOT.Text = quarter;
            }
        }

        private static void InitializeTextBox(TextBox textBox, string fileName)
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
            chkHomePossession.Checked = true;
            InitializeDown();
            InitializeQuarter();
        }

        private void LoadApplicationSettings()
        {
            txtPeriodDuration.Text = Properties.Settings.Default.DefaultPeriod;
            txtPlayClockDuration.Text = Properties.Settings.Default.DefaultPlay;
            txtGoalText.Text = Properties.Settings.Default.GoalText;
            txtOutputFolder.Text = Properties.Settings.Default.OutputPath;
            txtTimeoutsPerHalf.Text = Properties.Settings.Default.TimeoutsPerHalf;
            tmrClockRefresh.Interval = Properties.Settings.Default.RefreshInterval;
            chkAdvanceQuarter.Checked = Properties.Settings.Default.AdvanceQuarter;
            txtRefreshInterval.Text = Properties.Settings.Default.RefreshInterval.ToString();
            txtSettingPeriod1.Text = Properties.Settings.Default.Period1;
            txtSettingPeriod2.Text = Properties.Settings.Default.Period2;
            txtSettingPeriod3.Text = Properties.Settings.Default.Period3;
            txtSettingPeriod4.Text = Properties.Settings.Default.Period4;
            txtSettingDown1.Text = Properties.Settings.Default.Down1;
            txtSettingDown2.Text = Properties.Settings.Default.Down2;
            txtSettingDown3.Text = Properties.Settings.Default.Down3;
            txtSettingDown4.Text = Properties.Settings.Default.Down4;
        }

        private void LoadHotKeySettings()
        {
            txtHotKeyHome1.Text = Properties.Settings.Default.HotKeyHome1;
            txtHotKeyHome2.Text = Properties.Settings.Default.HotKeyHome2;
            txtHotKeyHome3.Text = Properties.Settings.Default.HotKeyHome3;
            txtHotKeyHome6.Text = Properties.Settings.Default.HotKeyHome6;
            txtHotKeyAway1.Text = Properties.Settings.Default.HotKeyAway1;
            txtHotKeyAway2.Text = Properties.Settings.Default.HotKeyAway2;
            txtHotKeyAway3.Text = Properties.Settings.Default.HotKeyAway3;
            txtHotKeyAway6.Text = Properties.Settings.Default.HotKeyAway6;
            txtHotKeyPossession.Text = Properties.Settings.Default.HotKeyPossession;
            txtHotKeyStartStopGameClock.Text = Properties.Settings.Default.HotKeyStartStopGameClock;
            txtHotKeyStartStopPlayClock.Text = Properties.Settings.Default.HotKeyStartStopPlayClock;
            txtHotKeyNewPlayClock.Text = Properties.Settings.Default.HotKeyNewPlayClock;
            txtHotKeyClearClocks.Text = Properties.Settings.Default.HotKeyClearClocks;
            txtHotKeyNextDown.Text = Properties.Settings.Default.HotKeyNextDown;
            txtHotKeyNextPeriod.Text = Properties.Settings.Default.HotKeyNextPeriod;
        }

        private void LoadSettings()
        {
            LoadApplicationSettings();
            LoadHotKeySettings();
        }

        private void NextDown()
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

        private void NextPeriod()
        {
            if (rbPeriodOne.Checked == true)
                rbPeriodTwo.Checked = true;
            else if (rbPeriodTwo.Checked == true)
                rbPeriodThree.Checked = true;
            else if (rbPeriodThree.Checked == true)
                rbPeriodFour.Checked = true;
            else
            {
                rbPeriodOne.Checked = true;
                txtDistance.Text = "10";
            }
        }

        private void RbDownBlank_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDownBlank.Checked)
            {
                _ = WriteFileAsync(file: downFile, content: string.Empty);
                txtDistance.Text = string.Empty;
            }
        }

        private void RbDownFour_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDownFour.Checked)
                _ = WriteFileAsync(file: downFile, content: Properties.Settings.Default.Down4);
        }

        private void RbDownOne_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDownOne.Checked)
            {
                _ = WriteFileAsync(file: downFile, content: Properties.Settings.Default.Down1);
                txtDistance.Text = "10";
            }
        }

        private void RbDownThree_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDownThree.Checked)
                _ = WriteFileAsync(file: downFile, content: Properties.Settings.Default.Down3);
        }

        private void RbDownTwo_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDownTwo.Checked)
                _ = WriteFileAsync(file: downFile, content: Properties.Settings.Default.Down2);
        }

        private void RbPeriodFour_CheckedChanged(object sender, EventArgs e)
        {
            _ = WriteFileAsync(file: periodFile, content: Properties.Settings.Default.Period4);
        }

        private void RbPeriodOne_CheckedChanged(object sender, EventArgs e)
        {
            _ = WriteFileAsync(file: periodFile, content: Properties.Settings.Default.Period1);
        }

        private void RbPeriodOT_CheckedChanged(object sender, EventArgs e)
        {
            _ = WriteFileAsync(file: periodFile, content: txtPeriodOT.Text);
        }

        private void RbPeriodThree_CheckedChanged(object sender, EventArgs e)
        {
            _ = WriteFileAsync(file: periodFile, content: Properties.Settings.Default.Period3);
        }

        private void RbPeriodTwo_CheckedChanged(object sender, EventArgs e)
        {
            _ = WriteFileAsync(file: periodFile, content: Properties.Settings.Default.Period2);
        }

        private void RegisterHotKeys()
        {
            String settingValue = Properties.Settings.Default.HotKeyStartStopGameClock;
            if (!string.IsNullOrEmpty(settingValue))
            {
                if (!GlobalHotKey.RegisterHotKey(settingValue, () => butStartStopGameClock.PerformClick()))
                    MessageBox.Show(text: "Unable to register Hot Key for Start/Stop Game Clock!", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
            }

            settingValue = Properties.Settings.Default.HotKeyStartStopPlayClock;
            if (!string.IsNullOrEmpty(settingValue))
            {
                if (!GlobalHotKey.RegisterHotKey(settingValue, () => butStartStopPlayClock.PerformClick()))
                    MessageBox.Show(text: "Unable to register Hot Key for Start/Stop Play Clock!", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
            }

            settingValue = Properties.Settings.Default.HotKeyNewPlayClock;
            if (!string.IsNullOrEmpty(settingValue))
            {
                if (!GlobalHotKey.RegisterHotKey(settingValue, () => butNewPlayClock.PerformClick()))
                    MessageBox.Show(text: "Unable to register Hot Key for New Play Clock!", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
            }

            settingValue = Properties.Settings.Default.HotKeyClearClocks;
            if (!string.IsNullOrEmpty(settingValue))
            {
                if (!GlobalHotKey.RegisterHotKey(settingValue, () => butClearClocks.PerformClick()))
                    MessageBox.Show(text: "Unable to register Hot Key for Clear Clocks!", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
            }

            settingValue = Properties.Settings.Default.HotKeyNextDown;
            if (!string.IsNullOrEmpty(settingValue))
            {
                if (!GlobalHotKey.RegisterHotKey(settingValue, () => NextDown()))
                    MessageBox.Show(text: "Unable to register Hot Key for Next Down!", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
            }

            settingValue = Properties.Settings.Default.HotKeyNextPeriod;
            if (!string.IsNullOrEmpty(settingValue))
            {
                if (!GlobalHotKey.RegisterHotKey(settingValue, () => NextPeriod()))
                    MessageBox.Show(text: "Unable to register Hot Key for Next Period!", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
            }

            settingValue = Properties.Settings.Default.HotKeyHome1;
            if (!string.IsNullOrEmpty(settingValue))
            {
                if (!GlobalHotKey.RegisterHotKey(settingValue, () => butHomeAddOne.PerformClick()))
                    MessageBox.Show(text: "Unable to register Hot Key for Home Team 1 point!", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
            }

            settingValue = Properties.Settings.Default.HotKeyHome2;
            if (!string.IsNullOrEmpty(settingValue))
            {
                if (!GlobalHotKey.RegisterHotKey(settingValue, () => butHomeAddTwo.PerformClick()))
                    MessageBox.Show(text: "Unable to register Hot Key for Home Team 2 point2!", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
            }

            settingValue = Properties.Settings.Default.HotKeyHome3;
            if (!string.IsNullOrEmpty(settingValue))
            {
                if (!GlobalHotKey.RegisterHotKey(settingValue, () => butHomeAddThree.PerformClick()))
                    MessageBox.Show(text: "Unable to register Hot Key for Home Team 3 point!", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
            }

            settingValue = Properties.Settings.Default.HotKeyHome6;
            if (!string.IsNullOrEmpty(settingValue))
            {
                if (!GlobalHotKey.RegisterHotKey(settingValue, () => butHomeAddSix.PerformClick()))
                    MessageBox.Show(text: "Unable to register Hot Key for Home Team 6 point!", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
            }

            settingValue = Properties.Settings.Default.HotKeyAway1;
            if (!string.IsNullOrEmpty(settingValue))
            {
                if (!GlobalHotKey.RegisterHotKey(settingValue, () => butAwayAddOne.PerformClick()))
                    MessageBox.Show(text: "Unable to register Hot Key for Away Team 1 point!", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
            }

            settingValue = Properties.Settings.Default.HotKeyAway2;
            if (!string.IsNullOrEmpty(settingValue))
            {
                if (!GlobalHotKey.RegisterHotKey(settingValue, () => butAwayAddTwo.PerformClick()))
                    MessageBox.Show(text: "Unable to register Hot Key for Away Team 2 points!", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
            }

            settingValue = Properties.Settings.Default.HotKeyAway3;
            if (!string.IsNullOrEmpty(settingValue))
            {
                if (!GlobalHotKey.RegisterHotKey(settingValue, () => butAwayAddThree.PerformClick()))
                    MessageBox.Show(text: "Unable to register Hot Key for Away Team 3 points!", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
            }

            settingValue = Properties.Settings.Default.HotKeyAway6;
            if (!string.IsNullOrEmpty(settingValue))
            {
                if (!GlobalHotKey.RegisterHotKey(settingValue, () => butAwayAddSix.PerformClick()))
                    MessageBox.Show(text: "Unable to register Hot Key for Away Team 6 points!", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
            }

            settingValue = Properties.Settings.Default.HotKeyPossession;
            if (!string.IsNullOrEmpty(settingValue))
            {
                if (!GlobalHotKey.RegisterHotKey(settingValue, () => TogglePossession()))
                    MessageBox.Show(text: "Unable to register Hot Key for Possession!", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
            }
        }

        private void TmrClockRefresh_Tick(object sender, EventArgs e)
        {
            if (gameClockRunning)
                DecrementGameClock();
            if (playClockRunning)
                DecementPlayClock();
        }

        private void TogglePossession()
        {
            if (!chkHomePossession.Checked)
                chkHomePossession.Checked = true;
            else
                chkAwayPossession.Checked = true;
        }

        private void ToolStripMenuItemAbout_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/hoga2443/AmericanFootballScoreboard/wiki");
        }

        private void ToolStripMenuItemCheckForUpdate_Click(object sender, EventArgs e)
        {
            CheckForUpdates();
        }

        private void ToolStripMenuItemClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
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
            Process.Start("https://github.com/hoga2443/AmericanFootballScoreboard/issues");
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
                    _ = CopyFileAsync(sourcePath: Path.Combine(Properties.Settings.Default.OutputPath, "AwayTimeouts\\0Timeouts.png"), destinationPath: Path.Combine(Properties.Settings.Default.OutputPath, "AwayTimeouts.png"));
                    break;
                case "1":
                    _ = CopyFileAsync(sourcePath: Path.Combine(Properties.Settings.Default.OutputPath, "AwayTimeouts\\1Timeouts.png"), destinationPath: Path.Combine(Properties.Settings.Default.OutputPath, "AwayTimeouts.png"));
                    break;
                case "2":
                    _ = CopyFileAsync(sourcePath: Path.Combine(Properties.Settings.Default.OutputPath, "AwayTimeouts\\2Timeouts.png"), destinationPath: Path.Combine(Properties.Settings.Default.OutputPath, "AwayTimeouts.png"));
                    break;
                case "3":
                    _ = CopyFileAsync(sourcePath: Path.Combine(Properties.Settings.Default.OutputPath, "AwayTimeouts\\3Timeouts.png"), destinationPath: Path.Combine(Properties.Settings.Default.OutputPath, "AwayTimeouts.png"));
                    break;
                default:
                    _ = CopyFileAsync(sourcePath: Path.Combine(Properties.Settings.Default.OutputPath, "AwayTimeouts\\0Timeouts.png"), destinationPath: Path.Combine(Properties.Settings.Default.OutputPath, "AwayTimeouts.png"));
                    break;
            }
        }

        private void TxtDistance_TextChanged(object sender, EventArgs e)
        {
            _ = WriteFileAsync(distanceFile, txtDistance.Text);
        }

        private void TxtGameClock_Leave(object sender, EventArgs e)
        {
            _ = WriteFileAsync(gameClockFile, txtGameClock.Text);
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
                    _ = CopyFileAsync(sourcePath: Path.Combine(Properties.Settings.Default.OutputPath, "HomeTimeouts\\0Timeouts.png"), destinationPath: Path.Combine(Properties.Settings.Default.OutputPath, "HomeTimeouts.png"));
                    break;
                case "1":
                    _ = CopyFileAsync(sourcePath: Path.Combine(Properties.Settings.Default.OutputPath, "HomeTimeouts\\1Timeouts.png"), destinationPath: Path.Combine(Properties.Settings.Default.OutputPath, "HomeTimeouts.png"));
                    break;
                case "2":
                    _ = CopyFileAsync(sourcePath: Path.Combine(Properties.Settings.Default.OutputPath, "HomeTimeouts\\2Timeouts.png"), destinationPath: Path.Combine(Properties.Settings.Default.OutputPath, "HomeTimeouts.png"));
                    break;
                case "3":
                    _ = CopyFileAsync(sourcePath: Path.Combine(Properties.Settings.Default.OutputPath, "HomeTimeouts\\3Timeouts.png"), destinationPath: Path.Combine(Properties.Settings.Default.OutputPath, "HomeTimeouts.png"));
                    break;
                default:
                    _ = CopyFileAsync(sourcePath: Path.Combine(Properties.Settings.Default.OutputPath, "HomeTimeouts\\0Timeouts.png"), destinationPath: Path.Combine(Properties.Settings.Default.OutputPath, "HomeTimeouts.png"));
                    break;
            }
        }

        private void TxtPeriodOT_TextChanged(object sender, EventArgs e)
        {
            _ = WriteFileAsync(periodFile, txtPeriodOT.Text);
        }

        private void TxtPlayClock_Leave(object sender, EventArgs e)
        {
            _ = WriteFileAsync(playClockFile, txtPlayClock.Text);
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
