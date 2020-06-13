using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.UI;
using Terraria.ModLoader;
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
			this.UIContext?.Update( gameTime );
		}


		public override void ModifyInterfaceLayers( List<GameInterfaceLayer> layers ) {
			int idx = layers.FindIndex( layer => layer.Name.Equals( "Vanilla: Mouse Text" ) );
			if( idx == -1 ) { return; }

			//

			GameInterfaceDrawMethod haggleUI = () => {
				this.UIContext?.Draw( Main.spriteBatch, Main._drawInterfaceGameTime );
				return true;
			};
			

			GameInterfaceDrawMethod raidUI = () => {
				var logic = PirateLogic.Instance;
				if( logic.IsRaiding ) {
					long remainingSeconds = PDYBConfig.Instance.RaidDurationTicks - logic.RaidElapsedTicks;
					remainingSeconds /= 60;
					long remainingMinutes = remainingSeconds / 60;
					remainingSeconds %= 60;
					string msg = "Pirate raid time left: " + remainingMinutes + ":" + remainingSeconds;

					Utils.DrawBorderStringFourWay(
						sb: Main.spriteBatch,
						font: Main.fontMouseText,
						text: msg,
						x: (Main.screenWidth / 2),
						y: Main.screenHeight - 160,
						textColor: Color.Yellow,
						borderColor: Color.Black,
						origin: Main.fontMouseText.MeasureString( msg ) * 0.5f,
						scale: 2f
					);
				}
				return true;
			};

			//

			var tradeLayer = new LegacyGameInterfaceLayer( "PDYB: Haggle UI", haggleUI, InterfaceScaleType.UI );
			layers.Insert( idx + 1, tradeLayer );
			var raidLayer = new LegacyGameInterfaceLayer( "PDYB: Raid UI", raidUI, InterfaceScaleType.UI );
			layers.Insert( idx + 1, tradeLayer );
		}
	}
}