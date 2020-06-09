using System;
using Terraria;


namespace PiratesDemandYourBooty {
	partial class HaggleLogic {
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

		public HaggleReplyType GaugeOffer( long offer ) {
			double measure = (double)offer / (double)this.PirateDemand;

			if( measure >= 1.75d ) {
				return HaggleReplyType.VeryHigh;
			} else if( measure >= 1.25d && measure < 1.75d ) {
				return HaggleReplyType.High;
			} else if( measure >= 1.0d && measure < 1.25d ) {
				return HaggleReplyType.Good;
			} else if( measure >= 0.5d && measure < 1.0d ) {
				return HaggleReplyType.Low;
			} else {    // if( measure < 0.5d )
				return HaggleReplyType.TooLow;
			}
		}


		////////////////

		public void GiveFinalOffer( long offerAmount ) {
		}
	}
}
