using System;
using Terraria;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;


namespace PiratesDemandYourBooty {
	class PDYBPlayer : ModPlayer {
		public override bool PreItemCheck() {
			bool isLocked = PDYBMod.Instance.UIContextComponents?.HagglePanel?.IsOpen ?? false;

			this.player.noItems = isLocked || this.player.noItems;
			return !isLocked && base.PreItemCheck();
		}
	}
}
