using System;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Classes.UI.Elements;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.Players;


namespace PiratesDemandYourBooty.UI {
	public partial class UIHagglePanel : UIThemedPanel {
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
	}
}
