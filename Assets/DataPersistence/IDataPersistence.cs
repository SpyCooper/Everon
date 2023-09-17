using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDataPersistence
{
    // Sets up the LoadData and SaveData function that have to be made for anything that extends IDataPersistence
    void LoadData(GameData data);
    void SaveData(ref GameData data);
}
