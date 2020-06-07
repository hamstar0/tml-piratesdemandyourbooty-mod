using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.Protocols.Packet.Interfaces;
using static Terraria.ModLoader.ModContent;


namespace PiratesDemandYourBooty.NetProtocols {
	class DemandReplyProtocol : PacketProtocolBroadcast {
		public static void BroadcastFromClient( long reply ) {
			if( Main.netMode != NetmodeID.MultiplayerClient ) {
				throw new ModHelpersException( "Not client" );
			}

			var protocol = new DemandReplyProtocol { Reply = reply };
			protocol.SendToServer( true );
		}



		////////////////

		public long Reply;



		////////////////

		private DemandReplyProtocol() { }


		////////////////

		protected override void ReceiveOnClient() {
			var myworld = GetInstance<PDYBWorld>();
			myworld.ReceiveReplyForPirate( this.Reply );
		}

		protected override void ReceiveOnServer( int fromWho ) {
			var myworld = GetInstance<PDYBWorld>();
			myworld.ReceiveReplyForPirate( this.Reply );
		}
	}
}
