using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityModManagerNet;
using Harmony12;
using UnityEngine;
using System.Reflection;

namespace UnencryptedSaveGameMod
{
    public class Main
    {

        private static bool isModEnabled;
        private static bool defaultUseEncryption;

        static void Load(UnityModManager.ModEntry modEntry)
        {
            var harmony = HarmonyInstance.Create(modEntry.Info.Id);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            modEntry.OnToggle = OnToggle;
        }

        static bool OnToggle(UnityModManager.ModEntry modEntry, bool isTogglingOn)
        {
            isModEnabled = isTogglingOn;
            if (SaveGameManager.Exists)
                SaveGameManager.Instance.useEncryption = isModEnabled ? false : defaultUseEncryption;
            return true;
        }

        [HarmonyPatch(typeof(SaveGameManager), "Start")]
        class SaveGameManager_GetPassword_Patch
        {
            [HarmonyPrefix]
            static void Prefix(SaveGameManager __instance)
            {
                defaultUseEncryption = __instance.useEncryption;
                if (isModEnabled) __instance.useEncryption = false;
            }
        }
    }
}
