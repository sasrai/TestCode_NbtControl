using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Mod_ID_shifter
{
	public partial class LogViewer : Form
	{
		private LogViewer()
		{
			InitializeComponent();
		}

		public LogViewer(string message)
			: this()
		{
			logBox.Text = message;
		}

		private void saveButton_Click(object sender, EventArgs e)
		{
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.FileName = "idfix.log";
			sfd.Filter = "ログファイル|*.log|テキストファイル|*.txt|すべてのファイル|*.*";

			if (sfd.ShowDialog() != System.Windows.Forms.DialogResult.OK)
				return;

			using (System.IO.StreamWriter sw = new System.IO.StreamWriter(sfd.OpenFile()))
			{
				sw.Write(logBox.Text);
			}
			sfd.Dispose();
		}

		private void closeButton_Click(object sender, EventArgs e)
		{
			DialogResult = System.Windows.Forms.DialogResult.OK;
			Close();
		}
	}
}
