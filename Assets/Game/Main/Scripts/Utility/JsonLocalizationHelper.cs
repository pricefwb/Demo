using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameFramework.Localization;
using UnityGameFramework.Runtime;
using System;
using LitJson;

namespace Game.Main
{
    public class LocalizationSerializableObject
    {
        public string language;
        public Dictionary<string, string> dic;
    }
    public class JsonLocalizationHelper : DefaultLocalizationHelper
    {

        public override bool ParseData(ILocalizationManager localizationManager, string dictionaryString, object userData)
        {
            try
            {
                string currentLanguage = GameEntryMain.Localization.Language.ToString();

                List<LocalizationSerializableObject> localizationSerializableObjects = JsonMapper.ToObject<List<LocalizationSerializableObject>>(dictionaryString);

                foreach (var localizationSerializableObject in localizationSerializableObjects)
                {
                    if (localizationSerializableObject.language != currentLanguage)
                    {
                        continue;
                    }

                    foreach (var item in localizationSerializableObject.dic)
                    {
                        if (!localizationManager.AddRawString(item.Key, item.Value))
                        {
                            Log.Warning("Can not add raw string with key '{0}' which may be invalid or duplicate.", item.Key);
                            return false;
                        }
                    }
                }
                return true;
            }
            catch (Exception exception)
            {
                Log.Warning("Can not parse dictionary data with exception '{0}'.", exception.ToString());
                return false;
            }
        }
    }
}



