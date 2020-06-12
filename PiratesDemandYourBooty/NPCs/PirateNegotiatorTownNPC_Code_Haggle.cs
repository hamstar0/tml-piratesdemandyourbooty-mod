using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.DotNET;
using PiratesDemandYourBooty.NetProtocols;
using static Terraria.ModLoader.ModContent;


namespace PiratesDemandYourBooty.NPCs {
	public partial class PirateNegotiatorTownNPC : ModNPC {
		public static void AllDealingsFinished( Player player, long offerAmount, bool sync ) {
			var logic = PirateLogic.Instance;

			if( player != null && offerAmount > 0 ) {
				logic.GiveFinalOffer( player, offerAmount, sync );
			} else {
				logic.GiveNoOffer( sync );
			}

			logic.SetNegotiatorArrivalTime( logic.IsRaiding );

			int negotType = NPCType<PirateNegotiatorTownNPC>();
			IEnumerable<NPC> negotiator = Main.npc.SafeWhere( n => n?.active == true && n.type == negotType );

			// Remove negotiator(s)
			foreach( NPC npc in negotiator ) {
				PirateNegotiatorTownNPC.Exit( npc, sync );
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
				PirateNegotiatorTownNPC.AllDealingsFinished( Main.LocalPlayer, this.OfferAmount, true );
			}
		}


		////////////////

		public bool GiveOffer( long offerAmount ) {
			var logic = PirateLogic.Instance;

			if( !this.OfferTested ) {
				this.OfferTested = true;

				HaggleAmount replyType = PirateLogic.GaugeOffer( logic.PirateDemand, offerAmount );
				Main.npcChatText = PirateNegotiatorTownNPC.HaggleReplies[replyType];
			} else {
				HaggleAmount replyType = PirateLogic.GaugeOffer( logic.PirateDemand, offerAmount );
				Main.npcChatText = PirateNegotiatorTownNPC.OfferReplies[ replyType ];

				this.OfferAmount = offerAmount;
			}

			return true;
		}
	}
}
