using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace Subnautica_Enhanced_Sleep
{
    class TirednessGui : MonoBehaviour
    {
        public static TirednessGui _Instance = null;
        public static string currentTiredness = "";
        public static float currentTirednessV = 0;
        public static bool isShow = false;
        public static bool showDevValues = false;

        public void Awake()
        {
            TirednessGui._Instance = this;
        }

        public void Update()
        {
            if (Player.main != null)
            {
                float _tir = Tiredness.tiredness;
                //Tiredness.tirednessDict.TryGetValue(Player.main, out float _tir);
                TirednessGui.currentTiredness = "Tir=" + _tir;
                //TirednessGui.currentTiredness = GameModeUtils.IsOptionActive(GameModeOption.Survival).ToString() + "|" + GameModeUtils.IsOptionActive(GameModeOption.NoSurvival).ToString();
                TirednessGui.currentTirednessV = (_tir > 100 ? 100 : _tir);
            }
        }

        public void OnGUI()
        {
            if (!TirednessGui.isShow)
            {
                //return;
            }

            if (Tiredness.isIngame && !uGUI.main.loading.IsLoading && !uGUI.main.intro.showing)
            {

                GUIStyle style = new GUIStyle();
                //style.normal.background = Texture2D.blackTexture;
                style.normal.textColor = Color.white;
                style.fontSize = 16;
                if (Main.isDev && showDevValues)
                {
                    GUI.Label(new Rect(100, 100, 400, 100), currentTiredness, style);
                }

                Texture2D txBar = new Texture2D(1, 1);
                txBar.wrapMode = TextureWrapMode.Repeat;
                txBar.SetPixel(0, 0, new Color(0, 125, 130, 255));
                // COLORS
                //  0 125 130
                //  0 145 140
                txBar.Apply();
                Texture2D txBlack = new Texture2D(1, 1);
                txBlack.wrapMode = TextureWrapMode.Repeat;
                txBlack.SetPixel(0, 0, new Color(0, 0, 0, 255));
                txBlack.Apply();
                GUI.DrawTexture(new Rect(Screen.width - Main.config.minusfromwidth, Screen.height - Main.config.minusfromheight, 100, 20), txBlack);
                GUI.DrawTexture(new Rect(Screen.width - Main.config.minusfromwidth, Screen.height - Main.config.minusfromheight, 100 / 100 * currentTirednessV, 20), txBar);
                GUIStyle styleprog = new GUIStyle();
                styleprog.alignment = TextAnchor.MiddleCenter;
                if (currentTirednessV >= 50)
                {
                    styleprog.normal.textColor = Color.black;
                }
                else
                {
                    styleprog.normal.textColor = Color.white;
                }
                GUI.Label(new Rect(Screen.width - Main.config.minusfromwidth, Screen.height - Main.config.minusfromheight, 100, 20), "" + (float)Math.Floor(currentTirednessV), styleprog);
            }
        }

        public static void Load()


        {
            GameObject gameObject = new GameObject("TirednessGUI").AddComponent<TirednessGui>().gameObject;

        }
    }

}
