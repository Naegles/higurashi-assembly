using System;
using System.IO;
using UnityEngine;

namespace MOD.Scripts.AudioSwitch
{
	public class AudioSwitchData
	{
		public static AudioSwitchData Instance
		{
			get;
			private set;
		}

		//Folders
		public static string[] AudioFolders = new string[7];

		public static string BGMRoot_Folder;

		public static string SERoot_Folder;
	}

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

		public static float Fade;

		public void ModSetAudioFolders(string OG_Folder, string April2019_Folder, string Console_Folder, string MG_Folder, string Anime_Folder, string Italo_Folder)
		// AST | On start up, set audio folders 1-6 based on what is in init.txt
		{
			AudioSwitchData.AudioFolders[0] = Application.streamingAssetsPath;
			AudioSwitchData.AudioFolders[1] = OG_Folder;
			AudioSwitchData.AudioFolders[2] = April2019_Folder;
			AudioSwitchData.AudioFolders[3] = Console_Folder;
			AudioSwitchData.AudioFolders[4] = MG_Folder;
			AudioSwitchData.AudioFolders[5] = Anime_Folder;
			AudioSwitchData.AudioFolders[6] = Italo_Folder;
			AudioSwitchData.BGMRoot_Folder = Path.Combine(AudioSwitchData.AudioFolders[0], "BGM\\"); ;
			AudioSwitchData.SERoot_Folder = Path.Combine(AudioSwitchData.AudioFolders[0], "SE\\"); ;
		}
	}
}
