namespace ItemSackFix
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
            this.inputRef = new System.Windows.Forms.Button();
            this.inputPath = new System.Windows.Forms.TextBox();
            this.outputPath = new System.Windows.Forms.TextBox();
            this.outputRef = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.showInfoGroup = new System.Windows.Forms.GroupBox();
            this.showInfoProgressBar = new System.Windows.Forms.ProgressBar();
            this.chunkCount = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.itemSackID = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.isBambooMod = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.startFix = new System.Windows.Forms.Button();
            this.chunkConvertWorker = new System.ComponentModel.BackgroundWorker();
            this.chunkConvertProgressBar = new System.Windows.Forms.ProgressBar();
            this.checkSavedataWorker = new System.ComponentModel.BackgroundWorker();
            this.showInfoGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // inputRef
            // 
            this.inputRef.Location = new System.Drawing.Point(227, 22);
            this.inputRef.Name = "inputRef";
            this.inputRef.Size = new System.Drawing.Size(45, 23);
            this.inputRef.TabIndex = 0;
            this.inputRef.Text = "参照";
            this.inputRef.UseVisualStyleBackColor = true;
            this.inputRef.Click += new System.EventHandler(this.inputRef_Click);
            // 
            // inputPath
            // 
            this.inputPath.Location = new System.Drawing.Point(12, 24);
            this.inputPath.Name = "inputPath";
            this.inputPath.Size = new System.Drawing.Size(209, 19);
            this.inputPath.TabIndex = 1;
            this.inputPath.Leave += new System.EventHandler(this.inputPath_Leave);
            // 
            // outputPath
            // 
            this.outputPath.Location = new System.Drawing.Point(12, 61);
            this.outputPath.Name = "outputPath";
            this.outputPath.Size = new System.Drawing.Size(209, 19);
            this.outputPath.TabIndex = 3;
            this.outputPath.TextChanged += new System.EventHandler(this.outputPath_TextChanged);
            // 
            // outputRef
            // 
            this.outputRef.Location = new System.Drawing.Point(227, 59);
            this.outputRef.Name = "outputRef";
            this.outputRef.Size = new System.Drawing.Size(45, 23);
            this.outputRef.TabIndex = 2;
            this.outputRef.Text = "参照";
            this.outputRef.UseVisualStyleBackColor = true;
            this.outputRef.Click += new System.EventHandler(this.outputRef_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(127, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "読み込み元セーブフォルダ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 5;
            this.label2.Text = "出力先";
            // 
            // showInfoGroup
            // 
            this.showInfoGroup.Controls.Add(this.showInfoProgressBar);
            this.showInfoGroup.Controls.Add(this.chunkCount);
            this.showInfoGroup.Controls.Add(this.label6);
            this.showInfoGroup.Controls.Add(this.itemSackID);
            this.showInfoGroup.Controls.Add(this.label4);
            this.showInfoGroup.Controls.Add(this.isBambooMod);
            this.showInfoGroup.Controls.Add(this.label3);
            this.showInfoGroup.Enabled = false;
            this.showInfoGroup.Location = new System.Drawing.Point(12, 86);
            this.showInfoGroup.Name = "showInfoGroup";
            this.showInfoGroup.Size = new System.Drawing.Size(260, 68);
            this.showInfoGroup.TabIndex = 6;
            this.showInfoGroup.TabStop = false;
            this.showInfoGroup.Text = "情報表示";
            this.showInfoGroup.EnabledChanged += new System.EventHandler(this.showInfoGroup_EnabledChanged);
            // 
            // showInfoProgressBar
            // 
            this.showInfoProgressBar.Location = new System.Drawing.Point(133, 40);
            this.showInfoProgressBar.Name = "showInfoProgressBar";
            this.showInfoProgressBar.Size = new System.Drawing.Size(121, 19);
            this.showInfoProgressBar.Step = 1;
            this.showInfoProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.showInfoProgressBar.TabIndex = 9;
            this.showInfoProgressBar.Visible = false;
            // 
            // chunkCount
            // 
            this.chunkCount.Location = new System.Drawing.Point(216, 15);
            this.chunkCount.Name = "chunkCount";
            this.chunkCount.ReadOnly = true;
            this.chunkCount.Size = new System.Drawing.Size(38, 19);
            this.chunkCount.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(131, 18);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(79, 12);
            this.label6.TabIndex = 11;
            this.label6.Text = "総チャンク数";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // itemSackID
            // 
            this.itemSackID.Location = new System.Drawing.Point(87, 40);
            this.itemSackID.Name = "itemSackID";
            this.itemSackID.ReadOnly = true;
            this.itemSackID.Size = new System.Drawing.Size(38, 19);
            this.itemSackID.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(6, 43);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(75, 16);
            this.label4.TabIndex = 9;
            this.label4.Text = "袋のアイテムID";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // isBambooMod
            // 
            this.isBambooMod.Location = new System.Drawing.Point(87, 15);
            this.isBambooMod.Name = "isBambooMod";
            this.isBambooMod.ReadOnly = true;
            this.isBambooMod.Size = new System.Drawing.Size(38, 19);
            this.isBambooMod.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(6, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "竹MOD導入";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // startFix
            // 
            this.startFix.Enabled = false;
            this.startFix.Location = new System.Drawing.Point(12, 160);
            this.startFix.Name = "startFix";
            this.startFix.Size = new System.Drawing.Size(260, 23);
            this.startFix.TabIndex = 7;
            this.startFix.Text = "修復開始";
            this.startFix.UseVisualStyleBackColor = true;
            this.startFix.Click += new System.EventHandler(this.startFix_Click);
            // 
            // chunkConvertWorker
            // 
            this.chunkConvertWorker.WorkerReportsProgress = true;
            this.chunkConvertWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.chunkConvertWorker_DoWork);
            this.chunkConvertWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.chunkConvertWorker_ProgressChanged);
            this.chunkConvertWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.chunkConvertWorker_RunWorkerCompleted);
            // 
            // chunkConvertProgressBar
            // 
            this.chunkConvertProgressBar.Location = new System.Drawing.Point(12, 160);
            this.chunkConvertProgressBar.Maximum = 0;
            this.chunkConvertProgressBar.Name = "chunkConvertProgressBar";
            this.chunkConvertProgressBar.Size = new System.Drawing.Size(260, 23);
            this.chunkConvertProgressBar.Step = 1;
            this.chunkConvertProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.chunkConvertProgressBar.TabIndex = 8;
            this.chunkConvertProgressBar.Visible = false;
            // 
            // checkSavedataWorker
            // 
            this.checkSavedataWorker.WorkerReportsProgress = true;
            this.checkSavedataWorker.WorkerSupportsCancellation = true;
            this.checkSavedataWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.checkSavedataWorker_DoWork);
            this.checkSavedataWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.checkSavedataWorker_ProgressChanged);
            this.checkSavedataWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.checkSavedataWorker_RunWorkerCompleted);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 195);
            this.Controls.Add(this.showInfoGroup);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.outputPath);
            this.Controls.Add(this.outputRef);
            this.Controls.Add(this.inputPath);
            this.Controls.Add(this.inputRef);
            this.Controls.Add(this.startFix);
            this.Controls.Add(this.chunkConvertProgressBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(284, 195);
            this.Name = "MainForm";
            this.Text = "竹MODの袋を修復するよー！";
            this.showInfoGroup.ResumeLayout(false);
            this.showInfoGroup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button inputRef;
        private System.Windows.Forms.TextBox inputPath;
        private System.Windows.Forms.TextBox outputPath;
        private System.Windows.Forms.Button outputRef;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox showInfoGroup;
        private System.Windows.Forms.TextBox chunkCount;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox itemSackID;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox isBambooMod;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button startFix;
        private System.ComponentModel.BackgroundWorker chunkConvertWorker;
        private System.Windows.Forms.ProgressBar chunkConvertProgressBar;
        private System.ComponentModel.BackgroundWorker checkSavedataWorker;
        private System.Windows.Forms.ProgressBar showInfoProgressBar;
    }
}

