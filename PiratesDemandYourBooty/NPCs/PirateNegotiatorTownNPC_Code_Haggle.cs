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
		public static void AllDealingsFinished_FromCurrentClient( long offerAmount ) {
			if( Main.netMode == NetmodeID.MultiplayerClient ) {
				DemandReplyProtocol.BroadcastFromClient( offerAmount );
			}
		}

		
		public static void AllDealingsFinished_ToClient( Player player, long offerAmount ) {
			var logic = PirateLogic.Instance;

			if( player != null && offerAmount > 0 ) {
				logic.GiveFinalOffer( player, offerAmount, false );
			} else {
				logic.GiveNoOffer( false );
			}

			logic.SetNegotiatorArrivalTime( logic.IsRaiding );

			int negotType = NPCType<PirateNegotiatorTownNPC>();
			IEnumerable<NPC> negotiator = Main.npc.SafeWhere( n => n?.active == true && n.type == negotType );

			// Remove negotiator(s)
			foreach( NPC npc in negotiator ) {
				PirateNegotiatorTownNPC.Exit( npc );
			}
		}


		public static void AllDealingsFinished_FromServer( Player player, long offerAmount ) {
			var logic = PirateLogic.Instance;

			if( player != null && offerAmount > 0 ) {
				logic.GiveFinalOffer( player, offerAmount, true );
			} else {
				logic.GiveNoOffer( true );
			}

			logic.SetNegotiatorArrivalTime( logic.IsRaiding );

			int negotType = NPCType<PirateNegotiatorTownNPC>();
			IEnumerable<NPC> negotiator = Main.npc.SafeWhere( n => n?.active == true && n.type == negotType );

			// Remove negotiator(s)
			foreach( NPC npc in negotiator ) {
				PirateNegotiatorTownNPC.Exit( npc );
			}
		}



		////////////////

		private void UpdateHaggleState() {
			if( Main.netMode != NetmodeID.Server ) {
				if( this.HagglingDone && Main.npcChatText == "" ) {
					PirateNegotiatorTownNPC.AllDealingsFinished_FromCurrentClient( this.OfferAmount );
				}
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
