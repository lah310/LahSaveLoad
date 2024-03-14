using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LahSaveLoad.Models
{
	public class Loadout
	{
		public string Id { get; set; }
		public List<ItemWrapper> Items { get; set; }


		public class ItemWrapper
		{
			public byte PosX { get; set; }

			public byte PosY { get; set; }

			public byte Page { get; set; }

			public byte Rotation { get; set; }

			public bool IsClothing { get; set; }

			public ItemWrapper2 ItemConfig { get; set; }
		}

		public class ItemWrapper2
		{
			public ushort Id { get; set; }
			public byte[] Metadata { get; set; }
			public byte Durability { get; set; }
			public byte Amount { get; set; }

		}
	}
}