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
            this.components = new System.ComponentModel.Container();
            this.gbHomePlayers = new System.Windows.Forms.GroupBox();
            this.gbAwayPlayers = new System.Windows.Forms.GroupBox();
            this.tmrPlayerAway = new System.Windows.Forms.Timer(this.components);
            this.tmrPlayerHome = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // gbHomePlayers
            // 
            this.gbHomePlayers.Location = new System.Drawing.Point(12, 12);
            this.gbHomePlayers.Name = "gbHomePlayers";
            this.gbHomePlayers.Size = new System.Drawing.Size(361, 373);
            this.gbHomePlayers.TabIndex = 1;
            this.gbHomePlayers.TabStop = false;
            this.gbHomePlayers.Text = "Home Players";
            // 
            // gbAwayPlayers
            // 
            this.gbAwayPlayers.Location = new System.Drawing.Point(379, 12);
            this.gbAwayPlayers.Name = "gbAwayPlayers";
            this.gbAwayPlayers.Size = new System.Drawing.Size(361, 373);
            this.gbAwayPlayers.TabIndex = 2;
            this.gbAwayPlayers.TabStop = false;
            this.gbAwayPlayers.Text = "Away Players";
            // 
            // tmrPlayerAway
            // 
            this.tmrPlayerAway.Interval = 15000;
            this.tmrPlayerAway.Tick += new System.EventHandler(this.TmrPlayerAway_Tick);
            // 
            // tmrPlayerHome
            // 
            this.tmrPlayerHome.Interval = 15000;
            this.tmrPlayerHome.Tick += new System.EventHandler(this.TmrPlayerHome_Tick);
            // 
            // FrmPlayerImages
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(753, 394);
            this.Controls.Add(this.gbAwayPlayers);
            this.Controls.Add(this.gbHomePlayers);
            this.Name = "FrmPlayerImages";
            this.Text = "Player Images";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbHomePlayers;
        private System.Windows.Forms.GroupBox gbAwayPlayers;
        private System.Windows.Forms.Timer tmrPlayerAway;
        private System.Windows.Forms.Timer tmrPlayerHome;
    }
}