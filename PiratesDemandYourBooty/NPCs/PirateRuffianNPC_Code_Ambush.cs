using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Classes.Tiles.TilePattern;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Fx;
using HamstarHelpers.Helpers.Tiles;
using static Terraria.ModLoader.ModContent;


namespace PiratesDemandYourBooty.NPCs {
	public partial class PirateRuffianNPC : ModNPC {
		public static void EmitSmoke( Vector2 pos, bool fake ) {
			ParticleFxHelpers.MakeDustCloud(
				position: fake
					? pos
					: pos - new Vector2( 16f ),
				quantity: 1,
				sprayAmount: 0.3f,
				scale: fake
					? 0.5f
					: 2f
			);

			if( !fake ) {
				PirateRuffianNPC.EmitSmoke( pos, true );
			}
		}

		////////////////

		public static Vector2? FindAmbushDestination( NPC ambusher, Entity target ) {
			for( int i=0; i<64; i++ ) {
				Vector2? dest = PirateRuffianNPC.AttemptToFindAmbushDestination( ambusher, target );
				if( dest.HasValue ) {
					return dest;
				}
			}

			return null;
		}

		private static Vector2? AttemptToFindAmbushDestination( NPC ambusher, Entity target ) {
			Vector2 ambushPos = target.Center;
			ambushPos.Y -= 128f;
			ambushPos.X -= 12f * 16f;
			ambushPos.X += Main.rand.Next( 0, 24 * 16 );
			
			float dist = ambushPos.X - target.Center.X;
			if( Math.Abs(dist) <= (ambusher.width * 2) ) {
				return null;
			}

			int tileX = (int)MathHelper.Clamp( ambushPos.X / 16f, 1, Main.maxTilesX -1 );
			int tileY = (int)MathHelper.Clamp( ambushPos.Y / 16f, 1, Main.maxTilesY -1 );

			int width = TileFinderHelpers.GetFloorWidth( TilePattern.NonSolid, tileX, tileY, 16, out int _, out tileY );
			if( width == 0 ) {
				return null;
			}

			return new Vector2( tileX, tileY-1 ) * 16f;
		}



		////////////////

		private void RunAmbushAI( Entity target ) {
			if( target == null ) {
				this.AmbushRunTimer = 0f;
				return;
			}

			if( this.AmbushCooldownTimer == 0 ) {
				if( this.CanAmbush(target, false) ) {
					this.AmbushCooldownTimer = PDYBConfig.Instance.PirateRuffianAmbushCooldownDurationTicks; // 20 seconds
					this.BeginAmbushAction( target );
				}
			} else {
				this.AmbushCooldownTimer -= 1f;
			}

			if( this.AmbushRunTimer >= 1f ) {
				this.RunAmbushActionAI( target );
			}
		}

		////

		private void RunAmbushActionAI( Entity target ) {
			if( this.AmbushRunTimer >= 2f ) {
				if( this.CanAmbush(target, true) ) {
					this.AmbushRunTimer--;
					this.RunAmbushActionBuildup( target );
				} else {
					this.AmbushRunTimer = 0;
				}
			} else if( this.AmbushRunTimer == 1f ) {
				this.AmbushRunTimer = 0f;
				this.EndAmbushAction( target );
			}
		}


		////////////////
		
		public bool CanAmbush( Entity target, bool skipMinimum ) {
			Vector2 offset = target.Center - this.npc.Center;

			float minDistSqr = 24 * 16; // minimum 24 tiles away
			minDistSqr *= minDistSqr;
			float maxDistSqr = 128 * 16; // maximum 128 tiles away
			maxDistSqr *= maxDistSqr;

			float distSqr = offset.LengthSquared();

			return (distSqr > minDistSqr || skipMinimum)
				&& (distSqr < maxDistSqr);
		}


		////////////////
		
		private void BeginAmbushAction( Entity target ) {
			this.AmbushRunTimer = PDYBConfig.Instance.PirateRuffianAmbushBuildupDurationTicks;
			PirateRuffianNPC.EmitSmoke( this.npc.Center, false );
		}

		private bool EndAmbushAction( Entity target ) {
			Vector2? ambushPos = PirateRuffianNPC.FindAmbushDestination( this.npc, target );
			if( !ambushPos.HasValue ) {
				return false;
			}

			this.npc.Center = ambushPos.Value;
			if( Main.netMode == NetmodeID.Server ) {
				NetMessage.SendData( MessageID.SyncNPC, -1, -1, null, this.npc.whoAmI );
			}

			PirateRuffianNPC.EmitSmoke( ambushPos.Value, false );

			return true;
		}

		////

		private void RunAmbushActionBuildup( Entity target ) {
			this.npc.velocity.X *= 0.7f;
			this.npc.ai[0] = 0f;
			this.npc.ai[1] = 0f;
			this.npc.ai[2] = 0f;
			this.npc.ai[3] = 0f;
	
			if( (this.AmbushRunTimer % 5) == 0 ) {
				Vector2? smokePos = PirateRuffianNPC.FindAmbushDestination( this.npc, target );

				if( smokePos.HasValue ) {
					PirateRuffianNPC.EmitSmoke( smokePos.Value, true );
				}
			}
		}
	}
}
