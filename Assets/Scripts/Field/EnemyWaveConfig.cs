using UnityEngine;
using UnityEditor;

public class EnemyWaveConfig : MonoBehaviour
{
  public EnemyWaveProperty props = new EnemyWaveProperty();

  private void OnDrawGizmosSelected()
  {
    Gizmos.color = new Color(1, 0, 0, 0.5f);
    Gizmos.DrawCube(transform.position, transform.localScale);
  }
}