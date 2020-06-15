using System;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.Protocols.Packet.Interfaces;
using PiratesDemandYourBooty.NPCs;
using static Terraria.ModLoader.ModContent;


namespace PiratesDemandYourBooty.NetProtocols {
	class DemandReplyProtocol : PacketProtocolBroadcast {
		public static void BroadcastFromClient( long offerTested, long offerAmount ) {
			if( Main.netMode != NetmodeID.MultiplayerClient ) {
				throw new ModHelpersException( "Not client" );
			}

			var protocol = new DemandReplyProtocol {
				WhoAmI = Main.myPlayer,
				OfferTested = offerTested,
				OfferAmount = offerAmount
			};
			protocol.SendToServer( true );
		}

		public static void BroadcastFromServer( long offerAmount ) {
			if( Main.netMode != NetmodeID.Server ) {
				throw new ModHelpersException( "Not server" );
			}

			var protocol = new DemandReplyProtocol {
				WhoAmI = -1,
				OfferAmount = offerAmount
			};
			protocol.SendToClient( -1, -1 );
		}



		////////////////

		public int WhoAmI;
		public long OfferTested;
		public long OfferAmount;



		////////////////

		private DemandReplyProtocol() { }


		////////////////

		protected override void ReceiveOnClient() {
			PirateNegotiatorTownNPC.AllDealingsFinished_ToClient( Main.player[this.WhoAmI], this.OfferTested, this.OfferAmount );
		}

		protected override void ReceiveOnServer( int fromWho ) {
			PirateNegotiatorTownNPC.AllDealingsFinished_FromServer( Main.player[this.WhoAmI], this.OfferTested, this.OfferAmount );
		}
	}
}
