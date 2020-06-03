using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;


namespace PiratesDemandYourBooty.NPCs {
	[AutoloadHead]
	public partial class PirateRuffianTownNPC : ModNPC {
		public static NPC GetNearbyPirate( Player player ) {
			for( int i = 0; i < Main.npc.Length; i++ ) {
				NPC npc = Main.npc[i];
				if( npc == null || !npc.active || npc.type != NPCType<PirateRuffianTownNPC>() ) {
					continue;
				}

				if( Vector2.DistanceSquared( player.position, npc.position ) < 9216 ) { //96
					return npc;
				}
			}
			return null;
		}


		////////////////

		private bool HasFirstChat = false;


		////////////////

		public DemandType CurrentDemand { get; private set; } = DemandType.Normal;

		////

		public override string Texture => "PiratesDemandYourBooty/NPCs/PirateRuffianTownNPC";



		////////////////

		public override bool Autoload( ref string name ) {
			name = "Pirate Ruffian";
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
			this.npc.damage = 10;
			this.npc.defense = 15;
			this.npc.lifeMax = 250;
			this.npc.HitSound = SoundID.NPCHit1;
			this.npc.DeathSound = SoundID.NPCDeath1;
			this.npc.knockBackResist = 0.5f;
			this.animationType = NPCID.Guide;
		}


		////

		public override bool CanTownNPCSpawn( int numTownNPCs, int money ) {
			return true;
		}

		public override string TownNPCName() {
			int i = WorldGen.genRand.Next( PirateRuffianTownNPC.Names.Count );
			return PirateRuffianTownNPC.Names[ i ];
		}

		public override string GetChat() {
			if( !this.HasFirstChat ) {
				this.HasFirstChat = true;
				return PirateRuffianTownNPC.Demands[ this.CurrentDemand ];
			}

			int i = Main.rand.Next( PirateRuffianTownNPC.Chats.Count );
			return PirateRuffianTownNPC.Chats[i];
		}


		////////////////

		public override void SetChatButtons( ref string button1, ref string button2 ) {
			button1 = "Repeat \"plea\"";
			button2 = "Offer booty...";
		}

		public override void OnChatButtonClicked( bool firstButton, ref bool shop ) {
			if( firstButton ) {
				Main.npcChatText = PirateRuffianTownNPC.Demands[ this.CurrentDemand ];
			} else {
				PDYBMod.Instance.HagglePanelUI.Open();
			}
		}


		////////////////

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
			multiplier = 8f;
			randomOffset = 2f;
		}
	}
}
