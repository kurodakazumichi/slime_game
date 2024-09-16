using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if _DEBUG

namespace SkillTester
{
  public class SkillTestScene : MonoBehaviour
  {
    private enum State
    {
      Boot,
      Playable,
    }

    public int TargetFrameRate = 60;
    public string StartSkillId;
    public GameObject EnemyObject;
    public GameObject PlayerObject;
    public Image uiIcon;

    private IActor enemy;
    private IActor player;

    private List<SkillId> skillIds = new List<SkillId>();
    private int currentIndex = 0;

    private StateMachine<State> stateMachine = new();


    private void Awake()
    {
      MyEnum.ForEach<SkillId>((id) => {
        skillIds.Add(id);
      });

      for (int i = 0; i < skillIds.Count; i++) {
        if (MyEnum.Parse<SkillId>(StartSkillId) == skillIds[i]) {
          currentIndex = i;
          break;
        }
      }

      stateMachine.Add(State.Boot, EnterBoot, UpdateBoot, ExitBoot);
      stateMachine.Add(State.Playable, null, UpdatePlayable);

      stateMachine.SetState(State.Boot);
    }

    // Update is called once per frame
    void Update()
    {
      stateMachine.Update();
    }

    private void LateUpdate()
    {
      if (stateMachine.StateKey is not State.Playable) {
        return;
      }

      var colliders = Physics.OverlapSphere(
        Vector3.zero, 
        10f, 
        LayerMask.GetMask(LayerName.PlayerBullet)
      );

      foreach (var collider in colliders)
      {
        collider.GetComponent<IBullet>().Intersect();
      }
    }

    private void OnGUI()
    {
      if (stateMachine.StateKey is not State.Playable) {
        return;
      }

      GUILayout.Label($"FPS  : {TargetFrameRate}");
      GUILayout.Label($"Space: Fire Bullet ({skillIds[currentIndex].ToString()})");
      GUILayout.Label("ZX    : Change Bullet");
      GUILayout.Label("V     : Bullet Terminate");
      GUILayout.Label("C     : Object Pool Clear");
    }


    private void EnterBoot()
    {
      DebugManager.Instance.Regist(SkillManager.Instance);
      DebugManager.Instance.Regist(BulletManager.Instance);
      DebugManager.Instance.Regist(ResourceManager.Instance);

      BulletManager.Instance.Load();
      IconManager.Instance.Load();

      enemy = EnemyObject.GetComponent<IActor>();
      player = PlayerObject.GetComponent<IActor>();
    }

    private void UpdateBoot()
    {
      if (ResourceManager.Instance.IsLoading) {
        return;
      }

      stateMachine.SetState(State.Playable);
    }

    private void ExitBoot()
    {
      UpdateSkillIcon();
    }

    private void UpdatePlayable()
    {
      Application.targetFrameRate = TargetFrameRate;



      if (Input.GetKeyDown(KeyCode.V)) {
        BulletManager.Instance.Terminate();
      }

      if (Input.GetKeyDown(KeyCode.C)) {
        BulletManager.Instance.Clear();
      }

      if (Input.GetKeyDown(KeyCode.Z)) {
        currentIndex = (skillIds.Count + currentIndex - 1) % skillIds.Count;
        UpdateSkillIcon();
      }
      if (Input.GetKeyDown(KeyCode.X)) {
        currentIndex = (currentIndex + 1) % skillIds.Count;
        UpdateSkillIcon();
      }

      if (!Input.GetKeyDown(KeyCode.Space)) {
        return;
      }

      var id = skillIds[currentIndex];

      if (id == SkillId.Undefined) {
        return;
      }

      SkillManager.Instance.SetActiveSkill(0, id);

      Skill skill = SkillManager.Instance.GetActiveSkill(0) as Skill;

      switch (skill.Aiming) {
        case SkillAimingType.None: skill.ManualFire(player.Position, null); break;
        case SkillAimingType.Player: skill.ManualFire(player.Position, player); break;
        default: skill.ManualFire(player.Position, enemy); break;
      }
    }

    private void UpdateSkillIcon()
    {
      var id = skillIds[currentIndex];

      if (id != SkillId.Undefined) {
        uiIcon.sprite = IconManager.Instance.Skill(id);
      }
    }
  }
}
#endif