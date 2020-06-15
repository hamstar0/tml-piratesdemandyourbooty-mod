using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET;
using PiratesDemandYourBooty.NetProtocols;
using static Terraria.ModLoader.ModContent;


namespace PiratesDemandYourBooty.NPCs {
	public partial class PirateNegotiatorTownNPC : ModNPC {
		public static void AllDealingsFinished_FromLocal( long offerTested, long offerAmount ) {
			if( Main.netMode == NetmodeID.MultiplayerClient ) {
				DemandReplyProtocol.BroadcastFromClient( offerTested, offerAmount );
			} else if( Main.netMode == NetmodeID.SinglePlayer ) {
				PirateNegotiatorTownNPC.AllDealingsFinished_ToClient( Main.LocalPlayer, offerTested, offerAmount );
			}
		}

		
		public static void AllDealingsFinished_ToClient( Player player, long offerTested, long offerAmount ) {
			var logic = PirateLogic.Instance;

			if( player != null && offerAmount > 0 ) {
				logic.GiveFinalOffer( player, offerTested, offerAmount, false );
			} else {
				logic.GiveNoOffer( false );
			}

			logic.SetNextNegotiatorArrivalTime( logic.IsRaiding );

			int negotType = NPCType<PirateNegotiatorTownNPC>();
			IEnumerable<NPC> negotiator = Main.npc.SafeWhere( n => n?.active == true && n.type == negotType );

			// Remove negotiator(s)
			foreach( NPC npc in negotiator ) {
				PirateNegotiatorTownNPC.Exit( npc, false );
			}
		}


		public static void AllDealingsFinished_FromServer( Player player, long offerTested, long offerAmount ) {
			var logic = PirateLogic.Instance;

			if( player != null && offerAmount > 0 ) {
				logic.GiveFinalOffer( player, offerTested, offerAmount, true );
			} else {
				logic.GiveNoOffer( true );
			}

			logic.SetNextNegotiatorArrivalTime( logic.IsRaiding );

			int negotType = NPCType<PirateNegotiatorTownNPC>();
			IEnumerable<NPC> negotiator = Main.npc.SafeWhere( n => n?.active == true && n.type == negotType );

			// Remove negotiator(s)
			foreach( NPC npc in negotiator ) {
				PirateNegotiatorTownNPC.Exit( npc, false );
			}
		}



		////////////////

		private void UpdateHaggleState( NPC npc ) {
			if( Main.netMode != NetmodeID.Server ) {
				if( this.HagglingDone && Main.npcChatText == "" ) {
					PirateNegotiatorTownNPC.AllDealingsFinished_FromLocal( this.OfferTested, this.OfferAmount );
				}
			}
		}


		////////////////

		public bool GiveOffer( long offerAmount ) {
			var logic = PirateLogic.Instance;

			if( this.OfferTested == -1 ) {
				this.OfferTested = offerAmount;

				HaggleAmount replyType = PirateLogic.GaugeOffer( logic.ComputedDemand, offerAmount );
				Main.npcChatText = PirateNegotiatorTownNPC.HaggleReplies[replyType];
			} else {
				HaggleAmount replyType = PirateLogic.GaugeOffer( logic.ComputedDemand, offerAmount );
				Main.npcChatText = PirateNegotiatorTownNPC.OfferReplies[ replyType ];

				if( this.OfferTested > offerAmount ) {
					Main.npcChatText += "\n "+PirateNegotiatorTownNPC.OfferReduceReply;
				}

				this.OfferAmount = offerAmount;
			}

			return this.OfferAmount > 0;
		}
	}
}
