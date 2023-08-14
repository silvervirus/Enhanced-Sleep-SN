using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using HarmonyLib;
using BepInEx;
using Nautilus.Crafting;
using Nautilus.Handlers;
using UnityEngine;
using UnityEngine.SceneManagement;
using static CraftData;

namespace Subnautica_Enhanced_Sleep
{
	[BepInPlugin(GUID, MODNAME, VERSION)]

	public class Main : BaseUnityPlugin
	{
		#region[Declarations]

		public const string
			MODNAME = "EnhancedSleep",
			AUTHOR = "Cookay",
			GUID = AUTHOR + "_" + MODNAME,
			VERSION = "1.0.0.0";
        #endregion
        internal static Config config { get; } = OptionsPanelHandler.RegisterModOptions<Config>();

		// Token: 0x06000003 RID: 3 RVA: 0x000020E8 File Offset: 0x000002E8
		
		public void Awake()
		{
			Main.fileName = string.Concat(new string[]
			{
				DateTime.Now.Day.ToString(),
				"-",
				DateTime.Now.Month.ToString(),
				"-",
				DateTime.Now.Year.ToString(),
				"--",
				DateTime.Now.Hour.ToString(),
				"-",
				DateTime.Now.Minute.ToString(),
				"-",
				DateTime.Now.Second.ToString(),
				".log"
			});
			try
			{
				Main.hinstance = new Harmony("subnauticaenhancedsleep");
				SleepPatcher.invokeAssets();
				Tiredness.initAssets();
				SceneManager.sceneLoaded += Main.OnSceneLoaded;
				SceneManager.sceneUnloaded += Main.OnSceneUnloaded;
				Main.hinstance.PatchAll(Assembly.GetExecutingAssembly());
				bool flag = Main.isDev;
				if (flag)
				{
					Main.Log("Keep in mind that this is a developer version.");
				}
				else
				{
					bool flag2 = Main.isBeta;
					if (flag2)
					{
						Main.Log("Keep in mind that this is a beta version.");
					}
				}
				Main.Log("Patched Successfully");
			}
			catch (Exception ex)
			{
				Main.Log("[FATAL CRITICAL ERROR] Could not Patch: " + ex.Message);
				Main.Log("[FATAL CRITICAL ERROR] Stack Trace: " + ex.StackTrace);
			}
			CraftDataHandler.GetRecipeData(TechType.NarrowBed);
			RecipeData techData = new RecipeData();
			techData = new RecipeData
			{
				craftAmount = 1,
				Ingredients = new List<Ingredient>
				{
					new Ingredient(TechType.FiberMesh, 2),
					new Ingredient(TechType.Titanium, 3),
					new Ingredient(TechType.Silicone, 1)
				}
			};
			CraftDataHandler.SetRecipeData(TechType.NarrowBed, techData);
			KnownTechHandler.UnlockOnStart(TechType.NarrowBed);
			Main.Log("PATCHING BED AND UNLOCKING AT START ");
		}

		// Token: 0x06000004 RID: 4 RVA: 0x00002310 File Offset: 0x00000510
		public static void Log(string message)
		{
			Console.WriteLine("[Enhanced Sleep] <" + DateTime.Now.ToString("HH:mm:ss") + "> " + message);
			Main.logDir = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
			File.AppendAllText(Main.logDir + "/" + Main.fileName, string.Concat(new string[]
			{
				"<",
				DateTime.Now.ToString("HH:mm:ss"),
				"> ",
				message,
				"\n"
			}));
		}

		// Token: 0x06000005 RID: 5 RVA: 0x000023B0 File Offset: 0x000005B0
		public static string getLog()
		{
			string text = "";
			bool flag = Main.logDir != null && !Main.logDir.Equals("") && File.Exists(Main.logDir + "/" + Main.fileName);
			if (flag)
			{
				text += File.ReadAllText(Main.logDir + "/" + Main.fileName);
			}
			return text;
		}

		// Token: 0x06000006 RID: 6 RVA: 0x00002424 File Offset: 0x00000624
		public static string GetSaveGameDir()
		{
			return Path.Combine(Path.Combine(Path.GetFullPath("SNAppData"), "SavedGames"), SaveLoadManager.GetTemporarySavePath());
		}

		// Token: 0x06000007 RID: 7 RVA: 0x00002454 File Offset: 0x00000654
		private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
		{
			Main.Log("[DEBUG] Scene loaded: " + scene.name);
			bool flag = scene.name == "Main";
			if (flag)
			{
				Main.Log("Initiating Tiredness for current world.");
				Tiredness.onEnable();
			}
		}

		// Token: 0x06000008 RID: 8 RVA: 0x000024A4 File Offset: 0x000006A4
		private static void OnSceneUnloaded(Scene scene)
		{
			Main.Log("[DEBUG] Scene unloaded: " + scene.name);
			bool flag = scene.name == "Main";
			if (flag)
			{
				Main.Log("Unloading Tiredness.");
				Tiredness.onDisable();
			}
		}

		// Token: 0x0400000D RID: 13
		public static Harmony hinstance;

		// Token: 0x0400000E RID: 14
		public static bool isBeta = true;

		// Token: 0x0400000F RID: 15
		public static bool isDev = true;

		// Token: 0x04000010 RID: 16
		public static string fileName;

		// Token: 0x04000011 RID: 17
		public static string logDir;

		// Token: 0x02000008 RID: 8
		public class EnhancedSleepComp : MonoBehaviour
		{
			// Token: 0x06000021 RID: 33 RVA: 0x000030A7 File Offset: 0x000012A7
			public void Patch()
			{
				Main.EnhancedSleepComp._instance = this;
			}

			// Token: 0x06000022 RID: 34 RVA: 0x000030B0 File Offset: 0x000012B0
			public void Update()
			{
				bool flag = SaveLoadManager.main.isSaving;
				bool flag2 = !Main.EnhancedSleepComp.isSaving && flag;
				if (flag2)
				{
					Main.EnhancedSleepComp.isSaving = true;
					Main.Log("[DEBUG] Saving Tiredness");
					Tiredness.SaveTiredness();
					Main.Log("[DEBUG] Saved Tiredness");
				}
				else
				{
					bool flag3 = !flag;
					if (flag3)
					{
						Main.EnhancedSleepComp.isSaving = false;
					}
				}
			}

			// Token: 0x06000023 RID: 35 RVA: 0x00003114 File Offset: 0x00001314
			public static void Load()
			{
				GameObject gameObject = new GameObject("EnhancedSleepComp").AddComponent<Main.EnhancedSleepComp>().gameObject;
			}

			// Token: 0x04000035 RID: 53
			public static Main.EnhancedSleepComp _instance;

			// Token: 0x04000036 RID: 54
			public static bool isSaving;
		}
	}
}
