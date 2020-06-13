using System;
using System.Linq;
using Terraria;
using Terraria.ID;
using PiratesDemandYourBooty.NPCs;
using static Terraria.ModLoader.ModContent;


namespace PiratesDemandYourBooty {
	partial class PirateLogic {
		public void SetNextNegotiatorArrivalTime( bool postInvasion ) {
			var config = PDYBConfig.Instance;

			this.TicksSinceLastArrival = 0;
			this.TicksUntilNextArrival = config.NegotiatorMinimumTicksUntilReturn;

			if( postInvasion ) {
				this.TicksUntilNextArrival += config.NegotiatorAddedTicksUntilReturnAfterRaid;
			}
		}


		public bool CanNegotiatorMoveIn() {
			if( this.TicksSinceLastArrival > this.TicksUntilNextArrival ) {
				return true;
			}

			return false;
		}


		////////////////
		
		public void CheckAndApplyNegotiatorTimeUp_FromServer() {
			int negotType = NPCType<PirateNegotiatorTownNPC>();
			NPC negotiator = Main.npc.FirstOrDefault( n => n?.active == true && n.type == negotType );
			if( negotiator == null ) {
				return;
			}

			PirateNegotiatorTownNPC.AllDealingsFinished_FromServer( null, 0 );
		}


		////////////////

		private void UpdateForNegotiator() {
			if( Main.netMode != NetmodeID.MultiplayerClient ) {
				if( this.WasDaySinceLastCheck != Main.dayTime ) {
					this.WasDaySinceLastCheck = Main.dayTime;
					if( Main.dayTime ) {    // morning
						this.CheckAndApplyNegotiatorTimeUp_FromServer();
					}
				}
			}

			if( !this.IsRaiding ) {
				this.TicksSinceLastArrival++;
			}
		}
	}
}
