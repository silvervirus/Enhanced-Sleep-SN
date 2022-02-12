using System;
using System.IO;
using System.Reflection;
using System.Threading;
using HarmonyLib;
using UnityEngine;
using UnityEngine.SceneManagement;
using QModManager.API.ModLoading;
using SMLHelper.V2.Handlers;
using SMLHelper.V2.Crafting;
using System.Collections.Generic;

namespace Subnautica_Enhanced_Sleep
{
    [QModCore]
    public class Main : MonoBehaviour
    {
        public static Harmony hinstance;

        public static bool isBeta = true;
        public static bool isDev = true;

        public static string fileName;
        public static string logDir;
        internal static Config config { get; } = OptionsPanelHandler.Main.RegisterModOptions<Config>();
        [QModPatch]
        public static void Patch()
        {
            
            
            fileName = "" + DateTime.Now.Day + "-" + DateTime.Now.Month + "-" + DateTime.Now.Year + "--" +
                       DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" + DateTime.Now.Second + ".log";
            try
            {
                hinstance = new Harmony("subnauticaenhancedsleep");
                SleepPatcher.invokeAssets();
                Tiredness.initAssets();
                SceneManager.sceneLoaded += OnSceneLoaded;
                SceneManager.sceneUnloaded += OnSceneUnloaded;
                hinstance.PatchAll(Assembly.GetExecutingAssembly());
                if (isDev) Log("Keep in mind that this is a developer version.");
                else if (isBeta) Log("Keep in mind that this is a beta version.");
                Log("Patched Successfully");
            }
            catch (Exception ex)
            {
                Log("[FATAL CRITICAL ERROR] Could not Patch: " + ex.Message);
                Log("[FATAL CRITICAL ERROR] Stack Trace: " + ex.StackTrace);
            }
            CraftDataHandler.GetTechData(TechType.NarrowBed);
            TechData techData = new TechData();
            techData = new TechData
                 {
                     craftAmount = 1,
                     Ingredients = new List<Ingredient>
             {
                    new Ingredient(TechType.FiberMesh, 2),
                    new Ingredient(TechType.Titanium, 3),
                    new Ingredient(TechType.Silicone, 1)
             }
                 };
            CraftDataHandler.SetTechData(TechType.NarrowBed, techData);
            KnownTechHandler.UnlockOnStart(TechType.NarrowBed);
            Log("PATCHING BED AND UNLOCKING AT START ");
        }

        public static void Log(string message)
        {
            Console.WriteLine("[Enhanced Sleep] <" + DateTime.Now.ToString("HH:mm:ss") + "> " + message);
            logDir = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
            File.AppendAllText(logDir + "/" + fileName,
                "<" + DateTime.Now.ToString("HH:mm:ss") + "> " + message + "\n");
        }

        public static string getLog()
        {
            string s = "";
            if ((logDir != null && !logDir.Equals("")) && File.Exists(logDir + "/" + fileName))
            {
                s += File.ReadAllText(logDir + "/" + fileName);
            }

            return s;
        }

        public static string GetSaveGameDir()
        {
            return Path.Combine(Path.Combine(Path.GetFullPath("SNAppData"), "SavedGames"), SaveLoadManager.GetTemporarySavePath());
        }


        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            Log("[DEBUG] Scene loaded: " + scene.name);
            if (scene.name == "Main")
            {
                Log("Initiating Tiredness for current world.");
                Tiredness.onEnable();
            }
        }

        private static void OnSceneUnloaded(Scene scene)
        {
            Log("[DEBUG] Scene unloaded: " + scene.name);
            if (scene.name == "Main")
            {
                Log("Unloading Tiredness.");
                Tiredness.onDisable();
            }
        }

        public class EnhancedSleepComp : MonoBehaviour
        {
            public static EnhancedSleepComp _instance = null;
            public static bool isSaving = false;

            public void Awake()
            {
                EnhancedSleepComp._instance = this;
            }

            public void Update()
            {
                bool _saving = SaveLoadManager.main.isSaving;

                if (!isSaving && _saving)
                {
                    isSaving = true;
                    Main.Log("[DEBUG] Saving Tiredness");
                    Tiredness.SaveTiredness();
                    Main.Log("[DEBUG] Saved Tiredness");
                }
                else if (!_saving)
                {
                    isSaving = false;

                }
                else
                {

                }
            }

            public static void Load()
            {
                GameObject gameObject = new GameObject("EnhancedSleepComp").AddComponent<EnhancedSleepComp>().gameObject;

                 
            }
        }
    }
}
