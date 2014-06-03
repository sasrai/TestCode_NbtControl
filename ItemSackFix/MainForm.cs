using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace ItemSackFix
{
    public partial class MainForm : Form
    {
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
                // ファイルパスからフォルダを取得してinputPathへ登録
                inputPath.Text = Path.GetDirectoryName(ofd.FileName);
                inputPath_Leave(null, null);
            }
        }

        private void outputRef_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            fbd.ShowDialog();

            outputPath.Text = fbd.SelectedPath;
        }

        private void startFix_Click(object sender, EventArgs e)
        {
            chunkConvertWorker.RunWorkerAsync();
        }

        private void chunkConvertWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            chunkConvertProgressBar.Value = e.ProgressPercentage;
        }

        private void chunkConvertWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            chunkConvertProgressBar.Visible = false;
            startFix.Visible = true;

            inputPath.Enabled = true;
            inputRef.Enabled = true;

            outputPath.Enabled = true;
            outputRef.Enabled = true;
        }

        private void chunkConvertWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // 出力先ディレクトリチェック
            if (Directory.Exists(outputPath.Text))
            {
                DialogResult result = MessageBox.Show("出力先ディレクトリを上書きします。", "確認", MessageBoxButtons.OKCancel);

                if (result != System.Windows.Forms.DialogResult.OK)
                    return;
            }
            else
            {
                Directory.CreateDirectory(outputPath.Text);
            }

            // 総チャンク数をプログレスバーの最大値にセット
            if (chunkConvertProgressBar.InvokeRequired)
            {
                chunkConvertProgressBar.Invoke((Action)(() =>
                {
                    chunkConvertProgressBar.Maximum = int.Parse(chunkCount.Text);

                    inputPath.Enabled = false;
                    inputRef.Enabled = false;

                    outputPath.Enabled = false;
                    outputRef.Enabled = false;

                    startFix.Visible = false;
                    chunkConvertProgressBar.Visible = true;
                }));
            }

            // level.datのPlayerデータを書き換え
            FixPlayerSackData(inputPath.Text + "\\level.dat",
                InputToOutputPath(inputPath.Text + "\\level.dat"), 
                "//Data/Player/Inventory");


            // Playerフォルダの個別データを書き換え
            Directory.CreateDirectory(outputPath.Text + "\\players");
            foreach (string playerFile in Directory.GetFiles(inputPath.Text + "\\players", "*.dat"))
            {
                FixPlayerSackData(playerFile,
                    InputToOutputPath(playerFile), 
                    "//Inventory");
            }


            // 処理済みチャンク数
            int fixChunkNum = 0;

            // 袋のID
            int itemSackId = int.Parse(itemSackID.Text);

            foreach (string regionDir in Directory.GetDirectories(inputPath.Text, "region"))
            {
                // 出力先regionフォルダの作成
                Directory.CreateDirectory(InputToOutputPath(regionDir));

                foreach (string mcaFile in Directory.GetFiles(regionDir, "*.mca"))
                {
                    using (RegionFileAccess.RegionFile rf = new RegionFileAccess.RegionFile(mcaFile))
                    {
                        rf.LoadFile();

                        for (int i = 0; i < rf.ChunkData.Count; i++)
                        {
                            if (null != rf.ChunkData[i])
                            {
                                fNbt.NbtCompound nbtRoot = rf.ChunkData[i].GetRootNBT();

#if DEBUG
                                using (StreamWriter sw = new StreamWriter(InputToOutputPath(mcaFile) + "_nbt_" + i + ".txt", false, Encoding.UTF8)) {
                                    sw.Write(nbtRoot.ToString());
                                }
#endif

                                // TileEntities差し替え
                                fNbt.NbtList tileEntities = fNbt.NbtQuery.Get<fNbt.NbtList>(nbtRoot, "//Level/TileEntities");
                                foreach (fNbt.NbtCompound tileEntity in tileEntities)
                                {
                                    fNbt.NbtList items;
									if (tileEntity.TryGet<fNbt.NbtList>("Items", out items))
									{
                                        foreach (fNbt.NbtCompound item in items)
                                            FixItemSackData(item, itemSackId);
                                    }
                                }

                                // Entities差し替え
								fNbt.NbtList entities = fNbt.NbtQuery.Get<fNbt.NbtList>(nbtRoot, "//Level/Entities");
                                foreach (fNbt.NbtCompound entity in entities)
                                {
									fNbt.NbtList items;
                                    // チェストトロッコ等のインベントリ
									if (entity.TryGet<fNbt.NbtList>("Items", out items))
									{
										foreach (fNbt.NbtCompound item in items)
											FixItemSackData(item, itemSackId);
									}

									fNbt.NbtCompound tag;
                                    // ドロップアイテム
									if (entity.TryGet<fNbt.NbtCompound>("Item", out tag))
	                                    FixItemSackData(tag, itemSackId);
                                }

                                // チャンクデータ更新
                                rf.ChunkData[i].SetRootNbt(nbtRoot);

                                chunkConvertWorker.ReportProgress(++fixChunkNum);
                            }

                            if (i % 16 == 0)
                                System.Threading.Thread.Sleep(2);
                        }

                        rf.SaveFile(InputToOutputPath(mcaFile));
                    }
                }
            }
        }

        private string InputToOutputPath(string path) { return InputToOutputPath(path, inputPath.Text, outputPath.Text); }
        private string InputToOutputPath(string path, string inputDir, string outputDir)
        {
            if (Path.GetFullPath(path).IndexOf(Path.GetFullPath(inputDir)) != 0)
                return outputDir + "\\" + Path.GetRandomFileName() + "\\" + Path.GetFileName(path);

            return Path.GetFullPath(outputDir) + Path.GetFullPath(path).Substring(Path.GetFullPath(inputDir).Length);
        }

		private void FixPlayerSackData(string nbtFile, string nbtOutputFile, string nbtPath)
		{
			fNbt.NbtFile nbt = new fNbt.NbtFile(nbtFile);

			fNbt.NbtList playerInv = fNbt.NbtQuery.Get<fNbt.NbtList>(nbt.RootTag, nbtPath);

			foreach (fNbt.NbtCompound item in playerInv)
			{
				FixItemSackData(item, int.Parse(itemSackID.Text));
			}

			nbt.SaveToFile(nbtOutputFile, fNbt.NbtCompression.GZip);
		}
		private void FixItemSackData(fNbt.NbtCompound item, int itemSackId)
		{
			if (((fNbt.NbtShort)item["id"]).Value == (short)itemSackId)
			{
				fNbt.NbtCompound tag;

				if (!item.TryGet<fNbt.NbtCompound>("tag", out tag) || tag["type"].GetType() != typeof(fNbt.NbtShort))
					return;

				fNbt.NbtShort type = (fNbt.NbtShort)tag["type"];

				tag["type"] = new fNbt.NbtString("type", VanillaIDTable.BlockName[type.Value]);

				System.Diagnostics.Debug.WriteLine("Converted type: " + type.Value + "=>" + tag.Get<fNbt.NbtString>("type").Value);
			}
		}

        private void CheckFixPreparation()
        {
            if (!Directory.Exists(inputPath.Text))
                return;

            if (inputPath.Text == outputPath.Text)
                return;

            int sackId = 0;
            int chunkNum = 0;

            if (int.TryParse(itemSackID.Text, out sackId) && int.TryParse(chunkCount.Text, out chunkNum))
            {
                if (sackId > 0 && chunkNum >= 0)
                    startFix.Enabled = true;
                else
                    startFix.Enabled = false;
            }
        }

        private bool CheckBambooMod(string filename) { return CheckModID(filename, "BambooMod"); }
        private bool CheckModID(string filename, string modid)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException("level.datが見つかりません。");

            if (string.Empty == modid)
                return false;

            fNbt.NbtFile nbt = new fNbt.NbtFile(filename);

			fNbt.NbtList modList = fNbt.NbtQuery.Get<fNbt.NbtList>(nbt.RootTag, "//FML/ModList");

                foreach (fNbt.NbtCompound mod in modList)
                {
                    if (mod.Get<fNbt.NbtString>("ModId").Value == modid)
                        return true;
                }

                return false;
            
        }

        private enum FMLItemType
        {
            Block,
            Item,
        }
        private int GetItemSackID(string filename){return GetFMLItemID(filename, "BambooMod", "itemsack", FMLItemType.Item);}
        private int GetFMLItemID(string filename, string modid, string itemname, FMLItemType itemType)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException("level.datが見つかりません。");

            if (string.Empty == modid || string.Empty == itemname)
                return -1;

            string fmlKeyName = ((itemType == FMLItemType.Block) ? "\u0001" : "\u0002") + modid + ":" + itemname;

			fNbt.NbtFile nbt = new fNbt.NbtFile(filename);

			fNbt.NbtList idList = fNbt.NbtQuery.Get<fNbt.NbtList>(nbt.RootTag, "//FML/ItemData");

                foreach (fNbt.NbtCompound itemData in idList)
                {
                    if (itemData.Get<fNbt.NbtString>("K").Value == fmlKeyName)
                        return itemData.Get<fNbt.NbtInt>("V").Value;
                }

                return -1;
        }

        private int GetChunkCount(string filename)
        {
            if (!File.Exists(filename))
                throw new FileNotFoundException("指定されたmcaファイルが見つかりません。");

            using (RegionFileAccess.RegionFile rf = new RegionFileAccess.RegionFile(filename))
            {
                rf.LoadFile();

                int chunkNum = 0;
                for (int i = 0; i < rf.ChunkData.Count; i++)
                {
                    // 未使用チャンクはnullになってる
                    if (null != rf.ChunkData[i])
                        chunkNum++;

                    if (i % 32 == 0)
                        System.Threading.Thread.Sleep(1);
                }
                return chunkNum;
            }
        }

        private void checkSavedataWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            // 修復ボタンを無効化
            if (startFix.InvokeRequired)
            {
                startFix.BeginInvoke((Action)(() =>
                {
                    startFix.Enabled = false;
                }));
            }

            // level.datのファイルパス
            string leveldatPath = inputPath.Text + "\\level.dat";

            // 竹MODが導入されているか確認
            if (CheckBambooMod(leveldatPath))
            {
                if (isBambooMod.InvokeRequired)
                {
                    isBambooMod.BeginInvoke((Action)(() =>
                    {
                        isBambooMod.ForeColor = Color.Black;
                        isBambooMod.Text = "OK";
                    }));
                }
            }
            else
            {
                if (isBambooMod.InvokeRequired)
                {
                    isBambooMod.BeginInvoke((Action)(() =>
                    {
                        isBambooMod.ForeColor = Color.DarkRed;
                        isBambooMod.Text = "NG";
                    }));
                    return;
                }
            }

            // level.datから袋のアイテムIDを取得し表示
            int sackId = GetItemSackID(leveldatPath);
            if (sackId > 0)
            {
                if (itemSackID.InvokeRequired)
                {
                    itemSackID.BeginInvoke((Action)(() =>
                    {
                        itemSackID.Text = "" + sackId;
                    }));
                }
            }
            else
            {
                return;
            }

            // regionフォルダ一覧の取得
            string[] regionDirs = Directory.GetDirectories(inputPath.Text, "region", SearchOption.AllDirectories);
            if (showInfoProgressBar.InvokeRequired)
            {
                showInfoProgressBar.Invoke((Action)(() =>
                {
                    showInfoProgressBar.Maximum = 0;
                    foreach (string dir in regionDirs) {
                        showInfoProgressBar.Maximum += Directory.GetFiles(dir, "*.mca").Length;
                    }
                    showInfoProgressBar.Visible = true;
                }));
            }

            // チャンク数の確認
            int chunkNum = 0;
            int mcaFileNum = 0;
            foreach (string dir in regionDirs)
            {
                try
                {
                    foreach (string mcaFile in Directory.GetFiles(dir, "*.mca"))
                    {
                        if (checkSavedataWorker.CancellationPending)
                        {
                            if (chunkCount.InvokeRequired)
                            {
                                chunkCount.Invoke((Action)(()=>
                                {
                                    showInfoProgressBar.Visible = false;
                                    showInfoGroup.Enabled = false;
                                }));
                            }
                            return;
                        }
                        chunkNum += GetChunkCount(mcaFile);
                        checkSavedataWorker.ReportProgress(++mcaFileNum);
                        System.Diagnostics.Debug.WriteLine("Time [" + DateTime.Now.Ticks + "] fileNum=>" + mcaFileNum);
                    }
                }
                catch (FileNotFoundException ex)
                {
                    MessageBox.Show("内部でエラーが発生しました。\n\n" + dir + "\n" + ex);
                    break;
                }
            }
            if (chunkCount.InvokeRequired)
            {
                chunkCount.Invoke((Action)(() =>
                {
                    chunkCount.Text = chunkNum.ToString();
                    showInfoProgressBar.Visible = false;
                }));
            }
        }

        private void checkSavedataWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            CheckFixPreparation();
        }

        private void outputPath_TextChanged(object sender, EventArgs e)
        {
            CheckFixPreparation();
        }

        private void showInfoGroup_EnabledChanged(object sender, EventArgs e)
        {
            if (!showInfoGroup.Enabled)
            {
                showInfoProgressBar.Visible = false;
                showInfoProgressBar.Value = 0;
                showInfoProgressBar.Maximum = 0;
                isBambooMod.Text = "";
                isBambooMod.Refresh();
                itemSackID.Text = "";
                itemSackID.Refresh();
                chunkCount.Text = "";
                chunkCount.Refresh();
            }
        }

        private void checkSavedataWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            showInfoProgressBar.Value = e.ProgressPercentage;
            System.Diagnostics.Debug.WriteLine("Time [" + DateTime.Now.Ticks + "] ProgressCanged=>" + e.ProgressPercentage + "(" + Math.Round((double)e.ProgressPercentage / (double)showInfoProgressBar.Maximum * 100, 1) + ")");
        }

        private void inputPath_Leave(object sender, EventArgs e)
        {
            // フォルダが存在してるかチェック
            if (!Directory.Exists(inputPath.Text))
            {
                //MessageBox.Show("セーブデータフォルダが見つかりません。");
                return;
            }
            if (!File.Exists(inputPath.Text + "\\level.dat"))
            {
                //MessageBox.Show("フォルダ内にlevel.datが存在していません。\nMinecraftのセーブデータではないか違うフォルダが指定されています。");
                return;
            }

            // 出力先ディレクトリが指定されてなかった場合、自動で「_converted」フォルダを設定する
            if (string.Empty == outputPath.Text)
            {
                outputPath.Text = inputPath.Text + "_converted";
            }

            // 一度情報表示を無効化してリセット
            showInfoGroup.Enabled = false;

            // バックグラウンド処理が動いてたら止める
            if (checkSavedataWorker.IsBusy)
            {
                checkSavedataWorker.CancelAsync();

                while (true)
                {
                    Application.DoEvents();
                    System.Threading.Thread.Sleep(10);

                    if (!checkSavedataWorker.IsBusy)
                        break;

                    inputRef.Enabled = false;
                    inputPath.Enabled = false;
                }
                inputRef.Enabled = true;
                inputPath.Enabled = true;
            }

            // 情報表示を有効化
            showInfoGroup.Enabled = true;

            // バックグラウンドでセーブデータのチェック
            checkSavedataWorker.RunWorkerAsync();
        }
    }
}
