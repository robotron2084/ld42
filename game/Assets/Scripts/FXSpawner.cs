using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class FXSpawner : MonoBehaviour
{

  public List<NamedTransform> namedTransforms = new List<NamedTransform>();
  public List<FXData> fxData = new List<FXData>();

  public Dictionary<string, FXData> dataMap = new Dictionary<string, FXData>();
  public Dictionary<string, FXUnit> instantiatedPrefabs = new Dictionary<string, FXUnit>();

  public Dictionary<string, NamedTransform> namedTransformsMap = new Dictionary<string, NamedTransform>();

  Managers m;

  void Awake()
  {
    foreach(FXData data in fxData)
    {
      dataMap[data.key] = data;
    }
    foreach(NamedTransform nt in namedTransforms)
    {
      namedTransformsMap[nt.name] = nt;
    }

  }

  void Start()
  {
    m = Managers.Get();
  }

  public FXUnit Spawn(string key)
  {
    if(m == null){
      m = Managers.Get();
    }
    // todo: stop multiple spawns?
    FXData data = getFXData(key);
    if(data == null && this != m.units.baseFX)
    {
      data = m.units.baseFX.getFXData(key);
      if(data != null)
      {
        return spawnInternal(data);
      }
      // FXUnit unit = (FXUnit)Instantiate(data.prefab);
      // unit.Init(data);
      // if(data.persist)
      // {
      //   instantiatedPrefabs[data.key] = unit;
      // }
      // return unit;
    }else{
      //we have data
      return spawnInternal(data);
    }
    Debug.Log("[FXSpawner] Attempted to spawn fx with key:"+key + " but no fx unit found!", gameObject);
    foreach(FXData fx in dataMap.Values)
    {
      Debug.Log("[FXSpawner] we have:" + fx.key);
    }
    return null;
  }

  public FXData getFXData(string key)
  {
    FXData data = null;
    dataMap.TryGetValue(key, out data);
    return data;
  }

  FXUnit spawnInternal(FXData data)
  {
    FXUnit unit = (FXUnit)Instantiate(data.prefab);
    unit.Init(data, namedTransformsMap[data.spawnPoint].t);
    if(data.persist)
    {
      instantiatedPrefabs[data.key] = unit;
    }
    return unit;
  }

  public void Despawn(string key)
  {
    FXUnit unit = null;
    if(instantiatedPrefabs.TryGetValue(key, out unit))
    {
      // Debug.Log("[FXSpawner] despawn!");
      unit.Despawn();
    }
  }

}
