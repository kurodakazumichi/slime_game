using UnityEditor;
using UnityEngine;

public class PlayerManager : SingletonMonoBehaviour<PlayerManager>
{
  [SerializeField]
  private GameObject playerPrefab;

  private Player player;

  protected override void MyAwake()
  {
    base.MyAwake();

    player = Instantiate(playerPrefab).GetComponent<Player>();
  }

  public void RespawnPlayer()
  {
    player.Respawn();
  }

  public void Playable()
  {
    player.SetStateUsual();
  }

  public bool PlayerExists {
    get { return player != null; } 
  }

  public bool PlayerIsDead {
    get { return player.IsDead; }
  }

  public void AttackPlayer(AttackStatus p)
  {
    if (!PlayerExists) return;

    if (PlayerIsDead) return;

    player.TakeDamage(p);
  }

  public Vector3 Position {
    get { return player.Position; }
  }

  public SphereCollider Collider {
    get { return player.collider; }
  }

  public Player Player {
    get { return player; }
  }
}
