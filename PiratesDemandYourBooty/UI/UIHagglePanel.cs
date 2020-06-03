using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Classes.UI.Elements;
using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Players;
using PiratesDemandYourBooty.NPCs;


namespace PiratesDemandYourBooty.UI {
	public partial class UIHagglePanel : UIThemedPanel {
		public bool IsOpen { get; private set; }

		public long Value { get; private set; }



		////////////////

		public UIHagglePanel() : base( UITheme.Vanilla, false ) {
			this.Width.Set( 256f, 0f );
			this.Height.Set( 32f, 0f );
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

		public bool SetValue( int newTypedValue, int coinItemType ) {
			if( newTypedValue < 0 || newTypedValue >= 100 ) {
				return false;
			}

			long value = this.Value;

			switch( coinItemType ) {
			case ItemID.PlatinumCoin:
				value %= 1000000L;
				value += (long)newTypedValue * 1000000L;
				break;
			case ItemID.GoldCoin:
				long plats = this.Value / 1000000L;
				value %= 10000;
				value += (long)newTypedValue * 10000;
				value += plats * 1000000L;
				break;
			case ItemID.SilverCoin:
				long golds = this.Value / 10000L;
				value %= 100;
				value += (long)newTypedValue * 100L;
				value += golds * 10000L;
				break;
			case ItemID.CopperCoin:
				long silvs = this.Value / 100L;
				value += (long)newTypedValue;
				value += silvs * 100L;
				break;
			default:
				return false;
			}

			long max = PlayerItemHelpers.CountMoney( Main.LocalPlayer, false );
			if( value > max ) {
				return false;
			}

			this.Value = value;

			return true;
		}


		////////////////

		public override void Update( GameTime gameTime ) {
			if( !this.IsOpen ) {
				return;
			}

			Player plr = Main.LocalPlayer;

			if( !plr.dead && PirateRuffianTownNPC.GetNearbyPirate(plr) != null ) {
				base.Update( gameTime );
			} else {
				if( this.IsOpen ) {
					this.Close();
				}
			}
		}


		////////////////

		public override void Draw( SpriteBatch spriteBatch ) {
			if( !this.IsOpen ) {
				return;
			}

			base.Draw( spriteBatch );
		}
	}
}
