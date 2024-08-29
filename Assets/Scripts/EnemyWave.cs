using System.Collections.Generic;
using UnityEngine;

public class EnemyWave
{
  //============================================================================
  // Enum
  //============================================================================

  /// <summary>
  /// ���
  /// </summary>
  private enum State
  {
    Idle,              // �A�C�h��
    ProductionWaiting, // ���Y�ҋ@
    Production,        // �G�𐶎Y
    ProductionEnded,   // ���Y�I��
  }

  //============================================================================
  // Variables
  //============================================================================

  /// <summary>
  /// �X�e�[�g�}�V��
  /// </summary>
  private StateMachine<State> state;

  /// <summary>
  /// Wave�p�����[�^
  /// </summary>
  private EnemyWaveParam waveParam;

  /// <summary>
  /// �ėp�^�C�}�[
  /// </summary>
  private float timer = 0;

  /// <summary>
  /// Wave�����Y����G�̎c�����Ǘ�
  /// </summary>
  private int stock = 0;

  /// <summary>
  /// ���݂�Wave�����Ǘ�
  /// </summary>
  private int currentWaveIndex = 0;

  /// <summary>
  /// �������Ă���G�̐�
  /// </summary>
  private int currentEnemyCount = 0;

  //============================================================================
  // Properities
  //============================================================================

  /// <summary>
  /// Idle��Ԃ͂���Wave���ғ����Ă��炸�A�܂�Wave�����������G�����݂��Ă��Ȃ����Ƃ�ۏ؂���B
  /// </summary>
  public bool IsIdle {
    get { return state.StateKey == State.Idle; }
  }

  /// <summary>
  /// �S�Ă̓G�̐��Y���I��������true��Ԃ�
  /// </summary>
  public bool IsEmpty {
    get { return stock <= 0; }
  }

  /// <summary>
  /// Wave��WaitTime���ݒ肳��Ă����true��Ԃ�
  /// </summary>
  private bool hasWaitTime {
    get { return waveParam != null && 0 < waveParam.WaitTime; }
  }

  /// <summary>
  /// �I����
  /// </summary>
  public bool IsTerminating {
    get; private set;
  }

  //============================================================================
  // Methods
  //============================================================================

  //----------------------------------------------------------------------------
  // Public
  //----------------------------------------------------------------------------

  /// <summary>
  /// �R���X�g���N�^
  /// </summary>
  public EnemyWave()
  {
    Logger.Log("[EnemyWave] New EnemyWave.");
    state = new StateMachine<State>();
    state.Add(State.Idle, EnterIdle);
    state.Add(State.ProductionWaiting, EnterProductionWaiting, UpdateProductionWaiting);
    state.Add(State.Production, EnterProduction, UpdateProduction);
    state.Add(State.ProductionEnded, null, UpdateProductionEnded);

    Logger.Log("[EnemyWave] StateSet: --> Idle.");
    state.SetState(State.Idle);
  }

  /// <summary>
  /// ������
  /// </summary>
  public void Init(EnemyWaveParam param)
  {
    if (!IsIdle) {
      Logger.Error("[EnemyWave] Init() can only be called in the Idle state.");
      return;
    }
    
    waveParam         = param;
    stock             = 0;
    currentWaveIndex  = 0;
    currentEnemyCount = 0;
    timer             = 0;
  }

  /// <summary>
  /// Wave�����s����
  /// </summary>
  public void Run()
  {
    if (waveParam == null) {
      Logger.Error("[EnemyWave] WaveParam is null. Call the Init method in advance.");
      return;
    }

    if (!IsIdle) {
      Logger.Error("[EnemyWave] Run() can only be called in the Idle state.");
      return;
    }

    if (hasWaitTime) {
      state.SetState(State.ProductionWaiting);
      Logger.Log("[EnemyWave] StateChange Idle --> ProductionWaiting.");
    } else {
      state.SetState(State.Production);
      Logger.Log("[EnemyWave] StateChange Idle --> Production.");
    }
  }

  /// <summary>
  /// Wave�o�R�œG���������
  /// </summary>
  public void Release(Enemy enemy)
  {
    currentEnemyCount--;
    EnemyManager.Instance.Release(enemy);
  }

  /// <summary>
  /// Wave���I������
  /// </summary>
  public void Terminate()
  {
    Logger.Log("[EnemyWave] Terminate is called.");

    // Idle��Ԃł���Γ��ɂ��邱�Ƃ͂Ȃ�
    if (IsIdle) {
      return;
    }

    IsTerminating = true;
  }



  //----------------------------------------------------------------------------
  // for Update
  //----------------------------------------------------------------------------

  public void Update()
  {
    state.Update();
  }

  //----------------------------------------------------------------------------
  // for �A�C�h��
  private void EnterIdle()
  {
    timer = 0;
    stock = 0;
    currentEnemyCount = 0;
    currentWaveIndex  = 0;
    IsTerminating     = false;
  }

  //----------------------------------------------------------------------------
  // for �G�����ҋ@���

  private void EnterProductionWaiting()
  {
    timer = 0;
  }

  private void UpdateProductionWaiting()
  {
    // �I���t���O�������Ă�����Idle�֖߂�
    if (IsTerminating) {
      state.SetState(State.Idle);
      Logger.Log("[EnemyWave] StateChange ProductionWaiting --> Idle.");
      return;
    }

    timer += TimeSystem.Wave.DeltaTime;

    // �ҋ@���Ԃ��o�߂�����G������Ԃ֑J��
    if (waveParam.WaitTime <= timer) 
    {
      state.SetState(State.Production);
      Logger.Log("[EnemyWave] StateChange ProductionWaiting --> Production.");
      return;
    }
  }

  //----------------------------------------------------------------------------
  // for �G�������

  private void EnterProduction()
  {
    currentEnemyCount = 0;
    currentWaveIndex  = 0;
    timer             = 0;
    stock             = waveParam.TotalEnemyCount;
  }

  private void UpdateProduction()
  {
    // �I���t���O�������Ă�����X�g�b�N��0�ɂ���A�X�g�b�N��0�ɂ���ΓG�͐��Y����Ȃ��B
    if (IsTerminating) {
      stock = 0;
    }

    // �X�g�b�N���Ȃ��Ȃ����琶�Y�I����Ԃ֑J��
    if (stock <= 0) 
    {
      state.SetState(State.ProductionEnded);
      Logger.Log("[EnemyWave] StateChange Production --> ProductionEnded.");
      return;
    }

    // ���Ԃ��o�߂�����timer��0�ȉ��ɂȂ�̂�҂�
    timer -= TimeSystem.Wave.DeltaTime;

    if (0 < timer){
      return;
    }

    // Wave�ݒ�Ɋ�Â��ēG�����
    Logger.Log($"[EnemyWave] Make wave[{currentWaveIndex}] enemies.");

    switch (waveParam.Shape) {
      case WaveShape.Circle : MakeWaveEnemiesCircle(); break;
      case WaveShape.Line   : MakeWaveEnemiesLine();   break;
      case WaveShape.Random : MakeWaveEnemiesRandom(); break;
      default               : MakeWaveEnemiesPoint();  break;
    }

    currentWaveIndex++;
    timer = waveParam.WaveInterval;
  }

  /// <summary>
  /// ��_����G�𔭐�������B
  /// </summary>
  private void MakeWaveEnemiesPoint()
  {
    int   max     = waveParam.EnemyAmountPerWave;
    float offsetX = waveParam.WaveOffsetX;
    float offsetY = waveParam.WaveOffsetZ;

    for(int i = 0; i < max; ++i) 
    {
      var enemy = GetEnemy();

      var p  = waveParam.BasePosition;
          p += new Vector3(offsetX, offsetY, 0);

      enemy.transform.position = p;

      stock--;
      currentEnemyCount++;
    }
  }

  /// <summary>
  /// �~��ɓG�𔭐�������B
  /// </summary>
  private void MakeWaveEnemiesCircle()
  {
    int     max         = waveParam.EnemyAmountPerWave;
    float   originAngle = waveParam.OriginAngle;
    float   offsetAngle = waveParam.WaveOffsetAngle;
    Vector3 area        = waveParam.Area;

    for (int i = 0; i < max; ++i) 
    {
      
      var offset  = MyMath.Deg2Rad(originAngle);
          offset += MyMath.Deg2Rad(offsetAngle * currentWaveIndex);

      var radian  = MyMath.Rate2Rad((float)i /  max);
          radian += offset;

      var x = area.x * 0.5f * Mathf.Cos(radian);
      var z = area.y * 0.5f * Mathf.Sin(radian);

      var enemy = GetEnemy();
      enemy.transform.position = waveParam.BasePosition + new Vector3(x, 0, z);

      stock--;
      currentEnemyCount++;
    }
  }

  /// <summary>
  /// ����ɓG�𔭐�������B
  /// </summary>
  public void MakeWaveEnemiesLine()
  {
    int     max         = waveParam.EnemyAmountPerWave;
    float   offsetX = waveParam.WaveOffsetX;
    float   offsetY = waveParam.WaveOffsetZ;
    Vector3 area        = waveParam.Area / 2f;

    for (int i = 0; i < max; ++i) 
    {
      var x  = Mathf.Lerp(-area.x, area.x, i / Mathf.Max(max-1, 1f));
          x += offsetX * currentWaveIndex;
      var z  = Mathf.Lerp(-area.y, area.y, i / Mathf.Max(max-1, 1f));
          z += offsetY * currentWaveIndex;

      if (waveParam.InverseX == true) { x *= -1; }

      if (waveParam.InverseZ == true) { z *= -1; }

      var enemy = GetEnemy();
      enemy.transform.position = waveParam.BasePosition + new Vector3(x, 0, z);

      stock--;
      currentEnemyCount++;
    }
  }

  /// <summary>
  /// �w��͈͓��̃����_���ʒu�ɓG�𔭐�������B
  /// </summary>
  private void MakeWaveEnemiesRandom()
  {
    int max = waveParam.EnemyAmountPerWave;

    for (int i = 0; i < max; ++i) 
    {
      var enemy = GetEnemy();

      var p = waveParam.BasePosition;
          p += MyVector3.Random(waveParam.Area / 2f);

      enemy.transform.position = p;

      stock--;
      currentEnemyCount++;
    }
  }

  //----------------------------------------------------------------------------
  // for �G�����I�����

  private void UpdateProductionEnded()
  {
    if (currentEnemyCount <= 0) {
      state.SetState(State.Idle);
      Logger.Log("[EnemyWave] StateChange ProductionEnded --> Idle.");
      return;
    }
  }

  //----------------------------------------------------------------------------
  // for me
  //----------------------------------------------------------------------------
  private Enemy GetEnemy()
  {
    // �G�𐶐�
    var enemy = EnemyManager.Instance.Get(waveParam.Id);
    enemy.Init(waveParam.Id);
    enemy.SetBelongsTo(this);

    return enemy;
  }
}
