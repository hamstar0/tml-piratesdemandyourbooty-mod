using System;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.Protocols.Packet.Interfaces;
using PiratesDemandYourBooty.NPCs;
using static Terraria.ModLoader.ModContent;


namespace PiratesDemandYourBooty.NetProtocols {
	class DemandReplyProtocol : PacketProtocolBroadcast {
		public static void BroadcastFromClient( long offerAmount ) {
			if( Main.netMode != NetmodeID.MultiplayerClient ) {
				throw new ModHelpersException( "Not client" );
			}

			var protocol = new DemandReplyProtocol {
				WhoAmI = Main.myPlayer,
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
		public long OfferAmount;



		////////////////

		private DemandReplyProtocol() { }


		////////////////

		protected override void ReceiveOnClient() {
			PirateNegotiatorTownNPC.AllDealingsFinished( Main.player[this.WhoAmI], this.OfferAmount, false );
		}

		protected override void ReceiveOnServer( int fromWho ) {
			PirateNegotiatorTownNPC.AllDealingsFinished( Main.player[this.WhoAmI], this.OfferAmount, false );
		}
	}
}
