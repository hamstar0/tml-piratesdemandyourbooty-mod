using System;
using Microsoft.Xna.Framework;
using Terraria;


namespace PiratesDemandYourBooty {
	partial class PirateLogic {
		public void BeginInvasion() {
			this.InvasionDurationTicks = PDYBConfig.Instance.InvasionDurationTicks;

			Main.NewText( "Pirates are invading your town!", new Color(175, 75, 255) );
		}
	}
}
