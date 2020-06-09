using System;
using System.IO;
using Terraria;
using Terraria.ModLoader.IO;


namespace PiratesDemandYourBooty {
	public enum PirateMood {
		Normal,
		Impatient,
		Menacing
	}

	public enum HaggleAmount {
		VeryHigh,
		High,
		Good,
		Low,
		TooLow
	}




	partial class PirateLogic {
		public long PirateDemand { get; private set; } = 10 * 100 * 100;    // 10 gold, initially

		public PirateMood Patience { get; private set; } = PirateMood.Normal;

		public long InvasionDurationTicks { get; private set; } = 0;

		////

		public bool IsInvading => this.InvasionDurationTicks > 0;



		////////////////

		public void Load( TagCompound tag ) {
			if( tag.ContainsKey( "PirateDemand" ) ) {
				this.PirateDemand = tag.GetLong( "PirateDemand" );
			}
			if( tag.ContainsKey( "PirateInvasionTicks" ) ) {
				this.InvasionDurationTicks = tag.GetLong( "PirateInvasionTicks" );
			}
		}

		public void Save( TagCompound tag ) {
			tag["PirateDemand"] = (long)this.PirateDemand;
			tag["PirateInvasionTicks"] = (long)this.InvasionDurationTicks;
		}

		////

		public void NetSend( BinaryWriter writer ) {
			writer.Write( (long)this.PirateDemand );
			writer.Write( (long)this.InvasionDurationTicks );
		}

		public void NetReceive( BinaryReader reader ) {
			this.PirateDemand = reader.ReadInt64();
			this.InvasionDurationTicks = reader.ReadInt64();
		}
	}
}
