namespace American_Football_Scoreboard
{
    partial class FrmPlayerImages
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmPlayerImages));
            gbHomePlayers = new System.Windows.Forms.GroupBox();
            gbAwayPlayers = new System.Windows.Forms.GroupBox();
            tmrPlayerAway = new System.Windows.Forms.Timer(components);
            tmrPlayerHome = new System.Windows.Forms.Timer(components);
            SuspendLayout();
            // 
            // gbHomePlayers
            // 
            gbHomePlayers.Location = new System.Drawing.Point(14, 14);
            gbHomePlayers.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            gbHomePlayers.Name = "gbHomePlayers";
            gbHomePlayers.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            gbHomePlayers.Size = new System.Drawing.Size(421, 430);
            gbHomePlayers.TabIndex = 1;
            gbHomePlayers.TabStop = false;
            gbHomePlayers.Text = "Home Players";
            // 
            // gbAwayPlayers
            // 
            gbAwayPlayers.Location = new System.Drawing.Point(442, 14);
            gbAwayPlayers.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            gbAwayPlayers.Name = "gbAwayPlayers";
            gbAwayPlayers.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            gbAwayPlayers.Size = new System.Drawing.Size(421, 430);
            gbAwayPlayers.TabIndex = 2;
            gbAwayPlayers.TabStop = false;
            gbAwayPlayers.Text = "Away Players";
            // 
            // tmrPlayerAway
            // 
            tmrPlayerAway.Interval = 15000;
            tmrPlayerAway.Tick += TmrPlayerAway_Tick;
            // 
            // tmrPlayerHome
            // 
            tmrPlayerHome.Interval = 15000;
            tmrPlayerHome.Tick += TmrPlayerHome_Tick;
            // 
            // FrmPlayerImages
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(878, 455);
            Controls.Add(gbAwayPlayers);
            Controls.Add(gbHomePlayers);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "FrmPlayerImages";
            Text = "Player Images";
            FormClosing += FrmPlayerImages_FormClosing;
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.GroupBox gbHomePlayers;
        private System.Windows.Forms.GroupBox gbAwayPlayers;
        private System.Windows.Forms.Timer tmrPlayerAway;
        private System.Windows.Forms.Timer tmrPlayerHome;
    }
}