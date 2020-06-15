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
	partial class PDYBNPC : GlobalNPC {
		public static void DropCoinsForRaider( NPC npc, bool sync ) {
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

		private void InitializeForRaid( NPC npc ) {
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
				spawnRate = 300;
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
			pool[ NPCID.Parrot ] = average;
			pool[ NPCID.PirateCaptain ] = average / 9f;
			pool[ NPCType<PirateRuffianNPC>() ] = average * 3f; // handle in PirateRuffianNPC.SpawnChance?
		}


		////////////////

		private void DropCustomLootForRaider( NPC npc ) {
			var logic = PirateLogic.Instance;

			PDYBNPC.DropCoinsForRaider( npc, true );

			if( logic.IsRaiding ) {
				logic.AddDeathAtNearbyTownNPC( npc );
			}
		}
	}
}
