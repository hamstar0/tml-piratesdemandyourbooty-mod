using System;
using System.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static Terraria.ModLoader.ModContent;


namespace PiratesDemandYourBooty {
	class PDYBWorld : ModWorld {
		public static PirateLogic PirateLogic => GetInstance<PDYBWorld>().PirateLogicInstance;



		////////////////

		private PirateLogic PirateLogicInstance = new PirateLogic();



		////////////////

		public override void Load( TagCompound tag ) {
			this.PirateLogicInstance.Load( tag );
		}

		public override TagCompound Save() {
			var tag = new TagCompound();
			this.PirateLogicInstance.Save( tag );
			return tag;
		}

		////

		public override void NetSend( BinaryWriter writer ) {
			try {
				this.PirateLogicInstance.NetSend( writer );
			} catch { }
		}

		public override void NetReceive( BinaryReader reader ) {
			try {
				this.PirateLogicInstance.NetReceive( reader );
			} catch { }
		}
	}
}
