using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace NBT_View_test
{
    public partial class Form1 : Form
    {
        SortedDictionary<string, int> blockId = new SortedDictionary<string, int>();
        SortedDictionary<string, int> itemId = new SortedDictionary<string, int>();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            ofd.FileName = "level.dat";

            DialogResult result = ofd.ShowDialog();

            if (result == System.Windows.Forms.DialogResult.OK)
            {
                LibNbt.NbtFile nbt = new LibNbt.NbtFile(ofd.FileName);
                nbt.LoadFile();

                LibNbt.Tags.NbtList itemDatas = nbt.Query<LibNbt.Tags.NbtList>("//FML/ItemData");

                foreach (LibNbt.Tags.NbtTag tag in itemDatas.Tags)
                {
                    LibNbt.Tags.NbtCompound itemData = (LibNbt.Tags.NbtCompound)tag;

                    string key = itemData.Query<LibNbt.Tags.NbtString>("//K").Value;
                    int value = itemData.Query<LibNbt.Tags.NbtInt>("//V").Value;

                    if (key.StartsWith("\u0001"))
                    {
                        blockId.Add(key.Substring(1), value);
                    }
                    else if (key.StartsWith("\u0002"))
                    {
                        itemId.Add(key.Substring(1), value);
                    }
                }

                foreach (string k in blockId.Keys.Where(key_name => key_name.StartsWith("minecraft:")))
                {
                    System.Diagnostics.Debug.WriteLine(k + "=>" + blockId[k]);
                }

                System.Diagnostics.Debug.WriteLine("--------------------");

                foreach (string k in itemId.Keys.Where(key_name => !key_name.StartsWith("minecraft:")))
                {
                    System.Diagnostics.Debug.WriteLine(k + "=>" + itemId[k]);
                }

            }
        }
    }
}
