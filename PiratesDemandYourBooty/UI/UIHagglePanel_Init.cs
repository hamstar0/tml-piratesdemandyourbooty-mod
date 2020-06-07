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
			var logic = PDYBWorld.HaggleLogic;
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

			var offerBut = new UITextPanelButton( this.Theme, "Make Offer" );
			offerBut.Left.Set( 0f, 0f );
			offerBut.Top.Set( yOffset, 0f );
			offerBut.OnClick += ( _, __ ) => {
				if( logic.ReceiveReplyForPirate(this.Value) ) {
					this.Close();
				} else {
					this.Reset();
				}
			};

			string unit = HaggleLogic.GetHighestCoinTypeOfGivenDemand( logic.PirateDemand, out bool tensOf );
			string range = tensOf ? "10-99" : "0-10";

			var titleElem = new UIThemedText( this.Theme, false, "Pirate hints at "+range+" "+unit );
			titleElem.Left.Set( -256f, 1f );
			titleElem.Top.Set( yOffset, 0f );
			this.AppendThemed( titleElem );

			this.AppendThemed( offerBut );
			this.Components.Add( offerBut );
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
				string str = strBuild.ToString();
				if( !Int32.TryParse(str, out int rawVal) ) {
					return str != "";
				}
				return true;
			};
			inputElem.OnUnfocus += () => {
				if( !Int32.TryParse(inputElem.Text, out int rawVal) ) {
					inputElem.SetTextDirect( "0" );
					return;
				}

				int newVal = (int)MathHelper.Clamp( rawVal, 0, 99 );
				if( newVal != rawVal ) {
					inputElem.SetTextDirect( ""+newVal );
				}

				if( newVal > 0 && !valueFunc(newVal) ) {
					inputElem.SetTextDirect( "0" );
					Main.NewText( "Not enough money!", Color.Yellow );
					return;
				}
			};
			this.AppendThemed( inputElem );
			this.Components.Add( inputElem );

			xOffset += 128f;
		}
	}
}
