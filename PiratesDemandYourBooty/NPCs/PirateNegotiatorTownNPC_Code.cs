using System;
using HamstarHelpers.Helpers.Fx;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;


namespace PiratesDemandYourBooty.NPCs {
	[AutoloadHead]
	public partial class PirateNegotiatorTownNPC : ModNPC {
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
				PirateNegotiatorTownNPC.EmitSmoke( pos, true );
			}
		}

		////////////////

		public static NPC GetNearbyNegotiator( Player player ) {
			int negotType = NPCType<PirateNegotiatorTownNPC>();

			for( int i = 0; i < Main.npc.Length; i++ ) {
				NPC npc = Main.npc[i];
				if( npc == null || !npc.active || npc.type != negotType ) {
					continue;
				}

				if( Vector2.DistanceSquared( player.position, npc.position ) < 9216 ) { //96
					return npc;
				}
			}
			return null;
		}


		public static void Exit( NPC npc, bool syncFromServer ) {
			npc.active = false;

			if( syncFromServer ) {
				NetMessage.SendData( MessageID.SyncNPC, -1, -1, null, npc.whoAmI );
			}

			PirateNegotiatorTownNPC.EmitSmoke( npc.Center, false );
		}



		////////////////

		private bool HasFirstChat = false;

		private long OfferTested = -1;

		private long OfferAmount = -1;


		////////////////

		public bool HagglingDone => this.OfferAmount != -1;

		////

		public override string Texture => "PiratesDemandYourBooty/NPCs/PirateNegotiatorTownNPC";



		////////////////

		public override bool Autoload( ref string name ) {
			name = "Pirate Negotiator";
			return this.mod.Properties.Autoload;
		}

		public override void SetStaticDefaults() {
			Main.npcFrameCount[ this.npc.type ] = 25;
			NPCID.Sets.ExtraFramesCount[ this.npc.type ] = 9;
			NPCID.Sets.AttackFrameCount[ this.npc.type ] = 4;
			NPCID.Sets.DangerDetectRange[ this.npc.type ] = 700;
			NPCID.Sets.AttackType[ this.npc.type ] = 0;
			NPCID.Sets.AttackTime[ this.npc.type ] = 90;
			NPCID.Sets.AttackAverageChance[ this.npc.type ] = 30;
			NPCID.Sets.HatOffsetY[ this.npc.type ] = 4;
		}

		public override void SetDefaults() {
			this.npc.townNPC = true;
			this.npc.friendly = true;
			this.npc.width = 18;
			this.npc.height = 40;
			this.npc.aiStyle = 7;
			this.npc.damage = 15;
			this.npc.defense = 15;
			this.npc.lifeMax = 250;
			this.npc.HitSound = SoundID.NPCHit1;
			this.npc.DeathSound = SoundID.NPCDeath1;
			this.npc.knockBackResist = 0.5f;
			this.animationType = NPCID.Guide;
		}


		////////////////

		public override string TownNPCName() {
			int i = WorldGen.genRand.Next( PirateNegotiatorTownNPC.Names.Count );
			return PirateNegotiatorTownNPC.Names[i];
		}

		public override void NPCLoot() {
			Item.NewItem( this.npc.getRect(), ItemID.PoisonedKnife, 99 );
		}

		public override bool CanGoToStatue( bool toKingStatue ) {
			return false;
		}


		////////////////

		public override void TownNPCAttackStrength( ref int damage, ref float knockback ) {
			damage = 20;
			knockback = 4f;
		}

		public override void TownNPCAttackCooldown( ref int cooldown, ref int randExtraCooldown ) {
			cooldown = 30;
			randExtraCooldown = 30;
		}

		public override void TownNPCAttackProj( ref int projType, ref int attackDelay ) {
			projType = ProjectileID.PoisonedKnife;
			attackDelay = 1;
		}

		public override void TownNPCAttackProjSpeed( ref float multiplier, ref float gravityCorrection, ref float randomOffset ) {
			multiplier = 10f;
			randomOffset = 2f;
		}


		////////////////

		public override bool PreAI() {
			if( PirateLogic.Instance.CheckAndValidateNegotiatorPresence(npc) ) {
				this.UpdateHaggleState( this.npc );
			} else {
				PirateNegotiatorTownNPC.Exit( npc, false );
				return false;
			}
				
			return base.PreAI();
		}
	}
}
