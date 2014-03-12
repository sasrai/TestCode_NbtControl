using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Mod_ID_shifter
{
	public partial class MainForm : Form
	{
		LibNbt.NbtFile levelNbt = null;
		string loadingNbtFilePath = null;
		ModInfo selectedModInfo = null;

		public MainForm()
		{
			InitializeComponent();
		}

		private void inputRef_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();

			ofd.FileName = "level.dat";
			ofd.Filter = "Minecraft Savedata|level.dat";

			DialogResult result = ofd.ShowDialog();

			if (result == System.Windows.Forms.DialogResult.OK)
			{
				// inputPathへ登録
				inputPath.Text = ofd.FileName;

				inputPath_Leave(null, null);
			}
		}

		private void NbtLoad()
		{
			if (!File.Exists(inputPath.Text))
				return;

			saveLevelDatButton.Enabled = false;
			updateLevelDatButton.Enabled = false;

			if (null != levelNbt)
				levelNbt.Dispose();

			loadingNbtFilePath = inputPath.Text;

			levelNbt = new LibNbt.NbtFile();
			levelNbt.LoadFile(loadingNbtFilePath);
		}

		private void NbtSave()
		{
			if (null == levelNbt)
				return;

			int backupNum = 0;
			while (true)
			{
				try
				{
					File.Copy(loadingNbtFilePath, Path.GetDirectoryName(loadingNbtFilePath) + "\\" + Path.GetFileName(loadingNbtFilePath) + ((backupNum > 0)? "_" + (backupNum - 1): "") + ".bak");
					break;
				}
				catch (IOException) { backupNum++; }
			}
			levelNbt.SaveFile(loadingNbtFilePath);
		}

		private List<LibNbt.Tags.NbtCompound> GetModListFromNBT(LibNbt.Tags.NbtCompound rootNbt)
		{
			LibNbt.Tags.NbtList nbtModList = rootNbt.Query<LibNbt.Tags.NbtList>("//FML/ModList");
			if (null == nbtModList)
			{
				System.Diagnostics.Debug.WriteLine("Not 1.7.x level.dat");
				return null;
			}

			List<LibNbt.Tags.NbtCompound> result = new List<LibNbt.Tags.NbtCompound>();
			foreach (LibNbt.Tags.NbtCompound modInfo in nbtModList.Tags)
				result.Add(modInfo);

			return result;
		}

		private void UpdateModList()
		{
			if (null == levelNbt)
				return;

			modList.Items.Clear();

			// コンボボックスにModIdを追加
			foreach (LibNbt.Tags.NbtCompound modInfo in GetModListFromNBT(levelNbt.RootTag))
			{
				try
				{
					modList.Items.Add(modInfo.Get<LibNbt.Tags.NbtString>("ModId").Value);
				}
				catch (KeyNotFoundException)
				{
					System.Diagnostics.Trace.Write("NBTのMOD情報が1.7のFMLと違う構造になってるぽい？");
				}
			}

			// mcp,FML,Forge以外のMODが読み込まれている場合は初期値として選択
			if (modList.Items.Count > 0)
			{
				int idx = 0;
				foreach (string modId in modList.Items)
				{
					if (modId == "mcp" || modId == "FML" || modId == "Forge")
						idx++;
					else
						break;
				}
				if (modList.Items.Count > idx)
					modList.SelectedIndex = idx;
			}
		}

		private void UpdateModInfo(string modId)
		{
			if (string.IsNullOrWhiteSpace(modId) || levelNbt == null)
				return;

			idMisalignmentFixButton.Visible = false;
			updateLevelDatButton.Enabled = false;

			try
			{
				selectedModInfo = new ModInfo(modId, levelNbt.Query<LibNbt.Tags.NbtList>("//FML/ItemData"));
			}
			catch (KeyNotFoundException)
			{
				MessageBox.Show("1.7より前のバージョンのlevel.datを読み込んだ可能性があります。");

				inputPath.Text = "";
				return;
			}
			useIDText_TextChanged(useBlockIDText, null);
			useBlockIDText.Text = selectedModInfo.GetBlockNumberIDListString();
			useIDText_TextChanged(useItemIDText, null);
			useItemIDText.Text = selectedModInfo.GetItemNumberIDListString();

			ModInfo allInfo = new ModInfo("", levelNbt.Query<LibNbt.Tags.NbtList>("//FML/ItemData"));

			// ブロックIDのずらせる範囲を簡易調整
			blockIDShiftNum.Value = 0;
			if (selectedModInfo.BlockIDs.Count > 0)
				blockIDShiftNum.Maximum = 4095 - selectedModInfo.GetBlockNumberIDList().Max();
			else
				blockIDShiftNum.Maximum = 0;
			List<int> allBlockNumID = allInfo.BlockIDs.Values.ToList();
			allBlockNumID.Sort();
			if (selectedModInfo.BlockIDs.Count > 0)
				blockIDShiftNum.Minimum = allBlockNumID[allBlockNumID.IndexOf(selectedModInfo.GetBlockNumberIDList().Min()) - 1] - allBlockNumID[allBlockNumID.IndexOf(selectedModInfo.GetBlockNumberIDList().Min())] + 1;
			else
				blockIDShiftNum.Minimum = 0;

			// アイテムIDのずらせる範囲を簡易調整
			itemIDShiftNum.Value = 0;
			if (selectedModInfo.ItemIDs.Count > 0)
			{
				itemIDShiftNum.Minimum = 4096 - selectedModInfo.GetItemNumberIDList().Min((a) =>
				{
					// ブロックIDに含まれている物と4096未満のアイテムIDを最小値計算に利用しない(バグ有り処理)
					if (selectedModInfo.BlockIDs.Values.Contains(a) || a < 4096)
						return 32000;
					else
						return a;
				});
				itemIDShiftNum.Maximum = 32000 - selectedModInfo.GetItemNumberIDList().Max();
			}
			else
				itemIDShiftNum.Minimum = itemIDShiftNum.Maximum = 0;

			foreach (var checkID in selectedModInfo.GetBlockAndItemIDList())
			{
				if (checkID.Value[0] != checkID.Value[1])
				{
					if (!idMisalignmentFixButton.Visible)
						MessageBox.Show("ブロックIDとアイテムIDの不一致を検出しました。\n簡易修正機能を有効化します。");
					idMisalignmentFixButton.Enabled = true;
					idMisalignmentFixButton.Visible = true;

					System.Diagnostics.Trace.WriteLine("[IDMisalignment-Checker] " + checkID.Key + " = B:" + checkID.Value[0] + ", I:" + checkID.Value[1]);
				}
			}

			return;
		}

		private void inputPath_Leave(object sender, EventArgs e)
		{
			selectedModInfo = null;

			NbtLoad();

			UpdateModList();
		}

		private void modList_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (modList.SelectedIndex >= 0)
				UpdateModInfo((string)modList.SelectedItem);
		}

		private void showIDListWindow_Click(object sender, EventArgs e)
		{
			IDListWindow dialog = new IDListWindow(selectedModInfo);

			dialog.ShowDialog(this);
		}

		private void blockIDShiftNum_ValueChanged(object sender, EventArgs e)
		{
			selectedModInfo.BlockIDShiftNum = (int)blockIDShiftNum.Value;
			if (!CheckConflictID(selectedModInfo, new List<string>(modList.Items.OfType<string>()), levelNbt.Query<LibNbt.Tags.NbtList>("//FML/ItemData")))
				blockIDShiftNum.Value++;

			useBlockIDText.Text = selectedModInfo.GetBlockNumberIDListString();
			useItemIDText.Text = selectedModInfo.GetItemNumberIDListString();

			updateLevelDatButton.Enabled = true;
		}

		private void itemIDShiftNum_ValueChanged(object sender, EventArgs e)
		{
			selectedModInfo.ItemIDShiftNum = (int)itemIDShiftNum.Value;
			if (!CheckConflictID(selectedModInfo, new List<string>(modList.Items.OfType<string>()), levelNbt.Query<LibNbt.Tags.NbtList>("//FML/ItemData")))
				blockIDShiftNum.Value++;

			useItemIDText.Text = selectedModInfo.GetItemNumberIDListString();

			updateLevelDatButton.Enabled = true;
		}

		/// <summary>
		/// ModInfoクラスの現在の内容でIDが干渉していないか確認する。
		/// </summary>
		/// <param name="checkModInfo">確認したいModInfo</param>
		/// <param name="modIdList">チェックするModIDリスト</param>
		/// <param name="originalNBTItemDataList">level.datのItemDataリスト</param>
		/// <returns>true=干渉無し、false=干渉有り</returns>
		private bool CheckConflictID(ModInfo checkModInfo, List<string> modIdList, LibNbt.Tags.NbtList originalNBTItemDataList)
		{
			// ID被りチェック
			try
			{
				foreach (string modid in modIdList)
				{
					if (modid == checkModInfo.ModID)
						continue;

					ModInfo mi = new ModInfo(modid, originalNBTItemDataList);

					foreach (int id in checkModInfo.BlockIDs.Values)
						if (mi.BlockIDs.Values.Contains(id))
							throw new ApplicationException("ID被ってるよ！");
					foreach (int id in checkModInfo.ItemIDs.Values)
						if (mi.ItemIDs.Values.Contains(id))
							throw new ApplicationException("ID被ってるよ！");
				}
			}
			catch (ApplicationException)
			{
				MessageBox.Show("移動先が別IDと被ってる為ID移動を適用できませんっ");
				return false;
			}

			return true;
		}

		private void updateLevelDatButton_Click(object sender, EventArgs e)
		{
			if (!CheckConflictID(selectedModInfo, new List<string>(modList.Items.OfType<string>()), levelNbt.Query<LibNbt.Tags.NbtList>("//FML/ItemData")))
				return;

			selectedModInfo.MargeNBTItemData(levelNbt.Query<LibNbt.Tags.NbtList>("//FML/ItemData"));

			UpdateModInfo(selectedModInfo.ModID);

			saveLevelDatButton.Enabled = true;
		}

		private void useIDText_TextChanged(object sender, EventArgs e)
		{
			TextBox senderTB = (TextBox)sender;

			if (selectedModInfo.BlockIDShiftNum != 0 || (useItemIDText == sender && selectedModInfo.ItemIDShiftNum != 0))
				senderTB.Font = new Font(senderTB.Font, FontStyle.Bold);
			else
				senderTB.Font = new Font(senderTB.Font, FontStyle.Regular);
		}

		private void saveLevelDatButton_Click(object sender, EventArgs e)
		{
			NbtSave();

			saveLevelDatButton.Enabled = false;
		}

		private void idMisalignmentFixButton_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show("この機能は1.6.x以前のワールドを引き継いだ場合にブロックIDとアイテムIDが不一致状態になった場合の不具合を簡易的に解消するものです。\r\n仕様上この修正を行うとインベントリ内の対象ブロックは消滅します。消えたアイテムは非公式フォーラムで公開されているBlockIDConverter等を利用しID移動を行ってください。\r\n\r\nlevel.datを自動でバックアップしますが念の為セーブデータを丸々バックアップしてから利用する事を薦めます。\r\n\r\n自動修正を実行して宜しいですか？\r\n(※保存ボタンを押すまで保存されません)") != System.Windows.Forms.DialogResult.OK)
			{
				MessageBox.Show("ID修正を中断しました。");
				return;
			}

			string fixLog = "";

			int backupNum = 0;
			while (true)
			{
				try
				{
					string backupFilePath = Path.GetDirectoryName(loadingNbtFilePath) + "\\" + Path.GetFileName(loadingNbtFilePath) + "_idfix" + ((backupNum > 0) ? "_" + (backupNum - 1) : "") + ".bak";
					File.Copy(loadingNbtFilePath, backupFilePath);

					fixLog += "[FileCopy] " + loadingNbtFilePath + " => " + backupFilePath + "\r\n";
					break;
				}
				catch (IOException) { backupNum++; }
			}

			idMisalignmentFixButton.Enabled = false;


			foreach (var block in selectedModInfo.BlockIDs)
			{
				string logmsg = "[IDMisalignmentFix] " + block.Key + " = " + selectedModInfo.ItemIDs[block.Key] + " -> " + block.Value;
				fixLog += logmsg + "\r\n";
				System.Diagnostics.Trace.WriteLine(logmsg);
				selectedModInfo.ItemIDs[block.Key] = block.Value;
			}

			selectedModInfo.MargeNBTItemData(levelNbt.Query<LibNbt.Tags.NbtList>("//FML/ItemData"));

			LogViewer lv = new LogViewer(fixLog);
			lv.ShowDialog(this);

			useItemIDText.Text = selectedModInfo.GetItemNumberIDListString();
			saveLevelDatButton.Enabled = true;

			UpdateModInfo(selectedModInfo.ModID);
		}
	}
}
