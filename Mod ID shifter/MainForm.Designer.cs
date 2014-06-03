namespace Mod_ID_shifter
{
	partial class MainForm
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
			this.inputPath = new System.Windows.Forms.TextBox();
			this.inputRef = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.modInfoGroupBox = new System.Windows.Forms.GroupBox();
			this.idMisalignmentFixButton = new System.Windows.Forms.Button();
			this.useItemIDText = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.useBlockIDText = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.showIDListWindow = new System.Windows.Forms.Button();
			this.modList = new System.Windows.Forms.ComboBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.blockIDShiftNum = new System.Windows.Forms.NumericUpDown();
			this.itemIDShiftNum = new System.Windows.Forms.NumericUpDown();
			this.label6 = new System.Windows.Forms.Label();
			this.updateLevelDatButton = new System.Windows.Forms.Button();
			this.saveLevelDatButton = new System.Windows.Forms.Button();
			this.modInfoGroupBox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.blockIDShiftNum)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.itemIDShiftNum)).BeginInit();
			this.SuspendLayout();
			// 
			// inputPath
			// 
			this.inputPath.Location = new System.Drawing.Point(12, 24);
			this.inputPath.Name = "inputPath";
			this.inputPath.Size = new System.Drawing.Size(209, 19);
			this.inputPath.TabIndex = 3;
			this.inputPath.Leave += new System.EventHandler(this.inputPath_Leave);
			// 
			// inputRef
			// 
			this.inputRef.Location = new System.Drawing.Point(227, 22);
			this.inputRef.Name = "inputRef";
			this.inputRef.Size = new System.Drawing.Size(45, 23);
			this.inputRef.TabIndex = 2;
			this.inputRef.Text = "参照";
			this.inputRef.UseVisualStyleBackColor = true;
			this.inputRef.Click += new System.EventHandler(this.inputRef_Click);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 9);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(125, 12);
			this.label1.TabIndex = 4;
			this.label1.Text = "弄っちゃうlevel.datの指定";
			// 
			// modInfoGroupBox
			// 
			this.modInfoGroupBox.Controls.Add(this.idMisalignmentFixButton);
			this.modInfoGroupBox.Controls.Add(this.useItemIDText);
			this.modInfoGroupBox.Controls.Add(this.label4);
			this.modInfoGroupBox.Controls.Add(this.useBlockIDText);
			this.modInfoGroupBox.Controls.Add(this.label3);
			this.modInfoGroupBox.Controls.Add(this.showIDListWindow);
			this.modInfoGroupBox.Location = new System.Drawing.Point(12, 87);
			this.modInfoGroupBox.Name = "modInfoGroupBox";
			this.modInfoGroupBox.Size = new System.Drawing.Size(260, 119);
			this.modInfoGroupBox.TabIndex = 5;
			this.modInfoGroupBox.TabStop = false;
			this.modInfoGroupBox.Text = "MOD情報";
			// 
			// idMisalignmentFixButton
			// 
			this.idMisalignmentFixButton.Enabled = false;
			this.idMisalignmentFixButton.Location = new System.Drawing.Point(8, 90);
			this.idMisalignmentFixButton.Name = "idMisalignmentFixButton";
			this.idMisalignmentFixButton.Size = new System.Drawing.Size(75, 23);
			this.idMisalignmentFixButton.TabIndex = 14;
			this.idMisalignmentFixButton.Text = "IDズレ修正";
			this.idMisalignmentFixButton.UseVisualStyleBackColor = true;
			this.idMisalignmentFixButton.Visible = false;
			this.idMisalignmentFixButton.Click += new System.EventHandler(this.idMisalignmentFixButton_Click);
			// 
			// useItemIDText
			// 
			this.useItemIDText.Location = new System.Drawing.Point(8, 67);
			this.useItemIDText.Name = "useItemIDText";
			this.useItemIDText.ReadOnly = true;
			this.useItemIDText.Size = new System.Drawing.Size(246, 19);
			this.useItemIDText.TabIndex = 10;
			this.useItemIDText.TextChanged += new System.EventHandler(this.useIDText_TextChanged);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(6, 52);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(77, 12);
			this.label4.TabIndex = 9;
			this.label4.Text = "使用アイテムID";
			// 
			// useBlockIDText
			// 
			this.useBlockIDText.Location = new System.Drawing.Point(8, 30);
			this.useBlockIDText.Name = "useBlockIDText";
			this.useBlockIDText.ReadOnly = true;
			this.useBlockIDText.Size = new System.Drawing.Size(246, 19);
			this.useBlockIDText.TabIndex = 8;
			this.useBlockIDText.TextChanged += new System.EventHandler(this.useIDText_TextChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(6, 15);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(73, 12);
			this.label3.TabIndex = 8;
			this.label3.Text = "使用ブロックID";
			// 
			// showIDListWindow
			// 
			this.showIDListWindow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.showIDListWindow.Location = new System.Drawing.Point(179, 92);
			this.showIDListWindow.Name = "showIDListWindow";
			this.showIDListWindow.Size = new System.Drawing.Size(75, 21);
			this.showIDListWindow.TabIndex = 8;
			this.showIDListWindow.Text = "リスト表示";
			this.showIDListWindow.UseVisualStyleBackColor = true;
			this.showIDListWindow.Click += new System.EventHandler(this.showIDListWindow_Click);
			// 
			// modList
			// 
			this.modList.FormattingEnabled = true;
			this.modList.Location = new System.Drawing.Point(12, 61);
			this.modList.Name = "modList";
			this.modList.Size = new System.Drawing.Size(260, 20);
			this.modList.TabIndex = 6;
			this.modList.SelectedIndexChanged += new System.EventHandler(this.modList_SelectedIndexChanged);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(12, 46);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(135, 12);
			this.label2.TabIndex = 7;
			this.label2.Text = "ID移動させるMOD選んでね";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(12, 214);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(85, 12);
			this.label5.TabIndex = 9;
			this.label5.Text = "ブロックID移動量";
			// 
			// blockIDShiftNum
			// 
			this.blockIDShiftNum.Location = new System.Drawing.Point(107, 212);
			this.blockIDShiftNum.Maximum = new decimal(new int[] {
            4095,
            0,
            0,
            0});
			this.blockIDShiftNum.Name = "blockIDShiftNum";
			this.blockIDShiftNum.Size = new System.Drawing.Size(84, 19);
			this.blockIDShiftNum.TabIndex = 10;
			this.blockIDShiftNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.blockIDShiftNum.ValueChanged += new System.EventHandler(this.blockIDShiftNum_ValueChanged);
			// 
			// itemIDShiftNum
			// 
			this.itemIDShiftNum.Location = new System.Drawing.Point(107, 237);
			this.itemIDShiftNum.Maximum = new decimal(new int[] {
            31999,
            0,
            0,
            0});
			this.itemIDShiftNum.Name = "itemIDShiftNum";
			this.itemIDShiftNum.Size = new System.Drawing.Size(84, 19);
			this.itemIDShiftNum.TabIndex = 12;
			this.itemIDShiftNum.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
			this.itemIDShiftNum.ValueChanged += new System.EventHandler(this.itemIDShiftNum_ValueChanged);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(12, 239);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(89, 12);
			this.label6.TabIndex = 11;
			this.label6.Text = "アイテムID移動量";
			// 
			// updateLevelDatButton
			// 
			this.updateLevelDatButton.Enabled = false;
			this.updateLevelDatButton.Location = new System.Drawing.Point(197, 234);
			this.updateLevelDatButton.Name = "updateLevelDatButton";
			this.updateLevelDatButton.Size = new System.Drawing.Size(75, 23);
			this.updateLevelDatButton.TabIndex = 13;
			this.updateLevelDatButton.Text = "適用";
			this.updateLevelDatButton.UseVisualStyleBackColor = true;
			this.updateLevelDatButton.Click += new System.EventHandler(this.updateLevelDatButton_Click);
			// 
			// saveLevelDatButton
			// 
			this.saveLevelDatButton.Enabled = false;
			this.saveLevelDatButton.Location = new System.Drawing.Point(197, 209);
			this.saveLevelDatButton.Name = "saveLevelDatButton";
			this.saveLevelDatButton.Size = new System.Drawing.Size(75, 23);
			this.saveLevelDatButton.TabIndex = 14;
			this.saveLevelDatButton.Text = "上書き保存";
			this.saveLevelDatButton.UseVisualStyleBackColor = true;
			this.saveLevelDatButton.Click += new System.EventHandler(this.saveLevelDatButton_Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(284, 272);
			this.Controls.Add(this.saveLevelDatButton);
			this.Controls.Add(this.updateLevelDatButton);
			this.Controls.Add(this.itemIDShiftNum);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.blockIDShiftNum);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.modList);
			this.Controls.Add(this.modInfoGroupBox);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.inputPath);
			this.Controls.Add(this.inputRef);
			this.Name = "MainForm";
			this.Text = "ID一括で移動させちゃう？";
			this.modInfoGroupBox.ResumeLayout(false);
			this.modInfoGroupBox.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.blockIDShiftNum)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.itemIDShiftNum)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox inputPath;
		private System.Windows.Forms.Button inputRef;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.GroupBox modInfoGroupBox;
		private System.Windows.Forms.ComboBox modList;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button showIDListWindow;
		private System.Windows.Forms.TextBox useItemIDText;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TextBox useBlockIDText;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.NumericUpDown blockIDShiftNum;
		private System.Windows.Forms.NumericUpDown itemIDShiftNum;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Button updateLevelDatButton;
		private System.Windows.Forms.Button saveLevelDatButton;
		private System.Windows.Forms.Button idMisalignmentFixButton;
	}
}

