using UnityEngine;
public static class Util
{
  
  // randomizes the position between -amount and +amount
  public static void RandomizePosition(Transform t, float x, float y)
  {
    Vector3 pos = t.position;
    pos.x += x * ((Random.value * 2.0f) - 1.0f);
    pos.y += y * ((Random.value * 2.0f) - 1.0f);
    t.position = pos;
  }
}