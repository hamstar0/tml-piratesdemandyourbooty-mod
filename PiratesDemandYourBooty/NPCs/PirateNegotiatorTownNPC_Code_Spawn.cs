using System;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;


namespace PiratesDemandYourBooty.NPCs {
	public partial class PirateNegotiatorTownNPC : ModNPC {
		public override bool CanTownNPCSpawn( int numTownNPCs, int money ) {
			var logic = PirateLogic.Instance;

			if( logic.IsRaiding ) {
				return false;
			}
			if( Main.hardMode ) {
				return false;
			}
			if( numTownNPCs < PDYBConfig.Instance.NegotiatorMinimumTownNPCsForArrival ) {
				return false;
			}
			if( money < PDYBConfig.Instance.NegotiatorMinimumMoneyForArrival ) {
				return false;
			}
			// First 5 "minutes" of day time
			if( !Main.dayTime || Main.time > (5 * 60 * 60) ) {
				return false;
			}

			return logic.CanNegotiatorMoveIn();
		}
	}
}
