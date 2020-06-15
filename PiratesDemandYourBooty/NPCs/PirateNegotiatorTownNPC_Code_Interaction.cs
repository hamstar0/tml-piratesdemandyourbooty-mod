using System;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;


namespace PiratesDemandYourBooty.NPCs {
	public partial class PirateNegotiatorTownNPC : ModNPC {
		public override string GetChat() {
			if( !this.HasFirstChat ) {
				this.HasFirstChat = true;

				PirateMood patience = PirateLogic.Instance.Patience;
				return PirateNegotiatorTownNPC.Demands[ patience ];
			}

			int i = Main.rand.Next( PirateNegotiatorTownNPC.Chats.Count );
			return PirateNegotiatorTownNPC.Chats[i];
		}


		////////////////

		public override void SetChatButtons( ref string button1, ref string button2 ) {
			if( this.HagglingDone ) {
				return;
			}

			button1 = "Repeat \"plea\"";
			button2 = "Offer booty...";
		}

		public override void OnChatButtonClicked( bool firstButton, ref bool shop ) {
			if( this.HagglingDone ) {
				return;
			}

			if( firstButton ) {
				PirateMood patience = PirateLogic.Instance.Patience;
				Main.npcChatText = PirateNegotiatorTownNPC.Demands[ patience ];
			} else {
				PDYBMod.Instance.UIContextComponents.OpenHaggleUI( this.OfferTested >= 0 );
				Main.npcChatText = "";
				//Main.LocalPlayer.talkNPC = -1;
			}
		}
	}
}
