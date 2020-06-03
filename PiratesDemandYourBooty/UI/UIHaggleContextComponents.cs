using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.UI;
using HamstarHelpers.Helpers.Debug;


namespace PiratesDemandYourBooty.UI {
	class UIHaggleContextComponents : UIState {
		public UIHagglePanel Panel { get; private set; }



		////////////////

		public override void OnInitialize() {
			this.Panel = new UIHagglePanel();
			this.UpdatePanelLayout();
			base.Append( this.Panel );
		}


		////////////////

		public override void Update( GameTime gameTime ) {
			if( this.Panel.IsOpen ) {
				this.Recalculate();
				this.UpdatePanelLayout();
				this.Recalculate();
			}

			base.Update( gameTime );
		}


		////

		public void UpdatePanelLayout() {
			CalculatedStyle dim = this.Panel.GetOuterDimensions();
			float x = (float)(Main.screenWidth / 2) - (dim.Width * 0.5f);
			float y = (float)(Main.screenHeight / 2) - (dim.Height * 0.5f);

			this.Panel.Left.Set( x, 0f );
			this.Panel.Top.Set( y, 0f );
		}
	}
}
