using System;
using Terraria;
using HamstarHelpers.Classes.UI.Elements;
using PiratesDemandYourBooty.NPCs;
using static Terraria.ModLoader.ModContent;


namespace PiratesDemandYourBooty.UI {
	public partial class UIHagglePanel : UIThemedPanel {
		private void MakeOffer() {
			var npc = PirateNegotiatorTownNPC.GetNearbyNegotiator( Main.LocalPlayer );
			var mynpc = npc?.modNPC as PirateNegotiatorTownNPC;
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
