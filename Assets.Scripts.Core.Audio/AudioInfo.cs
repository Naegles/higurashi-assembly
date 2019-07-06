using Assets.Scripts.Core.Buriko;
using System;

namespace Assets.Scripts.Core.Audio
{
	[Serializable]
	public class AudioInfo
	{
		public static AudioInfo Instance;

		public float Volume;

		public string Filename;

		public int Channel;

		public AudioInfo(float volume, string filename, int channel)
		{
			Volume = volume;
			Filename = filename;
			Channel = channel;
		}
	}

	public class MODAudioInfo
	{
		public static MODAudioInfo Instance;

		public static int BGMChannel;

		public static string OG_BGMFilename;

		public static string Console_BGMFilename;

		public static string MG_BGMFilename;

		public static float BGMVolume;

		public static float BGMFade;

		public MODAudioInfo(float BGMvolume, string OG_BGMfilename, string Console_BGMfilename, string MG_BGMfilename, int BGMchannel)
		{
			BGMChannel = BGMchannel;
			OG_BGMFilename = OG_BGMfilename;
			Console_BGMFilename = Console_BGMfilename;
			MG_BGMFilename = MG_BGMfilename;
			BGMVolume = BGMvolume;
			
		}
	}
}
