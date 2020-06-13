using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Helpers.DotNET;
using PiratesDemandYourBooty.NetProtocols;


namespace PiratesDemandYourBooty {
	partial class PirateLogic {
		public void BeginRaid( bool syncFromServer ) {
			if( Main.netMode != NetmodeID.SinglePlayer && syncFromServer ) {
				if( Main.netMode == NetmodeID.Server ) {
					RaidStateProtocol.BroadcastFromServer( true );
				}
			} else {
				this.RaidElapsedTicks = PDYBConfig.Instance.RaidDurationTicks;

				Main.NewText( "Pirates are raiding your town!", new Color( 175, 75, 255 ) );
			}
		}

		public void EndRaid( bool syncFromServer ) {
			if( Main.netMode != NetmodeID.SinglePlayer && syncFromServer ) {
				if( Main.netMode == NetmodeID.Server ) {
					RaidStateProtocol.BroadcastFromServer( false );
				}
			} else {
				this.RaidElapsedTicks = 0;
				this.KillsNearTownNPC.Clear();

				Main.NewText( "Pirate raid has ended!", new Color( 175, 75, 255 ) );
			}
		}


		////////////////

		private IList<NPC> GetNearbyTownNPCs( Vector2 worldPosition ) {
			var nearbyTownNpcs = new List<NPC>();

			foreach( NPC townNpc in this.TownNPCs ) {
				float testDist = Vector2.DistanceSquared( worldPosition, townNpc.Center );

				if( testDist < 16384 ) {
					nearbyTownNpcs.Add( townNpc );
				}
			}

			return nearbyTownNpcs;
		}

		////

		public void AddDeathAtNearbyTownNPC( NPC npc ) {
			IList<NPC> nearbyTownNpcs = this.GetNearbyTownNPCs( npc.Center );

			foreach( NPC nearbyTownNpc in nearbyTownNpcs ) {
				if( this.KillsNearTownNPC.ContainsKey( nearbyTownNpc.type ) ) {
					this.KillsNearTownNPC[ nearbyTownNpc.type ] += 1;
				} else {
					this.KillsNearTownNPC[ nearbyTownNpc.type ] = 1;
				}
			}
		}


		////////////////
		
		public bool ValidateRaidForPlayer( Player player ) {
			IList<NPC> nearbyTownNpcs = this.GetNearbyTownNPCs( player.Center );

			foreach( NPC nearbyTownNpc in nearbyTownNpcs ) {
				int deaths;
				if( this.KillsNearTownNPC.TryGetValue( nearbyTownNpc.type, out deaths ) ) {
					if( deaths < PDYBConfig.Instance.PirateRaiderKillsNearTownNPCBeforeClear ) {
						return true;
					}
				}
			}

			return false;
		}


		////////////////

		private void UpdateForRaid() {
			this.RaidElapsedTicks++;

			// SP or server only
			if( Main.netMode != NetmodeID.MultiplayerClient ) {
				this.UpdateForRaid_Host();
			}
		}

		private void UpdateForRaid_Host() {
			this.TownNPCs = Main.npc.SafeWhere( n => n.active == true && n.townNPC ).ToList();

			if( this.TownNPCs.Count == 0 ) {
				this.EndRaid( true );
			}

			if( this.RaidElapsedTicks >= PDYBConfig.Instance.RaidDurationTicks ) {
				this.EndRaid( true );
			}
		}
	}
}
