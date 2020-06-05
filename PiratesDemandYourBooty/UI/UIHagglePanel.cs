using System;
using Terraria;
using HamstarHelpers.Classes.UI.Elements;
using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Helpers.Debug;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace PiratesDemandYourBooty.UI {
	public partial class UIHagglePanel : UIThemedPanel {
		public bool IsOpen { get; private set; } = false;

		public long Value { get; private set; }



		////////////////

		public UIHagglePanel() : base( UITheme.Vanilla, false ) {
			this.Width.Set( 480f, 0f );
			this.Height.Set( 96f, 0f );
		}


		////////////////
		
		public void Open() {
			this.IsOpen = true;

			foreach( IToggleable comp in this.Components ) {
				var themedComp = comp as IThemeable;
				themedComp?.Show();
				comp.Enable();
			}
		}

		public void Close() {
			foreach( IToggleable comp in this.Components ) {
				var themedComp = comp as IThemeable;
				themedComp?.Hide();
				comp.Disable();
			}

			this.IsOpen = false;
		}


		////////////////

		public override void Update( GameTime gameTime ) {
			if( this.IsOpen ) {
				base.Update( gameTime );
			}
		}


		////////////////

		public override void Draw( SpriteBatch spriteBatch ) {
			if( this.IsOpen ) {
				base.Draw( spriteBatch );
			}
		}
	}
}
