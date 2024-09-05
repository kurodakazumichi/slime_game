using UnityEngine;
using UnityEditor;

public class EnemyWaveConfig : MyMonoBehaviour
{
  public EnemyWaveProperty props = new EnemyWaveProperty();

  protected override void MyAwake()
  {
    props.Area         = CachedTransform.localScale;
    props.BasePosition = CachedTransform.position;
  }

  private void OnDrawGizmosSelected()
  {
    Gizmos.color = new Color(1, 0, 0, 0.5f);
    Gizmos.DrawCube(transform.position, transform.localScale);
  }
}