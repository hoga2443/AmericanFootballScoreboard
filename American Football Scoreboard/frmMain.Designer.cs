
namespace American_Football_Scoreboard
{
    partial class frmMain
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.butClearClocks = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.butSendSupplemental = new System.Windows.Forms.Button();
            this.txtSupplemental = new System.Windows.Forms.TextBox();
            this.butNewPlayClock = new System.Windows.Forms.Button();
            this.gbDown = new System.Windows.Forms.GroupBox();
            this.butDistanceGoal = new System.Windows.Forms.Button();
            this.butDownClear = new System.Windows.Forms.Button();
            this.txtDistance = new System.Windows.Forms.TextBox();
            this.rbDownFour = new System.Windows.Forms.RadioButton();
            this.rbDownThree = new System.Windows.Forms.RadioButton();
            this.rbDownTwo = new System.Windows.Forms.RadioButton();
            this.rbDownOne = new System.Windows.Forms.RadioButton();
            this.gbPeriod = new System.Windows.Forms.GroupBox();
            this.txtPeriodOT = new System.Windows.Forms.TextBox();
            this.butPeriodClear = new System.Windows.Forms.Button();
            this.rbPeriodOT = new System.Windows.Forms.RadioButton();
            this.rbPeriod4 = new System.Windows.Forms.RadioButton();
            this.rbPeriod3 = new System.Windows.Forms.RadioButton();
            this.rbPeriod2 = new System.Windows.Forms.RadioButton();
            this.rbPeriod1 = new System.Windows.Forms.RadioButton();
            this.gbAway = new System.Windows.Forms.GroupBox();
            this.butAwayTimeoutsAdd = new System.Windows.Forms.Button();
            this.butAwayTimeoutsSubtract = new System.Windows.Forms.Button();
            this.txtAwayTimeouts = new System.Windows.Forms.TextBox();
            this.lblAwayTimeouts = new System.Windows.Forms.Label();
            this.butClearAway = new System.Windows.Forms.Button();
            this.butAwayAddSix = new System.Windows.Forms.Button();
            this.butAwayAddThree = new System.Windows.Forms.Button();
            this.butAwayAddTwo = new System.Windows.Forms.Button();
            this.butAwayAddOne = new System.Windows.Forms.Button();
            this.txtAwayScore = new System.Windows.Forms.TextBox();
            this.lblAwayTeam = new System.Windows.Forms.Label();
            this.txtAwayTeam = new System.Windows.Forms.TextBox();
            this.gbHome = new System.Windows.Forms.GroupBox();
            this.butHomeTimeoutsAdd = new System.Windows.Forms.Button();
            this.butHomeTimeoutsSubtract = new System.Windows.Forms.Button();
            this.txtHomeTimeouts = new System.Windows.Forms.TextBox();
            this.lblHomeTimeouts = new System.Windows.Forms.Label();
            this.butClearHome = new System.Windows.Forms.Button();
            this.butHomeAddSix = new System.Windows.Forms.Button();
            this.butHomeAddThree = new System.Windows.Forms.Button();
            this.butHomeAddTwo = new System.Windows.Forms.Button();
            this.butHomeAddOne = new System.Windows.Forms.Button();
            this.txtHomeScore = new System.Windows.Forms.TextBox();
            this.lblHomeTeam = new System.Windows.Forms.Label();
            this.txtHomeTeam = new System.Windows.Forms.TextBox();
            this.butStopPlayClock = new System.Windows.Forms.Button();
            this.butStartPlayClock = new System.Windows.Forms.Button();
            this.txtPlayClock = new System.Windows.Forms.TextBox();
            this.butStopGameClock = new System.Windows.Forms.Button();
            this.butStartGameClock = new System.Windows.Forms.Button();
            this.txtGameClock = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lblTimeoutsPerHalf = new System.Windows.Forms.Label();
            this.txtTimeoutsPerHalf = new System.Windows.Forms.TextBox();
            this.txtGoalText = new System.Windows.Forms.TextBox();
            this.lblGoalDistanceLabel = new System.Windows.Forms.Label();
            this.butOutputFolder = new System.Windows.Forms.Button();
            this.butSaveSettings = new System.Windows.Forms.Button();
            this.txtOutputFolder = new System.Windows.Forms.TextBox();
            this.lblOutputFolder = new System.Windows.Forms.Label();
            this.txtPlayClockDuration = new System.Windows.Forms.TextBox();
            this.lblPlayClockDuration = new System.Windows.Forms.Label();
            this.txtPeriodDuration = new System.Windows.Forms.TextBox();
            this.lblPeriodDuration = new System.Windows.Forms.Label();
            this.fbdOutput = new System.Windows.Forms.FolderBrowserDialog();
            this.tmrClockRefresh = new System.Windows.Forms.Timer(this.components);
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.gbDown.SuspendLayout();
            this.gbPeriod.SuspendLayout();
            this.gbAway.SuspendLayout();
            this.gbHome.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(776, 357);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.butClearClocks);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.butNewPlayClock);
            this.tabPage1.Controls.Add(this.gbDown);
            this.tabPage1.Controls.Add(this.gbPeriod);
            this.tabPage1.Controls.Add(this.gbAway);
            this.tabPage1.Controls.Add(this.gbHome);
            this.tabPage1.Controls.Add(this.butStopPlayClock);
            this.tabPage1.Controls.Add(this.butStartPlayClock);
            this.tabPage1.Controls.Add(this.txtPlayClock);
            this.tabPage1.Controls.Add(this.butStopGameClock);
            this.tabPage1.Controls.Add(this.butStartGameClock);
            this.tabPage1.Controls.Add(this.txtGameClock);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(768, 331);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Scoreboard";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // butClearClocks
            // 
            this.butClearClocks.Location = new System.Drawing.Point(441, 106);
            this.butClearClocks.Name = "butClearClocks";
            this.butClearClocks.Size = new System.Drawing.Size(108, 23);
            this.butClearClocks.TabIndex = 17;
            this.butClearClocks.Text = "Clear Clocks";
            this.butClearClocks.UseVisualStyleBackColor = true;
            this.butClearClocks.Click += new System.EventHandler(this.ButClearClocks_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.butSendSupplemental);
            this.groupBox1.Controls.Add(this.txtSupplemental);
            this.groupBox1.Location = new System.Drawing.Point(6, 273);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(755, 52);
            this.groupBox1.TabIndex = 16;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Supplemental";
            // 
            // butSendSupplemental
            // 
            this.butSendSupplemental.Location = new System.Drawing.Point(674, 19);
            this.butSendSupplemental.Name = "butSendSupplemental";
            this.butSendSupplemental.Size = new System.Drawing.Size(75, 23);
            this.butSendSupplemental.TabIndex = 1;
            this.butSendSupplemental.Text = "Send";
            this.butSendSupplemental.UseVisualStyleBackColor = true;
            this.butSendSupplemental.Click += new System.EventHandler(this.ButSendSupplemental_Click);
            // 
            // txtSupplemental
            // 
            this.txtSupplemental.Location = new System.Drawing.Point(8, 21);
            this.txtSupplemental.Name = "txtSupplemental";
            this.txtSupplemental.Size = new System.Drawing.Size(660, 20);
            this.txtSupplemental.TabIndex = 0;
            // 
            // butNewPlayClock
            // 
            this.butNewPlayClock.Location = new System.Drawing.Point(335, 106);
            this.butNewPlayClock.Name = "butNewPlayClock";
            this.butNewPlayClock.Size = new System.Drawing.Size(100, 23);
            this.butNewPlayClock.TabIndex = 15;
            this.butNewPlayClock.Text = "New Play Clock";
            this.butNewPlayClock.UseVisualStyleBackColor = true;
            this.butNewPlayClock.Click += new System.EventHandler(this.ButNewPlayClock_Click);
            // 
            // gbDown
            // 
            this.gbDown.Controls.Add(this.butDistanceGoal);
            this.gbDown.Controls.Add(this.butDownClear);
            this.gbDown.Controls.Add(this.txtDistance);
            this.gbDown.Controls.Add(this.rbDownFour);
            this.gbDown.Controls.Add(this.rbDownThree);
            this.gbDown.Controls.Add(this.rbDownTwo);
            this.gbDown.Controls.Add(this.rbDownOne);
            this.gbDown.Location = new System.Drawing.Point(222, 146);
            this.gbDown.Name = "gbDown";
            this.gbDown.Size = new System.Drawing.Size(162, 120);
            this.gbDown.TabIndex = 14;
            this.gbDown.TabStop = false;
            this.gbDown.Text = "Down - Distance";
            // 
            // butDistanceGoal
            // 
            this.butDistanceGoal.Location = new System.Drawing.Point(106, 45);
            this.butDistanceGoal.Name = "butDistanceGoal";
            this.butDistanceGoal.Size = new System.Drawing.Size(46, 23);
            this.butDistanceGoal.TabIndex = 6;
            this.butDistanceGoal.Text = "Goal";
            this.butDistanceGoal.UseVisualStyleBackColor = true;
            this.butDistanceGoal.Click += new System.EventHandler(this.ButDistanceGoal_Click);
            // 
            // butDownClear
            // 
            this.butDownClear.Location = new System.Drawing.Point(81, 91);
            this.butDownClear.Name = "butDownClear";
            this.butDownClear.Size = new System.Drawing.Size(75, 23);
            this.butDownClear.TabIndex = 5;
            this.butDownClear.Text = "Clear";
            this.butDownClear.UseVisualStyleBackColor = true;
            this.butDownClear.Click += new System.EventHandler(this.ButDownClear_Click);
            // 
            // txtDistance
            // 
            this.txtDistance.Location = new System.Drawing.Point(106, 19);
            this.txtDistance.Name = "txtDistance";
            this.txtDistance.Size = new System.Drawing.Size(46, 20);
            this.txtDistance.TabIndex = 4;
            this.txtDistance.TextChanged += new System.EventHandler(this.TxtDistance_TextChanged);
            // 
            // rbDownFour
            // 
            this.rbDownFour.AutoSize = true;
            this.rbDownFour.Location = new System.Drawing.Point(6, 92);
            this.rbDownFour.Name = "rbDownFour";
            this.rbDownFour.Size = new System.Drawing.Size(40, 17);
            this.rbDownFour.TabIndex = 3;
            this.rbDownFour.TabStop = true;
            this.rbDownFour.Text = "4th";
            this.rbDownFour.UseVisualStyleBackColor = true;
            this.rbDownFour.CheckedChanged += new System.EventHandler(this.RbDownFour_CheckedChanged);
            // 
            // rbDownThree
            // 
            this.rbDownThree.AutoSize = true;
            this.rbDownThree.Location = new System.Drawing.Point(6, 69);
            this.rbDownThree.Name = "rbDownThree";
            this.rbDownThree.Size = new System.Drawing.Size(40, 17);
            this.rbDownThree.TabIndex = 2;
            this.rbDownThree.TabStop = true;
            this.rbDownThree.Text = "3rd";
            this.rbDownThree.UseVisualStyleBackColor = true;
            this.rbDownThree.CheckedChanged += new System.EventHandler(this.RbDownThree_CheckedChanged);
            // 
            // rbDownTwo
            // 
            this.rbDownTwo.AutoSize = true;
            this.rbDownTwo.Location = new System.Drawing.Point(6, 46);
            this.rbDownTwo.Name = "rbDownTwo";
            this.rbDownTwo.Size = new System.Drawing.Size(43, 17);
            this.rbDownTwo.TabIndex = 1;
            this.rbDownTwo.TabStop = true;
            this.rbDownTwo.Text = "2nd";
            this.rbDownTwo.UseVisualStyleBackColor = true;
            this.rbDownTwo.CheckedChanged += new System.EventHandler(this.RbDownTwo_CheckedChanged);
            // 
            // rbDownOne
            // 
            this.rbDownOne.AutoSize = true;
            this.rbDownOne.Location = new System.Drawing.Point(7, 23);
            this.rbDownOne.Name = "rbDownOne";
            this.rbDownOne.Size = new System.Drawing.Size(39, 17);
            this.rbDownOne.TabIndex = 0;
            this.rbDownOne.TabStop = true;
            this.rbDownOne.Text = "1st";
            this.rbDownOne.UseVisualStyleBackColor = true;
            this.rbDownOne.CheckedChanged += new System.EventHandler(this.RbDownOne_CheckedChanged);
            // 
            // gbPeriod
            // 
            this.gbPeriod.Controls.Add(this.txtPeriodOT);
            this.gbPeriod.Controls.Add(this.butPeriodClear);
            this.gbPeriod.Controls.Add(this.rbPeriodOT);
            this.gbPeriod.Controls.Add(this.rbPeriod4);
            this.gbPeriod.Controls.Add(this.rbPeriod3);
            this.gbPeriod.Controls.Add(this.rbPeriod2);
            this.gbPeriod.Controls.Add(this.rbPeriod1);
            this.gbPeriod.Location = new System.Drawing.Point(418, 165);
            this.gbPeriod.Name = "gbPeriod";
            this.gbPeriod.Size = new System.Drawing.Size(131, 101);
            this.gbPeriod.TabIndex = 13;
            this.gbPeriod.TabStop = false;
            this.gbPeriod.Text = "Period";
            // 
            // txtPeriodOT
            // 
            this.txtPeriodOT.Location = new System.Drawing.Point(84, 43);
            this.txtPeriodOT.Name = "txtPeriodOT";
            this.txtPeriodOT.Size = new System.Drawing.Size(33, 20);
            this.txtPeriodOT.TabIndex = 6;
            this.txtPeriodOT.Text = "OT";
            this.txtPeriodOT.TextChanged += new System.EventHandler(this.TxtPeriodOT_TextChanged);
            // 
            // butPeriodClear
            // 
            this.butPeriodClear.Location = new System.Drawing.Point(68, 70);
            this.butPeriodClear.Name = "butPeriodClear";
            this.butPeriodClear.Size = new System.Drawing.Size(49, 23);
            this.butPeriodClear.TabIndex = 5;
            this.butPeriodClear.Text = "Clear";
            this.butPeriodClear.UseVisualStyleBackColor = true;
            this.butPeriodClear.Click += new System.EventHandler(this.ButPeriodClear_Click);
            // 
            // rbPeriodOT
            // 
            this.rbPeriodOT.AutoSize = true;
            this.rbPeriodOT.Location = new System.Drawing.Point(68, 46);
            this.rbPeriodOT.Name = "rbPeriodOT";
            this.rbPeriodOT.Size = new System.Drawing.Size(14, 13);
            this.rbPeriodOT.TabIndex = 4;
            this.rbPeriodOT.TabStop = true;
            this.rbPeriodOT.UseVisualStyleBackColor = true;
            this.rbPeriodOT.CheckedChanged += new System.EventHandler(this.RbPeriodOT_CheckedChanged);
            // 
            // rbPeriod4
            // 
            this.rbPeriod4.AutoSize = true;
            this.rbPeriod4.Location = new System.Drawing.Point(68, 19);
            this.rbPeriod4.Name = "rbPeriod4";
            this.rbPeriod4.Size = new System.Drawing.Size(31, 17);
            this.rbPeriod4.TabIndex = 3;
            this.rbPeriod4.TabStop = true;
            this.rbPeriod4.Text = "4";
            this.rbPeriod4.UseVisualStyleBackColor = true;
            this.rbPeriod4.CheckedChanged += new System.EventHandler(this.RbPeriod4_CheckedChanged);
            // 
            // rbPeriod3
            // 
            this.rbPeriod3.AutoSize = true;
            this.rbPeriod3.Location = new System.Drawing.Point(7, 68);
            this.rbPeriod3.Name = "rbPeriod3";
            this.rbPeriod3.Size = new System.Drawing.Size(31, 17);
            this.rbPeriod3.TabIndex = 2;
            this.rbPeriod3.TabStop = true;
            this.rbPeriod3.Text = "3";
            this.rbPeriod3.UseVisualStyleBackColor = true;
            this.rbPeriod3.CheckedChanged += new System.EventHandler(this.RbPeriod3_CheckedChanged);
            // 
            // rbPeriod2
            // 
            this.rbPeriod2.AutoSize = true;
            this.rbPeriod2.Location = new System.Drawing.Point(7, 44);
            this.rbPeriod2.Name = "rbPeriod2";
            this.rbPeriod2.Size = new System.Drawing.Size(31, 17);
            this.rbPeriod2.TabIndex = 1;
            this.rbPeriod2.TabStop = true;
            this.rbPeriod2.Text = "2";
            this.rbPeriod2.UseVisualStyleBackColor = true;
            this.rbPeriod2.CheckedChanged += new System.EventHandler(this.RbPeriod2_CheckedChanged);
            // 
            // rbPeriod1
            // 
            this.rbPeriod1.AutoSize = true;
            this.rbPeriod1.Location = new System.Drawing.Point(7, 20);
            this.rbPeriod1.Name = "rbPeriod1";
            this.rbPeriod1.Size = new System.Drawing.Size(31, 17);
            this.rbPeriod1.TabIndex = 0;
            this.rbPeriod1.TabStop = true;
            this.rbPeriod1.Text = "1";
            this.rbPeriod1.UseVisualStyleBackColor = true;
            this.rbPeriod1.CheckedChanged += new System.EventHandler(this.RbPeriod1_CheckedChanged);
            // 
            // gbAway
            // 
            this.gbAway.Controls.Add(this.butAwayTimeoutsAdd);
            this.gbAway.Controls.Add(this.butAwayTimeoutsSubtract);
            this.gbAway.Controls.Add(this.txtAwayTimeouts);
            this.gbAway.Controls.Add(this.lblAwayTimeouts);
            this.gbAway.Controls.Add(this.butClearAway);
            this.gbAway.Controls.Add(this.butAwayAddSix);
            this.gbAway.Controls.Add(this.butAwayAddThree);
            this.gbAway.Controls.Add(this.butAwayAddTwo);
            this.gbAway.Controls.Add(this.butAwayAddOne);
            this.gbAway.Controls.Add(this.txtAwayScore);
            this.gbAway.Controls.Add(this.lblAwayTeam);
            this.gbAway.Controls.Add(this.txtAwayTeam);
            this.gbAway.Location = new System.Drawing.Point(562, 6);
            this.gbAway.Name = "gbAway";
            this.gbAway.Size = new System.Drawing.Size(200, 260);
            this.gbAway.TabIndex = 12;
            this.gbAway.TabStop = false;
            this.gbAway.Text = "Away";
            // 
            // butAwayTimeoutsAdd
            // 
            this.butAwayTimeoutsAdd.Location = new System.Drawing.Point(85, 233);
            this.butAwayTimeoutsAdd.Name = "butAwayTimeoutsAdd";
            this.butAwayTimeoutsAdd.Size = new System.Drawing.Size(18, 21);
            this.butAwayTimeoutsAdd.TabIndex = 22;
            this.butAwayTimeoutsAdd.Text = "+";
            this.butAwayTimeoutsAdd.UseVisualStyleBackColor = true;
            this.butAwayTimeoutsAdd.Click += new System.EventHandler(this.ButAwayTimeoutsAdd_Click);
            // 
            // butAwayTimeoutsSubtract
            // 
            this.butAwayTimeoutsSubtract.Location = new System.Drawing.Point(14, 234);
            this.butAwayTimeoutsSubtract.Name = "butAwayTimeoutsSubtract";
            this.butAwayTimeoutsSubtract.Size = new System.Drawing.Size(18, 20);
            this.butAwayTimeoutsSubtract.TabIndex = 21;
            this.butAwayTimeoutsSubtract.Text = "-";
            this.butAwayTimeoutsSubtract.UseVisualStyleBackColor = true;
            this.butAwayTimeoutsSubtract.Click += new System.EventHandler(this.ButAwayTimeoutsSubtract_Click);
            // 
            // txtAwayTimeouts
            // 
            this.txtAwayTimeouts.Location = new System.Drawing.Point(38, 234);
            this.txtAwayTimeouts.Name = "txtAwayTimeouts";
            this.txtAwayTimeouts.Size = new System.Drawing.Size(41, 20);
            this.txtAwayTimeouts.TabIndex = 20;
            this.txtAwayTimeouts.TextChanged += new System.EventHandler(this.TxtAwayTimeouts_TextChanged);
            // 
            // lblAwayTimeouts
            // 
            this.lblAwayTimeouts.AutoSize = true;
            this.lblAwayTimeouts.Location = new System.Drawing.Point(35, 218);
            this.lblAwayTimeouts.Name = "lblAwayTimeouts";
            this.lblAwayTimeouts.Size = new System.Drawing.Size(44, 13);
            this.lblAwayTimeouts.TabIndex = 19;
            this.lblAwayTimeouts.Text = "Timouts";
            // 
            // butClearAway
            // 
            this.butClearAway.Location = new System.Drawing.Point(119, 231);
            this.butClearAway.Name = "butClearAway";
            this.butClearAway.Size = new System.Drawing.Size(75, 23);
            this.butClearAway.TabIndex = 18;
            this.butClearAway.Text = "Clear";
            this.butClearAway.UseVisualStyleBackColor = true;
            this.butClearAway.Click += new System.EventHandler(this.ButClearAway_Click);
            // 
            // butAwayAddSix
            // 
            this.butAwayAddSix.Location = new System.Drawing.Point(24, 163);
            this.butAwayAddSix.Name = "butAwayAddSix";
            this.butAwayAddSix.Size = new System.Drawing.Size(44, 23);
            this.butAwayAddSix.TabIndex = 17;
            this.butAwayAddSix.Text = "+6";
            this.butAwayAddSix.UseVisualStyleBackColor = true;
            this.butAwayAddSix.Click += new System.EventHandler(this.ButAwayAddSix_Click);
            // 
            // butAwayAddThree
            // 
            this.butAwayAddThree.Location = new System.Drawing.Point(24, 134);
            this.butAwayAddThree.Name = "butAwayAddThree";
            this.butAwayAddThree.Size = new System.Drawing.Size(44, 23);
            this.butAwayAddThree.TabIndex = 16;
            this.butAwayAddThree.Text = "+3";
            this.butAwayAddThree.UseVisualStyleBackColor = true;
            this.butAwayAddThree.Click += new System.EventHandler(this.ButAwayAddThree_Click);
            // 
            // butAwayAddTwo
            // 
            this.butAwayAddTwo.Location = new System.Drawing.Point(24, 105);
            this.butAwayAddTwo.Name = "butAwayAddTwo";
            this.butAwayAddTwo.Size = new System.Drawing.Size(44, 23);
            this.butAwayAddTwo.TabIndex = 15;
            this.butAwayAddTwo.Text = "+2";
            this.butAwayAddTwo.UseVisualStyleBackColor = true;
            this.butAwayAddTwo.Click += new System.EventHandler(this.ButAwayAddTwo_Click);
            // 
            // butAwayAddOne
            // 
            this.butAwayAddOne.Location = new System.Drawing.Point(24, 76);
            this.butAwayAddOne.Name = "butAwayAddOne";
            this.butAwayAddOne.Size = new System.Drawing.Size(44, 23);
            this.butAwayAddOne.TabIndex = 14;
            this.butAwayAddOne.Text = "+1";
            this.butAwayAddOne.UseVisualStyleBackColor = true;
            this.butAwayAddOne.Click += new System.EventHandler(this.ButAwayAddOne_Click);
            // 
            // txtAwayScore
            // 
            this.txtAwayScore.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAwayScore.Location = new System.Drawing.Point(74, 76);
            this.txtAwayScore.Name = "txtAwayScore";
            this.txtAwayScore.Size = new System.Drawing.Size(100, 53);
            this.txtAwayScore.TabIndex = 10;
            this.txtAwayScore.Text = "0";
            this.txtAwayScore.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtAwayScore.TextChanged += new System.EventHandler(this.TxtAwayScore_TextChanged);
            // 
            // lblAwayTeam
            // 
            this.lblAwayTeam.AutoSize = true;
            this.lblAwayTeam.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAwayTeam.Location = new System.Drawing.Point(47, 16);
            this.lblAwayTeam.Name = "lblAwayTeam";
            this.lblAwayTeam.Size = new System.Drawing.Size(127, 26);
            this.lblAwayTeam.TabIndex = 1;
            this.lblAwayTeam.Text = "Away Team";
            // 
            // txtAwayTeam
            // 
            this.txtAwayTeam.Location = new System.Drawing.Point(50, 50);
            this.txtAwayTeam.Name = "txtAwayTeam";
            this.txtAwayTeam.Size = new System.Drawing.Size(124, 20);
            this.txtAwayTeam.TabIndex = 9;
            this.txtAwayTeam.Leave += new System.EventHandler(this.TxtAwayTeam_Leave);
            // 
            // gbHome
            // 
            this.gbHome.Controls.Add(this.butHomeTimeoutsAdd);
            this.gbHome.Controls.Add(this.butHomeTimeoutsSubtract);
            this.gbHome.Controls.Add(this.txtHomeTimeouts);
            this.gbHome.Controls.Add(this.lblHomeTimeouts);
            this.gbHome.Controls.Add(this.butClearHome);
            this.gbHome.Controls.Add(this.butHomeAddSix);
            this.gbHome.Controls.Add(this.butHomeAddThree);
            this.gbHome.Controls.Add(this.butHomeAddTwo);
            this.gbHome.Controls.Add(this.butHomeAddOne);
            this.gbHome.Controls.Add(this.txtHomeScore);
            this.gbHome.Controls.Add(this.lblHomeTeam);
            this.gbHome.Controls.Add(this.txtHomeTeam);
            this.gbHome.Location = new System.Drawing.Point(6, 6);
            this.gbHome.Name = "gbHome";
            this.gbHome.Size = new System.Drawing.Size(200, 260);
            this.gbHome.TabIndex = 11;
            this.gbHome.TabStop = false;
            this.gbHome.Text = "Home";
            // 
            // butHomeTimeoutsAdd
            // 
            this.butHomeTimeoutsAdd.Location = new System.Drawing.Point(86, 231);
            this.butHomeTimeoutsAdd.Name = "butHomeTimeoutsAdd";
            this.butHomeTimeoutsAdd.Size = new System.Drawing.Size(18, 21);
            this.butHomeTimeoutsAdd.TabIndex = 18;
            this.butHomeTimeoutsAdd.Text = "+";
            this.butHomeTimeoutsAdd.UseVisualStyleBackColor = true;
            this.butHomeTimeoutsAdd.Click += new System.EventHandler(this.ButHomeTimeoutsAdd_Click);
            // 
            // butHomeTimeoutsSubtract
            // 
            this.butHomeTimeoutsSubtract.Location = new System.Drawing.Point(15, 232);
            this.butHomeTimeoutsSubtract.Name = "butHomeTimeoutsSubtract";
            this.butHomeTimeoutsSubtract.Size = new System.Drawing.Size(18, 20);
            this.butHomeTimeoutsSubtract.TabIndex = 17;
            this.butHomeTimeoutsSubtract.Text = "-";
            this.butHomeTimeoutsSubtract.UseVisualStyleBackColor = true;
            this.butHomeTimeoutsSubtract.Click += new System.EventHandler(this.ButHomeTimeoutsSubtract_Click);
            // 
            // txtHomeTimeouts
            // 
            this.txtHomeTimeouts.Location = new System.Drawing.Point(39, 232);
            this.txtHomeTimeouts.Name = "txtHomeTimeouts";
            this.txtHomeTimeouts.Size = new System.Drawing.Size(41, 20);
            this.txtHomeTimeouts.TabIndex = 16;
            this.txtHomeTimeouts.TextChanged += new System.EventHandler(this.TxtHomeTimeouts_TextChanged);
            // 
            // lblHomeTimeouts
            // 
            this.lblHomeTimeouts.AutoSize = true;
            this.lblHomeTimeouts.Location = new System.Drawing.Point(36, 216);
            this.lblHomeTimeouts.Name = "lblHomeTimeouts";
            this.lblHomeTimeouts.Size = new System.Drawing.Size(44, 13);
            this.lblHomeTimeouts.TabIndex = 15;
            this.lblHomeTimeouts.Text = "Timouts";
            // 
            // butClearHome
            // 
            this.butClearHome.Location = new System.Drawing.Point(119, 231);
            this.butClearHome.Name = "butClearHome";
            this.butClearHome.Size = new System.Drawing.Size(75, 23);
            this.butClearHome.TabIndex = 14;
            this.butClearHome.Text = "Clear";
            this.butClearHome.UseVisualStyleBackColor = true;
            this.butClearHome.Click += new System.EventHandler(this.ButClearHome_Click);
            // 
            // butHomeAddSix
            // 
            this.butHomeAddSix.Location = new System.Drawing.Point(7, 163);
            this.butHomeAddSix.Name = "butHomeAddSix";
            this.butHomeAddSix.Size = new System.Drawing.Size(44, 23);
            this.butHomeAddSix.TabIndex = 13;
            this.butHomeAddSix.Text = "+6";
            this.butHomeAddSix.UseVisualStyleBackColor = true;
            this.butHomeAddSix.Click += new System.EventHandler(this.ButHomeAddSix_Click);
            // 
            // butHomeAddThree
            // 
            this.butHomeAddThree.Location = new System.Drawing.Point(7, 134);
            this.butHomeAddThree.Name = "butHomeAddThree";
            this.butHomeAddThree.Size = new System.Drawing.Size(44, 23);
            this.butHomeAddThree.TabIndex = 12;
            this.butHomeAddThree.Text = "+3";
            this.butHomeAddThree.UseVisualStyleBackColor = true;
            this.butHomeAddThree.Click += new System.EventHandler(this.ButHomeAddThree_Click);
            // 
            // butHomeAddTwo
            // 
            this.butHomeAddTwo.Location = new System.Drawing.Point(7, 105);
            this.butHomeAddTwo.Name = "butHomeAddTwo";
            this.butHomeAddTwo.Size = new System.Drawing.Size(44, 23);
            this.butHomeAddTwo.TabIndex = 11;
            this.butHomeAddTwo.Text = "+2";
            this.butHomeAddTwo.UseVisualStyleBackColor = true;
            this.butHomeAddTwo.Click += new System.EventHandler(this.ButHomeAddTwo_Click);
            // 
            // butHomeAddOne
            // 
            this.butHomeAddOne.Location = new System.Drawing.Point(7, 76);
            this.butHomeAddOne.Name = "butHomeAddOne";
            this.butHomeAddOne.Size = new System.Drawing.Size(44, 23);
            this.butHomeAddOne.TabIndex = 10;
            this.butHomeAddOne.Text = "+1";
            this.butHomeAddOne.UseVisualStyleBackColor = true;
            this.butHomeAddOne.Click += new System.EventHandler(this.ButHomeAddOne_Click);
            // 
            // txtHomeScore
            // 
            this.txtHomeScore.Font = new System.Drawing.Font("Microsoft Sans Serif", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHomeScore.Location = new System.Drawing.Point(57, 76);
            this.txtHomeScore.Name = "txtHomeScore";
            this.txtHomeScore.Size = new System.Drawing.Size(100, 53);
            this.txtHomeScore.TabIndex = 9;
            this.txtHomeScore.Text = "0";
            this.txtHomeScore.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtHomeScore.TextChanged += new System.EventHandler(this.TxtHomeScore_TextChanged);
            // 
            // lblHomeTeam
            // 
            this.lblHomeTeam.AutoSize = true;
            this.lblHomeTeam.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHomeTeam.Location = new System.Drawing.Point(28, 16);
            this.lblHomeTeam.Name = "lblHomeTeam";
            this.lblHomeTeam.Size = new System.Drawing.Size(132, 26);
            this.lblHomeTeam.TabIndex = 0;
            this.lblHomeTeam.Text = "Home Team";
            // 
            // txtHomeTeam
            // 
            this.txtHomeTeam.Location = new System.Drawing.Point(33, 50);
            this.txtHomeTeam.Name = "txtHomeTeam";
            this.txtHomeTeam.Size = new System.Drawing.Size(124, 20);
            this.txtHomeTeam.TabIndex = 8;
            this.txtHomeTeam.Leave += new System.EventHandler(this.TxtHomeTeam_Leave);
            // 
            // butStopPlayClock
            // 
            this.butStopPlayClock.Location = new System.Drawing.Point(441, 66);
            this.butStopPlayClock.Name = "butStopPlayClock";
            this.butStopPlayClock.Size = new System.Drawing.Size(108, 23);
            this.butStopPlayClock.TabIndex = 7;
            this.butStopPlayClock.Text = "Stop Play Clock";
            this.butStopPlayClock.UseVisualStyleBackColor = true;
            this.butStopPlayClock.Click += new System.EventHandler(this.ButStopPlayClock_Click);
            // 
            // butStartPlayClock
            // 
            this.butStartPlayClock.Location = new System.Drawing.Point(221, 66);
            this.butStartPlayClock.Name = "butStartPlayClock";
            this.butStartPlayClock.Size = new System.Drawing.Size(108, 23);
            this.butStartPlayClock.TabIndex = 6;
            this.butStartPlayClock.Text = "Start Play Clock";
            this.butStartPlayClock.UseVisualStyleBackColor = true;
            this.butStartPlayClock.Click += new System.EventHandler(this.ButStartPlayClock_Click);
            // 
            // txtPlayClock
            // 
            this.txtPlayClock.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPlayClock.Location = new System.Drawing.Point(335, 56);
            this.txtPlayClock.Name = "txtPlayClock";
            this.txtPlayClock.Size = new System.Drawing.Size(100, 44);
            this.txtPlayClock.TabIndex = 5;
            this.txtPlayClock.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // butStopGameClock
            // 
            this.butStopGameClock.Location = new System.Drawing.Point(495, 6);
            this.butStopGameClock.Name = "butStopGameClock";
            this.butStopGameClock.Size = new System.Drawing.Size(66, 54);
            this.butStopGameClock.TabIndex = 4;
            this.butStopGameClock.Text = "Stop Game Clock";
            this.butStopGameClock.UseVisualStyleBackColor = true;
            this.butStopGameClock.Click += new System.EventHandler(this.ButStopGameClock_Click);
            // 
            // butStartGameClock
            // 
            this.butStartGameClock.Location = new System.Drawing.Point(212, 6);
            this.butStartGameClock.Name = "butStartGameClock";
            this.butStartGameClock.Size = new System.Drawing.Size(68, 54);
            this.butStartGameClock.TabIndex = 3;
            this.butStartGameClock.Text = "Start Game Clock";
            this.butStartGameClock.UseVisualStyleBackColor = true;
            this.butStartGameClock.Click += new System.EventHandler(this.ButStartGameClock_Click);
            // 
            // txtGameClock
            // 
            this.txtGameClock.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGameClock.Location = new System.Drawing.Point(286, 10);
            this.txtGameClock.Name = "txtGameClock";
            this.txtGameClock.Size = new System.Drawing.Size(203, 44);
            this.txtGameClock.TabIndex = 2;
            this.txtGameClock.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.lblTimeoutsPerHalf);
            this.tabPage2.Controls.Add(this.txtTimeoutsPerHalf);
            this.tabPage2.Controls.Add(this.txtGoalText);
            this.tabPage2.Controls.Add(this.lblGoalDistanceLabel);
            this.tabPage2.Controls.Add(this.butOutputFolder);
            this.tabPage2.Controls.Add(this.butSaveSettings);
            this.tabPage2.Controls.Add(this.txtOutputFolder);
            this.tabPage2.Controls.Add(this.lblOutputFolder);
            this.tabPage2.Controls.Add(this.txtPlayClockDuration);
            this.tabPage2.Controls.Add(this.lblPlayClockDuration);
            this.tabPage2.Controls.Add(this.txtPeriodDuration);
            this.tabPage2.Controls.Add(this.lblPeriodDuration);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(768, 331);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Settings";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // lblTimeoutsPerHalf
            // 
            this.lblTimeoutsPerHalf.AutoSize = true;
            this.lblTimeoutsPerHalf.Location = new System.Drawing.Point(16, 128);
            this.lblTimeoutsPerHalf.Name = "lblTimeoutsPerHalf";
            this.lblTimeoutsPerHalf.Size = new System.Drawing.Size(91, 13);
            this.lblTimeoutsPerHalf.TabIndex = 13;
            this.lblTimeoutsPerHalf.Text = "Timeouts Per Half";
            // 
            // txtTimeoutsPerHalf
            // 
            this.txtTimeoutsPerHalf.Location = new System.Drawing.Point(140, 121);
            this.txtTimeoutsPerHalf.Name = "txtTimeoutsPerHalf";
            this.txtTimeoutsPerHalf.Size = new System.Drawing.Size(100, 20);
            this.txtTimeoutsPerHalf.TabIndex = 5;
            // 
            // txtGoalText
            // 
            this.txtGoalText.Location = new System.Drawing.Point(140, 95);
            this.txtGoalText.Name = "txtGoalText";
            this.txtGoalText.Size = new System.Drawing.Size(100, 20);
            this.txtGoalText.TabIndex = 4;
            // 
            // lblGoalDistanceLabel
            // 
            this.lblGoalDistanceLabel.AutoSize = true;
            this.lblGoalDistanceLabel.Location = new System.Drawing.Point(16, 98);
            this.lblGoalDistanceLabel.Name = "lblGoalDistanceLabel";
            this.lblGoalDistanceLabel.Size = new System.Drawing.Size(103, 13);
            this.lblGoalDistanceLabel.TabIndex = 10;
            this.lblGoalDistanceLabel.Text = "Goal Distance Label";
            // 
            // butOutputFolder
            // 
            this.butOutputFolder.Location = new System.Drawing.Point(256, 66);
            this.butOutputFolder.Name = "butOutputFolder";
            this.butOutputFolder.Size = new System.Drawing.Size(85, 23);
            this.butOutputFolder.TabIndex = 3;
            this.butOutputFolder.Text = "Select Folder";
            this.butOutputFolder.UseVisualStyleBackColor = true;
            this.butOutputFolder.Click += new System.EventHandler(this.ButOutputFolder_Click);
            // 
            // butSaveSettings
            // 
            this.butSaveSettings.Location = new System.Drawing.Point(140, 147);
            this.butSaveSettings.Name = "butSaveSettings";
            this.butSaveSettings.Size = new System.Drawing.Size(100, 23);
            this.butSaveSettings.TabIndex = 6;
            this.butSaveSettings.Text = "Save";
            this.butSaveSettings.UseVisualStyleBackColor = true;
            this.butSaveSettings.Click += new System.EventHandler(this.ButSaveSettings_Click);
            // 
            // txtOutputFolder
            // 
            this.txtOutputFolder.Location = new System.Drawing.Point(140, 68);
            this.txtOutputFolder.Name = "txtOutputFolder";
            this.txtOutputFolder.Size = new System.Drawing.Size(100, 20);
            this.txtOutputFolder.TabIndex = 2;
            // 
            // lblOutputFolder
            // 
            this.lblOutputFolder.AutoSize = true;
            this.lblOutputFolder.Location = new System.Drawing.Point(16, 71);
            this.lblOutputFolder.Name = "lblOutputFolder";
            this.lblOutputFolder.Size = new System.Drawing.Size(71, 13);
            this.lblOutputFolder.TabIndex = 4;
            this.lblOutputFolder.Text = "Output Folder";
            // 
            // txtPlayClockDuration
            // 
            this.txtPlayClockDuration.Location = new System.Drawing.Point(140, 42);
            this.txtPlayClockDuration.Name = "txtPlayClockDuration";
            this.txtPlayClockDuration.Size = new System.Drawing.Size(100, 20);
            this.txtPlayClockDuration.TabIndex = 1;
            // 
            // lblPlayClockDuration
            // 
            this.lblPlayClockDuration.AutoSize = true;
            this.lblPlayClockDuration.Location = new System.Drawing.Point(17, 45);
            this.lblPlayClockDuration.Name = "lblPlayClockDuration";
            this.lblPlayClockDuration.Size = new System.Drawing.Size(94, 13);
            this.lblPlayClockDuration.TabIndex = 2;
            this.lblPlayClockDuration.Text = "Default Play Clock";
            // 
            // txtPeriodDuration
            // 
            this.txtPeriodDuration.Location = new System.Drawing.Point(140, 15);
            this.txtPeriodDuration.Name = "txtPeriodDuration";
            this.txtPeriodDuration.Size = new System.Drawing.Size(100, 20);
            this.txtPeriodDuration.TabIndex = 0;
            // 
            // lblPeriodDuration
            // 
            this.lblPeriodDuration.AutoSize = true;
            this.lblPeriodDuration.Location = new System.Drawing.Point(17, 18);
            this.lblPeriodDuration.Name = "lblPeriodDuration";
            this.lblPeriodDuration.Size = new System.Drawing.Size(117, 13);
            this.lblPeriodDuration.TabIndex = 0;
            this.lblPeriodDuration.Text = "Default Period Duration";
            // 
            // tmrClockRefresh
            // 
            this.tmrClockRefresh.Interval = 900;
            this.tmrClockRefresh.Tick += new System.EventHandler(this.TmrClockRefresh_Tick);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 380);
            this.Controls.Add(this.tabControl1);
            this.Name = "frmMain";
            this.Text = "American Football Scoreboard";
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.gbDown.ResumeLayout(false);
            this.gbDown.PerformLayout();
            this.gbPeriod.ResumeLayout(false);
            this.gbPeriod.PerformLayout();
            this.gbAway.ResumeLayout(false);
            this.gbAway.PerformLayout();
            this.gbHome.ResumeLayout(false);
            this.gbHome.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label lblAwayTeam;
        private System.Windows.Forms.Label lblHomeTeam;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox txtGameClock;
        private System.Windows.Forms.TextBox txtPeriodDuration;
        private System.Windows.Forms.Label lblPeriodDuration;
        private System.Windows.Forms.Button butStopGameClock;
        private System.Windows.Forms.Button butStartGameClock;
        private System.Windows.Forms.TextBox txtPlayClock;
        private System.Windows.Forms.TextBox txtPlayClockDuration;
        private System.Windows.Forms.Label lblPlayClockDuration;
        private System.Windows.Forms.Button butStopPlayClock;
        private System.Windows.Forms.Button butStartPlayClock;
        private System.Windows.Forms.TextBox txtAwayTeam;
        private System.Windows.Forms.TextBox txtHomeTeam;
        private System.Windows.Forms.TextBox txtOutputFolder;
        private System.Windows.Forms.Label lblOutputFolder;
        private System.Windows.Forms.FolderBrowserDialog fbdOutput;
        private System.Windows.Forms.GroupBox gbAway;
        private System.Windows.Forms.GroupBox gbHome;
        private System.Windows.Forms.TextBox txtHomeScore;
        private System.Windows.Forms.Button butAwayAddSix;
        private System.Windows.Forms.Button butAwayAddThree;
        private System.Windows.Forms.Button butAwayAddTwo;
        private System.Windows.Forms.Button butAwayAddOne;
        private System.Windows.Forms.TextBox txtAwayScore;
        private System.Windows.Forms.Button butHomeAddSix;
        private System.Windows.Forms.Button butHomeAddThree;
        private System.Windows.Forms.Button butHomeAddTwo;
        private System.Windows.Forms.Button butHomeAddOne;
        private System.Windows.Forms.Button butSaveSettings;
        private System.Windows.Forms.GroupBox gbPeriod;
        private System.Windows.Forms.GroupBox gbDown;
        private System.Windows.Forms.RadioButton rbDownOne;
        private System.Windows.Forms.RadioButton rbDownFour;
        private System.Windows.Forms.RadioButton rbDownThree;
        private System.Windows.Forms.RadioButton rbDownTwo;
        private System.Windows.Forms.TextBox txtDistance;
        private System.Windows.Forms.Timer tmrClockRefresh;
        private System.Windows.Forms.Button butOutputFolder;
        private System.Windows.Forms.Button butNewPlayClock;
        private System.Windows.Forms.RadioButton rbPeriodOT;
        private System.Windows.Forms.RadioButton rbPeriod4;
        private System.Windows.Forms.RadioButton rbPeriod3;
        private System.Windows.Forms.RadioButton rbPeriod2;
        private System.Windows.Forms.RadioButton rbPeriod1;
        private System.Windows.Forms.Button butClearAway;
        private System.Windows.Forms.Button butClearHome;
        private System.Windows.Forms.Button butDownClear;
        private System.Windows.Forms.Button butPeriodClear;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button butSendSupplemental;
        private System.Windows.Forms.TextBox txtSupplemental;
        private System.Windows.Forms.TextBox txtPeriodOT;
        private System.Windows.Forms.Button butDistanceGoal;
        private System.Windows.Forms.TextBox txtGoalText;
        private System.Windows.Forms.Label lblGoalDistanceLabel;
        private System.Windows.Forms.Label lblTimeoutsPerHalf;
        private System.Windows.Forms.TextBox txtTimeoutsPerHalf;
        private System.Windows.Forms.Button butAwayTimeoutsAdd;
        private System.Windows.Forms.Button butAwayTimeoutsSubtract;
        private System.Windows.Forms.TextBox txtAwayTimeouts;
        private System.Windows.Forms.Label lblAwayTimeouts;
        private System.Windows.Forms.Button butHomeTimeoutsAdd;
        private System.Windows.Forms.Button butHomeTimeoutsSubtract;
        private System.Windows.Forms.TextBox txtHomeTimeouts;
        private System.Windows.Forms.Label lblHomeTimeouts;
        private System.Windows.Forms.Button butClearClocks;
    }
}

