using System;
using System.IO;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using static Terraria.ModLoader.ModContent;


namespace PiratesDemandYourBooty {
	class PDYBWorld : ModWorld {
		public static string GetHighestCoinTypeOfGivenDemand( long demand, out bool tensOf ) {
			int baseLog10= (int)Math.Log10( demand );
			//int baseLog100 = (int)(Math.Log10( demand ) * 0.5d);
			//long logged = (long)Math.Pow( 100, baseLog100 );

			tensOf = (baseLog10 % 2) == 0;

			switch( baseLog10 ) {
			case 0:
			case 1:
				return "copper coins";
			case 2:
			case 3:
				return "silver coins";
			case 4:
			case 5:
				return "gold coins";
			case 6:
			case 7:
				return "platinum coins";
			default:
				return "unknown currency";
			}
		}



		////////////////

		public long PirateDemand { get; private set; } = 10 * 100 * 100;	// 10 gold, initially



		////////////////
		
		public override void Load( TagCompound tag ) {
			if( !tag.ContainsKey("PirateDemand") ) {
				return;
			}

			this.PirateDemand = tag.GetLong( "PirateDemand" );
		}

		public override TagCompound Save() {
			return new TagCompound { { "PirateDemand", this.PirateDemand } };
		}

		////

		public override void NetSend( BinaryWriter writer ) {
			try {
				writer.Write( this.PirateDemand );
			} catch { }
		}

		public override void NetReceive( BinaryReader reader ) {
			try {
				this.PirateDemand = reader.ReadInt64();
			} catch { }
		}


		////////////////

		public void ReceiveReplyForPirate( long replyAmount ) {

		}
	}
}
