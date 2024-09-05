using System.Collections.Generic;
using UnityEngine;

public class EnemyWave
{
  //============================================================================
  // Enum
  //============================================================================

  /// <summary>
  /// 状態
  /// </summary>
  private enum State
  {
    Idle,              // アイドル
    ProductionWaiting, // 生産待機
    Production,        // 敵を生産
    ProductionEnded,   // 生産終了
  }

  //============================================================================
  // Variables
  //============================================================================

  /// <summary>
  /// ステートマシン
  /// </summary>
  private StateMachine<State> state;

  /// <summary>
  /// Waveパラメータ
  /// </summary>
  private EnemyWaveProperty waveParam;

  /// <summary>
  /// 汎用タイマー
  /// </summary>
  private float timer = 0;

  /// <summary>
  /// Waveが生産する敵の残数を管理
  /// </summary>
  private int stock = 0;

  /// <summary>
  /// 現在のWave数を管理
  /// </summary>
  private int currentWaveIndex = 0;

  /// <summary>
  /// 現存している敵の数
  /// </summary>
  private int currentEnemyCount = 0;

  //============================================================================
  // Properities
  //============================================================================

  /// <summary>
  /// Idle状態はこのWaveが稼働しておらず、またWaveが生成した敵が存在していないことを保証する。
  /// </summary>
  public bool IsIdle {
    get { return state.StateKey == State.Idle; }
  }

  /// <summary>
  /// 全ての敵の生産が終了したらtrueを返す
  /// </summary>
  public bool IsEmpty {
    get { return stock <= 0; }
  }

  /// <summary>
  /// WaveにWaitTimeが設定されていればtrueを返す
  /// </summary>
  private bool hasWaitTime {
    get { return waveParam != null && 0 < waveParam.WaitTime; }
  }

  /// <summary>
  /// 終了中
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
  /// コンストラクタ
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
  /// 初期化
  /// </summary>
  public void Init(EnemyWaveProperty param)
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
  /// Waveを実行する
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
  /// Wave経由で敵を解放する
  /// </summary>
  public void Release(IEnemy enemy)
  {
    currentEnemyCount--;
    EnemyManager.Instance.Release(enemy);
  }

  /// <summary>
  /// Waveを終了する
  /// </summary>
  public void Terminate()
  {
    Logger.Log("[EnemyWave] Terminate is called.");

    // Idle状態であれば特にすることはない
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
  // for アイドル
  private void EnterIdle()
  {
    timer = 0;
    stock = 0;
    currentEnemyCount = 0;
    currentWaveIndex  = 0;
    IsTerminating     = false;
  }

  //----------------------------------------------------------------------------
  // for 敵生成待機状態

  private void EnterProductionWaiting()
  {
    timer = 0;
  }

  private void UpdateProductionWaiting()
  {
    // 終了フラグが立っていたらIdleへ戻る
    if (IsTerminating) {
      state.SetState(State.Idle);
      Logger.Log("[EnemyWave] StateChange ProductionWaiting --> Idle.");
      return;
    }

    timer += TimeSystem.Wave.DeltaTime;

    // 待機時間が経過したら敵生成状態へ遷移
    if (waveParam.WaitTime <= timer) 
    {
      state.SetState(State.Production);
      Logger.Log("[EnemyWave] StateChange ProductionWaiting --> Production.");
      return;
    }
  }

  //----------------------------------------------------------------------------
  // for 敵生成状態

  private void EnterProduction()
  {
    currentEnemyCount = 0;
    currentWaveIndex  = 0;
    timer             = 0;
    stock             = waveParam.TotalEnemyCount;
  }

  private void UpdateProduction()
  {
    // 終了フラグがたっていたらストックを0にする、ストックを0にすれば敵は生産されない。
    if (IsTerminating) {
      stock = 0;
    }

    // ストックがなくなったら生産終了状態へ遷移
    if (stock <= 0) 
    {
      state.SetState(State.ProductionEnded);
      Logger.Log("[EnemyWave] StateChange Production --> ProductionEnded.");
      return;
    }

    // 時間を経過させてtimerが0以下になるのを待つ
    timer -= TimeSystem.Wave.DeltaTime;

    if (0 < timer){
      return;
    }

    // Wave設定に基づいて敵を作る
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
  /// 定点から敵を発生させる。
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

      enemy.CachedTransform.position = p;

      stock--;
      currentEnemyCount++;
    }
  }

  /// <summary>
  /// 円上に敵を発生させる。
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
      var z = area.z * 0.5f * Mathf.Sin(radian);

      var enemy = GetEnemy();
      enemy.CachedTransform.position = waveParam.BasePosition + new Vector3(x, 0, z);

      stock--;
      currentEnemyCount++;
    }
  }

  /// <summary>
  /// 線上に敵を発生させる。
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
      enemy.CachedTransform.position = waveParam.BasePosition + new Vector3(x, 0, z);

      stock--;
      currentEnemyCount++;
    }
  }

  /// <summary>
  /// 指定範囲内のランダム位置に敵を発生させる。
  /// </summary>
  private void MakeWaveEnemiesRandom()
  {
    int max = waveParam.EnemyAmountPerWave;

    for (int i = 0; i < max; ++i) 
    {
      var enemy = GetEnemy();

      var p = waveParam.BasePosition;
          p += MyVector3.Random(waveParam.Area / 2f);

      enemy.CachedTransform.position = p;

      stock--;
      currentEnemyCount++;
    }
  }

  //----------------------------------------------------------------------------
  // for 敵生成終了状態

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
  private IEnemy GetEnemy()
  {
    // 敵を生成
    var enemy = EnemyManager.Instance.Get(waveParam.Id);
    enemy.SetOwnerWave(this);
    enemy.Run();
    return enemy;
  }
}
