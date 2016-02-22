namespace CapacityOfReservoirBate2
{
    partial class CreateDEMDialog
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
            this.InputPathLabel = new System.Windows.Forms.Label();
            this.OutputPathLabel = new System.Windows.Forms.Label();
            this.OutputPathTextBox = new System.Windows.Forms.TextBox();
            this.OutputPathButton = new System.Windows.Forms.Button();
            this.HeightFieldLabel = new System.Windows.Forms.Label();
            this.HeightFieldComboBox = new System.Windows.Forms.ComboBox();
            this.CellSizeLabel = new System.Windows.Forms.Label();
            this.CellSizeTextBox = new System.Windows.Forms.TextBox();
            this.CancelButton = new System.Windows.Forms.Button();
            this.OKButton = new System.Windows.Forms.Button();
            this.InputFeatureComboBox = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // InputPathLabel
            // 
            this.InputPathLabel.AutoSize = true;
            this.InputPathLabel.Location = new System.Drawing.Point(39, 21);
            this.InputPathLabel.Name = "InputPathLabel";
            this.InputPathLabel.Size = new System.Drawing.Size(65, 12);
            this.InputPathLabel.TabIndex = 0;
            this.InputPathLabel.Text = "输入等高线";
            // 
            // OutputPathLabel
            // 
            this.OutputPathLabel.AutoSize = true;
            this.OutputPathLabel.Location = new System.Drawing.Point(39, 92);
            this.OutputPathLabel.Name = "OutputPathLabel";
            this.OutputPathLabel.Size = new System.Drawing.Size(47, 12);
            this.OutputPathLabel.TabIndex = 3;
            this.OutputPathLabel.Text = "输出DEM";
            // 
            // OutputPathTextBox
            // 
            this.OutputPathTextBox.Location = new System.Drawing.Point(41, 116);
            this.OutputPathTextBox.Name = "OutputPathTextBox";
            this.OutputPathTextBox.Size = new System.Drawing.Size(311, 21);
            this.OutputPathTextBox.TabIndex = 4;
            this.OutputPathTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.OutputPathTextBox_KeyPress);
            // 
            // OutputPathButton
            // 
            this.OutputPathButton.Location = new System.Drawing.Point(358, 116);
            this.OutputPathButton.Name = "OutputPathButton";
            this.OutputPathButton.Size = new System.Drawing.Size(69, 21);
            this.OutputPathButton.TabIndex = 5;
            this.OutputPathButton.Text = "浏览";
            this.OutputPathButton.UseVisualStyleBackColor = true;
            this.OutputPathButton.Click += new System.EventHandler(this.OutputPathButton_Click);
            // 
            // HeightFieldLabel
            // 
            this.HeightFieldLabel.AutoSize = true;
            this.HeightFieldLabel.Location = new System.Drawing.Point(39, 162);
            this.HeightFieldLabel.Name = "HeightFieldLabel";
            this.HeightFieldLabel.Size = new System.Drawing.Size(53, 12);
            this.HeightFieldLabel.TabIndex = 6;
            this.HeightFieldLabel.Text = "高程字段";
            // 
            // HeightFieldComboBox
            // 
            this.HeightFieldComboBox.FormattingEnabled = true;
            this.HeightFieldComboBox.Location = new System.Drawing.Point(41, 192);
            this.HeightFieldComboBox.Name = "HeightFieldComboBox";
            this.HeightFieldComboBox.Size = new System.Drawing.Size(386, 20);
            this.HeightFieldComboBox.TabIndex = 7;
            this.HeightFieldComboBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.HeightFieldComboBox_KeyPress);
            // 
            // CellSizeLabel
            // 
            this.CellSizeLabel.AutoSize = true;
            this.CellSizeLabel.Location = new System.Drawing.Point(41, 233);
            this.CellSizeLabel.Name = "CellSizeLabel";
            this.CellSizeLabel.Size = new System.Drawing.Size(71, 12);
            this.CellSizeLabel.TabIndex = 8;
            this.CellSizeLabel.Text = "DEM栅格大小";
            // 
            // CellSizeTextBox
            // 
            this.CellSizeTextBox.Location = new System.Drawing.Point(43, 262);
            this.CellSizeTextBox.Name = "CellSizeTextBox";
            this.CellSizeTextBox.Size = new System.Drawing.Size(384, 21);
            this.CellSizeTextBox.TabIndex = 9;
            this.CellSizeTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.CellSizeTextBox_KeyPress);
            // 
            // CancelButton
            // 
            this.CancelButton.Location = new System.Drawing.Point(358, 314);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(69, 23);
            this.CancelButton.TabIndex = 10;
            this.CancelButton.Text = "取消";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // OKButton
            // 
            this.OKButton.Location = new System.Drawing.Point(259, 314);
            this.OKButton.Name = "OKButton";
            this.OKButton.Size = new System.Drawing.Size(69, 23);
            this.OKButton.TabIndex = 11;
            this.OKButton.Text = "确定";
            this.OKButton.UseVisualStyleBackColor = true;
            this.OKButton.Click += new System.EventHandler(this.OKButton_Click);
            // 
            // InputFeatureComboBox
            // 
            this.InputFeatureComboBox.FormattingEnabled = true;
            this.InputFeatureComboBox.Location = new System.Drawing.Point(41, 53);
            this.InputFeatureComboBox.Name = "InputFeatureComboBox";
            this.InputFeatureComboBox.Size = new System.Drawing.Size(386, 20);
            this.InputFeatureComboBox.TabIndex = 12;
            this.InputFeatureComboBox.SelectedIndexChanged += new System.EventHandler(this.InputFeatureComboBox_SelectedIndexChanged);
            this.InputFeatureComboBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.InputFeatureComboBox_KeyPress);
            // 
            // CreateDEMDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(454, 361);
            this.Controls.Add(this.InputFeatureComboBox);
            this.Controls.Add(this.OKButton);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.CellSizeTextBox);
            this.Controls.Add(this.CellSizeLabel);
            this.Controls.Add(this.HeightFieldComboBox);
            this.Controls.Add(this.HeightFieldLabel);
            this.Controls.Add(this.OutputPathButton);
            this.Controls.Add(this.OutputPathTextBox);
            this.Controls.Add(this.OutputPathLabel);
            this.Controls.Add(this.InputPathLabel);
            this.MaximumSize = new System.Drawing.Size(470, 400);
            this.MinimumSize = new System.Drawing.Size(470, 400);
            this.Name = "CreateDEMDialog";
            this.Text = "CreateDEMDialog";
            this.Load += new System.EventHandler(this.CreateDEMDialog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label InputPathLabel;
        private System.Windows.Forms.Label OutputPathLabel;
        private System.Windows.Forms.TextBox OutputPathTextBox;
        private System.Windows.Forms.Button OutputPathButton;
        private System.Windows.Forms.Label HeightFieldLabel;
        private System.Windows.Forms.ComboBox HeightFieldComboBox;
        private System.Windows.Forms.Label CellSizeLabel;
        private System.Windows.Forms.TextBox CellSizeTextBox;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Button OKButton;
        private System.Windows.Forms.ComboBox InputFeatureComboBox;
    }
}