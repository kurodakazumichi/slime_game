using System;
using UnityEditor;
using UnityEngine;
using MyGame.Old;

namespace MyGame.Old
{
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

    public Action<int, float> OnChangePlayerHP {
      set {
        player.OnChangeHP = value;
      }
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

    public void AttackPlayer(AttackInfo info)
    {
      if (!PlayerExists) return;

      if (PlayerIsDead) return;

      player.TakeDamage(info);
    }

    new public Vector3 Position {
      get { return player.Position; }
    }

    public SphereCollider Collider {
      get { return player.Collider; }
    }

    public Player Player {
      get { return player; }
    }
  }


}