using System;
using System.IO;
using Terraria.ModLoader.IO;


namespace PiratesDemandYourBooty {
	partial class HaggleLogic {
		public static string GetHighestCoinTypeOfGivenDemand( long demand, out bool tensOf ) {
			int baseLog10 = (int)Math.Log10( demand );
			//int baseLog100 = (int)(Math.Log10( demand ) * 0.5d);
			//long logged = (long)Math.Pow( 100, baseLog100 );

			tensOf = ( baseLog10 % 2 ) != 0;

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

		public long PirateDemand { get; private set; } = 10 * 100 * 100;    // 10 gold, initially



		////////////////

		public void Load( TagCompound tag ) {
			if( !tag.ContainsKey( "PirateDemand" ) ) {
				return;
			}

			this.PirateDemand = tag.GetLong( "PirateDemand" );
		}

		public void Save( TagCompound tag ) {
			tag["PirateDemand"] = this.PirateDemand;
		}

		////

		public void NetSend( BinaryWriter writer ) {
			writer.Write( this.PirateDemand );
		}

		public void NetReceive( BinaryReader reader ) {
			this.PirateDemand = reader.ReadInt64();
		}


		////////////////

		public bool ReceiveReplyForPirate( long replyAmount ) {
			return true;
		}
	}
}
