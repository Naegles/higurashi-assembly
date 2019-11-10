using Assets.Scripts.Core.Buriko;
using MOD.Scripts.AudioSwitch;
using Assets.Scripts.Core.Audio;
using MOD.Scripts.Core;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Collections.Generic;

namespace Assets.Scripts.Core.State
{
	internal class StateNormal : IGameState
	{
		private GameSystem gameSystem;

		public StateNormal()
		{
			gameSystem = GameSystem.Instance;
		}

		public void RequestLeaveImmediate()
		{
		}

		public void RequestLeave()
		{
		}

		public void OnLeaveState()
		{
		}

		public void OnRestoreState()
		{
		}

		public bool InputHandler()
		{
			if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
			{
				if (!gameSystem.CanSkip)
				{
					return true;
				}
				gameSystem.IsSkipping = true;
				gameSystem.IsForceSkip = true;
				if (gameSystem.WaitList.Count > 0)
				{
					return true;
				}
				return true;
			}
			if (gameSystem.IsForceSkip)
			{
				gameSystem.IsSkipping = false;
				gameSystem.IsForceSkip = false;
			}
			if (Input.GetKeyDown(KeyCode.R))
			{
				var voices = gameSystem.TextHistory.LatestVoice;
				AudioController.Instance.PlayVoices(voices);
				return false;
			}
			if (Input.GetKeyDown(KeyCode.Space))
			{
				if (!gameSystem.MessageBoxVisible && gameSystem.GameState == GameState.Normal)
				{
					return false;
				}
				gameSystem.IsSkipping = false;
				gameSystem.IsForceSkip = false;
				gameSystem.IsAuto = false;
				gameSystem.SwitchToViewMode();
				return false;
			}
			if (Input.GetAxis("Mouse ScrollWheel") > 0f || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.PageUp))
			{
				if (!gameSystem.MessageBoxVisible && gameSystem.GameState == GameState.Normal)
				{
					return false;
				}
				gameSystem.SwitchToHistoryScreen();
				return false;
			}
			if (Input.GetMouseButtonDown(0) || Input.GetAxis("Mouse ScrollWheel") < 0f || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.PageDown) || Input.GetKeyDown(KeyCode.KeypadEnter))
			{
				if (gameSystem.IsSkipping)
				{
					gameSystem.IsSkipping = false;
				}
				if (gameSystem.IsAuto && !gameSystem.ClickDuringAuto)
				{
					gameSystem.IsAuto = false;
					if (gameSystem.WaitList.Exists((Wait a) => a.Type == WaitTypes.WaitForAuto))
					{
						gameSystem.AddWait(new Wait(0f, WaitTypes.WaitForInput, null));
					}
					return false;
				}
				if (UICamera.hoveredObject == gameSystem.SceneController.SceneCameras || UICamera.hoveredObject == null)
				{
					gameSystem.ClearWait();
				}
				return false;
			}
			if (!Input.GetMouseButtonDown(1) && !Input.GetKeyDown(KeyCode.Escape))
			{
				if (Input.GetKey(KeyCode.LeftShift))
				{
					if (Input.GetKeyDown(KeyCode.F10))
					{
						if (!gameSystem.MessageBoxVisible || gameSystem.IsAuto || gameSystem.IsSkipping || gameSystem.IsForceSkip)
						{
							return false;
						}
						if (!gameSystem.HasWaitOfType(WaitTypes.WaitForInput))
						{
							return false;
						}
						if (BurikoMemory.Instance.GetGlobalFlag("GMOD_DEBUG_MODE").IntValue() == 0)
						{
							return false;
						}
						if (BurikoMemory.Instance.GetGlobalFlag("GMOD_DEBUG_MODE").IntValue() == 1)
						{
							BurikoMemory.Instance.SetGlobalFlag("GMOD_DEBUG_MODE", 2);
							GameSystem.Instance.AudioController.PlaySystemSound("switchsound/enable.ogg");
							return true;
						}
						BurikoMemory.Instance.SetGlobalFlag("GMOD_DEBUG_MODE", 1);
						GameSystem.Instance.AudioController.PlaySystemSound("switchsound/disable.ogg");
						return true;
					}
					if (Input.GetKeyDown(KeyCode.F9))
					{
						if (!gameSystem.MessageBoxVisible || gameSystem.IsAuto || gameSystem.IsSkipping || gameSystem.IsForceSkip)
						{
							return false;
						}
						if (!gameSystem.HasWaitOfType(WaitTypes.WaitForInput))
						{
							return false;
						}
						int num = BurikoMemory.Instance.GetGlobalFlag("GMOD_SETTING_LOADER").IntValue();
						if (num < 3 && num >= 0)
						{
							num++;
							string str = num.ToString();
							string str2 = ".ogg";
							string filename = "switchsound/" + str + str2;
							GameSystem.Instance.AudioController.PlaySystemSound(filename);
							BurikoMemory.Instance.SetGlobalFlag("GMOD_SETTING_LOADER", num);
							return true;
						}
						num = 0;
						BurikoMemory.Instance.SetGlobalFlag("GMOD_SETTING_LOADER", num);
						GameSystem.Instance.AudioController.PlaySystemSound("switchsound/0.ogg");
					}
					if (Input.GetKeyDown(KeyCode.M))
					{
						if (!gameSystem.MessageBoxVisible || gameSystem.IsAuto || gameSystem.IsSkipping || gameSystem.IsForceSkip)
						{
							return false;
						}
						if (!gameSystem.HasWaitOfType(WaitTypes.WaitForInput))
						{
							return false;
						}
						int num2 = BurikoMemory.Instance.GetGlobalFlag("GVoiceVolume").IntValue();
						if (num2 == 100)
						{
							return true;
						}
						num2 = 100;
						float voiceVolume = (float)num2 / 100f;
						BurikoMemory.Instance.SetGlobalFlag("GVoiceVolume", num2);
						GameSystem.Instance.AudioController.VoiceVolume = voiceVolume;
						GameSystem.Instance.AudioController.RefreshLayerVolumes();
						return true;
					}
					if (Input.GetKeyDown(KeyCode.N))
					{
						if (!gameSystem.MessageBoxVisible || gameSystem.IsAuto || gameSystem.IsSkipping || gameSystem.IsForceSkip)
						{
							return false;
						}
						if (!gameSystem.HasWaitOfType(WaitTypes.WaitForInput))
						{
							return false;
						}
						if (BurikoMemory.Instance.GetGlobalFlag("GVoiceVolume").IntValue() == 0)
						{
							return true;
						}
						int num3 = 0;
						float voiceVolume2 = (float)num3 / 100f;
						BurikoMemory.Instance.SetGlobalFlag("GVoiceVolume", num3);
						GameSystem.Instance.AudioController.VoiceVolume = voiceVolume2;
						GameSystem.Instance.AudioController.RefreshLayerVolumes();
						return true;
					}
				}
				if (Input.GetKeyDown(KeyCode.F1))
				{
					if (!gameSystem.MessageBoxVisible || gameSystem.IsAuto || gameSystem.IsSkipping || gameSystem.IsForceSkip)
					{
						return false;
					}
					if (!gameSystem.HasWaitOfType(WaitTypes.WaitForInput))
					{
						return false;
					}
					if (BurikoMemory.Instance.GetFlag("NVL_in_ADV").IntValue() == 1)
					{
						return false;
					}
					int num = BurikoMemory.Instance.GetGlobalFlag("GTextbox").IntValue();
					int num2 = BurikoMemory.Instance.GetGlobalFlag("GTextboxMaxNum").IntValue();
					if (num < num2 && num >= 0)
					{
						num++;
						BurikoMemory.Instance.SetGlobalFlag("GTextbox", num);
						GameSystem.Instance.MainUIController.MODResetLayerBackground();
						return true;
					}
					num = 0;
					BurikoMemory.Instance.SetGlobalFlag("GTextbox", num);
					GameSystem.Instance.MainUIController.MODResetLayerBackground();
				}
				if (Input.GetKeyDown(KeyCode.F2))
				{
					if (!gameSystem.MessageBoxVisible || gameSystem.IsAuto || gameSystem.IsSkipping || gameSystem.IsForceSkip)
					{
						return false;
					}
					if (!gameSystem.HasWaitOfType(WaitTypes.WaitForInput))
					{
						return false;
					}
					if (BurikoMemory.Instance.GetFlag("DisableModHotkey").IntValue() == 1)
					{
						return false;
					}
					int num4 = BurikoMemory.Instance.GetGlobalFlag("GCensor").IntValue();
					int num5 = BurikoMemory.Instance.GetGlobalFlag("GCensorMaxNum").IntValue();
					if (num4 < num5 && num4 >= 0)
					{
						num4++;
						string str3 = num4.ToString();
						string str4 = ".ogg";
						string filename2 = "switchsound/" + str3 + str4;
						GameSystem.Instance.AudioController.PlaySystemSound(filename2);
						BurikoMemory.Instance.SetGlobalFlag("GCensor", num4);
						return true;
					}
					num4 = 0;
					BurikoMemory.Instance.SetGlobalFlag("GCensor", num4);
					GameSystem.Instance.AudioController.PlaySystemSound("switchsound/0.ogg");
				}
				if (Input.GetKeyDown(KeyCode.F3))
				{
					if (!gameSystem.MessageBoxVisible || gameSystem.IsAuto || gameSystem.IsSkipping || gameSystem.IsForceSkip)
					{
						return false;
					}
					if (!gameSystem.HasWaitOfType(WaitTypes.WaitForInput))
					{
						return false;
					}
					if (BurikoMemory.Instance.GetFlag("DisableModHotkey").IntValue() == 1)
					{
						return false;
					}
					int num6 = BurikoMemory.Instance.GetGlobalFlag("GEffectExtend").IntValue();
					int num7 = BurikoMemory.Instance.GetGlobalFlag("GEffectExtendMaxNum").IntValue();
					if (num6 < num7 && num6 >= 0)
					{
						num6++;
						string str5 = num6.ToString();
						string str6 = ".ogg";
						string filename3 = "switchsound/" + str5 + str6;
						GameSystem.Instance.AudioController.PlaySystemSound(filename3);
						BurikoMemory.Instance.SetGlobalFlag("GEffectExtend", num6);
						return true;
					}
					num6 = 0;
					BurikoMemory.Instance.SetGlobalFlag("GEffectExtend", num6);
					GameSystem.Instance.AudioController.PlaySystemSound("switchsound/0.ogg");
				}
				if (Input.GetKeyDown(KeyCode.F5))
				{
					if (!gameSystem.MessageBoxVisible || gameSystem.IsAuto || gameSystem.IsSkipping || gameSystem.IsForceSkip)
					{
						return false;
					}
					if (!gameSystem.HasWaitOfType(WaitTypes.WaitForInput))
					{
						return false;
					}
					if (!gameSystem.CanSave)
					{
						return false;
					}
					BurikoScriptSystem.Instance.SaveQuickSave();
					GameSystem.Instance.AudioController.PlaySystemSound("switchsound/enable.ogg");
				}
				if (Input.GetKeyDown(KeyCode.F7))
				{
					if (!gameSystem.MessageBoxVisible || gameSystem.IsAuto || gameSystem.IsSkipping || gameSystem.IsForceSkip)
					{
						return false;
					}
					if (!gameSystem.HasWaitOfType(WaitTypes.WaitForInput))
					{
						return false;
					}
					BurikoScriptSystem.Instance.LoadQuickSave();
				}
				if (Input.GetKeyDown(KeyCode.F10))
				{
					if (!gameSystem.MessageBoxVisible || gameSystem.IsAuto || gameSystem.IsSkipping || gameSystem.IsForceSkip)
					{
						return false;
					}
					if (!gameSystem.HasWaitOfType(WaitTypes.WaitForInput))
					{
						return false;
					}
					if (BurikoMemory.Instance.GetGlobalFlag("GMOD_DEBUG_MODE").IntValue() != 1 && BurikoMemory.Instance.GetGlobalFlag("GMOD_DEBUG_MODE").IntValue() != 2)
					{
						if (BurikoMemory.Instance.GetFlag("LFlagMonitor").IntValue() == 0)
						{
							BurikoMemory.Instance.SetFlag("LFlagMonitor", 1);
							return true;
						}
						if (BurikoMemory.Instance.GetFlag("LFlagMonitor").IntValue() == 1)
						{
							BurikoMemory.Instance.SetFlag("LFlagMonitor", 2);
							return true;
						}
						BurikoMemory.Instance.SetFlag("LFlagMonitor", 0);
						return true;
					}
					int num8 = BurikoMemory.Instance.GetFlag("LFlagMonitor").IntValue();
					if (num8 < 4)
					{
						num8++;
						BurikoMemory.Instance.SetFlag("LFlagMonitor", num8);
						return true;
					}
					if (num8 >= 4 || num8 < 0)
					{
						BurikoMemory.Instance.SetFlag("LFlagMonitor", 0);
						return true;
					}
				}
				if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0))
				{
					if (!gameSystem.MessageBoxVisible || gameSystem.IsAuto || gameSystem.IsSkipping || gameSystem.IsForceSkip)
					{
						return false;
					}
					if (!gameSystem.HasWaitOfType(WaitTypes.WaitForInput))
					{
						return false;
					}
					if (BurikoMemory.Instance.GetGlobalFlag("GMOD_DEBUG_MODE").IntValue() == 1 || BurikoMemory.Instance.GetGlobalFlag("GMOD_DEBUG_MODE").IntValue() == 2)
					{
						GameSystem.Instance.MainUIController.MODDebugFontSizeChanger();
					}
				}
				if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
				{
					if (!gameSystem.MessageBoxVisible || gameSystem.IsAuto || gameSystem.IsSkipping || gameSystem.IsForceSkip)
					{
						return false;
					}
					if (!gameSystem.HasWaitOfType(WaitTypes.WaitForInput))
					{
						return false;
					}
					if (BurikoMemory.Instance.GetFlag("DisableModHotkey").IntValue() == 1)
					{
						return false;
					}
					if (BurikoMemory.Instance.GetGlobalFlag("GAltBGM").IntValue() == 1)
					{
						BurikoMemory.Instance.SetGlobalFlag("GAltBGM", 0);
						GameSystem.Instance.AudioController.PlaySystemSound("switchsound/disable.ogg");
						return true;
					}
					BurikoMemory.Instance.SetGlobalFlag("GAltBGM", 1);
					GameSystem.Instance.AudioController.PlaySystemSound("switchsound/enable.ogg");
				}
				if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
				{
					if (!gameSystem.MessageBoxVisible || gameSystem.IsAuto || gameSystem.IsSkipping || gameSystem.IsForceSkip)
					{
						return false;
					}
					if (!gameSystem.HasWaitOfType(WaitTypes.WaitForInput))
					{
						return false;
					}
					if (BurikoMemory.Instance.GetFlag("DisableModHotkey").IntValue() == 1)
					{
						return false;
					}
					int num9 = BurikoMemory.Instance.GetGlobalFlag("GAltBGMflow").IntValue();
					int num10 = BurikoMemory.Instance.GetGlobalFlag("GAltBGMflowMaxNum").IntValue();
					string OG_BGM = AudioSwitch.OG_BGMFilename;
					string Console_BGM = AudioSwitch.Console_BGMFilename;
					string MG_BGM = AudioSwitch.MG_BGMFilename;
					int BGM_Channel = AudioSwitch.Channel;
					float BGM_Volume = AudioSwitch.Volume;
					float BGM_Fade = AudioSwitch.Fade;
					//AST | Checks if BGM directory exists, and skips to the next available that does exist, unfortunately if no others exists, the same track will restart with every button press
					List<string> BGMFolders = new List<string>
					{ "", AudioSwitchData.AudioFolders[1], AudioSwitchData.AudioFolders[2], AudioSwitchData.AudioFolders[3], AudioSwitchData.AudioFolders[4], AudioSwitchData.AudioFolders[5] };
					foreach (string BGMfolder in BGMFolders)
					{
						if (num9 == BGMFolders.IndexOf(BGMfolder) - 1 && !(Directory.Exists(AudioSwitchData.BGMRoot_Folder + BGMfolder)))
						{
							num9++;
						}	
					}
					num9++;
					if (num9 > num10)
					{
						num9 = 0;
					}
					string str7 = num9.ToString();
					string str8 = ".ogg";
					string filename4 = "switchsound/" + str7 + str8;
					GameSystem.Instance.AudioController.PlaySystemSound(filename4);
					BurikoMemory.Instance.SetGlobalFlag("GAltBGMflow", num9);
					//AST | Plays BGM of new OST selection if Italo flag is 0 or an Italo Remake doesn't exist
					if (BurikoMemory.Instance.GetGlobalFlag("GItaloVer").IntValue() == 0 || !File.Exists(AudioSwitchData.AudioFolders[0] + AudioSwitchData.AudioFolders[6] + "\\" + OG_BGM))
					{
						List<string> BGMs = new List<string>
							{ OG_BGM, AudioSwitchData.AudioFolders[1] + "\\" + OG_BGM, AudioSwitchData.AudioFolders[2] + "\\" + OG_BGM, AudioSwitchData.AudioFolders[3]+ "\\" + Console_BGM, AudioSwitchData.AudioFolders[4] + "\\" + MG_BGM, AudioSwitchData.AudioFolders[5] + "\\" + OG_BGM };
						foreach (string BGM in BGMs)
						{
							if (BurikoMemory.Instance.GetGlobalFlag("GAltBGMflow").IntValue() == BGMs.IndexOf(BGM))
							{
								AudioController.Instance.PlayAudio(BGM, Audio.AudioType.BGM, BGM_Channel, BGM_Volume, BGM_Fade); ;
							}
						}
					}
					return true;
				}
				if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
				{
					if (!gameSystem.MessageBoxVisible || gameSystem.IsAuto || gameSystem.IsSkipping || gameSystem.IsForceSkip)
					{
						return false;
					}
					if (!gameSystem.HasWaitOfType(WaitTypes.WaitForInput))
					{
						return false;
					}
					if (BurikoMemory.Instance.GetFlag("DisableModHotkey").IntValue() == 1)
					{
						return false;
					}
					if (BurikoMemory.Instance.GetGlobalFlag("GAltSE").IntValue() == 1)
					{
						BurikoMemory.Instance.SetGlobalFlag("GAltSE", 0);
						GameSystem.Instance.AudioController.PlaySystemSound("switchsound/disable.ogg");
						return true;
					}
					BurikoMemory.Instance.SetGlobalFlag("GAltSE", 1);
					GameSystem.Instance.AudioController.PlaySystemSound("switchsound/enable.ogg");
				}
				if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
				{
					if (!gameSystem.MessageBoxVisible || gameSystem.IsAuto || gameSystem.IsSkipping || gameSystem.IsForceSkip)
					{
						return false;
					}
					if (!gameSystem.HasWaitOfType(WaitTypes.WaitForInput))
					{
						return false;
					}
					if (BurikoMemory.Instance.GetFlag("DisableModHotkey").IntValue() == 1)
					{
						return false;
					}
					int num11 = BurikoMemory.Instance.GetGlobalFlag("GAltSEflow").IntValue();
					int num12 = BurikoMemory.Instance.GetGlobalFlag("GAltSEflowMaxNum").IntValue();
					//AST | Same as BGM, checks for SE folder, skips to next available one that folder exists for
					List<string> SEFolders = new List<string>
					{ "", AudioSwitchData.AudioFolders[1], AudioSwitchData.AudioFolders[2], AudioSwitchData.AudioFolders[3], AudioSwitchData.AudioFolders[4] };
					foreach (string SEfolder in SEFolders)
					{
						if (num11 == SEFolders.IndexOf(SEfolder) - 1 && !(Directory.Exists(AudioSwitchData.SERoot_Folder + SEfolder)))
						{
							num11++;
						}
					}
					num11++;
					if (num11 > num12)
					{
						num11 = 0;
					}
					string str9 = num11.ToString();
					string str10 = ".ogg";
					string filename5 = "switchsound/" + str9 + str10;
					GameSystem.Instance.AudioController.PlaySystemSound(filename5);
					BurikoMemory.Instance.SetGlobalFlag("GAltSEflow", num11);
					return true;
				}
				if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5))
				{
					if (!gameSystem.MessageBoxVisible || gameSystem.IsAuto || gameSystem.IsSkipping || gameSystem.IsForceSkip)
					{
						return false;
					}
					if (!gameSystem.HasWaitOfType(WaitTypes.WaitForInput))
					{
						return false;
					}
					if (BurikoMemory.Instance.GetFlag("DisableModHotkey").IntValue() == 1)
					{
						return false;
					}
					if (BurikoMemory.Instance.GetGlobalFlag("GAltVoice").IntValue() == 1)
					{
						BurikoMemory.Instance.SetGlobalFlag("GAltVoice", 0);
						GameSystem.Instance.AudioController.PlaySystemSound("switchsound/disable.ogg");
						return true;
					}
					BurikoMemory.Instance.SetGlobalFlag("GAltVoice", 1);
					GameSystem.Instance.AudioController.PlaySystemSound("switchsound/enable.ogg");
				}
				if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6))
				{
					if (!gameSystem.MessageBoxVisible || gameSystem.IsAuto || gameSystem.IsSkipping || gameSystem.IsForceSkip)
					{
						return false;
					}
					if (!gameSystem.HasWaitOfType(WaitTypes.WaitForInput))
					{
						return false;
					}
					if (BurikoMemory.Instance.GetFlag("DisableModHotkey").IntValue() == 1)
					{
						return false;
					}
					if (BurikoMemory.Instance.GetGlobalFlag("GAltVoicePriority").IntValue() == 1)
					{
						BurikoMemory.Instance.SetGlobalFlag("GAltVoicePriority", 0);
						GameSystem.Instance.AudioController.PlaySystemSound("switchsound/disable.ogg");
						return true;
					}
					BurikoMemory.Instance.SetGlobalFlag("GAltVoicePriority", 1);
					GameSystem.Instance.AudioController.PlaySystemSound("switchsound/enable.ogg");
				}
				if (Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Keypad7))
				{
					if (!gameSystem.MessageBoxVisible || gameSystem.IsAuto || gameSystem.IsSkipping || gameSystem.IsForceSkip)
					{
						return false;
					}
					if (!gameSystem.HasWaitOfType(WaitTypes.WaitForInput))
					{
						return false;
					}
					if (BurikoMemory.Instance.GetFlag("DisableModHotkey").IntValue() == 1)
					{
						return false;
					}
					if (BurikoMemory.Instance.GetGlobalFlag("GLipSync").IntValue() == 1)
					{
						BurikoMemory.Instance.SetGlobalFlag("GLipSync", 0);
						GameSystem.Instance.AudioController.PlaySystemSound("switchsound/disable.ogg");
						return true;
					}
					BurikoMemory.Instance.SetGlobalFlag("GLipSync", 1);
					GameSystem.Instance.AudioController.PlaySystemSound("switchsound/enable.ogg");
				}
				if (Input.GetKeyDown(KeyCode.M))
				{
					if (!gameSystem.MessageBoxVisible || gameSystem.IsAuto || gameSystem.IsSkipping || gameSystem.IsForceSkip)
					{
						return false;
					}
					if (!gameSystem.HasWaitOfType(WaitTypes.WaitForInput))
					{
						return false;
					}
					int num13 = BurikoMemory.Instance.GetGlobalFlag("GVoiceVolume").IntValue();
					if (num13 == 100)
					{
						return true;
					}
					num13 = ((num13 > 95 || num13 < 0) ? 50 : (num13 + 5));
					float voiceVolume3 = (float)num13 / 100f;
					BurikoMemory.Instance.SetGlobalFlag("GVoiceVolume", num13);
					GameSystem.Instance.AudioController.VoiceVolume = voiceVolume3;
					GameSystem.Instance.AudioController.RefreshLayerVolumes();
					return true;
				}
				if (!Input.GetKeyDown(KeyCode.N))
				{
					if (Input.GetKeyDown(KeyCode.A))
					{
						gameSystem.IsAuto = !gameSystem.IsAuto;
						if (gameSystem.IsAuto)
						{
							return true;
						}
						if (gameSystem.WaitList.Exists((Wait a) => a.Type == WaitTypes.WaitForAuto))
						{
							gameSystem.AddWait(new Wait(0f, WaitTypes.WaitForInput, null));
						}
					}
					if (Input.GetKeyDown(KeyCode.S))
					{
						gameSystem.IsSkipping = !gameSystem.IsSkipping;
					}
					if (Input.GetKeyDown(KeyCode.F))
					{
						if (GameSystem.Instance.IsFullscreen)
						{
							int num14 = PlayerPrefs.GetInt("width");
							int num15 = PlayerPrefs.GetInt("height");
							if (num14 == 0 || num15 == 0)
							{
								num14 = 640;
								num15 = 480;
							}
							GameSystem.Instance.DeFullscreen(width: num14, height: num15);
						}
						else
						{
							GameSystem.Instance.GoFullscreen();
						}
					}
					if (Input.GetKeyDown(KeyCode.L))
					{
						if (!gameSystem.MessageBoxVisible || gameSystem.IsAuto || gameSystem.IsSkipping || gameSystem.IsForceSkip)
						{
							return false;
						}
						if (!gameSystem.HasWaitOfType(WaitTypes.WaitForText))
						{
							GameSystem.Instance.UseEnglishText = !GameSystem.Instance.UseEnglishText;
							int val = 0;
							if (gameSystem.UseEnglishText)
							{
								val = 1;
							}
							gameSystem.TextController.SwapLanguages();
							BurikoMemory.Instance.SetGlobalFlag("GLanguage", val);
						}
					}
					if (Input.GetKeyDown(KeyCode.P))
					{
						if (!gameSystem.MessageBoxVisible || gameSystem.IsAuto || gameSystem.IsSkipping || gameSystem.IsForceSkip)
						{
							return false;
						}
						if (!gameSystem.HasWaitOfType(WaitTypes.WaitForInput))
						{
							return false;
						}
						MODSystem.instance.modTextureController.ToggleArtStyle();
					}
					if (Input.GetKeyDown(KeyCode.I)) //AST | Hotkey to toggle between using Italo's remakes in place of what would play normally, if no version exists for the current section, it will instead play the current OST
					{
						if (!gameSystem.MessageBoxVisible || gameSystem.IsAuto || gameSystem.IsSkipping || gameSystem.IsForceSkip)
						{
							return false;
						}
						if (!gameSystem.HasWaitOfType(WaitTypes.WaitForInput))
						{
							return false;
						}
						if (BurikoMemory.Instance.GetFlag("DisableModHotkey").IntValue() == 1)
						{
							return false;
						}
						string OG_BGM = AudioSwitch.OG_BGMFilename;
						string Console_BGM = AudioSwitch.Console_BGMFilename;
						string MG_BGM = AudioSwitch.MG_BGMFilename;
						int BGM_Channel = AudioSwitch.Channel;
						float BGM_Volume = AudioSwitch.Volume;
						float BGM_Fade = AudioSwitch.Fade;
						if (BurikoMemory.Instance.GetGlobalFlag("GItaloVer").IntValue() == 1)
						{
							BurikoMemory.Instance.SetGlobalFlag("GItaloVer", 0);
							if (File.Exists(AudioSwitchData.AudioFolders[6] + "\\" + OG_BGM))
							{
								List<string> BGMs = new List<string>
								{ OG_BGM, AudioSwitchData.AudioFolders[1] + "\\" + OG_BGM, AudioSwitchData.AudioFolders[2] + "\\" + OG_BGM, AudioSwitchData.AudioFolders[3] + "\\" + Console_BGM, AudioSwitchData.AudioFolders[4] + "\\" + MG_BGM, AudioSwitchData.AudioFolders[5] + "\\" + OG_BGM };
								foreach (string BGM in BGMs)
								{
									if (BurikoMemory.Instance.GetGlobalFlag("GAltBGMflow").IntValue() == BGMs.IndexOf(BGM))
									{
										AudioController.Instance.PlayAudio(BGM, Audio.AudioType.BGM, BGM_Channel, BGM_Volume, BGM_Fade); ;
									}
								}
							}
							GameSystem.Instance.AudioController.PlaySystemSound("switchsound/disable.ogg");
							return true;
						}
						BurikoMemory.Instance.SetGlobalFlag("GItaloVer", 1);
						if (BurikoMemory.Instance.GetGlobalFlag("GItaloVer").IntValue() == 1 && File.Exists(AudioSwitchData.AudioFolders[6] + "\\" + OG_BGM))
						{
							AudioController.Instance.PlayAudio(AudioSwitchData.AudioFolders[6]  + "\\" + OG_BGM, Audio.AudioType.BGM, BGM_Channel, BGM_Volume, BGM_Fade);
						}
						GameSystem.Instance.AudioController.PlaySystemSound("switchsound/enable.ogg");
					}
					return true;
				}
				if (!gameSystem.MessageBoxVisible || gameSystem.IsAuto || gameSystem.IsSkipping || gameSystem.IsForceSkip)
				{
					return false;
				}
				if (!gameSystem.HasWaitOfType(WaitTypes.WaitForInput))
				{
					return false;
				}
				int num16 = BurikoMemory.Instance.GetGlobalFlag("GVoiceVolume").IntValue();
				if (num16 == 0)
				{
					return true;
				}
				num16 = ((num16 < 5 || num16 > 100) ? 50 : (num16 - 5));
				float voiceVolume4 = (float)num16 / 100f;
				BurikoMemory.Instance.SetGlobalFlag("GVoiceVolume", num16);
				GameSystem.Instance.AudioController.VoiceVolume = voiceVolume4;
				GameSystem.Instance.AudioController.RefreshLayerVolumes();
				return true;
			}
			if (!gameSystem.MessageBoxVisible && gameSystem.GameState == GameState.Normal)
			{
				return false;
			}
			if (gameSystem.IsAuto && gameSystem.ClickDuringAuto)
			{
				gameSystem.IsAuto = false;
				if (gameSystem.WaitList.Exists((Wait a) => a.Type == WaitTypes.WaitForAuto))
				{
					gameSystem.AddWait(new Wait(0f, WaitTypes.WaitForInput, null));
				}
				return false;
			}
			gameSystem.IsSkipping = false;
			gameSystem.IsForceSkip = false;
			if (gameSystem.RightClickMenu)
			{
				gameSystem.SwitchToRightClickMenu();
			}
			else
			{
				gameSystem.SwitchToHiddenWindow2();
			}
			return false;
		}

		public GameState GetStateType()
		{
			return GameState.Normal;
		}
	}
}
