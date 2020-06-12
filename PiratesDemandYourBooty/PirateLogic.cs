using System;
using System.IO;
using System.Collections.Generic;
using System.Collections.Concurrent;
using Terraria;
using Terraria.ModLoader.IO;
using HamstarHelpers.Helpers.Debug;
using static Terraria.ModLoader.ModContent;


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
		public static PirateLogic Instance => PDYBWorld.PirateLogic;



		////////////////

		private bool WasDaySinceLastCheck = true;

		private IList<NPC> TownNPCs;

		private IDictionary<int, int> KillsNearTownNPC = new ConcurrentDictionary<int, int>();


		////////////////

		public long PirateDemand { get; private set; } = 10 * 100 * 100;    // 10 gold, initially

		public PirateMood Patience { get; private set; } = PirateMood.Normal;

		public long RaidDurationTicks { get; private set; } = 0;

		public long TicksSinceLastArrival { get; private set; } = 0;

		public long TicksUntilNextArrival { get; private set; } = 0;

		////

		public bool IsRaiding => this.RaidDurationTicks > 0;



		////////////////

		public void Load( TagCompound tag ) {
			if( tag.ContainsKey("PirateDemand") ) {
				this.PirateDemand = tag.GetLong( "PirateDemand" );
			}
			if( tag.ContainsKey("PirateRaidTicks") ) {
				this.RaidDurationTicks = tag.GetLong( "PirateRaidTicks" );
			}
			if( tag.ContainsKey("TicksSinceLastArrival") ) {
				this.TicksSinceLastArrival = tag.GetLong( "TicksSinceLastArrival" );
			}
			if( tag.ContainsKey("TicksUntilNextArrival") ) {
				this.TicksUntilNextArrival = tag.GetLong( "TicksUntilNextArrival" );
			}
		}

		public void Save( TagCompound tag ) {
			tag["PirateDemand"] = (long)this.PirateDemand;
			tag["PirateRaidTicks"] = (long)this.RaidDurationTicks;
			tag["TicksSinceLastArrival"] = (long)this.TicksSinceLastArrival;
			tag["TicksUntilNextArrival"] = (long)this.TicksUntilNextArrival;
		}

		////
		
		public void NetSend( BinaryWriter writer ) {
			writer.Write( (long)this.PirateDemand );
			writer.Write( (long)this.RaidDurationTicks );
			writer.Write( (long)this.TicksSinceLastArrival );
			writer.Write( (long)this.TicksUntilNextArrival );
		}

		public void NetReceive( BinaryReader reader ) {
			this.PirateDemand = reader.ReadInt64();
			this.RaidDurationTicks = reader.ReadInt64();
			this.TicksSinceLastArrival = reader.ReadInt64();
			this.TicksUntilNextArrival = reader.ReadInt64();
		}


		////////////////
		
		internal void Update() {
			this.UpdateForNegotiator();

			if( this.IsRaiding ) {
				this.UpdateForRaid();
			}
		}
	}
}
