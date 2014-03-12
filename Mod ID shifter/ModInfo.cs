using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mod_ID_shifter
{
	public class ModInfo
	{
		Dictionary<string, int> blockIdList = new Dictionary<string, int>();
		Dictionary<string, int> itemIdList = new Dictionary<string, int>();

		public Dictionary<string, int> BlockIDs
		{
			get
			{
				return blockIdList;
			}
		}
		public Dictionary<string, int> ItemIDs
		{
			get
			{
				return itemIdList;
			}
		}

		public string ModID { get; protected set; }

		public int BlockIDShiftNum { get; set; }
		public int ItemIDShiftNum { get; set; }

		public ModInfo(string modId, LibNbt.Tags.NbtList itemList)
		{
			if (itemList.Name != "ItemData")
				throw new ArgumentException("多分FMLのItemDataじゃ無いんですがコレ…");

			this.ModID = (string.IsNullOrWhiteSpace(modId))? "": modId;

			LoadBlockIDFromItemList(itemList);

			LoadItemIDFromItemList(itemList);
		}

		private void LoadBlockIDFromItemList(LibNbt.Tags.NbtList itemList) { LoadIDFromItemList(blockIdList, "\u0001", itemList); }
		private void LoadItemIDFromItemList(LibNbt.Tags.NbtList itemList) { LoadIDFromItemList(itemIdList, "\u0002", itemList); }
		private void LoadIDFromItemList(Dictionary<string, int> registList, string header, LibNbt.Tags.NbtList itemList)
		{
			foreach (LibNbt.Tags.NbtCompound itemData in itemList.Tags)
			{
				string key = itemData.Get<LibNbt.Tags.NbtString>("K").Value;

				if (key.IndexOf(header) == 0 && key.Substring(1).StartsWith(this.ModID))
				{
					registList.Add(key.Substring(1), itemData.Get<LibNbt.Tags.NbtInt>("V").Value);
				}
			}
		}

		/// <summary>
		/// BlockとItemの両方に登録されてるIDのリストを取得
		/// </summary>
		/// <returns>Key名、BlockIDとItemIDのリスト</returns>
		public Dictionary<string, List<int>> GetBlockAndItemIDList()
		{
			Dictionary<string, List<int>> resultList = new Dictionary<string, List<int>>();

			foreach (string key in blockIdList.Keys.Intersect<string>(itemIdList.Keys))
			{
				resultList.Add(key, new List<int>(new int[2] { blockIdList[key], itemIdList[key] }));
			}

			return resultList;
		}

		public List<int> GetBlockNumberIDList()
		{
			if (0 == BlockIDShiftNum)
				return new List<int>(blockIdList.Values);
			else
				return blockIdList.Values.Select(i => i + BlockIDShiftNum).ToList();
		}
		public List<int> GetItemNumberIDList()
		{
			if (0 == ItemIDShiftNum && 0 == BlockIDShiftNum)
				return new List<int>(itemIdList.Values);
			else
			{
				var resultList = new List<int>();
				var intersectList = GetBlockAndItemIDList().Keys;

				foreach (var item in itemIdList)
				{
					if (intersectList.Contains(item.Key))
						resultList.Add(item.Value + BlockIDShiftNum);
					else
						resultList.Add(item.Value + ItemIDShiftNum);
				}

				return resultList;
			}
		}

		public string GetBlockNumberIDListString(){return GetNumberIDListString(GetBlockNumberIDList());}
		public string GetItemNumberIDListString() { return GetNumberIDListString(GetItemNumberIDList()); }
		protected string GetNumberIDListString(List<int> idList)
		{
			idList.Sort();

			string result = "";

			int idx = 0;
			for (int j = 1; j < idList.Count; j++)
			{
				if (j == idList.Count - 1 || idList[j - 1] + 1 < idList[j])
				{
					if (idx + 1 == j)
					{
						result += ((idx != 0) ? "," : "") + idList[idx].ToString();
					}
					else
					{
						result += ((idx != 0) ? "," : "") + idList[idx].ToString() + "-" + idList[j - 1].ToString();
					}
					idx = j;
				}
			}

			return ("" == result) ? "Don't use id." : result;
		}

		public Dictionary<string, int> GetShiftedBlockIDs()
		{
			Dictionary<string, int> result = new Dictionary<string,int>();

			foreach (var block in BlockIDs)
				result.Add(block.Key, block.Value + BlockIDShiftNum);

			return result;
		}
		public Dictionary<string, int> GetShiftedItemIDs()
		{
			Dictionary<string, int> result = new Dictionary<string, int>();
			var intersectList = GetBlockAndItemIDList().Keys;

			foreach (var item in ItemIDs)
			{
				if (intersectList.Contains(item.Key))
				{
					result.Add(item.Key, item.Value + BlockIDShiftNum);
				}
				else
				{
					result.Add(item.Key, item.Value + ItemIDShiftNum);
				}
			}

			return result;
		}

		public void MargeNBTItemData(LibNbt.Tags.NbtList itemList)
		{
			var newBlockID = GetShiftedBlockIDs();
			var newItemID = GetShiftedItemIDs();

			foreach (var block in newBlockID)
			{
				foreach (LibNbt.Tags.NbtCompound itemData in itemList.Tags)
				{
					if (itemData.Get<LibNbt.Tags.NbtString>("K").Value == ("\u0001" + block.Key))
					{
						itemData.Get<LibNbt.Tags.NbtInt>("V").Value = block.Value;
						break;
					}
				}
			}

			foreach (var item in newItemID)
			{
				foreach (LibNbt.Tags.NbtCompound itemData in itemList.Tags)
				{
					if (itemData.Get<LibNbt.Tags.NbtString>("K").Value == ("\u0002" + item.Key))
					{
						itemData.Get<LibNbt.Tags.NbtInt>("V").Value = item.Value;
						break;
					}
				}
			}

		}
	}
}
