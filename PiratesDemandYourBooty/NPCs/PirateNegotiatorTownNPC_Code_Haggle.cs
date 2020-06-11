using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.NPCs;
using HamstarHelpers.Helpers.DotNET;
using PiratesDemandYourBooty.NetProtocols;
using static Terraria.ModLoader.ModContent;


namespace PiratesDemandYourBooty.NPCs {
	public partial class PirateRuffianTownNPC : ModNPC {
		public static void AllDealingsFinished( Player player, long offerAmount, bool sync ) {
			var logic = PirateLogic.Instance;

			if( player != null && offerAmount > 0 ) {
				logic.GiveFinalOffer( player, offerAmount );
			} else {
				logic.GiveNoOffer();
			}

			logic.SetNegotiatorArrivalTime( logic.IsInvading );

			int negotType = NPCType<PirateRuffianTownNPC>();
			IEnumerable<NPC> negotiator = Main.npc.SafeWhere( n => n?.active == true && n.type == negotType );

			// Remove negotiator(s)
			foreach( NPC npc in negotiator ) {
				PirateRuffianTownNPC.Exit( npc, sync );
			}

			if( sync ) {
				if( Main.netMode == NetmodeID.MultiplayerClient ) {
					DemandReplyProtocol.BroadcastFromClient( offerAmount );
				} else if( Main.netMode == NetmodeID.Server ) {
					DemandReplyProtocol.BroadcastFromServer( offerAmount );
				}
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

				HaggleAmount replyType = PirateLogic.Instance.GaugeOffer( offerAmount );
				Main.npcChatText = PirateRuffianTownNPC.HaggleReplies[replyType];
			} else {
				HaggleAmount replyType = PirateLogic.Instance.GaugeOffer( offerAmount );
				Main.npcChatText = PirateRuffianTownNPC.OfferReplies[ replyType ];

				this.OfferAmount = offerAmount;
			}

			return true;
		}
	}
}
