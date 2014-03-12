namespace Mod_ID_shifter
{
	partial class IDListWindow
	{
		/// <summary>
		/// 必要なデザイナー変数です。
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// 使用中のリソースをすべてクリーンアップします。
		/// </summary>
		/// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows フォーム デザイナーで生成されたコード

		/// <summary>
		/// デザイナー サポートに必要なメソッドです。このメソッドの内容を
		/// コード エディターで変更しないでください。
		/// </summary>
		private void InitializeComponent()
		{
			System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem("");
			System.Windows.Forms.ListViewItem listViewItem2 = new System.Windows.Forms.ListViewItem("");
			this.IDListTabControl = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.blockIDListView = new System.Windows.Forms.ListView();
			this.numIDColumnHeaderB = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.registNameColumnHeaderB = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.tabPage2 = new System.Windows.Forms.TabPage();
			this.itemIDListView = new System.Windows.Forms.ListView();
			this.numIDColumnHeaderI = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.registNameColumnHeaderI = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.IDListTabControl.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.tabPage2.SuspendLayout();
			this.SuspendLayout();
			// 
			// IDListTabControl
			// 
			this.IDListTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.IDListTabControl.Controls.Add(this.tabPage1);
			this.IDListTabControl.Controls.Add(this.tabPage2);
			this.IDListTabControl.Location = new System.Drawing.Point(0, 0);
			this.IDListTabControl.Margin = new System.Windows.Forms.Padding(0);
			this.IDListTabControl.Name = "IDListTabControl";
			this.IDListTabControl.SelectedIndex = 0;
			this.IDListTabControl.Size = new System.Drawing.Size(252, 269);
			this.IDListTabControl.TabIndex = 0;
			this.IDListTabControl.SelectedIndexChanged += new System.EventHandler(this.IDListTabControl_SelectedIndexChanged);
			// 
			// tabPage1
			// 
			this.tabPage1.Controls.Add(this.blockIDListView);
			this.tabPage1.Location = new System.Drawing.Point(4, 22);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(244, 243);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "BlockID";
			this.tabPage1.UseVisualStyleBackColor = true;
			// 
			// blockIDListView
			// 
			this.blockIDListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.blockIDListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.numIDColumnHeaderB,
            this.registNameColumnHeaderB});
			this.blockIDListView.GridLines = true;
			this.blockIDListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			listViewItem1.Checked = true;
			listViewItem1.StateImageIndex = 1;
			this.blockIDListView.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem1});
			this.blockIDListView.Location = new System.Drawing.Point(0, 0);
			this.blockIDListView.Name = "blockIDListView";
			this.blockIDListView.Size = new System.Drawing.Size(244, 243);
			this.blockIDListView.TabIndex = 1;
			this.blockIDListView.UseCompatibleStateImageBehavior = false;
			this.blockIDListView.View = System.Windows.Forms.View.Details;
			this.blockIDListView.ColumnWidthChanged += new System.Windows.Forms.ColumnWidthChangedEventHandler(this.IDListView_ColumnWidthChanged);
			this.blockIDListView.DrawItem += new System.Windows.Forms.DrawListViewItemEventHandler(this.ListView_DrawItem);
			this.blockIDListView.DrawSubItem += new System.Windows.Forms.DrawListViewSubItemEventHandler(this.blockIDListView_DrawSubItem);
			// 
			// numIDColumnHeaderB
			// 
			this.numIDColumnHeaderB.Width = 48;
			// 
			// registNameColumnHeaderB
			// 
			this.registNameColumnHeaderB.Width = 192;
			// 
			// tabPage2
			// 
			this.tabPage2.Controls.Add(this.itemIDListView);
			this.tabPage2.Location = new System.Drawing.Point(4, 22);
			this.tabPage2.Name = "tabPage2";
			this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage2.Size = new System.Drawing.Size(244, 243);
			this.tabPage2.TabIndex = 1;
			this.tabPage2.Text = "ItemID";
			this.tabPage2.UseVisualStyleBackColor = true;
			// 
			// itemIDListView
			// 
			this.itemIDListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.itemIDListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.numIDColumnHeaderI,
            this.registNameColumnHeaderI});
			this.itemIDListView.GridLines = true;
			this.itemIDListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
			listViewItem2.Checked = true;
			listViewItem2.StateImageIndex = 1;
			this.itemIDListView.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem2});
			this.itemIDListView.Location = new System.Drawing.Point(0, 0);
			this.itemIDListView.Name = "itemIDListView";
			this.itemIDListView.Size = new System.Drawing.Size(244, 243);
			this.itemIDListView.TabIndex = 2;
			this.itemIDListView.UseCompatibleStateImageBehavior = false;
			this.itemIDListView.View = System.Windows.Forms.View.Details;
			this.itemIDListView.ColumnWidthChanged += new System.Windows.Forms.ColumnWidthChangedEventHandler(this.IDListView_ColumnWidthChanged);
			// 
			// numIDColumnHeaderI
			// 
			this.numIDColumnHeaderI.Width = 48;
			// 
			// registNameColumnHeaderI
			// 
			this.registNameColumnHeaderI.Width = 192;
			// 
			// IDListWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(250, 268);
			this.Controls.Add(this.IDListTabControl);
			this.Name = "IDListWindow";
			this.Text = "割り当てID一覧";
			this.Load += new System.EventHandler(this.IDListWindow_Load);
			this.IDListTabControl.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage2.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl IDListTabControl;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage tabPage2;
		private System.Windows.Forms.ListView blockIDListView;
		private System.Windows.Forms.ColumnHeader numIDColumnHeaderB;
		private System.Windows.Forms.ColumnHeader registNameColumnHeaderB;
		private System.Windows.Forms.ListView itemIDListView;
		private System.Windows.Forms.ColumnHeader numIDColumnHeaderI;
		private System.Windows.Forms.ColumnHeader registNameColumnHeaderI;
	}
}