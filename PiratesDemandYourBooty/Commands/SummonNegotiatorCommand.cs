using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using PiratesDemandYourBooty.NPCs;
using static Terraria.ModLoader.ModContent;


namespace PiratesDemandYourBooty.Commands {
	public class DismissNegotiatorCommand : ModCommand {
		public override CommandType Type {
			get {
				if( Main.netMode == NetmodeID.SinglePlayer && !Main.dedServ ) {
					return CommandType.World;
				}
				return CommandType.Console | CommandType.World;
			}
		}
		public override string Command => "pdyb-dismiss-negotiator";
		public override string Usage => "/" + this.Command;
		public override string Description => "Forces negotiator to despawn. DebugModeCheats must be on.";



		////////////////

		/// @private
		public override void Action( CommandCaller caller, string input, string[] args ) {
			var config = PDYBConfig.Instance;
			if( !config.DebugModeCheats ) {
				caller.Reply( "Cheats disabled.", Color.Yellow );
			}

			int negotType = NPCType<PirateNegotiatorTownNPC>();
			IList<NPC> npcs = Main.npc.Where( n => n?.active == true && n.type == negotType ).ToList();
			if( npcs.Count == 0 ) {
				caller.Reply( "No negotiators spawned.", Color.Yellow );
			}

			foreach( NPC npc in npcs ) {
				PirateNegotiatorTownNPC.Exit( npc, Main.netMode == NetmodeID.Server );
			}

			caller.Reply( "Pirate negotiators despawned.", Color.Lime );
		}
	}
}
