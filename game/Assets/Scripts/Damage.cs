public class Damage
{
  public Attack source;

  //calculated
  public float amount;

  public AttackFlavor flavor;

  public Damage(Attack s)
  {
    source = s;
    flavor = s.attackFlavor;
  }
}