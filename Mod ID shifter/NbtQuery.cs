using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace fNbt
{
	class NbtQuery
	{
		public static T Get<T>(NbtCompound tag, string query) where T : NbtTag
		{
			if (!query.StartsWith("/"))
				throw new ArgumentException("Not query string. (Start with slash.)");
			List<string> names = query.Substring(1).Split("/".ToCharArray()).ToList();

			NbtTag result = tag;
			if (names[0] == result.Name)
				names.RemoveAt(0);
			else
				return null;

			foreach (string name in names)
				result = result[name];

			return (T)result;
		}
	}
}
