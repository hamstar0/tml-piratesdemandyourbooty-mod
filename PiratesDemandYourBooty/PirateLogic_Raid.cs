using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Helpers.DotNET;
using HamstarHelpers.Helpers.DotNET.Extensions;
using PiratesDemandYourBooty.NetProtocols;


namespace PiratesDemandYourBooty {
	partial class PirateLogic {
		public void BeginRaid( bool syncFromServer ) {
			if( Main.netMode != NetmodeID.SinglePlayer && syncFromServer ) {
				if( Main.netMode == NetmodeID.Server ) {
					RaidStateProtocol.BroadcastFromServer( true );
				}
			} else {
				this.RaidElapsedTicks = 1;

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
			int distSqr = 96 * 16;
			distSqr *= distSqr;

			foreach( NPC townNpc in this.TownNPCs ) {
				float testDist = Vector2.DistanceSquared( worldPosition, townNpc.Center );

				if( testDist < distSqr ) {
					nearbyTownNpcs.Add( townNpc );
				}
			}

			return nearbyTownNpcs;
		}

		////

		public bool AddDeathAtNearbyTownNPC( NPC npc ) {
			var config = PDYBConfig.Instance;
			IList<NPC> nearbyTownNpcs = this.GetNearbyTownNPCs( npc.Center );

			foreach( NPC nearbyTownNpc in nearbyTownNpcs ) {
				if( this.KillsNearTownNPC.ContainsKey( nearbyTownNpc.type ) ) {
					if( this.KillsNearTownNPC[nearbyTownNpc.type] >= config.PirateRaiderKillsNearTownNPCBeforeClear ) {
						continue;
					}

					this.KillsNearTownNPC[nearbyTownNpc.type] += 1;
					return true;
				} else {
					this.KillsNearTownNPC[ nearbyTownNpc.type ] = 1;
					return true;
				}
			}
			return false;
		}


		////////////////

		public bool ValidateRaidForPlayer( Player player ) {
			IList<NPC> nearbyTownNpcs = this.GetNearbyTownNPCs( player.Center );

			foreach( NPC nearbyTownNpc in nearbyTownNpcs ) {
				if( this.KillsNearTownNPC.TryGetValue( nearbyTownNpc.type, out int deaths ) ) {
					if( deaths < PDYBConfig.Instance.PirateRaiderKillsNearTownNPCBeforeClear ) {
						return true;
					}
				} else {
					return true;
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
			this.TownNPCs = Main.npc.SafeWhere( n => n?.active == true && n.townNPC ).ToList();

			if( this.RaidElapsedTicks >= PDYBConfig.Instance.RaidDurationTicks ) {
				this.EndRaid( Main.netMode == NetmodeID.Server );
			}

			if( !this.IsTownRaidable() ) {
				this.EndRaid( Main.netMode == NetmodeID.Server );
			}
		}


		////

		private bool IsTownRaidable() {
			if( this.KillsNearTownNPC.Count < this.TownNPCs.Count ) {
				return true;
			}

			foreach( (int townNpcType, int pirateDeaths) in this.KillsNearTownNPC ) {
				if( pirateDeaths < PDYBConfig.Instance.PirateRaiderKillsNearTownNPCBeforeClear ) {
					return true;
				}
			}

			return false;
		}
	}
}
