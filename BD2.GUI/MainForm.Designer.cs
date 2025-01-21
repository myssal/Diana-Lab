namespace BD2.GUI
{
	partial class MainForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.outputLog_txtBox = new System.Windows.Forms.RichTextBox();
			this.dataFolder_txtBox = new System.Windows.Forms.TextBox();
			this.outputFolder_txtBox = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.dataFolder_folderDialog = new System.Windows.Forms.Button();
			this.outputFolder_folderDialog = new System.Windows.Forms.Button();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.saveSettings_btn = new System.Windows.Forms.Button();
			this.start_btn = new System.Windows.Forms.Button();
			this.reset_btn = new System.Windows.Forms.Button();
			this.exportL2DBgs_checkBox = new System.Windows.Forms.CheckBox();
			this.deleteTemp_checkBox = new System.Windows.Forms.CheckBox();
			this.copy2Repo_checkBox = new System.Windows.Forms.CheckBox();
			this.sortAsset_checkBox = new System.Windows.Forms.CheckBox();
			this.exportAsset_checkBox = new System.Windows.Forms.CheckBox();
			this.label4 = new System.Windows.Forms.Label();
			this.audio_checkBox = new System.Windows.Forms.CheckBox();
			this.txtAsset_checkBox = new System.Windows.Forms.CheckBox();
			this.tex2d_checkBox = new System.Windows.Forms.CheckBox();
			this.label3 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			this.groupBox3.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.outputLog_txtBox);
			this.groupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
			this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.groupBox1.Location = new System.Drawing.Point(0, 355);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(773, 310);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Output log";
			// 
			// outputLog_txtBox
			// 
			this.outputLog_txtBox.Enabled = false;
			this.outputLog_txtBox.Location = new System.Drawing.Point(28, 29);
			this.outputLog_txtBox.Name = "outputLog_txtBox";
			this.outputLog_txtBox.ReadOnly = true;
			this.outputLog_txtBox.Size = new System.Drawing.Size(715, 269);
			this.outputLog_txtBox.TabIndex = 0;
			this.outputLog_txtBox.Text = "";
			// 
			// dataFolder_txtBox
			// 
			this.dataFolder_txtBox.Location = new System.Drawing.Point(167, 36);
			this.dataFolder_txtBox.Name = "dataFolder_txtBox";
			this.dataFolder_txtBox.Size = new System.Drawing.Size(518, 27);
			this.dataFolder_txtBox.TabIndex = 1;
			// 
			// outputFolder_txtBox
			// 
			this.outputFolder_txtBox.Location = new System.Drawing.Point(167, 83);
			this.outputFolder_txtBox.Name = "outputFolder_txtBox";
			this.outputFolder_txtBox.Size = new System.Drawing.Size(518, 27);
			this.outputFolder_txtBox.TabIndex = 2;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(25, 39);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(97, 20);
			this.label1.TabIndex = 3;
			this.label1.Text = "Data folder:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(25, 90);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(111, 20);
			this.label2.TabIndex = 4;
			this.label2.Text = "Output folder:";
			// 
			// dataFolder_folderDialog
			// 
			this.dataFolder_folderDialog.Location = new System.Drawing.Point(702, 36);
			this.dataFolder_folderDialog.Name = "dataFolder_folderDialog";
			this.dataFolder_folderDialog.Size = new System.Drawing.Size(41, 27);
			this.dataFolder_folderDialog.TabIndex = 5;
			this.dataFolder_folderDialog.Text = "...";
			this.dataFolder_folderDialog.UseVisualStyleBackColor = true;
			// 
			// outputFolder_folderDialog
			// 
			this.outputFolder_folderDialog.Location = new System.Drawing.Point(702, 83);
			this.outputFolder_folderDialog.Name = "outputFolder_folderDialog";
			this.outputFolder_folderDialog.Size = new System.Drawing.Size(41, 27);
			this.outputFolder_folderDialog.TabIndex = 6;
			this.outputFolder_folderDialog.Text = "...";
			this.outputFolder_folderDialog.UseVisualStyleBackColor = true;
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.dataFolder_txtBox);
			this.groupBox2.Controls.Add(this.outputFolder_folderDialog);
			this.groupBox2.Controls.Add(this.outputFolder_txtBox);
			this.groupBox2.Controls.Add(this.dataFolder_folderDialog);
			this.groupBox2.Controls.Add(this.label1);
			this.groupBox2.Controls.Add(this.label2);
			this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
			this.groupBox2.Location = new System.Drawing.Point(0, 0);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(773, 129);
			this.groupBox2.TabIndex = 7;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Assets";
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.saveSettings_btn);
			this.groupBox3.Controls.Add(this.start_btn);
			this.groupBox3.Controls.Add(this.reset_btn);
			this.groupBox3.Controls.Add(this.exportL2DBgs_checkBox);
			this.groupBox3.Controls.Add(this.deleteTemp_checkBox);
			this.groupBox3.Controls.Add(this.copy2Repo_checkBox);
			this.groupBox3.Controls.Add(this.sortAsset_checkBox);
			this.groupBox3.Controls.Add(this.exportAsset_checkBox);
			this.groupBox3.Controls.Add(this.label4);
			this.groupBox3.Controls.Add(this.audio_checkBox);
			this.groupBox3.Controls.Add(this.txtAsset_checkBox);
			this.groupBox3.Controls.Add(this.tex2d_checkBox);
			this.groupBox3.Controls.Add(this.label3);
			this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.groupBox3.Location = new System.Drawing.Point(0, 129);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(773, 226);
			this.groupBox3.TabIndex = 8;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Options";
			// 
			// saveSettings_btn
			// 
			this.saveSettings_btn.Location = new System.Drawing.Point(395, 171);
			this.saveSettings_btn.Name = "saveSettings_btn";
			this.saveSettings_btn.Size = new System.Drawing.Size(153, 30);
			this.saveSettings_btn.TabIndex = 18;
			this.saveSettings_btn.Text = "Save settings";
			this.saveSettings_btn.UseVisualStyleBackColor = true;
			// 
			// start_btn
			// 
			this.start_btn.Location = new System.Drawing.Point(581, 171);
			this.start_btn.Name = "start_btn";
			this.start_btn.Size = new System.Drawing.Size(96, 30);
			this.start_btn.TabIndex = 17;
			this.start_btn.Text = "Start";
			this.start_btn.UseVisualStyleBackColor = true;
			// 
			// reset_btn
			// 
			this.reset_btn.Location = new System.Drawing.Point(278, 171);
			this.reset_btn.Name = "reset_btn";
			this.reset_btn.Size = new System.Drawing.Size(96, 30);
			this.reset_btn.TabIndex = 16;
			this.reset_btn.Text = "Reset";
			this.reset_btn.UseVisualStyleBackColor = true;
			this.reset_btn.Click += new System.EventHandler(this.reset_btn_Click);
			// 
			// exportL2DBgs_checkBox
			// 
			this.exportL2DBgs_checkBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.exportL2DBgs_checkBox.AutoSize = true;
			this.exportL2DBgs_checkBox.Location = new System.Drawing.Point(395, 76);
			this.exportL2DBgs_checkBox.Name = "exportL2DBgs_checkBox";
			this.exportL2DBgs_checkBox.Size = new System.Drawing.Size(138, 24);
			this.exportL2DBgs_checkBox.TabIndex = 15;
			this.exportL2DBgs_checkBox.Text = "Export l2d bgs";
			this.exportL2DBgs_checkBox.UseVisualStyleBackColor = true;
			// 
			// deleteTemp_checkBox
			// 
			this.deleteTemp_checkBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.deleteTemp_checkBox.AutoSize = true;
			this.deleteTemp_checkBox.Location = new System.Drawing.Point(395, 118);
			this.deleteTemp_checkBox.Name = "deleteTemp_checkBox";
			this.deleteTemp_checkBox.Size = new System.Drawing.Size(247, 24);
			this.deleteTemp_checkBox.TabIndex = 14;
			this.deleteTemp_checkBox.Text = "Delete data and temp folders";
			this.deleteTemp_checkBox.UseVisualStyleBackColor = true;
			// 
			// copy2Repo_checkBox
			// 
			this.copy2Repo_checkBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.copy2Repo_checkBox.AutoSize = true;
			this.copy2Repo_checkBox.Location = new System.Drawing.Point(581, 76);
			this.copy2Repo_checkBox.Name = "copy2Repo_checkBox";
			this.copy2Repo_checkBox.Size = new System.Drawing.Size(126, 24);
			this.copy2Repo_checkBox.TabIndex = 13;
			this.copy2Repo_checkBox.Text = "Copy to repo";
			this.copy2Repo_checkBox.UseVisualStyleBackColor = true;
			// 
			// sortAsset_checkBox
			// 
			this.sortAsset_checkBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.sortAsset_checkBox.AutoSize = true;
			this.sortAsset_checkBox.Location = new System.Drawing.Point(239, 118);
			this.sortAsset_checkBox.Name = "sortAsset_checkBox";
			this.sortAsset_checkBox.Size = new System.Drawing.Size(108, 24);
			this.sortAsset_checkBox.TabIndex = 12;
			this.sortAsset_checkBox.Text = "Sort asset";
			this.sortAsset_checkBox.UseVisualStyleBackColor = true;
			// 
			// exportAsset_checkBox
			// 
			this.exportAsset_checkBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.exportAsset_checkBox.AutoSize = true;
			this.exportAsset_checkBox.Location = new System.Drawing.Point(239, 76);
			this.exportAsset_checkBox.Name = "exportAsset_checkBox";
			this.exportAsset_checkBox.Size = new System.Drawing.Size(125, 24);
			this.exportAsset_checkBox.TabIndex = 11;
			this.exportAsset_checkBox.Text = "Export asset";
			this.exportAsset_checkBox.UseVisualStyleBackColor = true;
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(222, 34);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(76, 20);
			this.label4.TabIndex = 10;
			this.label4.Text = "Process:";
			// 
			// audio_checkBox
			// 
			this.audio_checkBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.audio_checkBox.AutoSize = true;
			this.audio_checkBox.Enabled = false;
			this.audio_checkBox.Location = new System.Drawing.Point(44, 160);
			this.audio_checkBox.Name = "audio_checkBox";
			this.audio_checkBox.Size = new System.Drawing.Size(73, 24);
			this.audio_checkBox.TabIndex = 9;
			this.audio_checkBox.Text = "Audio";
			this.audio_checkBox.UseVisualStyleBackColor = true;
			// 
			// txtAsset_checkBox
			// 
			this.txtAsset_checkBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.txtAsset_checkBox.AutoSize = true;
			this.txtAsset_checkBox.Location = new System.Drawing.Point(44, 118);
			this.txtAsset_checkBox.Name = "txtAsset_checkBox";
			this.txtAsset_checkBox.Size = new System.Drawing.Size(111, 24);
			this.txtAsset_checkBox.TabIndex = 8;
			this.txtAsset_checkBox.Text = "Text Asset";
			this.txtAsset_checkBox.UseVisualStyleBackColor = true;
			// 
			// tex2d_checkBox
			// 
			this.tex2d_checkBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tex2d_checkBox.AutoSize = true;
			this.tex2d_checkBox.Location = new System.Drawing.Point(44, 76);
			this.tex2d_checkBox.Name = "tex2d_checkBox";
			this.tex2d_checkBox.Size = new System.Drawing.Size(109, 24);
			this.tex2d_checkBox.TabIndex = 7;
			this.tex2d_checkBox.Text = "Texture2D";
			this.tex2d_checkBox.UseVisualStyleBackColor = true;
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(25, 34);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(107, 20);
			this.label3.TabIndex = 4;
			this.label3.Text = "Export types:";
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 20F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(773, 665);
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Brown Dust 2 Tool v1.0";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			this.groupBox3.ResumeLayout(false);
			this.groupBox3.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RichTextBox outputLog_txtBox;
		private System.Windows.Forms.TextBox dataFolder_txtBox;
		private System.Windows.Forms.TextBox outputFolder_txtBox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button dataFolder_folderDialog;
		private System.Windows.Forms.Button outputFolder_folderDialog;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.CheckBox copy2Repo_checkBox;
		private System.Windows.Forms.CheckBox sortAsset_checkBox;
		private System.Windows.Forms.CheckBox exportAsset_checkBox;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.CheckBox audio_checkBox;
		private System.Windows.Forms.CheckBox txtAsset_checkBox;
		private System.Windows.Forms.CheckBox tex2d_checkBox;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.CheckBox exportL2DBgs_checkBox;
		private System.Windows.Forms.CheckBox deleteTemp_checkBox;
		private System.Windows.Forms.Button start_btn;
		private System.Windows.Forms.Button reset_btn;
		private System.Windows.Forms.Button saveSettings_btn;
	}
}

