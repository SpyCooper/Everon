using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
{
    [SerializeField] private List<TKey> keys = new List<TKey>();
    [SerializeField] private List<TValue> values = new List<TValue>();

    // save dictionaries to list
    public void OnBeforeSerialize()
    {
        // clears the lost of keys and values
        keys.Clear();
        values.Clear();
        
        // adds the pair of keys and values
        foreach(KeyValuePair<TKey, TValue> pair in this)
        {
            keys.Add(pair.Key);
            values.Add(pair.Value);
        }
    }

    // load dictionaries to list
    public void OnAfterDeserialize()
    {
        // clears
        this.Clear();

        // if the keys and values do not match, something went wrong
        if(keys.Count != values.Count)
        {
            Debug.LogError("Tried to deserialize a SerializableDictionary, but the keys (" + keys.Count + ") and values (" + values.Count + ") do nott match");
        }

        // adds the pair of keys and values
        for(int i = 0; i < keys.Count; i++)
        {
            this.Add(keys[i], values[i]);
        }
    }
}
