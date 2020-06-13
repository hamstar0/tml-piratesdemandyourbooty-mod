using System;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;


namespace PiratesDemandYourBooty {
	class PDYBPlayer : ModPlayer {
		public override bool PreItemCheck() {
			bool isLocked = PDYBMod.Instance.UIContextComponents?.HagglePanel?.IsOpen ?? false;

			this.player.noItems = isLocked || this.player.noItems;
			return !isLocked && base.PreItemCheck();
		}


		////

		public override void PreUpdate() {
			var config = PDYBConfig.Instance;

			if( config.DebugModeInfo ) {
				var logic = PirateLogic.Instance;

				DebugHelpers.Print( "pirate_negotiator_info", "Patience: "+logic.Patience
					+", demand: "+logic.PirateDemand
					+", TicksSinceLastArrival: "+logic.TicksSinceLastArrival
					+", TicksUntilNextArrival: "+logic.TicksUntilNextArrival);
				DebugHelpers.Print( "pirate_raid_info", "Is raiding: "+logic.IsRaiding
					+", elapsed ticks: "+logic.RaidElapsedTicks
					+", percent: "+((float)logic.RaidElapsedTicks / (float)config.RaidDurationTicks).ToString() );
			}
		}
	}
}
