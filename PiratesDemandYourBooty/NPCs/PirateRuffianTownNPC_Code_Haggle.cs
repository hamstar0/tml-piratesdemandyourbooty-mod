using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.NPCs;
using PiratesDemandYourBooty.NetProtocols;
using static Terraria.ModLoader.ModContent;


namespace PiratesDemandYourBooty.NPCs {
	public partial class PirateRuffianTownNPC : ModNPC {
		public static void AllDealingsFinished( Player player, long offerAmount, bool sync ) {
			PDYBWorld.HaggleLogic.GiveFinalOffer( offerAmount );
			player.BuyItem( (int)offerAmount );

			// Remove ALL pirate ruffians!
			int pirateType = NPCType<PirateRuffianTownNPC>();
			foreach( NPC npc in Main.npc ) {
				if( !npc.active && npc.type != pirateType ) { continue; }

				if( sync ) {
					NPCHelpers.Remove( npc );
				} else {
					npc.active = false;
				}

				// TODO: Poof!
			}

			if( sync && Main.netMode == NetmodeID.MultiplayerClient ) {
				DemandReplyProtocol.BroadcastFromClient( offerAmount );
			}
		}



		////////////////

		private void UpdateHaggleState() {
			if( this.HagglingDone && Main.npcChatText == "" ) {
				PirateRuffianTownNPC.AllDealingsFinished( Main.LocalPlayer, this.OfferAmount, true );
			}
		}


		////////////////

		public bool GiveOffer( long offerAmount ) {
			if( !this.OfferTested ) {
				this.OfferTested = true;

				HaggleReplyType replyType = PDYBWorld.HaggleLogic.GaugeOffer( offerAmount );
				Main.npcChatText = PirateRuffianTownNPC.HaggleReplies[replyType];
			} else {
				HaggleReplyType replyType = PDYBWorld.HaggleLogic.GaugeOffer( offerAmount );
				Main.npcChatText = PirateRuffianTownNPC.OfferReplies[ replyType ];

				this.OfferAmount = offerAmount;
			}

			return true;
		}
	}
}
