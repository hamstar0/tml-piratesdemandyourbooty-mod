using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Items;
using PiratesDemandYourBooty.NPCs;
using static Terraria.ModLoader.ModContent;


namespace PiratesDemandYourBooty {
	class PDYBNPC : GlobalNPC {
		internal bool IsRaider = false;


		////////////////

		public override bool InstancePerEntity => true;



		////////////////

		public override void SetDefaults( NPC npc ) {
			switch( npc.type ) {
			case NPCID.PirateCorsair:
			case NPCID.PirateCrossbower:
			case NPCID.PirateDeadeye:
			case NPCID.PirateDeckhand:
			case NPCID.Parrot:
			case NPCID.PirateCaptain:
				this.SetPirateRaider( npc );
				break;
			}
		}

		////////////////

		public void SetPirateRaider( NPC npc ) {
			if( Main.hardMode || !PirateLogic.Instance.IsRaiding ) {
				return;
			}

			this.IsRaider = true;

			npc.lifeMax /= 2;
			npc.life /= 2;
			npc.defense /= 2;
			npc.damage /= 2;
		}


		////////////////

		public override void EditSpawnRate( Player player, ref int spawnRate, ref int maxSpawns ) {
			var logic = PirateLogic.Instance;
//DebugHelpers.Print( "sr", "spawnRate: "+spawnRate+", maxSpawns: "+maxSpawns );
			if( logic.IsRaiding && player.townNPCs > 0f && logic.ValidateRaidForPlayer(player) ) {
				spawnRate = 600;
				maxSpawns = 5;
			}

			base.EditSpawnRate( player, ref spawnRate, ref maxSpawns );
		}

		public override void EditSpawnPool( IDictionary<int, float> pool, NPCSpawnInfo spawnInfo ) {
			if( !PirateLogic.Instance.IsRaidingForMe(spawnInfo.player) ) {
				return;
			}

			float average = pool.Sum( kv => kv.Value ) / (float)pool.Count;

			pool.Clear();

			pool[ NPCID.PirateCorsair ] = average * 3f;
			pool[ NPCID.PirateCrossbower ] = average * 3f;
			pool[ NPCID.PirateDeadeye ] = average * 3f;
			pool[ NPCID.PirateDeckhand ] = average * 3f;
			pool[ NPCID.Parrot ] = average * 3f;
			pool[ NPCID.PirateCaptain ] = average / 9f;
			pool[ NPCType<PirateRuffianNPC>() ] = average * 3f; // handle in PirateRuffianNPC.SpawnChance?
		}


		////////////////
		
		public override bool PreNPCLoot( NPC npc ) {
			if( !this.IsRaider ) {
				return base.PreNPCLoot( npc );
			}

			var logic = PirateLogic.Instance;

			if( logic.IsRaiding ) {
				if( Main.netMode != NetmodeID.MultiplayerClient ) {
					this.DropCoins( npc, true );
					logic.AddDeathAtNearbyTownNPC( npc );
				}

				return false;
			}

			return base.PreNPCLoot( npc );
		}


		////////////////

		private void DropCoins( NPC npc, bool sync ) {
			int coins = (int)( npc.value * 0.5f );
			coins = Main.rand.Next( (int)( coins * 0.75f ), (int)( coins * 1.25f ) );

			int[] coinWhos = ItemHelpers.CreateCoins( coins, npc.Center );

			if( sync && Main.netMode == NetmodeID.Server ) {
				foreach( int coinWho in coinWhos ) {
					NetMessage.SendData( MessageID.SyncItem, -1, -1, null, coinWho );
				}
			}
		}


		////////////////
		
		private int OldInvasion;

		public override bool PreAI( NPC npc ) {
			if( this.IsRaider ) {
				this.OldInvasion = Main.invasionType;
				Main.invasionType = 3;
			}
			return base.PreAI( npc );
		}

		public override void PostAI( NPC npc ) {
			if( this.IsRaider ) {
				Main.invasionType = this.OldInvasion;
			}
			base.PostAI( npc );
		}
	}
}
