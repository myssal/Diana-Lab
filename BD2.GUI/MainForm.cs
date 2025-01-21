using BD2.GUI.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace BD2.GUI
{
	public partial class MainForm : Form
	{
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
	}
}
