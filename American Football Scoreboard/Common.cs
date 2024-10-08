using American_Football_Scoreboard.Properties;
using Newtonsoft.Json.Linq;
using OBSWebsocketDotNet;
using System.Data.SQLite;
using System.IO;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

namespace American_Football_Scoreboard
{
    internal class Common
    {
        private string DestinationPath;
        private OBSWebsocket OBS;
        private readonly System.Timers.Timer PlayerVideoTimer = new();
        const string playerAwayHeightFile = "PlayerAwayHeight.txt";
        const string playerAwayTownFile = "PlayerAwayTown.txt";
        const string playerAwayNameFile = "PlayerAwayName.txt";
        const string playerAwayPositionFile = "PlayerAwayPosition.txt";
        const string playerAwayNumberFile = "PlayerAwayNumber.txt";
        const string playerAwayWeightFile = "PlayerAwayWeight.txt";
        const string playerAwayYearFile = "PlayerAwayYear.txt";
        const string playerHomeHeightFile = "PlayerHomeHeight.txt";
        const string playerHomeTownFile = "PlayerHomeTown.txt";
        const string playerHomeNameFile = "PlayerHomeName.txt";
        const string playerHomePositionFile = "PlayerHomePosition.txt";
        const string playerHomeNumberFile = "PlayerHomeNumber.txt";
        const string playerHomeWeightFile = "PlayerHomeWeight.txt";
        const string playerHomeYearFile = "PlayerHomeYear.txt";
        public static void CopyFile(string sourcePath, string destinationPath)
        {
            try
            {
                using Stream source = File.Open(path: sourcePath, mode: FileMode.Open);
                using Stream destination = File.Create(path: destinationPath);
                source.CopyTo(destination: destination);
            }
            catch { }
        }
        public static async Task CopyFileAsync(string sourcePath, string destinationPath)
        {
            using Stream source = File.Open(path: sourcePath, mode: FileMode.Open);
            using Stream destination = File.Create(path: destinationPath);
            await source.CopyToAsync(destination: destination);
        }
        public static SQLiteConnection CreateSQLiteConnection()
        {
            SQLiteConnection sQLiteConnection;
            string connectionString = "Data Source=" + Path.Combine(Properties.Settings.Default["OutputPath"].ToString(), "AmericanFootballScoreboard.sqlite3") + ";Version=3;Cache=Shared;";
            sQLiteConnection = new SQLiteConnection(connectionString);
            try
            {
                sQLiteConnection.Open();
            }
            catch { }
            return sQLiteConnection;
        }
        public static void HidePlayer(bool home)
        {
            if (home)
            {
                _ = CopyFileAsync(sourcePath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "HomePlayers\\Blank." + Properties.Settings.Default["PlayerImageFileType"]), destinationPath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "HomePlayer." + Properties.Settings.Default["PlayerImageFileType"]));
                _ = WriteFileAsync(file: playerHomeHeightFile, content: string.Empty);
                _ = WriteFileAsync(file: playerHomeTownFile, content: string.Empty);
                _ = WriteFileAsync(file: playerHomeNameFile, content: string.Empty);
                _ = WriteFileAsync(file: playerHomePositionFile, content: string.Empty);
                _ = WriteFileAsync(file: playerHomeNumberFile, content: string.Empty);
                _ = WriteFileAsync(file: playerHomeWeightFile, content: string.Empty);
                _ = WriteFileAsync(file: playerHomeYearFile, content: string.Empty);
            }
            else
            {
                _ = CopyFileAsync(sourcePath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "AwayPlayers\\Blank." + Properties.Settings.Default["PlayerImageFileType"]), destinationPath: Path.Combine(path1: Properties.Settings.Default.OutputPath, path2: "AwayPlayer." + Properties.Settings.Default["PlayerImageFileType"]));
                _ = WriteFileAsync(file: playerAwayHeightFile, content: string.Empty);
                _ = WriteFileAsync(file: playerAwayTownFile, content: string.Empty);
                _ = WriteFileAsync(file: playerAwayNameFile, content: string.Empty);
                _ = WriteFileAsync(file: playerAwayPositionFile, content: string.Empty);
                _ = WriteFileAsync(file: playerAwayNumberFile, content: string.Empty);
                _ = WriteFileAsync(file: playerAwayWeightFile, content: string.Empty);
                _ = WriteFileAsync(file: playerAwayYearFile, content: string.Empty);
            }
        }
        private static bool ReadPlayerFromDatabase(bool home, int number, out string height, out string homeTown, out string name, out string position, out string weight, out string year)
        {
            bool success = false;
            height = string.Empty;
            homeTown = string.Empty;
            name = string.Empty;
            position = string.Empty;
            weight = string.Empty;
            year = string.Empty;
            SQLiteConnection sqLiteConnection = CreateSQLiteConnection();
            SQLiteDataReader sqLiteDataReader;
            SQLiteCommand sqLiteCommand = sqLiteConnection.CreateCommand();
            sqLiteCommand.CommandText = "SELECT Name, Position, Height, Weight, Year, Hometown FROM players WHERE Home = " + home + " AND Number = '" + number + "';";
            sqLiteDataReader = sqLiteCommand.ExecuteReader();
            if (sqLiteDataReader.Read())
            {
                height = sqLiteDataReader["Height"].ToString();
                homeTown = sqLiteDataReader["Hometown"].ToString();
                name = sqLiteDataReader["Name"].ToString();
                position = sqLiteDataReader["Position"].ToString();
                weight = sqLiteDataReader["Weight"].ToString();
                year = sqLiteDataReader["Year"].ToString();
                success = true;
            }
            sqLiteConnection.Close();
            return success;
        }
        public static void ShowHidePlayer(bool home, Button button, TextBox textBox, OBSWebsocket obs)
        {
            if (button.Text == "Show")
            {
                button.Text = "Hide";
                if (!ShowPlayer(home: home, jersey: textBox.Text, obs: obs))
                {
                    textBox.Text = string.Empty;
                    button.Text = "Show";
                }
            }
            else
            {
                textBox.Text = string.Empty;
                button.Text = "Show";
                HidePlayer(home: home);
            }
        }
        public static bool ShowPlayer(bool home, string jersey, OBSWebsocket obs)
        {
            bool videoFile = false;
            if (Properties.Settings.Default["PlayerImageFileType"].ToString().Equals("MP4", System.StringComparison.CurrentCultureIgnoreCase) || Properties.Settings.Default["PlayerImageFileType"].ToString().Equals("MOV", System.StringComparison.CurrentCultureIgnoreCase))
                videoFile = true;
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
            /*
            if (!File.Exists(sourcePath))
            {
                MessageBox.Show(text: "Image not found.", caption: "AFS", buttons: MessageBoxButtons.OK, icon: MessageBoxIcon.Warning);
            }
            else
            */
            {
                CopyFile(sourcePath: sourcePath, destinationPath: destinationPath);
                success = ReadPlayerFromDatabase(home: home, number: int.Parse(jersey), out string height, out string homeTown, out string name, out string position, out string weight, out string year);
                if (home)
                {
                    _ = WriteFileAsync(file: playerHomeHeightFile, content: height);
                    _ = WriteFileAsync(file: playerHomeTownFile, content: homeTown);
                    _ = WriteFileAsync(file: playerHomeNameFile, content: name);
                    _ = WriteFileAsync(file: playerHomePositionFile, content: position);
                    _ = WriteFileAsync(file: playerHomeNumberFile, content: jersey);
                    _ = WriteFileAsync(file: playerHomeWeightFile, content: weight);
                    _ = WriteFileAsync(file: playerHomeYearFile, content: year);
                }
                else
                {
                    _ = WriteFileAsync(file: playerAwayHeightFile, content: height);
                    _ = WriteFileAsync(file: playerAwayTownFile, content: homeTown);
                    _ = WriteFileAsync(file: playerAwayNameFile, content: name);
                    _ = WriteFileAsync(file: playerAwayPositionFile, content: position);
                    _ = WriteFileAsync(file: playerAwayNumberFile, content: jersey);
                    _ = WriteFileAsync(file: playerAwayWeightFile, content: weight);
                    _ = WriteFileAsync(file: playerAwayYearFile, content: year);
                }
                if (videoFile)
                {
                    var c = new Common();
                    c.ShowPlayerVideo(obs, destinationPath);
                }
            }
            return success;
        }
        private void ShowPlayerVideo(OBSWebsocket obs, string destinationPath)
        {
            DestinationPath = destinationPath;
            OBS = obs;
            PlayerVideoTimer.Interval = 500;
            PlayerVideoTimer.Elapsed += new ElapsedEventHandler(PlayerVideoTimer_Elapsed);
            if (!obs.IsConnected)
                obs.ConnectAsync("ws://" + Settings.Default.WebSocketServer, Settings.Default.WebSocketPassword);
            if (obs.IsConnected)
            {
                var inputSettings = new JObject();
                var requestFields = new JObject
                {
                    { "inputName", "Home Player Video" }
                };
                inputSettings.Add("visible", false);
                inputSettings.Add("local_file", "");
                requestFields.Add("inputSettings", inputSettings);
                obs.SendRequest("SetInputSettings", requestFields);
                PlayerVideoTimer.Enabled = true;
            }
        }
        private void PlayerVideoTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            PlayerVideoTimer.Enabled = false;
            var requestFields2 = new JObject
                {
                    { "inputName", "Home Player Video" }
                };
            var inputSettings2 = new JObject
                {
                    { "visible", true },
                    { "local_file", this.DestinationPath }
                };
            requestFields2.Add("inputSettings", inputSettings2);
            OBS.SendRequest("SetInputSettings", requestFields2);

        }
        public static async Task WriteFileAsync(string file, string content)
        {
            using StreamWriter outputFile = new(Path.Combine(Properties.Settings.Default.OutputPath, file));
            await outputFile.WriteAsync(content);
        }
    }
}