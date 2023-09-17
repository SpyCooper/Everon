using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest : MonoBehaviour, IDataPersistence
{
    // **** you'll need to hit the generate guid for id in the three dots section of the script section of the game object this is one

    // tbh idk how this works rn

    [SerializeField] private string id;
    [ContextMenu("Generate guid for id")]
    private void GenerateGuid()
    {
        id = System.Guid.NewGuid().ToString();
    }

    private bool compeleted;

    public void LoadData(GameData data)
    {
        data.questsCompleted.TryGetValue(id, out compeleted);
        if(compeleted)
        {
            // quest is completed
        }
    }
    
    public void SaveData(ref GameData data)
    {
        if(data.questsCompleted.ContainsKey(id))
        {
            data.questsCompleted.Remove(id);
        }
        data.questsCompleted.Add(id, compeleted);
    }
}
