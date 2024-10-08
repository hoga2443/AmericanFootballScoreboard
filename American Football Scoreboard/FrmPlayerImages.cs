using OBSWebsocketDotNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace American_Football_Scoreboard
{
    public partial class FrmPlayerImages : Form
    {
        protected OBSWebsocket obs;
        public FrmPlayerImages()
        {
            InitializeComponent();
            PopulateImageButtonsAway();
            PopulateImageButtonsHome();
            obs = new OBSWebsocket();
            obs.Connected += OnConnect;
            ObsConnect();
        }
        private void ObsConnect()
        {
            if (!obs.IsConnected)
            {
                System.Threading.Tasks.Task.Run(() =>
                {
                    try
                    {
                        obs.ConnectAsync("ws://127.0.0.1:" + Properties.Settings.Default.WebSocketPort, Properties.Settings.Default.WebSocketPassword);
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
            BeginInvoke((MethodInvoker)(() =>
            {
            }));
        }
        private void PopulateImageButtonsAway()
        {
            PopulateImageButtons("AwayPlayers", gbAwayPlayers, "butAway", ShowAwayPlayer);
        }
        private void PopulateImageButtonsHome()
        {
            PopulateImageButtons("HomePlayers", gbHomePlayers, "butHome", ShowHomePlayer);
        }
        private static void PopulateImageButtons(string path, GroupBox groupBox, string buttonPrefix, EventHandler eventName)
        {
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
                    };
                    button.Click += eventName;
                    groupBox.Controls.Add(button);
                    buttonRow++;
                }
            }
        }
        private void ShowAwayPlayer(object sender, EventArgs e)
        {
            Common.ShowPlayer(false, (sender as Button).Text, obs: obs);
            tmrPlayerHome.Interval = Properties.Settings.Default.FlagDisplayDuration;
            tmrPlayerHome.Enabled = true;
            tmrPlayerHome.Start();
        }
        private void ShowHomePlayer(object sender, EventArgs e)
        {
            Common.ShowPlayer(true, (sender as Button).Text, obs: obs);
            tmrPlayerHome.Interval = Properties.Settings.Default.FlagDisplayDuration;
            tmrPlayerHome.Enabled = true;
            tmrPlayerHome.Start();
        }
        private void TmrPlayerAway_Tick(object sender, EventArgs e)
        {
            tmrPlayerAway.Enabled = false;
            Common.HidePlayer(home: false);
        }
        private void TmrPlayerHome_Tick(object sender, EventArgs e)
        {
            tmrPlayerHome.Enabled = false;
            Common.HidePlayer(home: true);
        }
        private void FrmPlayerImages_FormClosing(object sender, FormClosingEventArgs e)
        {
            obs.Disconnect();
        }
    }
}
