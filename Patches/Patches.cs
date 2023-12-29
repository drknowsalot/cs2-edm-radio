using System;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using Game.SceneFlow;
using Game.UI;
using HarmonyLib;
using UnityEngine;

namespace drknowsalot_EDM_Radio.Patches
{
	[HarmonyPatch(typeof(GameManager), "InitializeThumbnails")]
	internal class GameManager_InitializeThumbnails
	{	

		static readonly string pathToZip = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)+"\\CustomRadios.zip";
		static readonly string PathToCustomRadios = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)+"\\CustomRadios";

		static readonly string IconsResourceKey = $"{MyPluginInfo.PLUGIN_NAME.ToLower()}";

		public static readonly string COUIBaseLocation = $"coui://{IconsResourceKey}";

		static void Prefix(GameManager __instance)
		{	

			// This code extracts the zip if it exists.

			if(File.Exists(pathToZip)) {
				if(Directory.Exists(PathToCustomRadios)) Directory.Delete(PathToCustomRadios, true);
				ZipFile.ExtractToDirectory(pathToZip, Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
				File.Delete(pathToZip);
			}

			// This code add your custom .svg into the game "database", so now you can use "coui://your_assembly_name_to_lower/path_to_image.svg"

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

			// now this lins is going to register the directory of your radio, so the mod know they need to look at this directory to load your radio.

			ExtendedRadio.CustomRadios.RegisterCustomRadioDirectory(PathToCustomRadios);

		}
	}
}
