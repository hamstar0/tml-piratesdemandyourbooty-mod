using System;
using System.ComponentModel;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using HamstarHelpers.Classes.UI.ModConfig;


namespace PiratesDemandYourBooty {
	class MyFloatInputElement : FloatInputElement { }




	class PDYBConfig : ModConfig {
		public static PDYBConfig Instance => ModContent.GetInstance<PDYBConfig>();



		////////////

		public override ConfigScope Mode => ConfigScope.ServerSide;


		////////////

		[DefaultValue( 0.15f )]
		[CustomModConfigItem( typeof(MyFloatInputElement) )]
		public float NegotiatorInterestPercent { get; set; } = 0.15f;

		////
		
		[DefaultValue( 4 )]
		public int NegotiatorMinimumTownNPCsForArrival { get; set; } = 4;

		[DefaultValue( 100 )]
		public int NegotiatorMinimumMoneyForArrival { get; set; } = 100;

		[DefaultValue( 60 * 60 * 24 * 3 )]	// 3 "days"
		public int NegotiatorMinimumTicksUntilReturn { get; set; } = 60 * 60 * 24 * 3;

		[DefaultValue( 60 * 60 * 24 * 2 )]	// +2 "days"
		public int NegotiatorAddedTicksUntilReturnAfterRaid { get; set; } = 60 * 60 * 24 * 2;

		////

		[DefaultValue( 60 * 60 * 15 )]	// 15 minutes
		public int RaidDurationTicks { get; set; } = 60 * 60 * 15;

		[DefaultValue( 15 )]
		public int PirateRaiderKillsNearTownNPCBeforeClear { get; set; } = 15;
	}
}
