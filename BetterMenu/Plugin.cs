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
        public void Init(IPALogger logger)
        {
            Instance = this;
            Log = logger;
        }

        [OnStart]
        public void OnApplicationStart()
        {
            SceneManager.sceneLoaded += OnLoad;
        }

        public void OnLoad(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "MenuViewControllers")
            {
                SharedCoroutineStarter.instance.StartCoroutine(Organize());
            }
        }

        private IEnumerator Organize()
        {
            yield return new WaitUntil(() => GameObject.Find("MainButtons"));
            var mainButtons = GameObject.Find("MainButtons");

            Transform campaign = mainButtons.transform.Find("CampaignButton");
            Transform solo = mainButtons.transform.Find("SoloButton");
            Transform online = mainButtons.transform.Find("OnlineButton");
            Transform party = mainButtons.transform.Find("PartyButton");
            Transform options = mainButtons.transform.Find("OptionsButton");
            Transform editor = mainButtons.transform.Find("EditorButton");
            Transform help = mainButtons.transform.Find("HelpButton");
            Transform exit = mainButtons.transform.Find("ExitButton");
 
            // Set button positions
            campaign.position = new Vector3(-0.85f, 1.07f, 2.6f);
            solo.position     = new Vector3(-0.22f, 1.07f, 2.60f);
            online.position   = new Vector3(0.72f, 1.6f, 2.60f);
            party.position    = new Vector3(0.62f, 1.05f, 2.60f);
            options.position  = new Vector3(-0.66f, 0.75f, 2.60f);
            editor.position   = new Vector3(0.12f, 0.79f, 2.60f);
            help.position     = new Vector3(0.39f, 0.79f, 2.60f);
            exit.position     = new Vector3(0.83f, 0.63f, 2.60f);

            // Rescale Settings and Exit buttons
            options.localScale = new Vector3(1.35f, 1.35f, 1.0f);
            exit.localScale    = new Vector3(2.35f, 2.3f, 1.0f);

            // Change solo sprites
            ButtonSpriteSwap soloSpriteSwap = solo.GetComponentInChildren<ButtonSpriteSwap>();
            var soloNormalSprite = Utilities.Utils.LoadSpriteFromResources("BetterMenu.Resources.SoloNormal.png");
            var soloActiveSprite = Utilities.Utils.LoadSpriteFromResources("BetterMenu.Resources.SoloActive.png");
            soloSpriteSwap.SetPrivateField("_normalStateSprite", soloNormalSprite);
            soloSpriteSwap.SetPrivateField("_highlightStateSprite", soloActiveSprite);
            soloSpriteSwap.GetPrivateField<Image[]>("_images")[0].sprite = soloNormalSprite;

            // Change options sprites
            var settingsSpriteSwap = options.GetComponentInChildren<ButtonSpriteSwap>();
            var settingsNormalSprite = Utilities.Utils.LoadSpriteFromResources("BetterMenu.Resources.SettingsNormal.png");
            var settingsActiveSprite = Utilities.Utils.LoadSpriteFromResources("BetterMenu.Resources.SettingsActive.png");
            settingsSpriteSwap.SetPrivateField("_normalStateSprite", settingsNormalSprite);
            settingsSpriteSwap.SetPrivateField("_highlightStateSprite", settingsActiveSprite);
            settingsSpriteSwap.GetPrivateField<Image[]>("_images")[0].sprite = settingsNormalSprite;
            
            // Change exit sprites
            var exitSpriteSwap = exit.GetComponentInChildren<ButtonSpriteSwap>();
            Sprite exitNormalSprite = Utilities.Utils.LoadSpriteFromResources("BetterMenu.Resources.ExitNormal.png");
            Sprite exitActiveSprite = Utilities.Utils.LoadSpriteFromResources("BetterMenu.Resources.ExitActive.png");
            exitSpriteSwap.SetPrivateField("_normalStateSprite", exitNormalSprite);
            exitSpriteSwap.SetPrivateField("_highlightStateSprite", exitActiveSprite);
            exitSpriteSwap.GetPrivateField<Image[]>("_images")[0].sprite = exitNormalSprite;
        }
    }
}
