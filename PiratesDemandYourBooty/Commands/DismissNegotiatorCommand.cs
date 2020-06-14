using System.Linq;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using PiratesDemandYourBooty.NPCs;
using static Terraria.ModLoader.ModContent;


namespace PiratesDemandYourBooty.Commands {
	public class SummonNegotiatorCommand : ModCommand {
		public override CommandType Type {
			get {
				if( Main.netMode == NetmodeID.SinglePlayer && !Main.dedServ ) {
					return CommandType.World;
				}
				return CommandType.Console | CommandType.World;
			}
		}
		public override string Command => "pdyb-summon-negotiator";
		public override string Usage => "/" + this.Command;
		public override string Description => "Forces negotiator to spawn. DebugModeCheats must be on.";



		////////////////

		/// @private
		public override void Action( CommandCaller caller, string input, string[] args ) {
			var config = PDYBConfig.Instance;
			if( !config.DebugModeCheats ) {
				caller.Reply( "Cheats disabled.", Color.Yellow );
			}

			NPC npc = PirateNegotiatorTownNPC.GetNearbyNegotiator( caller.Player );
			if( npc != null ) {
				caller.Reply( "Negotiator is spawned nearby.", Color.Yellow );
			}

			int negotType = NPCType<PirateNegotiatorTownNPC>();
			npc = Main.npc.FirstOrDefault( n => n?.active == true && n.type == negotType );
			if( npc != null ) {
				caller.Reply( "Negotiator is spawned.", Color.Yellow );
			}

			PirateNegotiatorTownNPC.Exit( npc, Main.netMode == NetmodeID.Server );

			int who, x, y;
			if( caller.Player?.active == true ) {
				x = (int)caller.Player.position.X;
				y = (int)caller.Player.position.Y;
			} else {
				x = Main.spawnTileX << 4;
				y = Main.spawnTileY << 4;
			}
			who = NPC.NewNPC( x, y, negotType );

			if( Main.netMode == NetmodeID.Server ) {
				NetMessage.SendData( MessageID.SyncNPC, -1, -1, null, who );
			}

			caller.Reply( "Pirate negotiator spawned at "+x+", "+y, Color.Lime );
		}
	}
}
