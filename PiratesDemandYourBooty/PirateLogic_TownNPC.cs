using System;
using static Terraria.ModLoader.ModContent;


namespace PiratesDemandYourBooty {
	partial class PirateLogic {
		public void SetPirateNegotiatorArrivalTime() {
			f
		}


		public bool CanPirateNegotiatorMoveIn() {
			if( this.TicksSinceLastArrival > this.TicksUntilNextArrival ) {
				return true;
			}

			return false;
		}


		////////////////

		private void UpdateTownNPCArrival() {
			this.TicksSinceLastArrival++;
		}
	}
}
