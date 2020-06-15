using System;
using Terraria;
using Terraria.ID;
using HamstarHelpers.Helpers.Items;


namespace PiratesDemandYourBooty {
	partial class PirateLogic {
		public static HaggleAmount GaugeOffer( long demand, long offer ) {
			double measure = (double)offer / (double)demand;

			if( measure >= 1.75d ) {
				return HaggleAmount.VeryHigh;
			} else if( measure >= 1.25d && measure < 1.75d ) {
				return HaggleAmount.High;
			} else if( measure >= 1.0d && measure < 1.25d ) {
				return HaggleAmount.Good;
			} else if( measure >= 0.5d && measure < 1.0d ) {
				return HaggleAmount.Low;
			} else {    // if( measure < 0.5d )
				return HaggleAmount.TooLow;
			}
		}

		public static string GetHighestCoinTypeOfGivenDemand( long demand, out bool tensOf ) {
			int baseLog10 = (int)Math.Log10( demand );
			//int baseLog100 = (int)(Math.Log10( demand ) * 0.5d);
			//long logged = (long)Math.Pow( 100, baseLog100 );

			tensOf = ( baseLog10 % 2 ) != 0;

			switch( baseLog10 ) {
			case 0:
			case 1:
				return "copper coins";
			case 2:
			case 3:
				return "silver coins";
			case 4:
			case 5:
				return "gold coins";
			case 6:
			case 7:
				return "platinum coins";
			default:
				return "unknown currency";
			}
		}



		////////////////

		public void GiveNoOffer( bool syncFromServer ) {
			this.BeginRaid( syncFromServer );
		}


		public void GiveFinalOffer( Player player, long offerTested, long offerAmount, bool syncFromServer ) {
			HaggleAmount measure = PirateLogic.GaugeOffer( this.ComputedDemand, offerAmount );

			// Testing for lower amounts lowers negotiator's patience
			if( offerTested > offerAmount ) {
				if( measure != HaggleAmount.VeryHigh && measure != HaggleAmount.High ) {
					switch( this.Patience ) {
					case PirateMood.Normal:
						this.Patience = PirateMood.Impatient;
						break;
					case PirateMood.Impatient:
						this.Patience = PirateMood.Menacing;
						break;
					case PirateMood.Menacing:
						//this.BeginRaid( syncFromServer );
						break;
					}
				}
			}

			switch( measure ) {
			case HaggleAmount.VeryHigh:
			case HaggleAmount.High:
				switch( this.Patience ) {
				case PirateMood.Impatient:
					this.Patience = PirateMood.Normal;
					break;
				case PirateMood.Menacing:
					this.Patience = PirateMood.Impatient;
					break;
				}
				this.GiveGoodOffer( player, offerAmount, syncFromServer );
				break;

			case HaggleAmount.Good:
				this.GiveGoodOffer( player, offerAmount, syncFromServer );
				break;
			case HaggleAmount.Low:
				switch( this.Patience ) {
				case PirateMood.Normal:
					this.Patience = PirateMood.Impatient;
					this.GiveLowOffer( player, offerAmount, syncFromServer );
					break;
				case PirateMood.Impatient:
					this.Patience = PirateMood.Menacing;
					this.GiveLowOffer( player, offerAmount, syncFromServer );
					break;
				case PirateMood.Menacing:
					this.Patience = PirateMood.Normal;
					this.BeginRaid( syncFromServer );
					break;
				}
				break;

			case HaggleAmount.TooLow:
				this.BeginRaid( syncFromServer );
				break;
			}
		}

		////

		private void GiveGoodOffer( Player player, long offerAmount, bool syncFromServer ) {
			if( Main.netMode == NetmodeID.MultiplayerClient ) {
				return;
			}

			int[] itemWhos = ItemHelpers.CreateCoins(
				amount: (long)( (double)offerAmount * PDYBConfig.Instance.NegotiatorReturnInterestPercent ),
				position: player.Center
			);

			if( syncFromServer && Main.netMode == NetmodeID.Server ) {
				foreach( int itemWho in itemWhos ) {
					NetMessage.SendData(
						msgType: MessageID.SyncItem,
						remoteClient: -1,
						ignoreClient: -1,
						text: null,
						number: itemWho
					);
				}
			}
		}

		private void GiveLowOffer( Player player, long offerAmount, bool syncFromServer ) {
			player.BuyItem( (int)offerAmount );
		}
	}
}
