using System;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;


namespace PiratesDemandYourBooty.NPCs {
	public partial class PirateRuffianTownNPC : ModNPC {
		public override string GetChat() {
			if( !this.HasFirstChat ) {
				this.HasFirstChat = true;

				PirateMood patience = PirateLogic.Instance.Patience;
				return PirateRuffianTownNPC.Demands[ patience ];
			}

			int i = Main.rand.Next( PirateRuffianTownNPC.Chats.Count );
			return PirateRuffianTownNPC.Chats[i];
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
				Main.npcChatText = PirateRuffianTownNPC.Demands[ patience ];
			} else {
				PDYBMod.Instance.UIContextComponents.OpenHaggleUI();
				Main.npcChatText = "";
				//Main.LocalPlayer.talkNPC = -1;
			}
		}
	}
}
