namespace CapacityOfReservoirBate2
{
    partial class CreateWatershedDialog
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
            this.InputDEMLabel = new System.Windows.Forms.Label();
            this.InputDEMComboBox = new System.Windows.Forms.ComboBox();
            this.OKButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.ProgressBar = new System.Windows.Forms.ProgressBar();
            this.BinaryLabel = new System.Windows.Forms.Label();
            this.BinaryIndexTrackBar = new System.Windows.Forms.TrackBar();
            this.BinaryIndexTextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.BinaryIndexTrackBar)).BeginInit();
            this.SuspendLayout();
            // 
            // InputDEMLabel
            // 
            this.InputDEMLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InputDEMLabel.AutoSize = true;
            this.InputDEMLabel.Location = new System.Drawing.Point(28, 28);
            this.InputDEMLabel.Name = "InputDEMLabel";
            this.InputDEMLabel.Size = new System.Drawing.Size(47, 12);
            this.InputDEMLabel.TabIndex = 0;
            this.InputDEMLabel.Text = "DEM图层";
            // 
            // InputDEMComboBox
            // 
            this.InputDEMComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InputDEMComboBox.FormattingEnabled = true;
            this.InputDEMComboBox.Location = new System.Drawing.Point(30, 54);
            this.InputDEMComboBox.Name = "InputDEMComboBox";
            this.InputDEMComboBox.Size = new System.Drawing.Size(406, 20);
            this.InputDEMComboBox.TabIndex = 3;
            // 
            // OKButton
            // 
            this.OKButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.OKButton.Location = new System.Drawing.Point(268, 297);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(69, 23);
            this.OKButton.TabIndex = 13;
            this.OKButton.Text = "确定";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CancelButton.Location = new System.Drawing.Point(367, 297);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(69, 23);
            this.CancelButton.TabIndex = 12;
            this.CancelButton.Text = "取消";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // ProgressBar
            // 
            this.ProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ProgressBar.Location = new System.Drawing.Point(13, 326);
            this.ProgressBar.Name = "ProgressBar";
            this.ProgressBar.Size = new System.Drawing.Size(423, 23);
            this.ProgressBar.TabIndex = 14;
            // 
            // BinaryLabel
            // 
            this.BinaryLabel.AutoSize = true;
            this.BinaryLabel.Location = new System.Drawing.Point(28, 90);
            this.BinaryLabel.Name = "BinaryLabel";
            this.BinaryLabel.Size = new System.Drawing.Size(65, 12);
            this.BinaryLabel.TabIndex = 15;
            this.BinaryLabel.Text = "二值化比例";
            // 
            // BinaryIndexTrackBar
            // 
            this.BinaryIndexTrackBar.Location = new System.Drawing.Point(28, 125);
            this.BinaryIndexTrackBar.Maximum = 100;
            this.BinaryIndexTrackBar.Minimum = 1;
            this.BinaryIndexTrackBar.Name = "BinaryIndexTrackBar";
            this.BinaryIndexTrackBar.Size = new System.Drawing.Size(363, 45);
            this.BinaryIndexTrackBar.TabIndex = 16;
            this.BinaryIndexTrackBar.Value = 1;
            // 
            // BinaryIndexTextBox
            // 
            this.BinaryIndexTextBox.Location = new System.Drawing.Point(388, 130);
            this.BinaryIndexTextBox.Name = "BinaryIndexTextBox";
            this.BinaryIndexTextBox.Size = new System.Drawing.Size(48, 21);
            this.BinaryIndexTextBox.TabIndex = 17;
            // 
            // CreateWatershedDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(454, 361);
            this.Controls.Add(this.BinaryIndexTextBox);
            this.Controls.Add(this.BinaryIndexTrackBar);
            this.Controls.Add(this.BinaryLabel);
            this.Controls.Add(this.ProgressBar);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.InputDEMComboBox);
            this.Controls.Add(this.InputDEMLabel);
            this.MaximumSize = new System.Drawing.Size(470, 400);
            this.MinimumSize = new System.Drawing.Size(470, 400);
            this.Name = "CreateWatershedDialog";
            this.Text = "CreateWatershedDialog";
            this.Load += new System.EventHandler(this.CreateWatershedDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.BinaryIndexTrackBar)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label InputDEMLabel;
        private System.Windows.Forms.ComboBox InputDEMComboBox;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.ProgressBar ProgressBar;
        private System.Windows.Forms.Label BinaryLabel;
        private System.Windows.Forms.TrackBar BinaryIndexTrackBar;
        private System.Windows.Forms.TextBox BinaryIndexTextBox;
    }
}