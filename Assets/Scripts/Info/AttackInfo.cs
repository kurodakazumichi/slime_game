/// <summary>
/// 攻撃情報
/// </summary>
public class AttackInfo
{

  private float power = 0f;
  private Flag32 attributes = new Flag32();
  private float impact = 0f;

  public void Init(float power, uint attributes, float impact)
  {
    this.power            = power;
    this.attributes.Value = attributes;
    this.impact           = impact;
  }

  public float Impact => impact;

  public uint Attributes {
    get { return attributes.Value; }
  }

  public float Power 
  {
    get {
      if (attributes.Is((uint)Attribute.Non)) {
        return power;
      } else {
        return power * 1.2f;
      }
    }
  }
}
