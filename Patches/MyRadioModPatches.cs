using System.IO;
using System.Reflection;
using Game.SceneFlow;
using Game.UI;
using HarmonyLib;
using UnityEngine;

namespace MyRadioMod.Patches
{
	[HarmonyPatch(typeof(GameManager), "InitializeThumbnails")]
	internal class GameManager_InitializeThumbnails
	{	
		static readonly string IconsResourceKey = $"{MyPluginInfo.PLUGIN_NAME.ToLower()}";

		public static readonly string COUIBaseLocation = $"coui://{IconsResourceKey}";

		static void Prefix(GameManager __instance)
		{		

			Directory.CreateDirectory(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "CustomRadio"));

			string resources = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "resources");

			if(!Directory.Exists(resources)) {
				Directory.CreateDirectory(resources);
				File.Move(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "DefaultIcon.svg"), Path.Combine(resources , "DefaultIcon.svg"));
			}

			var gameUIResourceHandler = (GameUIResourceHandler)GameManager.instance.userInterface.view.uiSystem.resourceHandler;
			
			if (gameUIResourceHandler == null)
			{
				Debug.LogError("Failed retrieving GameManager's GameUIResourceHandler instance, exiting.");
				return;
			}
			
			gameUIResourceHandler.HostLocationsMap.Add(
				IconsResourceKey,
				[
					Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)
				]
			);

			ExtendedRadio.ExtendedRadio.CallOnRadioLoad += MyRadioMod.OnRadioLoad;

		}
	}
}
