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
		public static Vector2 FindAmbushDestination( Entity target ) {
			Vector2? dest;
			for( int i=0; i<32; i++ ) {
				dest = PirateRuffianNPC.AttemptToFindAmbushDestination( target );
				if( dest.HasValue ) {
					return dest.Value;
				}
			}

			return target.Center;
		}

		private static Vector2? AttemptToFindAmbushDestination( Entity target ) {
			Vector2 origin = target.Center;
			origin.Y -= 48f;
			origin.X -= 12 * 16;
			origin.X += Main.rand.Next( 0, 24 * 16 );

			int tileX = (int)MathHelper.Clamp( origin.X / 16f, 1, Main.maxTilesX -1 );
			int tileY = (int)MathHelper.Clamp( origin.Y / 16f, 1, Main.maxTilesY -1 );

			int width = TileFinderHelpers.GetFloorWidth( TilePattern.CommonSolid, tileX, tileY, 12, out tileX, out tileY );
			if( width == 0 ) {
				return null;
			}
			return new Vector2( tileX * 16, tileY * 16 );
		}



		////////////////

		private void RunAmbushAI() {
			Player target = Main.player[ this.npc.target ];
			Vector2 offset = target.Center - this.npc.Center;
			float minDistSqr = 24 * 16;	// minimum 24 tiles away
			minDistSqr *= minDistSqr;

			if( offset.LengthSquared() > minDistSqr ) {
				if( this.AmbushCooldownTimer == 0 ) {
					this.AmbushCooldownTimer = 60 * 20; // 20 seconds
				}
			}
			
			if( this.AmbushCooldownTimer > 0f ) {
				this.AmbushCooldownTimer -= 1f;
			}

			if( this.AmbushRunTimer > 0f ) {
				this.RunAmbushAction();
			}
		}


		////

		private void RunAmbushAction() {
			Player target = Main.player[this.npc.target];
			Vector2 smokePos;
			bool fake = false;

			this.npc.velocity.X *= 0.95f;
			this.npc.ai[0] = 0f;
			this.npc.ai[1] = 0f;
			this.npc.ai[2] = 0f;
			this.npc.ai[3] = 0f;

			if( this.AmbushRunTimer == 0f ) {
				smokePos = npc.Center;
			} else {
				smokePos = PirateRuffianNPC.FindAmbushDestination( target );
				fake = true;
			}

			if( this.AmbushRunTimer++ >= 60 ) {
				this.AmbushRunTimer = 0f;
				fake = false;

				this.npc.position = smokePos;

				if( Main.netMode == NetmodeID.Server ) {
					NetMessage.SendData( MessageID.SyncNPC, -1, -1, null, this.npc.whoAmI );
				}
			}

			if( (this.AmbushRunTimer % 5) == 0 ) {
				ParticleFxHelpers.MakeDustCloud(
					position: fake
						? smokePos
						: smokePos - new Vector2(16),
					quantity: 1,
					sprayAmount: 0.3f,
					scale: fake
						? 1f
						: 2f
				);
			}
		}
	}
}
