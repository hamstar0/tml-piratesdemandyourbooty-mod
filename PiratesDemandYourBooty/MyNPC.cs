using System;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Items;
using static Terraria.ModLoader.ModContent;


namespace PiratesDemandYourBooty {
	partial class PDYBNPC : GlobalNPC {
		internal bool IsRaider = false;

		private int OldInvasion;


		////////////////

		public override bool InstancePerEntity => true;



		////////////////

		public override void SetDefaults( NPC npc ) {
			this.InitializeForRaid( npc );
		}


		////////////////
		
		public override bool PreNPCLoot( NPC npc ) {
			if( !this.IsRaider ) {
				return base.PreNPCLoot( npc );
			}

			if( Main.netMode != NetmodeID.MultiplayerClient ) {
				this.DropCustomLootForRaider( npc );
			}

			return false;
		}


		////////////////
		
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
