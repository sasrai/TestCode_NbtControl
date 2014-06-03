using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace Mod_ID_shifter
{
	public partial class IDListWindow : Form
	{
		public ModInfo ModInfo { get; set; }

		public IDListWindow()
		{
			InitializeComponent();

			this.ModInfo = null;
		}
		public IDListWindow(ModInfo info) : this() { this.ModInfo = info; }

		public void UpdateWindowSetting()
		{
			// TabControlのクライアント領域周囲の余白を簡易計算
			Rectangle tabControlBorderSize = new Rectangle(
				IDListTabControl.SelectedTab.Bounds.Left - IDListTabControl.Location.X + 2,
				IDListTabControl.SelectedTab.Bounds.Top - IDListTabControl.Location.Y + 3,
				IDListTabControl.Width - IDListTabControl.SelectedTab.Bounds.Right,
				IDListTabControl.Height - IDListTabControl.SelectedTab.Bounds.Bottom);

			// SelectedTabのListViewからカラムサイズ取得しウィンドウサイズを計算する
			ListView selectedListView = (IDListTabControl.SelectedTab.Text == "BlockID") ? blockIDListView : itemIDListView;
			int lvColumnsSizeSum = 0;
			foreach (ColumnHeader ch in selectedListView.Columns)
			{
				lvColumnsSizeSum += ch.Width;
			}

			// ウィンドウの高さを計算
			int dialogHeight = tabControlBorderSize.Top + tabControlBorderSize.Height + SystemInformation.CaptionHeight + SystemInformation.FrameBorderSize.Height * 2 +
				((selectedListView.Items.Count > 0) ?
					selectedListView.Items.Count * selectedListView.Items[0].Bounds.Height : 128);

			// ウィンドウの高さが540px以上でデスクトップの作業領域の60%を超える場合縮小
			bool isScroll = false;
			if (dialogHeight > SystemInformation.WorkingArea.Height * 0.6)
			{
				if (SystemInformation.WorkingArea.Height * 0.6 > 540)
				{
					isScroll = true;
					dialogHeight = (int)(SystemInformation.WorkingArea.Height * 0.6);
				}
				else if (dialogHeight > 540)
				{
					isScroll = true;
					dialogHeight = 540;
				}
			}
			if (SystemInformation.WorkingArea.Height - this.Location.Y < dialogHeight)
			{
				this.Location = new Point(this.Location.X, SystemInformation.WorkingArea.Height - dialogHeight);
			}

			// ウィンドウを縮小できるよう一時的にMinimumSize解除
			this.MinimumSize = new Size(0, 0);

			// ウィンドウの大きさを変更
			this.Height = dialogHeight;
			this.Width = lvColumnsSizeSum + tabControlBorderSize.Left + tabControlBorderSize.Width + SystemInformation.FrameBorderSize.Width * 2 + ((isScroll)? SystemInformation.VerticalScrollBarWidth: 0);

			// ウィンドウの最小サイズを設定
			this.MinimumSize = new Size(this.Width, this.Height);
		}

		private void IDListView_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
		{
			if (e.ColumnIndex > 0)
			{
				System.Diagnostics.Debug.WriteLine("---ColumnWidthCanged---"); 
				if (sender == blockIDListView)
				{
					for (int i = 0; i < itemIDListView.Columns.Count; i++)
						if (itemIDListView.Columns[i].Width != blockIDListView.Columns[i].Width)
							itemIDListView.Columns[i].Width = blockIDListView.Columns[i].Width;
					System.Diagnostics.Debug.WriteLine("  => ColumnWidthCanged: from block");
				}
				if (sender == itemIDListView)
				{
					for (int i = 0; i < blockIDListView.Columns.Count; i++)
						if (blockIDListView.Columns[i].Width != itemIDListView.Columns[i].Width)
							blockIDListView.Columns[i].Width = itemIDListView.Columns[i].Width;
					System.Diagnostics.Debug.WriteLine("  => ColumnWidthCanged: from item");
				}
				// ウィンドウサイズの更新
				UpdateWindowSetting();
			}
		}

		private void IDListWindow_Load(object sender, EventArgs e)
		{
			// ModInfoからIDを読み込む
			if (null != this.ModInfo)
			{
				// ゴミが混じってる場合に備えて初期化
				blockIDListView.Items.Clear();
				itemIDListView.Items.Clear();

				// BlockID
				List<KeyValuePair<string, int>> valuesSortedBlockList = new List<KeyValuePair<string, int>>((this.ModInfo.BlockIDShiftNum != 0) ? this.ModInfo.GetShiftedBlockIDs() : this.ModInfo.BlockIDs);
				valuesSortedBlockList.Sort((a, b) => a.Value - b.Value);
				foreach (var id in valuesSortedBlockList)
				{
					blockIDListView.Items.Add(new ListViewItem(new string[2] { id.Value.ToString(), id.Key }));
				}

				// itemID
				List<KeyValuePair<string, int>> valuesSortedItemList = new List<KeyValuePair<string, int>>((this.ModInfo.BlockIDShiftNum != 0 || this.ModInfo.ItemIDShiftNum != 0) ? this.ModInfo.GetShiftedItemIDs() : this.ModInfo.ItemIDs);
				valuesSortedItemList.Sort((a, b) => a.Value - b.Value);
				foreach (var id in valuesSortedItemList)
				{
					itemIDListView.Items.Add(new ListViewItem(new string[2] { id.Value.ToString(), id.Key }));
				}
			}

			UpdateWindowSetting();
		}

		private void IDListTabControl_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateWindowSetting();
		}

		private void ListView_DrawItem(object sender, DrawListViewItemEventArgs e)
		{
			ListView senderLV = (ListView)sender;
/*
			if ((e.State & ListViewItemStates.Selected) != 0)
			{
				// Draw the background and focus rectangle for a selected item.
				e.Graphics.FillRectangle(Brushes.Maroon, e.Bounds);
				e.DrawFocusRectangle();
			}
			else
			{*/
				// Draw the background for an unselected item.
				e.DrawBackground();
			/*}
			*/
			// Draw the item text for views other than the Details view.
			if (senderLV.View != View.Details)
			{
				e.DrawText();
			}
		}

		private void blockIDListView_DrawSubItem(object sender, DrawListViewSubItemEventArgs e)
		{
			if ((e.ItemState & ListViewItemStates.Selected) != 0)
			{
				e.DrawFocusRectangle(e.Bounds);
			}
			//e.DrawBackground();
			e.DrawText();
		}

		private void toolStripIDCopy_Click(object sender, EventArgs e)
		{
			if (null == this.ModInfo)
				return;

			ListView selectLV = null;
			string copyStr = "";

			switch (IDListTabControl.SelectedIndex)
			{
				case 0:
					copyStr = this.ModInfo.ModID + " - BlockID\r\n";
					selectLV = blockIDListView;
					break;
				case 1:
					copyStr = this.ModInfo.ModID + " - ItemID\r\n";
					selectLV = itemIDListView;
					break;
				default:
					selectLV = null;
					break;
			}

			if (selectLV.Items.Count < 1)
			{
				MessageBox.Show("こぴれないよ！");
				return;
			}

			foreach (ListViewItem id in selectLV.Items)
			{
				copyStr += id.SubItems[0].Text + "\t#" + id.SubItems[1].Text + "\r\n";
			}

			Clipboard.SetText(copyStr, TextDataFormat.Text);

			MessageBox.Show("こぴるよ！");
		}

		private void contextMenu_Opening(object sender, CancelEventArgs e)
		{
			ContextMenuStrip senderMenu = (ContextMenuStrip)sender;

			if (!(senderMenu.SourceControl is ListView))
			{
				e.Cancel = true;
			}
		}
	}
}
