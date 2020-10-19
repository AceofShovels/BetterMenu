using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using IPA;
using IPA.Config;
using IPA.Config.Stores;
using UnityEngine.SceneManagement;
using UnityEngine;
using IPALogger = IPA.Logging.Logger;
using System.Runtime.Serialization.Json;
using IPA.Utilities;
using HMUI;
using BetterMenu.Utilities;
using UnityEngine.UI;

namespace BetterMenu
{

    [Plugin(RuntimeOptions.SingleStartInit)]
    public class Plugin
    {
        internal static Plugin Instance { get; private set; }
        internal static IPALogger Log { get; private set; }

        [Init]
        /// <summary>
        /// Called when the plugin is first loaded by IPA (either when the game starts or when the plugin is enabled if it starts disabled).
        /// [Init] methods that use a Constructor or called before regular methods like InitWithConfig.
        /// Only use [Init] with one Constructor.
        /// </summary>
        public void Init(IPALogger logger)
        {
            Instance = this;
            Log = logger;
            Log.Info("BetterMenu initialized.");
        }

        #region BSIPA Config
        //Uncomment to use BSIPA's config
        /*
        [Init]
        public void InitWithConfig(Config conf)
        {
            Configuration.PluginConfig.Instance = conf.Generated<Configuration.PluginConfig>();
            Log.Debug("Config loaded");
        }
        */
        #endregion

        [OnStart]
        public void OnApplicationStart()
        {
            Log.Debug("OnApplicationStart");
            new GameObject("BetterMenuController").AddComponent<BetterMenuController>();
            AddEvents();

        }

        [OnExit]
        public void OnApplicationQuit()
        {
            Log.Debug("OnApplicationQuit");

        }
        public void onLoad(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "MenuViewControllers")
            {
                SharedCoroutineStarter.instance.StartCoroutine(organize());
            }
        }
        private IEnumerator organize()
        {
            bool found = false;
            Transform mainButtons = null;
            while (!found) {
                yield return null;
                Scene menuCore = SceneManager.GetSceneByName("MenuCore");
                GameObject wrapper = menuCore.GetRootGameObjects().First();
                Transform mainMenuViewController = wrapper.transform.Find("ScreenSystem").transform.Find("ScreenContainer").transform.Find("MainScreen").transform.Find("MainMenuViewController");
                if (mainMenuViewController != null)
                {
                    mainButtons = mainMenuViewController.transform.Find("MainButtons");
                    found = true;
                }
            }

            Transform campaign = mainButtons.Find("CampaignButton");
            Transform solo = mainButtons.Find("SoloButton");
            Transform online = mainButtons.Find("OnlineButton");
            Transform party = mainButtons.Find("PartyButton");
            Transform options = mainButtons.Find("OptionsButton");
            Transform editor = mainButtons.Find("EditorButton");
            Transform help = mainButtons.Find("HelpButton");
            Transform exit = mainButtons.Find("ExitButton");

            

            
            

            campaign.position = new Vector3(-0.85f, 1.07f, 2.6f);
            solo.position = new Vector3(-0.22f, 1.07f, 2.60f);
            online.position = new Vector3(0.72f, 1.6f, 2.60f);
            party.position = new Vector3(0.62f, 1.07f, 2.60f);
            (party as RectTransform).sizeDelta = new Vector2(53.6f, 24.0f);

            // Options Button
            GameObject optionsGO = null;
            while (optionsGO == null)
            {
                optionsGO = options.gameObject;
                yield return null;
            }
            ButtonSpriteSwap settingsSpriteSwap = optionsGO.GetComponentInChildren<ButtonSpriteSwap>();
            Sprite settingsSpriteNormal = Utilities.Utils.LoadSpriteFromResources("BetterMenu.Resources.settings.png");
            Sprite settingsSpriteActive = Utilities.Utils.LoadSpriteFromResources("BetterMenu.Resources.settingsActive.png");
            settingsSpriteSwap.SetPrivateField("_normalStateSprite", settingsSpriteNormal);
            settingsSpriteSwap.SetPrivateField("_highlightStateSprite", settingsSpriteActive);

            settingsSpriteSwap.GetPrivateField<Image[]>("_images")[0].sprite = settingsSpriteNormal;
            
            options.position = new Vector3(-0.66f, 0.75f, 2.60f);
            options.localScale = new Vector3(1.35f, 1.35f, 1f);
            
            editor.position = new Vector3(0.12f, 0.79f, 2.60f);
            help.position = new Vector3(0.39f, 0.79f, 2.60f);


            GameObject exitGO = null;
            while (exitGO == null)
            {
                exitGO = exit.gameObject;
                yield return null;
            }
            ButtonSpriteSwap exitSpriteSwap = exitGO.GetComponentInChildren<ButtonSpriteSwap>();
            Sprite exitSpriteNormal = Utilities.Utils.LoadSpriteFromResources("BetterMenu.Resources.exit.png");
            Sprite exitSpriteActive = Utilities.Utils.LoadSpriteFromResources("BetterMenu.Resources.exitActive.png");
            exitSpriteSwap.SetPrivateField("_normalStateSprite", exitSpriteNormal);
            exitSpriteSwap.SetPrivateField("_highlightStateSprite", exitSpriteActive);

            exitSpriteSwap.GetPrivateField<Image[]>("_images")[0].sprite = exitSpriteNormal;
            exit.position = new Vector3(0.83f, 0.63f, 2.60f);
            exit.localScale = new Vector3(2.35f, 2.3f, 1f);
        }
        private void AddEvents()
        {
            RemoveEvents();
            SceneManager.sceneLoaded += onLoad;
        }

        private void RemoveEvents()
        {
            SceneManager.sceneLoaded -= onLoad;
        }
    }
}
