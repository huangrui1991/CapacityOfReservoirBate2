namespace CapacityOfReservoirBate2
{
    partial class VolumeComputerDialog
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
            this.StreamNetLyrLabel = new System.Windows.Forms.Label();
            this.StreamNetLyrComboBox = new System.Windows.Forms.ComboBox();
            this.DamLyrLabel = new System.Windows.Forms.Label();
            this.DamLyrComboBox = new System.Windows.Forms.ComboBox();
            this.OKButton = new System.Windows.Forms.Button();
            this.CancelButton = new System.Windows.Forms.Button();
            this.WorkSpacePathLabel = new System.Windows.Forms.Label();
            this.WorkSpacePathTextBox = new System.Windows.Forms.TextBox();
            this.BrowseButton = new System.Windows.Forms.Button();
            this.WatershedPolygonLabel = new System.Windows.Forms.Label();
            this.WatershedPolygonComboBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // StreamNetLyrLabel
            // 
            this.StreamNetLyrLabel.AutoSize = true;
            this.StreamNetLyrLabel.Location = new System.Drawing.Point(26, 23);
            this.StreamNetLyrLabel.Name = "StreamNetLyrLabel";
            this.StreamNetLyrLabel.Size = new System.Drawing.Size(53, 12);
            this.StreamNetLyrLabel.TabIndex = 0;
            this.StreamNetLyrLabel.Text = "河网图层";
            // 
            // StreamNetLyrComboBox
            // 
            this.StreamNetLyrComboBox.FormattingEnabled = true;
            this.StreamNetLyrComboBox.Location = new System.Drawing.Point(28, 50);
            this.StreamNetLyrComboBox.Name = "StreamNetLyrComboBox";
            this.StreamNetLyrComboBox.Size = new System.Drawing.Size(412, 20);
            this.StreamNetLyrComboBox.TabIndex = 1;
            // 
            // DamLyrLabel
            // 
            this.DamLyrLabel.AutoSize = true;
            this.DamLyrLabel.Location = new System.Drawing.Point(28, 214);
            this.DamLyrLabel.Name = "DamLyrLabel";
            this.DamLyrLabel.Size = new System.Drawing.Size(29, 12);
            this.DamLyrLabel.TabIndex = 2;
            this.DamLyrLabel.Text = "大坝";
            // 
            // DamLyrComboBox
            // 
            this.DamLyrComboBox.FormattingEnabled = true;
            this.DamLyrComboBox.Location = new System.Drawing.Point(28, 244);
            this.DamLyrComboBox.Name = "DamLyrComboBox";
            this.DamLyrComboBox.Size = new System.Drawing.Size(412, 20);
            this.DamLyrComboBox.TabIndex = 3;
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(268, 305);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(75, 23);
            this.OKButton.TabIndex = 4;
            this.OKButton.Text = "确定";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // CancelButton
            // 
            this.CancelButton.Location = new System.Drawing.Point(349, 305);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 23);
            this.CancelButton.TabIndex = 5;
            this.CancelButton.Text = "取消";
            this.CancelButton.UseVisualStyleBackColor = true;
            // 
            // WorkSpacePathLabel
            // 
            this.WorkSpacePathLabel.AutoSize = true;
            this.WorkSpacePathLabel.Location = new System.Drawing.Point(28, 90);
            this.WorkSpacePathLabel.Name = "WorkSpacePathLabel";
            this.WorkSpacePathLabel.Size = new System.Drawing.Size(53, 12);
            this.WorkSpacePathLabel.TabIndex = 6;
            this.WorkSpacePathLabel.Text = "输出路径";
            // 
            // WorkSpacePathTextBox
            // 
            this.WorkSpacePathTextBox.Location = new System.Drawing.Point(28, 115);
            this.WorkSpacePathTextBox.Name = "WorkSpacePathTextBox";
            this.WorkSpacePathTextBox.Size = new System.Drawing.Size(331, 21);
            this.WorkSpacePathTextBox.TabIndex = 7;
            // 
            // BrowseButton
            // 
            this.BrowseButton.Location = new System.Drawing.Point(365, 115);
            this.BrowseButton.Name = "BrowseButton";
            this.BrowseButton.Size = new System.Drawing.Size(75, 23);
            this.BrowseButton.TabIndex = 8;
            this.BrowseButton.Text = "浏览";
            this.BrowseButton.UseVisualStyleBackColor = true;
            this.BrowseButton.Click += new System.EventHandler(this.BrowseButton_Click);
            // 
            // WatershedPolygonLabel
            // 
            this.WatershedPolygonLabel.AutoSize = true;
            this.WatershedPolygonLabel.Location = new System.Drawing.Point(28, 154);
            this.WatershedPolygonLabel.Name = "WatershedPolygonLabel";
            this.WatershedPolygonLabel.Size = new System.Drawing.Size(41, 12);
            this.WatershedPolygonLabel.TabIndex = 9;
            this.WatershedPolygonLabel.Text = "汇水区";
            // 
            // WatershedPolygonComboBox
            // 
            this.WatershedPolygonComboBox.FormattingEnabled = true;
            this.WatershedPolygonComboBox.Location = new System.Drawing.Point(28, 179);
            this.WatershedPolygonComboBox.Name = "WatershedPolygonComboBox";
            this.WatershedPolygonComboBox.Size = new System.Drawing.Size(412, 20);
            this.WatershedPolygonComboBox.TabIndex = 10;
            // 
            // VolumeComputerDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(454, 361);
            this.Controls.Add(this.WatershedPolygonComboBox);
            this.Controls.Add(this.WatershedPolygonLabel);
            this.Controls.Add(this.BrowseButton);
            this.Controls.Add(this.WorkSpacePathTextBox);
            this.Controls.Add(this.WorkSpacePathLabel);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.DamLyrComboBox);
            this.Controls.Add(this.DamLyrLabel);
            this.Controls.Add(this.StreamNetLyrComboBox);
            this.Controls.Add(this.StreamNetLyrLabel);
            this.MaximumSize = new System.Drawing.Size(470, 400);
            this.MinimumSize = new System.Drawing.Size(470, 400);
            this.Name = "VolumeComputerDialog";
            this.Text = "VolumeComputerDialog";
            this.Load += new System.EventHandler(this.VolumeComputerDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label StreamNetLyrLabel;
        private System.Windows.Forms.ComboBox StreamNetLyrComboBox;
        private System.Windows.Forms.Label DamLyrLabel;
        private System.Windows.Forms.ComboBox DamLyrComboBox;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Label WorkSpacePathLabel;
        private System.Windows.Forms.TextBox WorkSpacePathTextBox;
        private System.Windows.Forms.Button BrowseButton;
        private System.Windows.Forms.Label WatershedPolygonLabel;
        private System.Windows.Forms.ComboBox WatershedPolygonComboBox;
    }
}