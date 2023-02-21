using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class JSONManager
{
    public GameData ReadJsonData()
    {
        TextAsset json = Resources.Load<TextAsset>("GameData");
        return Newtonsoft.Json.JsonConvert.DeserializeObject<GameData>(json.text);
    }

    public void WriteJsonData(GameData gameData)
    {
        string json = Newtonsoft.Json.JsonConvert.SerializeObject(gameData);
        File.WriteAllText(Application.dataPath + "/Resources/GameData.JSON", json);

    }



}
