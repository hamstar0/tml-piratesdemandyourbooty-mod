using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Fx;
using static Terraria.ModLoader.ModContent;


namespace PiratesDemandYourBooty.NPCs {
	public partial class PirateRuffianNPC : ModNPC {
		public override string Texture => "PiratesDemandYourBooty/NPCs/PirateRuffianNPC";

		////

		private float AmbushCooldownTimer {
			get => this.npc.localAI[0];
			set => this.npc.localAI[0] = value;
		}

		private float AmbushRunTimer {
			get => this.npc.localAI[1];
			set => this.npc.localAI[1] = value;
		}



		////////////////

		public override void SetStaticDefaults() {
			this.DisplayName.SetDefault( "Pirate Ruffian" );
			Main.npcFrameCount[npc.type] = Main.npcFrameCount[ NPCID.PirateDeckhand ];
		}

		public override void SetDefaults() {
			this.npc.width = 18;
			this.npc.height = 40;
			this.npc.damage = 35;
			this.npc.defense = 17;
			this.npc.lifeMax = 300;
			this.npc.HitSound = SoundID.NPCHit1;
			this.npc.DeathSound = SoundID.NPCDeath1;
			this.npc.value = 800f;
			this.npc.knockBackResist = 0.4f;
			this.npc.aiStyle = 3;
			this.npc.buffImmune[BuffID.Poisoned] = true;
			this.npc.buffImmune[BuffID.Confused] = false;

			this.aiType = NPCID.PirateDeckhand;
			this.animationType = NPCID.PirateDeckhand;
			this.banner = Item.NPCtoBanner( NPCID.PirateDeckhand );
			this.bannerItem = Item.BannerToItem( this.banner );

			var mynpc = this.npc.GetGlobalNPC<PDYBNPC>();
			mynpc.IsRaider = true;
		}


		////////////////

		public override float SpawnChance( NPCSpawnInfo spawnInfo ) {
			return SpawnCondition.Pirates.Chance;
		}

		public override void NPCLoot() {
			int itemWho = Item.NewItem( this.npc.getRect(), ItemID.PoisonedKnife, 99 );
			if( Main.netMode == NetmodeID.Server ) {
				NetMessage.SendData( MessageID.SyncItem, -1, -1, null, itemWho );
			}
		}


		////////////////
		
		public override bool PreAI() {
			if( this.npc.HasPlayerTarget ) {
				Player target = Main.player[this.npc.target];
				if( target?.active == true && !target.dead ) {
					this.RunAmbushAI();
				}
			}

//DebugHelpers.Print("ai", "ai: "+string.Join(", ",this.npc.ai)+", localAi: "+string.Join(", ",this.npc.localAI));
			return base.PreAI();
		}
	}
}
