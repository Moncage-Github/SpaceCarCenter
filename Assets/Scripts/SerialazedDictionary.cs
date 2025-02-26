using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerialazedDictionary<Tkey, Tvalue>
{
    [SerializeField] private List<Tkey> _keys = new List<Tkey>();
    [SerializeField] private List<Tvalue> _values = new List<Tvalue>();

    private Dictionary<Tkey, Tvalue> _dictionary = new Dictionary<Tkey, Tvalue>();

    public void OnBeforeSerialize()
    {

    }
}
