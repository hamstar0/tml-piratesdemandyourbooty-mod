using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.ID;
using HamstarHelpers.Classes.UI.Elements;
using HamstarHelpers.Classes.UI.Theme;
using HamstarHelpers.Helpers.Misc;
using static Terraria.ModLoader.ModContent;


namespace PiratesDemandYourBooty.UI {
	public partial class UIHagglePanel : UIThemedPanel {
		private IList<IToggleable> Components = new List<IToggleable>();



		////////////////

		public override void OnInitialize() {
			float xOffset = 8f;
			float yOffset = 0f;

			this.InitializeCoinInput(
				"Platinum coins",
				MiscHelpers.PlatinumCoinColor,
				v => this.SetValue( v, ItemID.PlatinumCoin ),
				ref xOffset,
				ref yOffset );
			this.InitializeCoinInput(
				"Gold coins",
				MiscHelpers.GoldCoinColor,
				v => this.SetValue( v, ItemID.GoldCoin ),
				ref xOffset,
				ref yOffset );
			this.InitializeCoinInput(
				"Silver coins",
				MiscHelpers.SilverCoinColor,
				v => this.SetValue( v, ItemID.SilverCoin ),
				ref xOffset,
				ref yOffset );
			this.InitializeCoinInput(
				"Copper coins",
				MiscHelpers.CopperCoinColor,
				v => this.SetValue( v, ItemID.CopperCoin ),
				ref xOffset,
				ref yOffset );

			yOffset += 48f;

			var offer = new UITextPanelButton( this.Theme, "Make Offer" );
			offer.Left.Set( 0f, 0f );
			offer.Top.Set( yOffset, 0f );

			var myworld = GetInstance<PDYBWorld>();
			string unit = PDYBWorld.GetHighestCoinTypeOfGivenDemand( myworld.PirateDemand, out bool tensOf );
			string range = tensOf ? "10-99" : "0-9";

			var titleElem = new UIThemedText( this.Theme, false, "Pirate hints at "+range+" "+unit );
			titleElem.Left.Set( -192f, 1f );
			titleElem.Top.Set( 0f, 0f );
			this.AppendThemed( titleElem );

			this.AppendThemed( offer );
			this.Components.Add( offer );
		}


		////

		private void InitializeCoinInput(
					string hint,
					Color color,
					Func<int, bool> valueFunc,
					ref float xOffset,
					ref float yOffset ) {
			var titleElem = new UIThemedText( this.Theme, false, hint );
			titleElem.Left.Set( xOffset, 0f );
			titleElem.Top.Set( yOffset, 0f );
			this.AppendThemed( titleElem );

			var inputElem = new UITextInputAreaPanel( this.Theme, "", 2 );
			inputElem.Left.Set( xOffset + 8f, 0f );
			inputElem.Top.Set( yOffset + 28f, 0f );
			inputElem.Width.Set( 80f, 0f );
			inputElem.Height.Set( 24f, 0f );
			inputElem.TextColor = color;
			inputElem.OnPreTextChange += strBuild => {
				int rawVal;
				if( !Int32.TryParse(strBuild.ToString(), out rawVal) ) {
					return false;
				}
				if( rawVal < 0 || rawVal >= 100 ) {
					return false;
				}
				return valueFunc( rawVal );
			};
			this.AppendThemed( inputElem );
			this.Components.Add( inputElem );

			xOffset += 112f;
		}
	}
}
