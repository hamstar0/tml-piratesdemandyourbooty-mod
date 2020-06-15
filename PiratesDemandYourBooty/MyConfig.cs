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

		public bool DebugModeInfo { get; set; } = false;

		public bool DebugModeCheats { get; set; } = false;

		////

		[DefaultValue( 0.25d )]
		public double BaseDemandIncreasePercentPerVisit { get; set; } = 0.25d;

		[DefaultValue( 3d )]
		public double DemandVariancePercentRange { get; set; } = 3d;

		//

		[DefaultValue( 0.15f )]
		[CustomModConfigItem( typeof(MyFloatInputElement) )]
		public float NegotiatorReturnInterestPercent { get; set; } = 0.15f;

		////
		
		[DefaultValue( 4 )]
		public int NegotiatorMinimumTownNPCsForArrival { get; set; } = 4;

		[DefaultValue( 100 )]
		public int NegotiatorMinimumMoneyForArrival { get; set; } = 100;

		[DefaultValue( 60 * 60 * 24 * 3 )]	// 3 "days"
		public int NegotiatorMinimumTicksUntilReturn { get; set; } = 60 * 60 * 24 * 3;

		[DefaultValue( 60 * 60 * 24 * 1 )]	// +1 "days"
		public int NegotiatorAddedTicksUntilReturnAfterRaid { get; set; } = 60 * 60 * 24 * 1;

		////
		
		[DefaultValue( 60 * 60 * 15 )]	// 15 minutes
		public int RaidDurationTicks { get; set; } = 60 * 60 * 15;

		[DefaultValue( 15 )]
		public int PirateRaiderKillsNearTownNPCBeforeClear { get; set; } = 15;

		////

		[DefaultValue( -16 )]
		public int RaidTimerPositionX { get; set; } = -16;

		[DefaultValue( -32 )]
		public int RaidTimerPositionY { get; set; } = -32;

		////

		[DefaultValue( 20 * 60 )]
		public int PirateRuffianAmbushCooldownDurationTicks { get; set; } = 20 * 60;

		[DefaultValue( 2 * 60 )]
		public int PirateRuffianAmbushBuildupDurationTicks { get; set; } = 2 * 60;
	}
}
