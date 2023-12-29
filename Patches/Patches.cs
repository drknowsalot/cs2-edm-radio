using System;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using Game.SceneFlow;
using Game.UI;
using HarmonyLib;
using UnityEngine;

namespace drknowsalot_EDM_Radio.patches
{
    [HarmonyPatch(typeof(GameManager), "InitializeThumbnails")]
    internal class GameManager_InitializeThumbnails
    {

        static readonly string pathToZip = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\CustomRadios.zip";
        static readonly string PathToCustomRadios = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\CustomRadios";

        static readonly string IconsResourceKey = $"{MyPluginInfo.PLUGIN_NAME.ToLower()}";

        public static readonly string COUIBaseLocation = $"coui://{IconsResourceKey}";

        static void Prefix(GameManager __instance)
        {

            if (File.Exists(pathToZip))
            {
                if (Directory.Exists(PathToCustomRadios)) Directory.Delete(PathToCustomRadios, true);
                ZipFile.ExtractToDirectory(pathToZip, Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
                File.Delete(pathToZip);
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

            ExtendedRadio.CustomRadios.RegisterCustomRadioDirectory(PathToCustomRadios);

        }
    }
}
