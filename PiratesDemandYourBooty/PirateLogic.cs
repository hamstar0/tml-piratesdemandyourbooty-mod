using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Collections.Concurrent;
using Terraria;
using Terraria.ID;
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

		internal IDictionary<int, int> KillsNearTownNPC = new ConcurrentDictionary<int, int>();


		////////////////

		public long PirateDemand { get; private set; } = 10 * 100 * 100;    // 10 gold, initially

		public PirateMood Patience { get; private set; } = PirateMood.Normal;

		public long RaidElapsedTicks { get; private set; } = 0;

		public long TicksWhileNegotiatorAway { get; private set; } = 0;

		public long TicksUntilNextArrival { get; private set; } = 0;

		////

		public bool IsRaiding => this.RaidElapsedTicks > 0;



		////////////////

		public void Load( TagCompound tag ) {
			if( tag.ContainsKey("PirateDemand") ) {
				this.PirateDemand = tag.GetLong( "PirateDemand" );
			}
			if( tag.ContainsKey( "RaidElapsedTicks" ) ) {
				this.RaidElapsedTicks = tag.GetLong( "RaidElapsedTicks" );
			}
			if( tag.ContainsKey( "TicksWhileNegotiatorAway" ) ) {
				this.TicksWhileNegotiatorAway = tag.GetLong( "TicksWhileNegotiatorAway" );
			}
			if( tag.ContainsKey("TicksUntilNextArrival") ) {
				this.TicksUntilNextArrival = tag.GetLong( "TicksUntilNextArrival" );
			}
		}

		public void Save( TagCompound tag ) {
			tag["PirateDemand"] = (long)this.PirateDemand;
			tag["RaidElapsedTicks"] = (long)this.RaidElapsedTicks;
			tag["TicksWhileNegotiatorAway"] = (long)this.TicksWhileNegotiatorAway;
			tag["TicksUntilNextArrival"] = (long)this.TicksUntilNextArrival;
		}

		////
		
		public void NetSend( BinaryWriter writer ) {
			writer.Write( (long)this.PirateDemand );
			writer.Write( (long)this.RaidElapsedTicks );
			writer.Write( (long)this.TicksWhileNegotiatorAway );
			writer.Write( (long)this.TicksUntilNextArrival );
		}

		public void NetReceive( BinaryReader reader ) {
			this.PirateDemand = reader.ReadInt64();
			this.RaidElapsedTicks = reader.ReadInt64();
			this.TicksWhileNegotiatorAway = reader.ReadInt64();
			this.TicksUntilNextArrival = reader.ReadInt64();
		}


		////////////////

		internal void Update() {
			this.UpdateForNegotiator();

			if( this.IsRaiding ) {
				this.UpdateForRaid();
			}

			if( Main.netMode != NetmodeID.Server ) {
				this.UpdateDebugInfo();
			}
		}


		////

		private void UpdateDebugInfo() {
			var config = PDYBConfig.Instance;
			if( !config.DebugModeInfo ) {
				return;
			}

			var logic = PirateLogic.Instance;

			DebugHelpers.Print( "pirate_negotiator_info",
				"Patience: " + logic.Patience
				+ ", demand: " + logic.PirateDemand
				+ ", TicksWhileNegotiatorAway: " + logic.TicksWhileNegotiatorAway
				+ ", TicksUntilNextArrival: " + logic.TicksUntilNextArrival
			);
			if( logic.IsRaiding ) {
				DebugHelpers.Print( "pirate_raid_info",
					"Elapsed ticks: " + logic.RaidElapsedTicks
					+ ", percent done: " + ( (float)logic.RaidElapsedTicks / (float)config.RaidDurationTicks ).ToString()
				);
				DebugHelpers.Print( "pirate_raid_deaths",
					string.Join( ", ", logic.KillsNearTownNPC.Select( kv => kv.Key + ": " + kv.Value ) )
				);
			}
		}
	}
}
