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




	partial class HaggleLogic {
		public long PirateDemand { get; private set; } = 10 * 100 * 100;    // 10 gold, initially

		public PirateMood Patience { get; private set; } = PirateMood.Normal;

		public long InvasionDurationTicks { get; private set; } = 0;

		////

		public bool IsInvading => this.InvasionDurationTicks > 0;



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

		public void BeginInvasion( Player player ) {
			this.InvasionDurationTicks = PDYBConfig.Instance.InvasionDurationTicks;
		}
	}
}
