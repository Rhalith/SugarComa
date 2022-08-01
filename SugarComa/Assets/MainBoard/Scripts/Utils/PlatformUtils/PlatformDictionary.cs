using System;
using UnityEngine;
using System.Collections.Generic;
using Assets.MainBoard.Scripts.Route;

namespace Assets.MainBoard.Scripts.Utils.PlatformUtils
{
    [Serializable]
    public class PlatformDictionary : Dictionary<Platform, List<Platform>>, ISerializationCallbackReceiver
    {
        [SerializeField, HideInInspector] private List<Platform> _keys = new();
        [SerializeField, HideInInspector] private List<PlatformListWrapper> _values = new();

        public void OnBeforeSerialize()
        {
            _keys.Clear();
            _values.Clear();

            foreach (var kvp in this)
            {
                _keys.Add(kvp.Key);
                _values.Add(new PlatformListWrapper() { nodes = kvp.Value });
            }
        }

        public void OnAfterDeserialize()
        {
            Clear();

            for (var i = 0; i != Math.Min(_keys.Count, _values.Count); i++)
            {
                if (_keys[i] == null || _values[i] == null) continue;

                Add(_keys[i], _values[i].nodes);
            }
        }
    }

    [Serializable]
    public class PlatformListWrapper
    {
        public List<Platform> nodes;
    }
}