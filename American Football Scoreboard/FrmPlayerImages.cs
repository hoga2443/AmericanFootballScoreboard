using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace American_Football_Scoreboard
{
    public partial class FrmPlayerImages : Form
    {
        public FrmPlayerImages()
        {
            InitializeComponent();
            PopulateImageButtonsAway();
            PopulateImageButtonsHome();
            tmrPlayerAway.Interval = Properties.Settings.Default.FlagDisplayDuration;
        }
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
        private void HidePlayer(bool home)
        {
            if (home)
                _ = CopyFileAsync(sourcePath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "HomePlayers\\Blank." + Properties.Settings.Default["PlayerImageFileType"]), destinationPath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "HomePlayer." + Properties.Settings.Default["PlayerImageFileType"]));
            else
                _ = CopyFileAsync(sourcePath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "AwayPlayers\\Blank." + Properties.Settings.Default["PlayerImageFileType"]), destinationPath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "AwayPlayer." + Properties.Settings.Default["PlayerImageFileType"]));
        }
        private void PopulateImageButtonsAway()
        {
            PopulateImageButtons("AwayPlayers", gbAwayPlayers, "butAway", ShowAwayPlayer);
        }
        private void PopulateImageButtonsHome()
        {
            PopulateImageButtons("HomePlayers", gbHomePlayers, "butHome", ShowHomePlayer);
        }
        private void PopulateImageButtons(string path, GroupBox groupBox, string buttonPrefix, EventHandler eventName)
        {
            string sourcePath = Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: path);
            string[] directoryContents = Directory.GetFiles(sourcePath);
            List<int> numbers = new List<int>();
            foreach (string file in directoryContents)
            {
                string playerNumber = file.Substring(file.LastIndexOf("\\") + 1, file.IndexOf(".") - file.LastIndexOf("\\") - 1);
                if (int.TryParse(playerNumber, out int number))
                {
                    numbers.Add(number);
                }
            }
            numbers.Sort((x, y) => x.CompareTo(y));
            int buttonRow = 0;
            int buttonColumn = 0;
            foreach (var number in numbers)
            {
                if (buttonRow >= 17)
                {
                    buttonRow = 0;
                    buttonColumn++;
                }
                Button button = new Button
                {
                    Name = buttonPrefix + number.ToString(),
                    Text = number.ToString(),
                    Top = 20 + buttonRow * 22,
                    Left = 10 + buttonColumn * 80,
                };
                button.Click += eventName;
                groupBox.Controls.Add(button);
                buttonRow++;
            }
        }
        private void ShowAwayPlayer(object sender, EventArgs e)
        {
            ShowPlayer(false, (sender as Button).Text);
        }
        private void ShowHomePlayer(object sender, EventArgs e)
        {
            ShowPlayer(true, (sender as Button).Text);
        }
        private bool ShowPlayer(bool home, string jersey)
        {
            bool success = false;
            string destinationPath;
            string sourcePath;
            if (home)
            {
                destinationPath = Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "HomePlayer." + Properties.Settings.Default["PlayerImageFileType"]);
                sourcePath = Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "HomePlayers\\" + jersey + "." + Properties.Settings.Default["PlayerImageFileType"]);
            }
            else
            {
                destinationPath = Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "AwayPlayer." + Properties.Settings.Default["PlayerImageFileType"]);
                sourcePath = Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "AwayPlayers\\" + jersey + "." + Properties.Settings.Default["PlayerImageFileType"]);
            }
            if (!File.Exists(sourcePath))
            {
                MessageBox.Show(text: "Image not found.", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Warning);
            }
            else
            {
                _ = CopyFileAsync(sourcePath: sourcePath, destinationPath: destinationPath);
                success = true;
                if (home)
                {
                    tmrPlayerHome.Interval = Properties.Settings.Default.FlagDisplayDuration;
                    tmrPlayerHome.Enabled = true;
                    tmrPlayerHome.Start();
                }
                else
                {
                    tmrPlayerAway.Interval = Properties.Settings.Default.FlagDisplayDuration;
                    tmrPlayerAway.Enabled = true;
                    tmrPlayerAway.Start();
                }
            }
            return success;
        }
        private void TmrPlayerAway_Tick(object sender, EventArgs e)
        {
            tmrPlayerAway.Enabled = false;
            HidePlayer(home: false);
        }
        private void TmrPlayerHome_Tick(object sender, EventArgs e)
        {
            tmrPlayerHome.Enabled = false;
            HidePlayer(home: true);
        }
    }
}
