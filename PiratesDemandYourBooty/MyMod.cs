using Terraria;
using Terraria.UI;
using Terraria.ModLoader;
using PiratesDemandYourBooty.UI;


namespace PiratesDemandYourBooty {
	public partial class PDYBMod : Mod {
		public static string GithubUserName => "hamstar0";
		public static string GithubProjectName => "tml-piratesdemandyourbooty-mod";


		////////////////

		public static PDYBMod Instance { get; private set; }



		////////////////

		private UserInterface UIContext;


		////////////////

		internal UIHaggleContextComponents UIContextComponents { get; private set; }


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


		////////////////

		public override void PostUpdateEverything() {
			PirateLogic.Instance.Update();
		}
	}
}