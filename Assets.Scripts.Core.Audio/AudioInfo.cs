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
}
