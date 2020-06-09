using System;
using Terraria.ModLoader;


namespace PiratesDemandYourBooty {
	class PDYBPlayer : ModPlayer {
		public override bool PreItemCheck() {
			bool isLocked = PDYBMod.Instance.UIContextComponents.HagglePanel.IsOpen;

			this.player.noItems = isLocked || this.player.noItems;
			return !isLocked && base.PreItemCheck();
		}
	}
}
