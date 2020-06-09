using System;
using Terraria;


namespace PiratesDemandYourBooty {
	partial class PirateLogic {
		public void BeginInvasion( Player player ) {
			this.InvasionDurationTicks = PDYBConfig.Instance.InvasionDurationTicks;
		}
	}
}
