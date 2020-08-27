using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Resource")]
public class ResourceScriptableObject : ScriptableObject
{
    public string sourceName;

    [Header("Resource per second")]
    public float hungerGainRate;
    public float thirstGainRate;
    public float sanityGainRate;
}
