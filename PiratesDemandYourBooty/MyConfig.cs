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
		public float PirateInterestPercent { get; set; } = 0.15f;
	}
}
