﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Terraria.ID;
using HamstarHelpers.Classes.UI.Elements;
using HamstarHelpers.Helpers.Misc;
using HamstarHelpers.Classes.UI.Theme;


namespace PiratesDemandYourBooty.UI {
	public partial class UIHagglePanel : UIThemedPanel {
		private IList<IToggleable> Components = new List<IToggleable>();



		////////////////

		public override void OnInitialize() {
			float xOffset = 0f;
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

			yOffset += 32f;

			var offer = new UITextPanelButton( this.Theme, "Make Offer" );
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
			var input = new UITextInputAreaPanel( this.Theme, hint, 2 );
			input.Left.Set( 0f, xOffset );
			input.Top.Set( 0f, yOffset );
			input.TextColor = color;
			input.OnPreTextChange += strBuild => {
				int rawVal;
				if( !Int32.TryParse(strBuild.ToString(), out rawVal) ) {
					return false;
				}
				if( rawVal < 0 || rawVal >= 100 ) {
					return false;
				}
				return valueFunc( rawVal );
			};

			this.AppendThemed( input );
			this.Components.Add( input );

			xOffset += 32f;
		}
	}
}