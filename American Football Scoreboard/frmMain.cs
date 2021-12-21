using Squirrel;
using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
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
        const string downDistanceFile = "DownDistance.txt";
        const string gameClockFile = "GameClock.txt";
        const string homeTeamNameFile = "HomeTeamName.txt";
        const string homeTeamScoreFile = "HomeTeamScore.txt";
        const string homeTimeoutsRemainingFile = "HomeTimeoutsRemaining.txt";
        const string periodFile = "Period.txt";
        const string playClockFile = "PlayClock.txt";
        const string scoreDescription = "ScoreDescription.txt";
        const string spotFile = "Spot.txt";
        const string supplementalFile = "Supplemental.txt";

        const char padZero = '0';

        private bool gameClockRunning = false;
        private DateTime periodClockEnd = DateTime.UtcNow;
        private TimeSpan periodTimeRemaining = new TimeSpan(hours: 0, minutes: 0, seconds: 0);

        private bool playClockRunning = false;
        private DateTime playTimeEnd = DateTime.UtcNow;
        private TimeSpan playTimeRemaining = new TimeSpan(hours: 0, minutes: 0, seconds: 0);

        private TimeSpan oneMinute = new TimeSpan(hours: 0, minutes: 1, seconds: 0);

        public FrmMain()
        {
            InitializeComponent();
            AddVersionNumber();
            LoadSettings();
            InitializeUI();
            RegisterHotKeys();
        }

        private void AddScore(TextBox textBox, int points, string message = "")
        {
            rbDownBlank.Checked = true;
            if (int.TryParse(s: textBox.Text, out int oldScore))
                textBox.Text = (oldScore + points).ToString();
            if (message != string.Empty)
            {
                _ = WriteFileAsync(file: scoreDescription, content: message);
                tmrScore.Interval = Properties.Settings.Default.FlagDisplayDuration;
                tmrScore.Enabled = true;
            }
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
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(fileName: assembly.Location);
            this.Text += $" v.{versionInfo.FileVersion }";
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

        private void ButAwayFieldGoal_Click(object sender, EventArgs e)
        {
            AddScore(textBox: txtAwayScore, points: Properties.Settings.Default.FieldGoal);
            WriteScore(text: "Field Goal");
        }

        private void ButAwayPatConversion_Click(object sender, EventArgs e)
        {
            AddScore(textBox: txtAwayScore, points: Properties.Settings.Default.PatConversion);
            WriteScore(text: "Conversion");
        }

        private void ButAwayPatKick_Click(object sender, EventArgs e)
        {
            AddScore(textBox: txtAwayScore, points: Properties.Settings.Default.PatKick);
            WriteScore(text: "PAT");
        }

        private void ShowHidePlayer(Boolean home, Button button, TextBox textBox)
        {
            if (button.Text == "Show")
            {
                button.Text = "Hide";
                if (!ShowPlayer(home: home, jersey: textBox.Text))
                {
                    textBox.Text = string.Empty;
                    button.Text = "Show";
                }
                tmrPlayer.Interval = Properties.Settings.Default.FlagDisplayDuration;
                tmrPlayer.Enabled = true;
                tmrPlayer.Start();
            }
            else
            {
                textBox.Text = string.Empty;
                button.Text = "Show";
                HidePlayer(home: home);
                tmrPlayer.Enabled = false;
            }
        }

        private void ButAwayPlayerShow_Click(object sender, EventArgs e)
        {
            ShowHidePlayer(home: false, button: butAwayPlayerShow, textBox: txtAwayPlayerNumber);
        }

        private void ButAwaySafety_Click(object sender, EventArgs e)
        {
            AddScore(textBox: txtAwayScore, points: Properties.Settings.Default.Safety);
            WriteScore(text: "Safety");
        }

        private void ButAwayTimeoutsAdd_Click(object sender, EventArgs e)
        {
            AddTimeout(control: txtAwayTimeouts, timeoutsToAdd: 1);
        }

        private void ButAwayTimeoutsSubtract_Click(object sender, EventArgs e)
        {
            AddTimeout(control: txtAwayTimeouts, timeoutsToAdd: -1);
        }

        private void ButAwayTouchdown_Click(object sender, EventArgs e)
        {
            AddScore(textBox: txtAwayScore, points: Properties.Settings.Default.Touchdown);
            WriteScore(text: "Touchdown");
        }

        private void ButClearAway_Click(object sender, EventArgs e)
        {
            txtAwayTimeouts.Text = Properties.Settings.Default.TimeoutsPerHalf;
            txtAwayScore.Text = "0";
            txtAwayTeam.Text = string.Empty;
            chkAwayPossession.Checked = false;
            _ = WriteFileAsync(file: awayTeamNameFile, content: txtAwayTeam.Text);
        }

        private void ButClearClocks_Click(object sender, EventArgs e)
        {
            ClearClocks();
        }

        private void ButClearHome_Click(object sender, EventArgs e)
        {
            txtHomeTimeouts.Text = Properties.Settings.Default.TimeoutsPerHalf;
            txtHomeScore.Text = "0";
            txtHomeTeam.Text = string.Empty;
            chkHomePossession.Checked = false;
            _ = WriteFileAsync(file: homeTeamNameFile, content: txtHomeTeam.Text);
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
            txtDistance.Text = String.Empty;
            txtSpot.Text = String.Empty;
        }

        private void ButHomeFieldGoal_Click(object sender, EventArgs e)
        {
            AddScore(textBox: txtHomeScore, points: Properties.Settings.Default.FieldGoal);
            WriteScore(text: "Field Goal");
        }

        private void ButHomePatConversion_Click(object sender, EventArgs e)
        {
            AddScore(textBox: txtHomeScore, points: Properties.Settings.Default.PatConversion);
            WriteScore(text: "Conversion");
        }

        private void ButHomePatKick_Click(object sender, EventArgs e)
        {
            AddScore(textBox: txtHomeScore, points: Properties.Settings.Default.PatKick);
            WriteScore(text: "PAT");
        }

        private void ButHomePlayerShow_Click(object sender, EventArgs e)
        {
            ShowHidePlayer(home: true, button: butHomePlayerShow, textBox: txtHomePlayerNumber);
        }

        private void ButHomeSafety_Click(object sender, EventArgs e)
        {
            AddScore(textBox: txtHomeScore, points: Properties.Settings.Default.Safety);
            WriteScore(text: "Safety");
        }

        private void ButHomeTimeoutsAdd_Click(object sender, EventArgs e)
        {
            AddTimeout(control: txtHomeTimeouts, timeoutsToAdd: 1);
        }

        private void ButHomeTimeoutsSubtract_Click(object sender, EventArgs e)
        {
            AddTimeout(control: txtHomeTimeouts, timeoutsToAdd: -1);
        }

        private void ButHomeTouchdown_Click(object sender, EventArgs e)
        {
            AddScore(textBox: txtHomeScore, points: Properties.Settings.Default.Touchdown);
            WriteScore(text: "Touchdown");
        }

        private void ButNewDefaultPlay_Click(object sender, EventArgs e)
        {
            SetStartPlayClock(duration: Properties.Settings.Default.DefaultPlayClock);
        }

        private void ButNewShortPlay_Click(object sender, EventArgs e)
        {
            SetStartPlayClock(duration: Properties.Settings.Default.ShortPlayClock);
        }

        private void ButOutputFolder_Click(object sender, EventArgs e)
        {
            DialogResult result = fbdOutput.ShowDialog();
            if (result == DialogResult.OK)
                txtOutputFolder.Text = fbdOutput.SelectedPath;
        }

        private void ButPeriodClear_Click(object sender, EventArgs e)
        {
            rbPeriodFinal.Checked = false;
            rbPeriodFour.Checked = false;
            rbPeriodHalf.Checked = false;
            rbPeriodOne.Checked = false;
            rbPeriodOT.Checked = false;
            rbPeriodThree.Checked = false;
            rbPeriodTwo.Checked = false;
            _ = WriteFileAsync(file: periodFile, content: string.Empty);
        }

        private void ButSaveHotKey_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default["HotKeyAwaySafety"] = txtHotKeyAwaySafety.Text;
            Properties.Settings.Default["HotKeyAwayPatKick"] = txtHotKeyAwayPatKick.Text;
            Properties.Settings.Default["HotKeyAwayPatConversion"] = txtHotKeyAwayPatConversion.Text;
            Properties.Settings.Default["HotKeyAwayFieldGoal"] = txtHotKeyAwayFieldGoal.Text;
            Properties.Settings.Default["HotKeyAwayTouchdown"] = txtHotKeyAwayTouchdown.Text;
            Properties.Settings.Default["HotKeyClearClocks"] = txtHotKeyClearClocks.Text;
            Properties.Settings.Default["HotKeyFlag"] = txtHotKeyFlag.Text;
            Properties.Settings.Default["HotKeyHomeSafety"] = txtHotKeyHomeSafety.Text;
            Properties.Settings.Default["HotKeyHomePatKick"] = txtHotKeyHomePatKick.Text;
            Properties.Settings.Default["HotKeyHomePatConversion"] = txtHotKeyHomePatConversion.Text;
            Properties.Settings.Default["HotKeyHomeFieldGoal"] = txtHotKeyHomeFieldGoal.Text;
            Properties.Settings.Default["HotKeyHomeTouchdown"] = txtHotKeyHomeTouchdown.Text;
            Properties.Settings.Default["HotKeyNewDefaultPlayClock"] = txtHotKeyNewDefaultPlayClock.Text;
            Properties.Settings.Default["HotKeyNewShortPlayClock"] = txtHotKeyNewShortPlayClock.Text;
            Properties.Settings.Default["HotKeyNextDown"] = txtHotKeyNextDown.Text;
            Properties.Settings.Default["HotKeyNextPeriod"] = txtHotKeyNextPeriod.Text;
            Properties.Settings.Default["HotKeyPossession"] = txtHotKeyPossession.Text;
            Properties.Settings.Default["HotKeyStartStopGameClock"] = txtHotKeyStartStopGameClock.Text;
            Properties.Settings.Default["HotKeyStartStopPlayClock"] = txtHotKeyStartStopPlayClock.Text;
            Properties.Settings.Default.Save();

            DialogResult result = MessageBox.Show(text: "Please re-start the application for new Hot Keys to take effect. Restart Now?", caption: "AFS", buttons: MessageBoxButtons.YesNo, icon: MessageBoxIcon.Information);
            if (result == DialogResult.Yes)
            {
                Application.Restart();
                Environment.Exit(exitCode: 0);
            }
        }

        private void ButSaveSettings_Click(object sender, EventArgs e)
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
            if (!int.TryParse(s: txtSettingPatConversion.Text, out int patConvertionPoints))
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
                Properties.Settings.Default["FlagDisplayDuration"] = flagDisplayDuration;
                Properties.Settings.Default["GoalText"] = txtGoalText.Text;
                Properties.Settings.Default["OutputPath"] = txtOutputFolder.Text;
                Properties.Settings.Default["Period1"] = txtSettingPeriod1.Text;
                Properties.Settings.Default["Period2"] = txtSettingPeriod2.Text;
                Properties.Settings.Default["Period3"] = txtSettingPeriod3.Text;
                Properties.Settings.Default["Period4"] = txtSettingPeriod4.Text;
                Properties.Settings.Default["PeriodHalf"] = txtSettingPeriodHalf.Text;
                Properties.Settings.Default["PeriodFinal"] = txtSettingPeriodFinal.Text;
                Properties.Settings.Default["RefreshInterval"] = refreshInterval;
                Properties.Settings.Default["ShortPlayClock"] = txtShortPlayClock.Text;
                Properties.Settings.Default["SubSecond"] = chkSubSecond.Checked;
                Properties.Settings.Default["TimeoutsPerHalf"] = txtTimeoutsPerHalf.Text;
                Properties.Settings.Default["Safety"] = safetyPoints;
                Properties.Settings.Default["PatKick"] = patKickPoints;
                Properties.Settings.Default["PatConversion"] = patConvertionPoints;
                Properties.Settings.Default["FieldGoal"] = patFieldGoalPoints;
                Properties.Settings.Default["Touchdown"] = patTouchdownPoints;

                Properties.Settings.Default.Save();
                tmrFlag.Interval = Properties.Settings.Default.FlagDisplayDuration;
                tmrClockRefresh.Interval = Properties.Settings.Default.RefreshInterval;
                this.TopMost = Properties.Settings.Default.AlwaysOnTop;
                MessageBox.Show(text: "Settings Saved Successfully", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Information);
            }
            else
                MessageBox.Show(text: errorMessage.Trim(), caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Warning);
        }

        private void ButSendSupplemental_Click(object sender, EventArgs e)
        {
            _ = WriteFileAsync(file: supplementalFile, content: txtSupplemental.Text);
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
                if (txtGameClock.Text.Trim() == string.Empty)
                    txtGameClock.Text = Properties.Settings.Default.DefaultPeriod.ToString();
                if (txtGameClock.Text.Contains("."))
                {
                    periodTimeRemaining = new TimeSpan(days: 0, hours: 0, minutes: int.Parse(txtGameClock.Text.Substring(startIndex: 0, length: 1)), seconds: int.Parse(txtGameClock.Text.Substring(startIndex: 2, length: 2)), milliseconds: int.Parse(txtGameClock.Text.Substring(startIndex: 5, length: 1)) * 100);
                }
                else
                {
                    periodTimeRemaining = new TimeSpan(hours: 0, minutes: int.Parse(txtGameClock.Text.Substring(startIndex: 0, length: 2)), seconds: int.Parse(txtGameClock.Text.Substring(startIndex: 3, length: 2)));
                }
                periodClockEnd = DateTime.UtcNow + periodTimeRemaining;
                butStartStopGameClock.Text = "Stop Game Clock";
            }
            gameClockRunning = !gameClockRunning;
            tmrClockRefresh.Enabled = gameClockRunning;
        }

        private void ButStartStopPlayClock_Click(object sender, EventArgs e)
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

        private async Task CheckForUpdates()
        {
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
        }

        private void ChkAwayPossession_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAwayPossession.Checked)
            {
                chkHomePossession.Checked = false;
                _ = CopyFileAsync(sourcePath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "Possession\\AwayPossession.png"), destinationPath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "AwayPossession.png"));
                rbDownOne.Checked = true;
            }
            else
                _ = CopyFileAsync(sourcePath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "Possession\\NonPossession.png"), destinationPath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "AwayPossession.png"));
        }

        private void ChkFlag_CheckedChanged(object sender, EventArgs e)
        {
            if (chkFlag.Checked)
            {
                _ = CopyFileAsync(sourcePath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "Flag\\Flag.png"), destinationPath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "Flag.png"));
                tmrFlag.Interval = Properties.Settings.Default.FlagDisplayDuration;
                tmrFlag.Enabled = true;
                tmrFlag.Start();
            }
            else
            {
                _ = CopyFileAsync(sourcePath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "Flag\\NoFlag.png"), destinationPath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "Flag.png"));
                tmrFlag.Stop();
                tmrFlag.Enabled = false;
            }
        }

        private void ChkHomePossession_CheckedChanged(object sender, EventArgs e)
        {
            if (chkHomePossession.Checked)
            {
                chkAwayPossession.Checked = false;
                _ = CopyFileAsync(sourcePath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "Possession\\HomePossession.png"), destinationPath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "HomePossession.png"));
                rbDownOne.Checked = true;
            }
            else
                _ = CopyFileAsync(sourcePath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "Possession\\NonPossession.png"), destinationPath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "HomePossession.png"));
        }

        private void ClearClocks()
        {
            tmrClockRefresh.Enabled = false;
            playClockRunning = false;
            txtGameClock.Text = Properties.Settings.Default.DefaultPeriod;
            _ = WriteFileAsync(file: gameClockFile, content: txtGameClock.Text);
            txtPlayClock.Text = Properties.Settings.Default.DefaultPlayClock;
            _ = WriteFileAsync(file: playClockFile, content: txtPlayClock.Text);
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
                    await source.CopyToAsync(destination: destination);
                }
            }
        }

        private void DecrementGameClock()
        {
            periodTimeRemaining = periodClockEnd - DateTime.UtcNow;
            if (periodTimeRemaining < oneMinute && Properties.Settings.Default.SubSecond)
                txtGameClock.Text = "0:" + periodTimeRemaining.Seconds.ToString().PadLeft(totalWidth: 2, paddingChar: padZero) + "." + periodTimeRemaining.Milliseconds.ToString().Substring(startIndex: 0, length: 1);
            else
                txtGameClock.Text = periodTimeRemaining.Minutes.ToString().PadLeft(totalWidth: 2, paddingChar: padZero) + ":" + periodTimeRemaining.Seconds.ToString().PadLeft(totalWidth: 2, paddingChar: padZero);
            _ = WriteFileAsync(file: gameClockFile, content: txtGameClock.Text);
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

        private void DecementPlayClock()
        {
            playTimeRemaining = playTimeEnd - DateTime.UtcNow;
            txtPlayClock.Text = ((int)playTimeRemaining.TotalSeconds).ToString();
            _ = WriteFileAsync(file: playClockFile, content: txtPlayClock.Text);
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

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            GlobalHotKey.DeRegisterHotKeys();
        }

        private void HidePlayer(bool home)
        {
            if (home)
                _ = CopyFileAsync(sourcePath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "HomePlayers\\blank.png"), destinationPath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "Player.png"));
            else
                _ = CopyFileAsync(sourcePath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "AwayPlayers\\blank.png"), destinationPath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "Player.png"));
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
            else if (quarter == Properties.Settings.Default.Period3)
                rbPeriodThree.Checked = true;
            else if (quarter == Properties.Settings.Default.Period4)
                rbPeriodFour.Checked = true;
            else if (quarter == Properties.Settings.Default.PeriodFinal)
                rbPeriodFinal.Checked = true;
            else
            {
                rbPeriodOT.Checked = true;
                txtPeriodOT.Text = quarter;
            }
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
            InitializeTextBox(textBox: txtSupplemental, fileName: supplementalFile);
            chkHomePossession.Checked = true;
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
            this.TopMost = Properties.Settings.Default.AlwaysOnTop;
        }

        private void LoadHotKeySettings()
        {
            txtHotKeyHomeSafety.Text = Properties.Settings.Default.HotKeyHomeSafety;
            txtHotKeyHomePatKick.Text = Properties.Settings.Default.HotKeyHomePatKick;
            txtHotKeyHomePatConversion.Text = Properties.Settings.Default.HotKeyHomePatConversion;
            txtHotKeyHomeFieldGoal.Text = Properties.Settings.Default.HotKeyHomeFieldGoal;
            txtHotKeyHomeTouchdown.Text = Properties.Settings.Default.HotKeyHomeTouchdown;
            txtHotKeyAwaySafety.Text = Properties.Settings.Default.HotKeyAwaySafety;
            txtHotKeyAwayPatKick.Text = Properties.Settings.Default.HotKeyAwayPatKick;
            txtHotKeyAwayPatConversion.Text = Properties.Settings.Default.HotKeyAwayPatConversion;
            txtHotKeyAwayFieldGoal.Text = Properties.Settings.Default.HotKeyAwayFieldGoal;
            txtHotKeyAwayTouchdown.Text = Properties.Settings.Default.HotKeyAwayTouchdown;
            txtHotKeyClearClocks.Text = Properties.Settings.Default.HotKeyClearClocks;
            txtHotKeyFlag.Text = Properties.Settings.Default.HotKeyFlag;
            txtHotKeyNewDefaultPlayClock.Text = Properties.Settings.Default.HotKeyNewDefaultPlayClock;
            txtHotKeyNewShortPlayClock.Text = Properties.Settings.Default.HotKeyNewShortPlayClock;
            txtHotKeyNextDown.Text = Properties.Settings.Default.HotKeyNextDown;
            txtHotKeyNextPeriod.Text = Properties.Settings.Default.HotKeyNextPeriod;
            txtHotKeyPossession.Text = Properties.Settings.Default.HotKeyPossession;
            txtHotKeyStartStopGameClock.Text = Properties.Settings.Default.HotKeyStartStopGameClock;
            txtHotKeyStartStopPlayClock.Text = Properties.Settings.Default.HotKeyStartStopPlayClock;
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
            UpdateDownAndDistance();
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
            UpdateDownAndDistance();
        }

        private void RbDownTwo_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDownTwo.Checked)
                _ = WriteFileAsync(file: downFile, content: Properties.Settings.Default.Down2);
            UpdateDownAndDistance();
        }

        private void RbPeriodFinal_CheckedChanged(object sender, EventArgs e)
        {
            _ = WriteFileAsync(file: periodFile, content: Properties.Settings.Default.PeriodFinal);
        }

        private void RbPeriodFour_CheckedChanged(object sender, EventArgs e)
        {
            _ = WriteFileAsync(file: periodFile, content: Properties.Settings.Default.Period4);
        }

        private void RbPeriodHalf_CheckedChanged(object sender, EventArgs e)
        {
            txtHomeTimeouts.Text = "0";
            txtAwayTimeouts.Text = "0";
            rbDownBlank.Checked = true;
            txtDistance.Text = string.Empty;
            txtSpot.Text = string.Empty;
            chkAwayPossession.Checked = false;
            chkHomePossession.Checked = false;
            _ = WriteFileAsync(file: periodFile, content: Properties.Settings.Default.PeriodHalf);
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
            txtHomeTimeouts.Text = Properties.Settings.Default.TimeoutsPerHalf;
            txtAwayTimeouts.Text = Properties.Settings.Default.TimeoutsPerHalf;
            _ = WriteFileAsync(file: periodFile, content: Properties.Settings.Default.Period3);
        }

        private void RbPeriodTwo_CheckedChanged(object sender, EventArgs e)
        {
            _ = WriteFileAsync(file: periodFile, content: Properties.Settings.Default.Period2);
        }

        private void RegisterHotKeys()
        {
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
            if (!GlobalHotKey.RegisterHotKey(aKeyGestureString: Properties.Settings.Default.HotKeyFlag, aAction: () => ToggleFlag()))
                MessageBox.Show(text: "Unable to register Hot Key for Flag!", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Error);
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

        private void SetStartPlayClock(string duration)
        {
            txtPlayClock.Text = duration;
            playTimeRemaining = new TimeSpan(hours: 0, minutes: 0, seconds: int.Parse(txtPlayClock.Text));
            playTimeEnd = DateTime.UtcNow + playTimeRemaining;
            butStartStopPlayClock.Text = "Stop Play Clock";
            if (!tmrClockRefresh.Enabled)
                tmrClockRefresh.Enabled = true;
            playClockRunning = true;
        }

        private bool ShowPlayer(bool home, string jersey)
        {
            bool success = false;
            if (home)
            {
                if (!File.Exists(Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "HomePlayers\\" + jersey + ".png")))
                {
                    MessageBox.Show(text: "Image not found.", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Warning);
                }
                else
                {
                    _ = CopyFileAsync(sourcePath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "HomePlayers\\" + jersey + ".png"), destinationPath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "Player.png"));
                    success = true;
                }
            }
            else
            {
                if (!File.Exists(Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "AwayPlayers\\" + jersey + ".png")))
                {
                    MessageBox.Show(text: "Image not found.", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Warning);
                }
                else
                {
                    _ = CopyFileAsync(sourcePath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "AwayPlayers\\" + jersey + ".png"), destinationPath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "Player.png"));
                    success = true;
                }
            }
            return success;
        }

        private void TmrClockRefresh_Tick(object sender, EventArgs e)
        {
            if (gameClockRunning)
                DecrementGameClock();
            if (playClockRunning)
                DecementPlayClock();
        }

        private void TmrFlag_Tick(object sender, EventArgs e)
        {
            chkFlag.Checked = false;
        }

        private void TmrPlayer_Tick(object sender, EventArgs e)
        {
            if (butHomePlayerShow.Text == "Hide")
                butHomePlayerShow.PerformClick();
            else if (butAwayPlayerShow.Text == "Hide")
                butAwayPlayerShow.PerformClick();
        }

        private void TmrScore_Tick(object sender, EventArgs e)
        {
            _ = WriteFileAsync(file: scoreDescription, content: String.Empty);
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
                _ = WriteFileAsync(file: awayTeamScoreFile, content: txtAwayScore.Text);
        }

        private void TxtAwayTeam_Leave(object sender, EventArgs e)
        {
            _ = WriteFileAsync(file: awayTeamNameFile, content: txtAwayTeam.Text);
        }

        private void TxtAwayTimeouts_TextChanged(object sender, EventArgs e)
        {
            _ = WriteFileAsync(file: awayTimeoutsRemainingFile, content: txtAwayTimeouts.Text);
            switch (txtAwayTimeouts.Text)
            {
                case "0":
                    _ = CopyFileAsync(sourcePath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "AwayTimeouts\\0Timeouts.png"), destinationPath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "AwayTimeouts.png"));
                    break;
                case "1":
                    _ = CopyFileAsync(sourcePath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "AwayTimeouts\\1Timeouts.png"), destinationPath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "AwayTimeouts.png"));
                    break;
                case "2":
                    _ = CopyFileAsync(sourcePath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "AwayTimeouts\\2Timeouts.png"), destinationPath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "AwayTimeouts.png"));
                    break;
                default:
                    _ = CopyFileAsync(sourcePath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "AwayTimeouts\\3Timeouts.png"), destinationPath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "AwayTimeouts.png"));
                    break;
            }
        }

        private void TxtDistance_TextChanged(object sender, EventArgs e)
        {
            _ = WriteFileAsync(file: distanceFile, content: txtDistance.Text);
            UpdateDownAndDistance();
        }

        private void TxtGameClock_Leave(object sender, EventArgs e)
        {
            if (!ValidTime(time: txtGameClock.Text))
                MessageBox.Show(text: "Please enter a valid time mm:ss.", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Exclamation);
            else
                _ = WriteFileAsync(file: gameClockFile, content: txtGameClock.Text);
        }

        private void TxtHomePlayerNumber_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                butHomePlayerShow.PerformClick();
            }
        }

        private void TxtHomeScore_TextChanged(object sender, EventArgs e)
        {
            if (!int.TryParse(s: txtHomeScore.Text, result: out _) && txtHomeScore.Text != string.Empty)
                MessageBox.Show(text: "Please enter an integer for home score.", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Exclamation);
            else
                _ = WriteFileAsync(file: homeTeamScoreFile, content: txtHomeScore.Text);
        }

        private void TxtHomeTeam_Leave(object sender, EventArgs e)
        {
            _ = WriteFileAsync(file: homeTeamNameFile, content: txtHomeTeam.Text);
        }

        private void TxtHomeTimeouts_TextChanged(object sender, EventArgs e)
        {
            _ = WriteFileAsync(file: homeTimeoutsRemainingFile, content: txtHomeTimeouts.Text);
            switch (txtHomeTimeouts.Text)
            {
                case "0":
                    _ = CopyFileAsync(sourcePath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "HomeTimeouts\\0Timeouts.png"), destinationPath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "HomeTimeouts.png"));
                    break;
                case "1":
                    _ = CopyFileAsync(sourcePath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "HomeTimeouts\\1Timeouts.png"), destinationPath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "HomeTimeouts.png"));
                    break;
                case "2":
                    _ = CopyFileAsync(sourcePath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "HomeTimeouts\\2Timeouts.png"), destinationPath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "HomeTimeouts.png"));
                    break;
                default:
                    _ = CopyFileAsync(sourcePath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "HomeTimeouts\\3Timeouts.png"), destinationPath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "HomeTimeouts.png"));
                    break;
            }
        }

        private void TxtPeriodOT_TextChanged(object sender, EventArgs e)
        {
            _ = WriteFileAsync(file: periodFile, content: txtPeriodOT.Text);
        }

        private void TxtPlayClock_Leave(object sender, EventArgs e)
        {
            if (!int.TryParse(s: txtPlayClock.Text, result: out _) && txtPlayClock.Text != string.Empty)
                MessageBox.Show(text: "Please enter an integer for play clock.", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Exclamation);
            else
                _ = WriteFileAsync(file: playClockFile, content: txtPlayClock.Text);
        }

        private void TxtSpot_TextChanged(object sender, EventArgs e)
        {
            _ = WriteFileAsync(file: spotFile, content: txtSpot.Text);
        }

        private void UpdateDownAndDistance()
        {
            if (rbDownOne.Checked)
                _ = WriteFileAsync(file: downDistanceFile, content: Properties.Settings.Default.Down1 + " & " + txtDistance.Text);
            if (rbDownTwo.Checked)
                _ = WriteFileAsync(file: downDistanceFile, content: Properties.Settings.Default.Down2 + " & " + txtDistance.Text);
            if (rbDownThree.Checked)
                _ = WriteFileAsync(file: downDistanceFile, content: Properties.Settings.Default.Down3 + " & " + txtDistance.Text);
            if (rbDownFour.Checked)
                _ = WriteFileAsync(file: downDistanceFile, content: Properties.Settings.Default.Down4 + " & " + txtDistance.Text);
            if (rbDownBlank.Checked)
                _ = WriteFileAsync(file: downDistanceFile, content: string.Empty);
        }

        /*
        Validate time in in a valid mm:ss format
        */
        private bool ValidTime(string time)
        {
            if (time == string.Empty)
                return true;
            if (time.Contains(value: "."))
            {
                if (time.Length > 6)
                    return false;
                if (time.Substring(startIndex: 0, length: 1) != "0")
                    return false;
                if (time.Substring(startIndex: 1, length: 1) != ":")
                    return false;
                if (!int.TryParse(time.Substring(startIndex: 2, length: 2), out int seconds))
                    return false;
                if (seconds > 59)
                    return false;
                if (time.Substring(startIndex: 4, length: 1) != ".")
                    return false;
                if (!int.TryParse(time.Substring(startIndex: 5, length: 1), out _))
                    return false;
            }
            else
            {
                if (time.Length != 5)
                    return false;
                if (!int.TryParse(time.Substring(startIndex: 0, length: 2), out _))
                    return false;
                if (time.Substring(startIndex: 2, length: 1) != ":")
                    return false;
                if (!int.TryParse(time.Substring(startIndex: 3, length: 2), out int seconds))
                    return false;
                if (seconds > 59)
                    return false;
            }
            return true;
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

        private void WriteScore(string text)
        {
            _ = WriteFileAsync(file: scoreDescription, content: text);
            tmrScore.Interval = Properties.Settings.Default.FlagDisplayDuration;
            tmrScore.Enabled = true;
            tmrScore.Start();
        }
    }
}
