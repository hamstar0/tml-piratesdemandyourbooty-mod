using System;
using Terraria;
using HamstarHelpers.Classes.UI.Elements;
using PiratesDemandYourBooty.NPCs;
using static Terraria.ModLoader.ModContent;


namespace PiratesDemandYourBooty.UI {
	public partial class UIHagglePanel : UIThemedPanel {
		private void MakeOffer() {
			var npc = PirateRuffianTownNPC.GetNearbyPirateNPC( Main.LocalPlayer );
			var mynpc = npc?.modNPC as PirateRuffianTownNPC;
			if( mynpc == null ) {
				this.Close();
			}

			if( mynpc.GiveOffer(this.OfferTotal) ) {
				this.Close();
			} else {
				this.Reset();
			}
		}
	}
}
