﻿using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.UI;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Classes.UI.Theme;
using PiratesDemandYourBooty.NPCs;


namespace PiratesDemandYourBooty.UI {
	class UIHaggleContextComponents : UIThemedState {
		public UIHagglePanel HagglePanel { get; private set; } = null;



		////////////////

		public UIHaggleContextComponents() : base( UITheme.Vanilla, false ) { }

		////

		public override void OnInitialize() {
			this.HagglePanel = new UIHagglePanel();
			this.LayoutPanel();
			this.AppendThemed( this.HagglePanel );
		}


		////

		public override void Recalculate() {
			if( this.HagglePanel != null ) {
				this.LayoutPanel();
			}
			base.Recalculate();
		}


		////

		private void LayoutPanel() {
			CalculatedStyle dim = this.HagglePanel.GetOuterDimensions();
			float x = (float)(Main.screenWidth / 2) - (dim.Width * 0.5f);
			float y = (float)(Main.screenHeight / 2) - (dim.Height * 0.5f);

			this.HagglePanel.Left.Set( x, 0f );
			this.HagglePanel.Top.Set( y, 0f );
		}


		////////////////

		public void OpenHaggleUI( bool offerTested ) {
			this.HagglePanel?.Open( offerTested );
		}

		public void CloseHaggleUI() {
			this.HagglePanel?.Close();
		}


		////////////////

		public override void Update( GameTime gt ) {
			this.UpdateHagglingUIState();
			base.Update( gt );
		}

		////

		private void UpdateHagglingUIState() {
			if( !this.HagglePanel?.IsOpen ?? true ) {
				return;
			}

			Player plr = Main.LocalPlayer;
			bool isHaggling = !plr.dead
				&& !Main.playerInventory
				&& !plr.CCed
				&& PirateNegotiatorTownNPC.GetNearbyNegotiator(plr) != null;

			if( !isHaggling ) {
				this.CloseHaggleUI();
			} else {
				//plr.noItems = true;
			}
		}
	}
}
