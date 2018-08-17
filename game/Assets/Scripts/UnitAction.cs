
[System.Flags]
public enum UnitAction
{
  Walking = 1 << 0,
  Attacking = 1 << 1,
  Patrolling = 1 << 2,
  Chasing = 1 << 3,

}