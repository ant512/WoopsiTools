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
			((System.ComponentModel.ISupportInitialize)(this.FontPictureBox)).BeginInit();
			this.SuspendLayout();
			// 
			// FontListBox
			// 
			this.FontListBox.CausesValidation = false;
			this.FontListBox.FormattingEnabled = true;
			this.FontListBox.Location = new System.Drawing.Point(12, 12);
			this.FontListBox.Name = "FontListBox";
			this.FontListBox.Size = new System.Drawing.Size(194, 303);
			this.FontListBox.TabIndex = 0;
			this.FontListBox.SelectedIndexChanged += new System.EventHandler(this.FontListBox_SelectedIndexChanged);
			// 
			// FontPictureBox
			// 
			this.FontPictureBox.Location = new System.Drawing.Point(213, 13);
			this.FontPictureBox.Name = "FontPictureBox";
			this.FontPictureBox.Size = new System.Drawing.Size(418, 302);
			this.FontPictureBox.TabIndex = 1;
			this.FontPictureBox.TabStop = false;
			// 
			// StyleListBox
			// 
			this.StyleListBox.FormattingEnabled = true;
			this.StyleListBox.Location = new System.Drawing.Point(12, 321);
			this.StyleListBox.Name = "StyleListBox";
			this.StyleListBox.Size = new System.Drawing.Size(194, 43);
			this.StyleListBox.TabIndex = 2;
			this.StyleListBox.SelectedIndexChanged += new System.EventHandler(this.StyleListBox_SelectedIndexChanged);
			// 
			// SaveButton
			// 
			this.SaveButton.Location = new System.Drawing.Point(556, 341);
			this.SaveButton.Name = "SaveButton";
			this.SaveButton.Size = new System.Drawing.Size(75, 23);
			this.SaveButton.TabIndex = 3;
			this.SaveButton.Text = "Save";
			this.SaveButton.UseVisualStyleBackColor = true;
			this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
			// 
			// ClassNameTextBox
			// 
			this.ClassNameTextBox.Location = new System.Drawing.Point(450, 343);
			this.ClassNameTextBox.Name = "ClassNameTextBox";
			this.ClassNameTextBox.Size = new System.Drawing.Size(100, 20);
			this.ClassNameTextBox.TabIndex = 4;
			// 
			// ClassNameLabel
			// 
			this.ClassNameLabel.AutoSize = true;
			this.ClassNameLabel.Location = new System.Drawing.Point(381, 346);
			this.ClassNameLabel.Name = "ClassNameLabel";
			this.ClassNameLabel.Size = new System.Drawing.Size(63, 13);
			this.ClassNameLabel.TabIndex = 5;
			this.ClassNameLabel.Text = "Class Name";
			// 
			// FontTypeListBox
			// 
			this.FontTypeListBox.FormattingEnabled = true;
			this.FontTypeListBox.Location = new System.Drawing.Point(213, 321);
			this.FontTypeListBox.Name = "FontTypeListBox";
			this.FontTypeListBox.Size = new System.Drawing.Size(120, 43);
			this.FontTypeListBox.TabIndex = 6;
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(643, 377);
			this.Controls.Add(this.FontTypeListBox);
			this.Controls.Add(this.ClassNameLabel);
			this.Controls.Add(this.ClassNameTextBox);
			this.Controls.Add(this.SaveButton);
			this.Controls.Add(this.StyleListBox);
			this.Controls.Add(this.FontPictureBox);
			this.Controls.Add(this.FontListBox);
			this.Name = "Form1";
			this.Text = "Form1";
			((System.ComponentModel.ISupportInitialize)(this.FontPictureBox)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.ListBox FontListBox;
		private System.Windows.Forms.PictureBox FontPictureBox;
		private System.Windows.Forms.ListBox StyleListBox;
		private System.Windows.Forms.Button SaveButton;
		private System.Windows.Forms.TextBox ClassNameTextBox;
		private System.Windows.Forms.Label ClassNameLabel;
		private System.Windows.Forms.ListBox FontTypeListBox;
	}
}

