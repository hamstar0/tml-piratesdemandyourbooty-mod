using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.UI;
using Terraria.ModLoader;
using HamstarHelpers.Helpers.Debug;
using PiratesDemandYourBooty.UI;


namespace PiratesDemandYourBooty {
	public partial class PDYBMod : Mod {
		private void LoadUI() {
			this.UIContextComponents = new UIHaggleContextComponents();
			this.UIContextComponents.Activate();

			this.UIContext = new UserInterface();
			this.UIContext.SetState( this.UIContextComponents );
		}


		////////////////

		public override void UpdateUI( GameTime gameTime ) {
			//this.UIContext?.Update( gameTime );
		}


		public override void ModifyInterfaceLayers( List<GameInterfaceLayer> layers ) {
			int idx = layers.FindIndex( layer => layer.Name.Equals( "Vanilla: Mouse Text" ) );
			if( idx == -1 ) { return; }

			//

			GameInterfaceDrawMethod haggleUI = () => {
				this.UIContext?.Update( Main._drawInterfaceGameTime );
				this.UIContext?.Draw( Main.spriteBatch, Main._drawInterfaceGameTime );
				return true;
			};

			GameInterfaceDrawMethod raidUI = () => {
				var logic = PirateLogic.Instance;
				if( Main.playerInventory || !logic.IsRaidingForMe(Main.LocalPlayer) ) {
					return true;
				}

				var config = PDYBConfig.Instance;
				long remainingSeconds = config.RaidDurationTicks - logic.RaidElapsedTicks;
				remainingSeconds /= 60;
				long remainingMinutes = remainingSeconds / 60;
				remainingSeconds %= 60;
				string msg = "Pirate raid time left: "+remainingMinutes.ToString("00")+":"+remainingSeconds.ToString("00");

				string msgAlt = "Pirate raid time left: 00:00";
				Vector2 strDim = Main.fontMouseText.MeasureString( msgAlt );
				float scale = 1f;
				
				Utils.DrawBorderStringFourWay(
					sb: Main.spriteBatch,
					font: Main.fontMouseText,
					text: msg,
					x: config.RaidTimerPositionX >= 0
						? config.RaidTimerPositionX
						: (Main.screenWidth - (int)(strDim.X * scale)) + config.RaidTimerPositionX,
					y: config.RaidTimerPositionY >= 0
						? config.RaidTimerPositionY
						: Main.screenHeight + config.RaidTimerPositionY,
					textColor: Color.Yellow,
					borderColor: Color.Black,
					origin: default(Vector2),
					scale: scale
				);

				return true;
			};

			//

			var tradeLayer = new LegacyGameInterfaceLayer( "PDYB: Haggle UI", haggleUI, InterfaceScaleType.UI );
			layers.Insert( idx, tradeLayer );
			var raidLayer = new LegacyGameInterfaceLayer( "PDYB: Raid UI", raidUI, InterfaceScaleType.UI );
			layers.Insert( idx, raidLayer );
		}
	}
}