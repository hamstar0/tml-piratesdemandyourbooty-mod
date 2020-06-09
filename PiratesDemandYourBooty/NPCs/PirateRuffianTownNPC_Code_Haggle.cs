using System;
using Terraria;
using Terraria.ModLoader;


namespace PiratesDemandYourBooty.NPCs {
	public partial class PirateRuffianTownNPC : ModNPC {
		public bool GiveOffer( long offerAmount ) {
			HaggleReplyType replyType = PDYBWorld.HaggleLogic.GaugeOffer( offerAmount );
			string[] replies = PirateRuffianTownNPC.HaggleReplies[ replyType ];

			if( this.HaggleAttempts < replies.Length ) {
				Main.npcChatText = replies[ this.HaggleAttempts++ ];

				if( replyType != HaggleReplyType.TooLow || this.HaggleAttempts < (replies.Length - 1) ) {
					return false;
				}
			}

			PDYBWorld.HaggleLogic.GiveFinalOffer( offerAmount );
			return true;
		}
	}
}
