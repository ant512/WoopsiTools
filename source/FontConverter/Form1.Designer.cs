namespace FontConverter
{
	partial class Form1
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
			this.FontListBox = new System.Windows.Forms.ListBox();
			this.FontPictureBox = new System.Windows.Forms.PictureBox();
			this.StyleListBox = new System.Windows.Forms.ListBox();
			this.SaveButton = new System.Windows.Forms.Button();
			this.ClassNameTextBox = new System.Windows.Forms.TextBox();
			this.ClassNameLabel = new System.Windows.Forms.Label();
			this.FontTypeListBox = new System.Windows.Forms.ListBox();
			this.TextColourButton = new System.Windows.Forms.Button();
			this.BackgroundColourButton = new System.Windows.Forms.Button();
			this.FontTypeGroupBox = new System.Windows.Forms.GroupBox();
			this.OptionsGroupBox = new System.Windows.Forms.GroupBox();
			this.PreviewGroupBox = new System.Windows.Forms.GroupBox();
			this.SizeListBox = new System.Windows.Forms.ListBox();
			((System.ComponentModel.ISupportInitialize)(this.FontPictureBox)).BeginInit();
			this.FontTypeGroupBox.SuspendLayout();
			this.OptionsGroupBox.SuspendLayout();
			this.PreviewGroupBox.SuspendLayout();
			this.SuspendLayout();
			// 
			// FontListBox
			// 
			this.FontListBox.CausesValidation = false;
			this.FontListBox.FormattingEnabled = true;
			this.FontListBox.Location = new System.Drawing.Point(6, 19);
			this.FontListBox.Name = "FontListBox";
			this.FontListBox.Size = new System.Drawing.Size(286, 147);
			this.FontListBox.TabIndex = 0;
			this.FontListBox.SelectedIndexChanged += new System.EventHandler(this.FontListBox_SelectedIndexChanged);
			// 
			// FontPictureBox
			// 
			this.FontPictureBox.Location = new System.Drawing.Point(6, 19);
			this.FontPictureBox.Name = "FontPictureBox";
			this.FontPictureBox.Size = new System.Drawing.Size(578, 239);
			this.FontPictureBox.TabIndex = 1;
			this.FontPictureBox.TabStop = false;
			// 
			// StyleListBox
			// 
			this.StyleListBox.FormattingEnabled = true;
			this.StyleListBox.Location = new System.Drawing.Point(298, 19);
			this.StyleListBox.Name = "StyleListBox";
			this.StyleListBox.Size = new System.Drawing.Size(149, 69);
			this.StyleListBox.TabIndex = 2;
			this.StyleListBox.SelectedIndexChanged += new System.EventHandler(this.StyleListBox_SelectedIndexChanged);
			// 
			// SaveButton
			// 
			this.SaveButton.Location = new System.Drawing.Point(510, 48);
			this.SaveButton.Name = "SaveButton";
			this.SaveButton.Size = new System.Drawing.Size(75, 23);
			this.SaveButton.TabIndex = 3;
			this.SaveButton.Text = "Save";
			this.SaveButton.UseVisualStyleBackColor = true;
			this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
			// 
			// ClassNameTextBox
			// 
			this.ClassNameTextBox.Location = new System.Drawing.Point(272, 22);
			this.ClassNameTextBox.Name = "ClassNameTextBox";
			this.ClassNameTextBox.Size = new System.Drawing.Size(161, 20);
			this.ClassNameTextBox.TabIndex = 4;
			// 
			// ClassNameLabel
			// 
			this.ClassNameLabel.AutoSize = true;
			this.ClassNameLabel.Location = new System.Drawing.Point(203, 25);
			this.ClassNameLabel.Name = "ClassNameLabel";
			this.ClassNameLabel.Size = new System.Drawing.Size(63, 13);
			this.ClassNameLabel.TabIndex = 5;
			this.ClassNameLabel.Text = "Class Name";
			// 
			// FontTypeListBox
			// 
			this.FontTypeListBox.FormattingEnabled = true;
			this.FontTypeListBox.Location = new System.Drawing.Point(298, 97);
			this.FontTypeListBox.Name = "FontTypeListBox";
			this.FontTypeListBox.Size = new System.Drawing.Size(149, 69);
			this.FontTypeListBox.TabIndex = 6;
			// 
			// TextColourButton
			// 
			this.TextColourButton.Location = new System.Drawing.Point(6, 19);
			this.TextColourButton.Name = "TextColourButton";
			this.TextColourButton.Size = new System.Drawing.Size(185, 23);
			this.TextColourButton.TabIndex = 7;
			this.TextColourButton.Text = "Text Colour";
			this.TextColourButton.UseVisualStyleBackColor = true;
			this.TextColourButton.Click += new System.EventHandler(this.TextColourButton_Click);
			// 
			// BackgroundColourButton
			// 
			this.BackgroundColourButton.Location = new System.Drawing.Point(6, 48);
			this.BackgroundColourButton.Name = "BackgroundColourButton";
			this.BackgroundColourButton.Size = new System.Drawing.Size(185, 23);
			this.BackgroundColourButton.TabIndex = 8;
			this.BackgroundColourButton.Text = "Back Colour";
			this.BackgroundColourButton.UseVisualStyleBackColor = true;
			this.BackgroundColourButton.Click += new System.EventHandler(this.BackgroundColourButton_Click);
			// 
			// FontTypeGroupBox
			// 
			this.FontTypeGroupBox.Controls.Add(this.SizeListBox);
			this.FontTypeGroupBox.Controls.Add(this.FontListBox);
			this.FontTypeGroupBox.Controls.Add(this.StyleListBox);
			this.FontTypeGroupBox.Controls.Add(this.FontTypeListBox);
			this.FontTypeGroupBox.Location = new System.Drawing.Point(12, 12);
			this.FontTypeGroupBox.Name = "FontTypeGroupBox";
			this.FontTypeGroupBox.Size = new System.Drawing.Size(591, 185);
			this.FontTypeGroupBox.TabIndex = 9;
			this.FontTypeGroupBox.TabStop = false;
			this.FontTypeGroupBox.Text = "Font Type";
			// 
			// OptionsGroupBox
			// 
			this.OptionsGroupBox.Controls.Add(this.TextColourButton);
			this.OptionsGroupBox.Controls.Add(this.BackgroundColourButton);
			this.OptionsGroupBox.Controls.Add(this.SaveButton);
			this.OptionsGroupBox.Controls.Add(this.ClassNameLabel);
			this.OptionsGroupBox.Controls.Add(this.ClassNameTextBox);
			this.OptionsGroupBox.Location = new System.Drawing.Point(12, 203);
			this.OptionsGroupBox.Name = "OptionsGroupBox";
			this.OptionsGroupBox.Size = new System.Drawing.Size(591, 80);
			this.OptionsGroupBox.TabIndex = 10;
			this.OptionsGroupBox.TabStop = false;
			this.OptionsGroupBox.Text = "Options";
			// 
			// PreviewGroupBox
			// 
			this.PreviewGroupBox.Controls.Add(this.FontPictureBox);
			this.PreviewGroupBox.Location = new System.Drawing.Point(12, 289);
			this.PreviewGroupBox.Name = "PreviewGroupBox";
			this.PreviewGroupBox.Size = new System.Drawing.Size(591, 264);
			this.PreviewGroupBox.TabIndex = 11;
			this.PreviewGroupBox.TabStop = false;
			this.PreviewGroupBox.Text = "PreviewGroupBox";
			// 
			// SizeListBox
			// 
			this.SizeListBox.FormattingEnabled = true;
			this.SizeListBox.Location = new System.Drawing.Point(453, 19);
			this.SizeListBox.Name = "SizeListBox";
			this.SizeListBox.Size = new System.Drawing.Size(120, 147);
			this.SizeListBox.TabIndex = 7;
			this.SizeListBox.SelectedIndexChanged += new System.EventHandler(this.SizeListBox_SelectedIndexChanged);
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(611, 565);
			this.Controls.Add(this.PreviewGroupBox);
			this.Controls.Add(this.OptionsGroupBox);
			this.Controls.Add(this.FontTypeGroupBox);
			this.Name = "Form1";
			this.Text = "Font2Font";
			((System.ComponentModel.ISupportInitialize)(this.FontPictureBox)).EndInit();
			this.FontTypeGroupBox.ResumeLayout(false);
			this.OptionsGroupBox.ResumeLayout(false);
			this.OptionsGroupBox.PerformLayout();
			this.PreviewGroupBox.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListBox FontListBox;
		private System.Windows.Forms.PictureBox FontPictureBox;
		private System.Windows.Forms.ListBox StyleListBox;
		private System.Windows.Forms.Button SaveButton;
		private System.Windows.Forms.TextBox ClassNameTextBox;
		private System.Windows.Forms.Label ClassNameLabel;
		private System.Windows.Forms.ListBox FontTypeListBox;
		private System.Windows.Forms.Button TextColourButton;
		private System.Windows.Forms.Button BackgroundColourButton;
		private System.Windows.Forms.GroupBox FontTypeGroupBox;
		private System.Windows.Forms.GroupBox OptionsGroupBox;
		private System.Windows.Forms.GroupBox PreviewGroupBox;
		private System.Windows.Forms.ListBox SizeListBox;
	}
}

