using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class SOFloat : ScriptableObject
{
    [SerializeField] private float _value;

    public float Value
    {
        get { return _value; }
        set 
        {
            float inc = value - _value;
            _value += inc * LevelManager.Instance.ScoreMultiplier;
        }
    }
}
