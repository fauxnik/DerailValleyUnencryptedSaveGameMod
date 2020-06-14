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

        static void Load(UnityModManager.ModEntry modEntry)
        {
            var harmony = HarmonyInstance.Create(modEntry.Info.Id);
            harmony.PatchAll(Assembly.GetExecutingAssembly());
            modEntry.OnToggle = OnToggle;
        }

        static bool OnToggle(UnityModManager.ModEntry modEntry, bool isTogglingOn)
        {
            isModEnabled = isTogglingOn;
            return true;
        }

        [HarmonyPatch(typeof(SaveGameManager), "GetPassword")]
        class SaveGameManager_GetPassword_Patch
        {
            [HarmonyPostfix]
            static void GetPasswordAlwaysReturnsNull(ref string __result)
            {
                if (isModEnabled)
                {
                    __result = null;
                }
            }
        }

        [HarmonyPatch(typeof(SaveGameManager), "GetSavePath")]
        class SaveGameManager_GetSavePath_Patch
        {
            [HarmonyPostfix]
            static void AppendJsonToPath(ref string __result)
            {
                if (isModEnabled && !__result.EndsWith(".json"))
                {
                    __result += ".json";
                }
            }
        }
    }
}
