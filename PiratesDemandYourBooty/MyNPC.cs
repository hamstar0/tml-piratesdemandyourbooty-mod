using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Items;


namespace PiratesDemandYourBooty {
	class PDYBNPC : GlobalNPC {
		private bool IsRaider = false;


		////////////////

		public override bool InstancePerEntity => true;



		////////////////

		public override void SetDefaults( NPC npc ) {
			if( Main.hardMode ) {
				return;
			}
			if( !PirateLogic.Instance.IsRaiding ) {
				return;
			}

			switch( npc.type ) {
			case NPCID.PirateCorsair:
			case NPCID.PirateCrossbower:
			case NPCID.PirateDeadeye:
			case NPCID.PirateDeckhand:
			case NPCID.Parrot:
			case NPCID.PirateCaptain:
				this.IsRaider = true;
				npc.lifeMax /= 2;
				npc.life /= 2;
				npc.defense /= 2;
				npc.damage /= 2;
				break;
			}
		}


		////////////////

		public override void EditSpawnPool( IDictionary<int, float> pool, NPCSpawnInfo spawnInfo ) {
			var logic = PirateLogic.Instance;

			if( !PirateLogic.Instance.IsRaiding ) {
				return;
			}
			if( !spawnInfo.playerInTown ) {
				return;
			}
			if( !logic.ValidateRaidForPlayer(spawnInfo.player) ) {
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
		}


		////////////////
		
		public override bool PreNPCLoot( NPC npc ) {
			if( this.IsRaider ) {
				if( Main.netMode != NetmodeID.MultiplayerClient ) {
					this.DropCoins( npc, true );
					PirateLogic.Instance.AddDeathAtNearbyTownNPC( npc );
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
	}
}
