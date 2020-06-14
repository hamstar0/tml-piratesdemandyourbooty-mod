using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria;
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
			var logic = PDYBWorld.PirateLogic;
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

			yOffset += 72f;

			this.OfferButtonElem = new UITextPanelButton( this.Theme, "Test Offer" );
			this.OfferButtonElem.Left.Set( 8f, 0f );
			this.OfferButtonElem.Top.Set( yOffset, 0f );
			this.OfferButtonElem.OnClick += ( _, __ ) => {
				foreach( var toggleable in this.Components ) {
					var input = toggleable as UITextInputAreaPanel;
					if( input?.HasFocus ?? false ) {
						input.Unfocus();
					}
				}
				this.MakeOffer();
			};

			string unit = PirateLogic.GetHighestCoinTypeOfGivenDemand( logic.PirateDemand, out bool tensOf );
			string range = tensOf ? "10-99" : "0-10";

			var titleElem = new UIThemedText( this.Theme, false, "Pirate hints at "+range+" "+unit );
			titleElem.Left.Set( -256f, 1f );
			titleElem.Top.Set( yOffset, 0f );
			this.AppendThemed( titleElem );

			this.AppendThemed( this.OfferButtonElem );
			this.Components.Add( this.OfferButtonElem );

			this.Close();
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
			inputElem.Left.Set( xOffset, 0f );
			inputElem.Top.Set( yOffset + 28f, 0f );
			inputElem.Width.Set( 96f, 0f );
			inputElem.Height.Pixels = 36f;
			//inputElem.SetPadding( 0f );
			inputElem.HAlign = 0f;
			inputElem.SetTextDirect( "0" );
			inputElem.TextColor = color;
			inputElem.OnPreTextChange += strBuild => {
Main.NewText("1! "+inputElem.GetHashCode());
				string str = strBuild.ToString();
				if( str == "" ) { return true; }
				return this.ProcessCoinInput( str, valueFunc, out string _ );
			};
			inputElem.OnUnfocus += () => {
Main.NewText("2! "+inputElem.GetHashCode());
				if( !this.ProcessCoinInput(inputElem.Text, valueFunc, out string output) ) {
					inputElem.SetTextDirect( output );
				}
			};
			this.AppendThemed( inputElem );
			this.Components.Add( inputElem );

			xOffset += 128f;
		}


		////

		private bool ProcessCoinInput( string input, Func<int, bool> valueFunc, out string output ) {
			output = "0";

			if( !Int32.TryParse( input, out int rawVal ) ) {
				return false;
			}

			int newVal = (int)MathHelper.Clamp( rawVal, 0, 99 );
			if( newVal != rawVal ) {
				output = "" + newVal;
			}

			if( !valueFunc(newVal) ) {
				Main.NewText( "Not enough money!", Color.Yellow );
				return false;
			}

			return true;
		}
	}
}
