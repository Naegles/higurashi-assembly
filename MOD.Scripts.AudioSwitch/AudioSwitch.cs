using Assets.Scripts.Core.Audio;
using Assets.Scripts.Core.Buriko;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MOD.Scripts.AudioSwitch
{
	[Serializable]
	public class AudioSwitch
	{
		public static AudioSwitch Instance
		{
			get;
			private set;
		}

		public static int Channel;

		public static float Volume;

		public static string OG_BGMFilename;

		public static string Console_BGMFilename;

		public static string MG_BGMFilename;

		public static string BGMRootFolder;

		public static string OriginalFolder;

		public static string April2019UpdateFolder;

		public static string ConsoleFolder;

		public static string MangaGamerFolder;

		public static string AnimeFolder;

		public static string ItaloFolder;

		public static float Fade;

		public void PlayBGM()
		{
			List<string> BGMs = new List<string>
			{ OG_BGMFilename, OriginalFolder + "\\" + OG_BGMFilename, April2019UpdateFolder + "\\" + OG_BGMFilename, ConsoleFolder + "\\" + Console_BGMFilename, MangaGamerFolder + "\\" + MG_BGMFilename, AnimeFolder + "\\" + OG_BGMFilename };
			foreach (string BGM in BGMs)
			{
				if (BurikoMemory.Instance.GetGlobalFlag("GAltBGMflow").IntValue() == BGMs.IndexOf(BGM))
				{
					AudioController.Instance.PlayAudio(BGM, AudioType.BGM, Channel, Volume, Fade);
				}
			}
		}

			//	if (BurikoMemory.Instance.GetGlobalFlag("GItaloVer").IntValue() == 1)
			//{
			//	BurikoMemory.Instance.SetGlobalFlag("GItaloVer", 0);
			//	if (File.Exists(BGMRootFolder + ItaloFolder + OG_BGMFilename))
			//	{
			//		if (BurikoMemory.Instance.GetGlobalFlag("GAltBGMflow").IntValue() == 0)
			//		{
			//			AudioController.Instance.PlayAudio(OG_BGMFilename, AudioType.BGM, Channel, Volume, Fade);
			//		}
			//		if (BurikoMemory.Instance.GetGlobalFlag("GAltBGMflow").IntValue() == 1)
			//		{
			//			AudioController.Instance.PlayAudio("Original\\" + OG_BGMFilename, AudioType.BGM, Channel, Volume, Fade);
			//		}
			//		if (BurikoMemory.Instance.GetGlobalFlag("GAltBGMflow").IntValue() == 2)
			//		{
			//			AudioController.Instance.PlayAudio("April2019Update\\" + OG_BGMFilename, AudioType.BGM, Channel, Volume, Fade);
			//		}
			//		if (BurikoMemory.Instance.GetGlobalFlag("GAltBGMflow").IntValue() == 3)
			//		{
			//			AudioController.Instance.PlayAudio("Console\\" + Console_BGMFilename, AudioType.BGM, Channel, Volume, Fade);
			//		}
			//		if (BurikoMemory.Instance.GetGlobalFlag("GAltBGMflow").IntValue() == 4)
			//		{
			//			AudioController.Instance.PlayAudio("MangaGamer\\" + MG_BGMFilename, AudioType.BGM, Channel, Volume, Fade);
			//		}
			//		if (BurikoMemory.Instance.GetGlobalFlag("GAltBGMflow").IntValue() == 5)
			//		{
			//			AudioController.Instance.PlayAudio("Anime\\" + OG_BGMFilename, AudioType.BGM, Channel, Volume, Fade);
			//		}
			//	}

			//	return;
		//	}
		//}
	}
}
