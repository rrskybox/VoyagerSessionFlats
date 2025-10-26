namespace SessionFlats
{
    partial class FormVoyagerSessionFlats
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
            this.TrafficTextBox = new System.Windows.Forms.TextBox();
            this.SessionStartDT = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SessionEndDT = new System.Windows.Forms.DateTimePicker();
            this.IncrementPM = new System.Windows.Forms.Button();
            this.DecrementPM = new System.Windows.Forms.Button();
            this.DecrementAM = new System.Windows.Forms.Button();
            this.IncrementAM = new System.Windows.Forms.Button();
            this.ListFlatsButton = new System.Windows.Forms.Button();
            this.TakeFlatsButton = new System.Windows.Forms.Button();
            this.FitsFileFolderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.VoyagerImageFolder = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ChooseButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // TrafficTextBox
            // 
            this.TrafficTextBox.Location = new System.Drawing.Point(12, 102);
            this.TrafficTextBox.Multiline = true;
            this.TrafficTextBox.Name = "TrafficTextBox";
            this.TrafficTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TrafficTextBox.Size = new System.Drawing.Size(488, 425);
            this.TrafficTextBox.TabIndex = 0;
            // 
            // SessionStartDT
            // 
            this.SessionStartDT.Location = new System.Drawing.Point(126, 48);
            this.SessionStartDT.Name = "SessionStartDT";
            this.SessionStartDT.Size = new System.Drawing.Size(200, 20);
            this.SessionStartDT.TabIndex = 1;
            this.SessionStartDT.ValueChanged += new System.EventHandler(this.SessionStartDT_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 52);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(94, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Session Start (PM)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Session End (AM)";
            // 
            // SessionEndDT
            // 
            this.SessionEndDT.Location = new System.Drawing.Point(126, 73);
            this.SessionEndDT.Name = "SessionEndDT";
            this.SessionEndDT.Size = new System.Drawing.Size(200, 20);
            this.SessionEndDT.TabIndex = 3;
            this.SessionEndDT.ValueChanged += new System.EventHandler(this.SessionEndDT_ValueChanged);
            // 
            // IncrementPM
            // 
            this.IncrementPM.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.IncrementPM.Location = new System.Drawing.Point(332, 47);
            this.IncrementPM.Name = "IncrementPM";
            this.IncrementPM.Size = new System.Drawing.Size(22, 22);
            this.IncrementPM.TabIndex = 5;
            this.IncrementPM.Text = "+";
            this.IncrementPM.UseVisualStyleBackColor = true;
            this.IncrementPM.Click += new System.EventHandler(this.IncrementPM_Click);
            // 
            // DecrementPM
            // 
            this.DecrementPM.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.DecrementPM.Location = new System.Drawing.Point(351, 47);
            this.DecrementPM.Name = "DecrementPM";
            this.DecrementPM.Size = new System.Drawing.Size(22, 22);
            this.DecrementPM.TabIndex = 6;
            this.DecrementPM.Text = "-";
            this.DecrementPM.UseVisualStyleBackColor = true;
            this.DecrementPM.Click += new System.EventHandler(this.DecrementPM_Click);
            // 
            // DecrementAM
            // 
            this.DecrementAM.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.DecrementAM.Location = new System.Drawing.Point(351, 72);
            this.DecrementAM.Name = "DecrementAM";
            this.DecrementAM.Size = new System.Drawing.Size(22, 22);
            this.DecrementAM.TabIndex = 8;
            this.DecrementAM.Text = "-";
            this.DecrementAM.UseVisualStyleBackColor = true;
            this.DecrementAM.Click += new System.EventHandler(this.DecrementAM_Click);
            // 
            // IncrementAM
            // 
            this.IncrementAM.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
            this.IncrementAM.Location = new System.Drawing.Point(332, 72);
            this.IncrementAM.Name = "IncrementAM";
            this.IncrementAM.Size = new System.Drawing.Size(22, 22);
            this.IncrementAM.TabIndex = 7;
            this.IncrementAM.Text = "+";
            this.IncrementAM.UseVisualStyleBackColor = true;
            this.IncrementAM.Click += new System.EventHandler(this.IncrementAM_Click);
            // 
            // ListFlatsButton
            // 
            this.ListFlatsButton.Location = new System.Drawing.Point(392, 51);
            this.ListFlatsButton.Name = "ListFlatsButton";
            this.ListFlatsButton.Size = new System.Drawing.Size(50, 40);
            this.ListFlatsButton.TabIndex = 9;
            this.ListFlatsButton.Text = "List Flats";
            this.ListFlatsButton.UseVisualStyleBackColor = true;
            this.ListFlatsButton.Click += new System.EventHandler(this.ListFlatsButton_Click);
            // 
            // TakeFlatsButton
            // 
            this.TakeFlatsButton.Location = new System.Drawing.Point(450, 52);
            this.TakeFlatsButton.Name = "TakeFlatsButton";
            this.TakeFlatsButton.Size = new System.Drawing.Size(50, 40);
            this.TakeFlatsButton.TabIndex = 10;
            this.TakeFlatsButton.Text = "Take Flats";
            this.TakeFlatsButton.UseVisualStyleBackColor = true;
            this.TakeFlatsButton.Click += new System.EventHandler(this.TakeFlatsButton_Click);
            // 
            // FitsFileFolderDialog
            // 
            this.FitsFileFolderDialog.RootFolder = System.Environment.SpecialFolder.MyComputer;
            this.FitsFileFolderDialog.SelectedPath = "D:\\Voyager";
            this.FitsFileFolderDialog.ShowNewFolderButton = false;
            // 
            // VoyagerImageFolder
            // 
            this.VoyagerImageFolder.Location = new System.Drawing.Point(126, 17);
            this.VoyagerImageFolder.Name = "VoyagerImageFolder";
            this.VoyagerImageFolder.Size = new System.Drawing.Size(316, 20);
            this.VoyagerImageFolder.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(110, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Voyager Image Folder";
            // 
            // ChooseButton
            // 
            this.ChooseButton.Location = new System.Drawing.Point(448, 17);
            this.ChooseButton.Name = "ChooseButton";
            this.ChooseButton.Size = new System.Drawing.Size(52, 23);
            this.ChooseButton.TabIndex = 13;
            this.ChooseButton.Text = "Choose";
            this.ChooseButton.UseVisualStyleBackColor = true;
            this.ChooseButton.Click += new System.EventHandler(this.ChooseButton_Click);
            // 
            // FormSessionFlats
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(507, 542);
            this.Controls.Add(this.ChooseButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.VoyagerImageFolder);
            this.Controls.Add(this.TakeFlatsButton);
            this.Controls.Add(this.ListFlatsButton);
            this.Controls.Add(this.DecrementAM);
            this.Controls.Add(this.IncrementAM);
            this.Controls.Add(this.DecrementPM);
            this.Controls.Add(this.IncrementPM);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.SessionEndDT);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.SessionStartDT);
            this.Controls.Add(this.TrafficTextBox);
            this.Name = "FormSessionFlats";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TrafficTextBox;
        private System.Windows.Forms.DateTimePicker SessionStartDT;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker SessionEndDT;
        private System.Windows.Forms.Button IncrementPM;
        private System.Windows.Forms.Button DecrementPM;
        private System.Windows.Forms.Button DecrementAM;
        private System.Windows.Forms.Button IncrementAM;
        private System.Windows.Forms.Button ListFlatsButton;
        private System.Windows.Forms.Button TakeFlatsButton;
        private System.Windows.Forms.FolderBrowserDialog FitsFileFolderDialog;
        private System.Windows.Forms.TextBox VoyagerImageFolder;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button ChooseButton;
    }
}

