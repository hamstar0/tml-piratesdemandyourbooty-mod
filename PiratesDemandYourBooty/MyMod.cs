using Terraria;
using Terraria.ModLoader;


namespace PiratesDemandYourBooty {
	public partial class PDYBMod : Mod {
		public static string GithubUserName => "hamstar0";
		public static string GithubProjectName => "tml-piratesdemandyourbooty-mod";


		////////////////

		public static PDYBMod Instance { get; private set; }



		////////////////

		public PDYBMod() {
			PDYBMod.Instance = this;
		}

		////////////////

		public override void Load() {
			if( !Main.dedServ ) {
				this.LoadUI();
			}
		}

		public override void Unload() {
			PDYBMod.Instance = null;
		}
	}
}