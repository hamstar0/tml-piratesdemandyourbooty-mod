using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;
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
			this.npc.damage = 30;	//35
			this.npc.defense = 14;	//16
			this.npc.lifeMax = 250;	//300
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
			mynpc.SetPirateRaider( this.npc );
		}


		////////////////

		/*public override float SpawnChance( NPCSpawnInfo spawnInfo ) {
			if( PirateLogic.Instance.IsRaidingForMe(spawnInfo.player) ) {
				return SpawnCondition.TownCritter.Chance;
			}
			return SpawnCondition.Pirates.Chance;
		}*/


		////////////////

		public override void OnHitPlayer( Player target, int damage, bool crit ) {
			target.AddBuff( BuffID.Poisoned, 5 * 60 );
		}


		////////////////

		public override bool PreAI() {
			Player target = null;

			if( this.npc.HasPlayerTarget ) {
				target = Main.player[ this.npc.target ];
				if( target?.active != true || target.dead ) {
					target = null;
				}
			}

			this.RunAmbushAI( target );

//DebugHelpers.Print("ai", "ai: "+string.Join(", ",this.npc.ai)
//	+", localAi: "+string.Join(", ",this.npc.localAI));
			return base.PreAI();
		}
	}
}
