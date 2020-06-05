using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.UI;
using Terraria.ModLoader;
using PiratesDemandYourBooty.UI;


namespace PiratesDemandYourBooty {
	public partial class PDYBMod : Mod {
		internal UIHaggleContextComponents UIContextComponents { get; private set; }


		////////////////

		private UserInterface UIContext;
		private UIHagglePanel HagglePanelUI;



		////////////////

		private void LoadUI() {
			this.UIContextComponents = new UIHaggleContextComponents();
			this.UIContextComponents.Activate();

			this.HagglePanelUI = this.UIContextComponents.HagglePanel;

			this.UIContext = new UserInterface();
			this.UIContext.SetState( this.UIContextComponents );
		}


		////////////////

		public override void UpdateUI( GameTime gameTime ) {
			this.UIContext?.Update( Main._drawInterfaceGameTime );
		}


		public override void ModifyInterfaceLayers( List<GameInterfaceLayer> layers ) {
			int idx = layers.FindIndex( layer => layer.Name.Equals( "Vanilla: Mouse Text" ) );
			if( idx == -1 ) { return; }

			GameInterfaceDrawMethod haggleUI = () => {
				this.UIContext.Draw( Main.spriteBatch, Main._drawInterfaceGameTime );
				return true;
			};

			////

			var tradeLayer = new LegacyGameInterfaceLayer( "PDYB: Haggle UI", haggleUI, InterfaceScaleType.UI );
			layers.Insert( idx, tradeLayer );
		}
	}
}