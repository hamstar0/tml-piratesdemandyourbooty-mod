using System;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Classes.Errors;
using HamstarHelpers.Classes.Protocols.Packet.Interfaces;
using static Terraria.ModLoader.ModContent;


namespace PiratesDemandYourBooty.NetProtocols {
	class RaidStateProtocol : PacketProtocolSendToClient {
		public static void BroadcastFromServer( bool raidOn ) {
			if( Main.netMode != NetmodeID.Server ) {
				throw new ModHelpersException( "Not server" );
			}

			var protocol = new RaidStateProtocol { BeginRaid = raidOn };
			protocol.SendToClient( -1, -1 );
		}



		////////////////

		public bool BeginRaid;



		////////////////

		private RaidStateProtocol() { }

		////

		protected override void InitializeServerSendData( int toWho ) {
		}


		////////////////

		protected override void Receive() {
			if( this.BeginRaid ) {
				PirateLogic.Instance.BeginRaid( false );
			} else {
				PirateLogic.Instance.EndRaid( false );
			}
		}
	}
}
