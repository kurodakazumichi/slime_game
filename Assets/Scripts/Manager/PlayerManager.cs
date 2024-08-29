using UnityEditor;
using UnityEngine;

public class PlayerManager : SingletonMonoBehaviour<PlayerManager>
{
  [SerializeField]
  private GameObject playerPrefab;

  private Player player;

  protected override void MyAwake()
  {
    player = Instantiate(playerPrefab).GetComponent<Player>();
  }

  // Update is called once per frame
  void Update()
  {

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

  public void takeDamage(float value)
  {
    if (!PlayerExists) return;

    if (PlayerIsDead) return;

    player.takeDamage(value);
  }

  public Vector3 PlayerPosition {
    get { return player.VisualPosition; }
  }

  public SphereCollider PlayerCollider {
    get { return player.collider; }
  }
}
