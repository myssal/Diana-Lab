using BD2.GUI.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
namespace BD2.GUI
{
	public partial class MainForm : Form
	{
		// global check
		LoadUpSettings loadUpSettings;
		public MainForm()
		{
			InitializeComponent();
		}
		private void Form1_Load(object sender, EventArgs e)
		{
			loadUpSettings = new LoadUpSettings();
			if (!loadUpSettings.isSettingsJsonExist)
			{
				MessageBox.Show("Settings.json not found, please create one", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			LoadSettings();
		}

		private void LoadSettings()
		{
			tex2d_checkBox.Checked = loadUpSettings.settings.options.Assettypes.Texture2D;
			txtAsset_checkBox.Checked = loadUpSettings.settings.options.Assettypes.TextAsset;
			audio_checkBox.Checked = loadUpSettings.settings.options.Assettypes.Audio;

			exportAsset_checkBox.Checked = loadUpSettings.settings.options.Process.Exportasset;
			sortAsset_checkBox.Checked = loadUpSettings.settings.options.Process.Sortasset;
			copy2Repo_checkBox.Checked = loadUpSettings.settings.options.Process.Copytorepo;
			exportL2DBgs_checkBox.Checked = loadUpSettings.settings.options.Process.Exportl2dbgs;
			deleteTemp_checkBox.Checked = loadUpSettings.settings.options.Process.Deletedata;

			dataFolder_txtBox.Text = loadUpSettings.settings.paths.Data;
			outputFolder_txtBox.Text = loadUpSettings.settings.paths.Output;
		}

		private void reset_btn_Click(object sender, EventArgs e)
		{

		}

		private void dataFolder_folderDialog_Click(object sender, EventArgs e)
		{
			FolderBrowserDialog getInput = new FolderBrowserDialog();
			getInput.Description = "Choose data folder contains assets files.";
			if (getInput.ShowDialog() == DialogResult.OK)
			{
				dataFolder_txtBox.Text = getInput.SelectedPath;
			}
		}

		private void outputFolder_folderDialog_Click(object sender, EventArgs e)
		{
			FolderBrowserDialog getInput = new FolderBrowserDialog();
			getInput.Description = "Choose output folder to save assets.";
			if (getInput.ShowDialog() == DialogResult.OK)
			{
				dataFolder_txtBox.Text = getInput.SelectedPath;
			}
		}

		private void saveSettings_btn_Click(object sender, EventArgs e)
		{
			loadUpSettings.settings.options.Assettypes.Texture2D = tex2d_checkBox.Checked;
			loadUpSettings.settings.options.Assettypes.TextAsset = txtAsset_checkBox.Checked;
			loadUpSettings.settings.options.Assettypes.Audio = audio_checkBox.Checked;

			loadUpSettings.settings.options.Process.Exportasset = exportAsset_checkBox.Checked;
			loadUpSettings.settings.options.Process.Sortasset = sortAsset_checkBox.Checked;
			loadUpSettings.settings.options.Process.Copytorepo = copy2Repo_checkBox.Checked;
			loadUpSettings.settings.options.Process.Exportl2dbgs = exportL2DBgs_checkBox.Checked;
			loadUpSettings.settings.options.Process.Deletedata = deleteTemp_checkBox.Checked;

			loadUpSettings.settings.paths.Data = dataFolder_txtBox.Text;
			loadUpSettings.settings.paths.Output = outputFolder_txtBox.Text;
			loadUpSettings.SaveSettings();
			MessageBox.Show("Settings saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void start_btn_Click(object sender, EventArgs e)
		{

		}
	}
}
