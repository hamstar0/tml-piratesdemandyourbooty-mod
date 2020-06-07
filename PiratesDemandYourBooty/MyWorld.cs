using System;
using System.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static Terraria.ModLoader.ModContent;


namespace PiratesDemandYourBooty {
	class PDYBWorld : ModWorld {
		public static HaggleLogic HaggleLogic => GetInstance<PDYBWorld>().HaggleLogicInstance;



		////////////////

		private HaggleLogic HaggleLogicInstance = new HaggleLogic();



		////////////////

		public override void Load( TagCompound tag ) {
			this.HaggleLogicInstance.Load( tag );
		}

		public override TagCompound Save() {
			var tag = new TagCompound();
			this.HaggleLogicInstance.Save( tag );
			return tag;
		}

		////

		public override void NetSend( BinaryWriter writer ) {
			try {
				this.HaggleLogicInstance.NetSend( writer );
			} catch { }
		}

		public override void NetReceive( BinaryReader reader ) {
			try {
				this.HaggleLogicInstance.NetReceive( reader );
			} catch { }
		}
	}
}
