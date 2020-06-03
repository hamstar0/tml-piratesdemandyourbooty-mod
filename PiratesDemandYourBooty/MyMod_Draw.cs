using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.UI;
using Terraria.ModLoader;
using PiratesDemandYourBooty.UI;


namespace PiratesDemandYourBooty {
	public partial class PDYBMod : Mod {
		public UIHagglePanel HagglePanelUI { get; private set; }


		////////////////

		private UserInterface UIContext;



		////////////////

		private void LoadUI() {
			var uiContextComponents = new UIHaggleContextComponents();
			uiContextComponents.Width.Set( 0f, 1f );
			uiContextComponents.Height.Set( 0f, 1f );
			uiContextComponents.Activate();

			this.HagglePanelUI = uiContextComponents.Panel;

			this.UIContext = new UserInterface();
			this.UIContext.IsVisible = true;
			this.UIContext.SetState( uiContextComponents );
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