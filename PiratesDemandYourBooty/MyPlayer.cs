using System;
using Terraria.ModLoader;


namespace PiratesDemandYourBooty {
	class PDYBPlayer : ModPlayer {
		public override void PreUpdate() {
			if( PDYBMod.Instance.UIContextComponents.HagglePanel.IsOpen ) {
				this.player.noItems = true;
			}
		}
	}
}
