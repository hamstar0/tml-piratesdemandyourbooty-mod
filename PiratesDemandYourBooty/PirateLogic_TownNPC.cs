using System;
using static Terraria.ModLoader.ModContent;


namespace PiratesDemandYourBooty {
	partial class PirateLogic {
		public void SetPirateNegotiatorArrivalTime( bool postInvasion ) {
			var config = PDYBConfig.Instance;

			this.TicksSinceLastArrival = 0;
			this.TicksUntilNextArrival = config.NegotiatorMinimumTicksUntilReturn;

			if( postInvasion ) {
				this.TicksUntilNextArrival += config.NegotiatorAddedTicksUntilReturnAfterRaid;
			}
		}


		public bool CanPirateNegotiatorMoveIn() {
			if( this.TicksSinceLastArrival > this.TicksUntilNextArrival ) {
				return true;
			}

			return false;
		}


		////////////////

		private void UpdateTownNPCArrival() {
			if( !this.IsInvading ) {
				this.TicksSinceLastArrival++;
			}
		}
	}
}
