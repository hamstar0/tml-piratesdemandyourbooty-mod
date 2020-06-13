using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using HamstarHelpers.Classes.UI.Elements;
using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Players;


namespace PiratesDemandYourBooty.UI {
	public partial class UIHagglePanel : UIThemedPanel {
		private UITextPanelButton OfferButtonElem;


		////////////////

		public bool IsOpen { get; private set; } = false;

		public long OfferTotal { get; private set; }



		////////////////
		
		public UIHagglePanel() : base( UITheme.Vanilla, false ) {
			this.Width.Set( 512f, 0f );
			this.Height.Set( 120f, 0f );
		}


		////////////////
		
		public void Open( bool offerTested ) {
			this.IsOpen = true;

			foreach( IToggleable comp in this.Components ) {
				var themedComp = comp as IThemeable;
				themedComp?.Show();
				comp.Enable();
			}

			if( offerTested ) {
				this.OfferButtonElem.SetText( "Confirm Offer" );
			} else {
				this.OfferButtonElem.SetText( "Test Offer" );
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
		
		public void Reset() {
			this.OfferTotal = 0;

			foreach( IToggleable elem in this.Components ) {
				var textElem = elem as UITextInputAreaPanel;
				if( textElem == null ) { continue; }

				textElem.SetTextDirect( "0" );
			}
		}


		////////////////

		public override void Update( GameTime gameTime ) {
			if( !this.IsOpen ) {
				return;
			}

			base.Update( gameTime );

			long money = PlayerItemHelpers.CountMoney( Main.LocalPlayer, false );
			if( this.OfferTotal > money ) {
				this.Reset();
				Main.NewText( "Don't be tryin t' pull a fast one, matey!", Color.Yellow );
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
