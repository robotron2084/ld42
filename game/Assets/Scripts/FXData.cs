using UnityEngine;

[System.Serializable]
public class FXData
{
  public string key;
  public FXUnit prefab;
  public string spawnPoint;
  public SpawnMode spawnMode = SpawnMode.Global;
  public bool persist = false;
}
