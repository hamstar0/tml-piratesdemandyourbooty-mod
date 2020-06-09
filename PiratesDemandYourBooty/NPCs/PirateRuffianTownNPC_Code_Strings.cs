using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using static Terraria.ModLoader.ModContent;


namespace PiratesDemandYourBooty.NPCs {
	public partial class PirateRuffianTownNPC : ModNPC {
		public static IReadOnlyList<string> Names { get; private set; } = new List<string> {
			"Gillard",
			"Jon",
			"Kilsome",
			"Roger",
			"Rodney",
			"Crabbe",
			"Turgut",
			"Bouff",
			"Nutt",
			"Morgan",
			"Mud",
			"Tom",
			"Blubber",
			"Bones",
			"Priqu�",
			"Crunch",
			"Gavin",
			"Murat",
			"Riskinner",
			"Sores",
			"Wittebol",
			"Worst",
			"Blackburne"
		}.AsReadOnly();


		////

		public static IReadOnlyList<string> Chats { get; private set; } = new List<string> {
			"Stumble upon some booty ye be not wanting to share wi' our jolly crew? Say it ain't so!",
			"Say, those be some mighty fine pickins ye've chanced upon. Don't be mindin' us partakin'.",
			"A fine establishment ye be hole-in up in, but me sea legs witherin' up on this dry turf. Ye catch me drift?",
			"Spare a copper or two for a hungry sea traveler? Hehehe!",
			"Though ye get used to the scurvy afters a while, sometimes a nice juicy apple is worth more than a handful o' dubloons.",
			"Spinach? Pffeh! Gives us some fresh fish. Raw and wriggling! Ye keep yer nasty green baby food.",
			"T'would almost be a shame dispensin' precious gunpowder to attain our due. Almost.",
			"Yaargh!"
		}.AsReadOnly();

		public static IDictionary<PirateMood, string> Demands { get; } = new Dictionary<PirateMood, string> {
			/*{
				DemandType.Impressed, "Yarrharr! Top o' th' mornin' to ye, scallywag!"
					+ " Y'know, we pirate folk haves our reputations n' all, but nary ye'll meet a more honest n' fair crew than mine!"
					+ " Th'captain likes to reward our more generous, err, patrons with a special offer like no other!"
					+ " If yer bringin' us another grand haul like before, we'll let you keep it all, AND add a bonus!"
					+ " Harharhar, a catch ye say? Nay! There bein' just but one wee stipulation, however:"
					+ " Seein' as how we've got our other merry donors, we can only extend this fine offer to but one lucky soul."
					+ " If'n ye brings us the greatest booty of them all, our offer is yers!"
			},*/
			{
				PirateMood.Normal, "  Ahoy thar, matey! A swell day it is!"
					+ " I come on behalf of me shipmates to beleager ye fer a humble donation."
					+ " T'would be a shame if me mates had to leave empty handed..."
					+ "\n  Don't thinkin' we be all greed n' huff. We've donors from all abouts, and we likes to treat 'em fair-like."
					+ " If'n ye be the most generous of the lot, ye'll keep yer booty, with interest!"
					+ "\n  Whilst awaitin' yer reply, I be layin' up me sea legs hereabouts until the next sunrise."
			},
			{
				PirateMood.Impatient, "Now see here, friend. We be havin' a hard time o' the sea farin' life."
					+ " Surely ye must find it in yer heart to spot us scurvy sea dogs a wee pitance or two."
					+ " I gives ye til' another sunrise to think it over."
			},
			{
				PirateMood.Menacing, "Yarr! I be more than generous leavin' ye breadth fer settlin' yer doubtin' noodle."
					+ " We haven't time fer ye t' be runnin' a rig on us poor blokes."
					+ " Ye gots one more sunrise. Savvy?"
			}
		};


		public static IDictionary<HaggleReplyType, string> HaggleReplies { get; } = new Dictionary<HaggleReplyType, string> {
			{ HaggleReplyType.VeryHigh, "Shiver me t... er, ye gots the right idea, laddy. Ehehehe!" },
			{ HaggleReplyType.High, "*strokes chin*" },
			{ HaggleReplyType.Good, "..." },
			{ HaggleReplyType.Low, "What do I looks like to ye, a street corner wretch?! Ye should plumb yer pock... er, heart fer moren' jus' spare change." },
			{ HaggleReplyType.TooLow, "Yar har har har HAR! Ye've gots a sense o' humor, ye do!" },
		};


		public static IDictionary<HaggleReplyType, string> OfferReplies { get; } = new Dictionary<HaggleReplyType, string> {
			{ HaggleReplyType.VeryHigh, "Hohohoho! Much obliged t' ye!" },
			{ HaggleReplyType.High, "Pleasure doin' business wit' ye!" },
			{ HaggleReplyType.Good, "'Tis acceptable." },
			{ HaggleReplyType.Low, "Arrr! No good! I be takin' what you got, but ye best nah be as stingy th' next time." },
			{ HaggleReplyType.TooLow, "Scupper that! 'Tis an insult jus' t' look at. I suppose'n we jus' take our due th' hard way, then. Unhand yer booty." },
			//{ HaggleReplyType.TooLow, "No sense o' charity? Mayhaps we take some o' the burden off o' ye. Unhand yer booty." },
		};
	}
}
