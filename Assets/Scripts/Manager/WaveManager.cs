using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WaveManager : SingletonMonoBehaviour<WaveManager>
{
  //============================================================================
  // Enum
  //============================================================================

  /// <summary>
  /// 状態
  /// </summary>
  public enum State
  {
    Idle,
    Running,
  }

  //============================================================================
  // Variables
  //============================================================================

  /// <summary>
  /// ステートマシン
  /// </summary>
  private StateMachine<State> state;

  /// <summary>
  /// EnemyWaveプロパティ一式
  /// </summary>
  private Dictionary<int, List<EnemyWaveProperty>> enemyWavePropertySet = null;

  /// <summary>
  /// Waveデータ
  /// </summary>
  private Dictionary<int, List<EnemyWave>> waveData = null;

  /// <summary>
  /// 現在のWaveを表すIndex
  /// </summary>
  private int currentWaveIndex = 0;

  /// <summary>
  /// 現在実行中のWaveリスト
  /// </summary>
  private List<EnemyWave> currentWaves;

  /// <summary>
  /// 終了フラグ
  /// </summary>
  private bool isTerminating = false;

  //============================================================================
  // Properities
  //============================================================================
  /// <summary>
  /// WaveManagerがIdleならばtrue
  /// </summary>
  public bool IsIdle {
    get { return state.StateKey == State.Idle; }
  }

  /// <summary>
  /// 登録されている全てのWaveがIdle状態ならばtrue
  /// </summary>
  private bool AllWavesAreIdle {
    get 
    {
      foreach (var waves in waveData) {
        foreach (var wave in waves.Value) {
          if (!wave.IsIdle) {
            return false;
          }
        }
      }

      return true;
    }
  }

  /// <summary>
  /// 現在設定されているwavesが全てIdle状態ならばtrue
  /// </summary>
  private bool CurrentWavesAreIdle {
    get {
      if (currentWaves == null) return false;

      foreach (var wave in currentWaves) {
        if (!wave.IsIdle) return false;
      }

      return true;
    }
  }

  /// <summary>
  /// 次のWaveがあるかどうか
  /// 終了要請が出ていない、かつwaveDataが設定されているならばtrue
  /// </summary>
  private bool HasNextWave {
    get {
      return isTerminating == false && waveData.ContainsKey(currentWaveIndex + 1);
    }
  }

  //============================================================================
  // Methods
  //============================================================================

  //----------------------------------------------------------------------------
  // Public
  //----------------------------------------------------------------------------

  /// <summary>
  /// WaveManagerをリセットする。
  /// Idle状態で呼ばないとエラーになる。
  /// </summary>
  public void Reset()
  {
    if (!IsIdle) {
      Logger.Error("[WaveManager] Reset can only called in state of Idle.");
      return;
    }

    enemyWavePropertySet = null;
    waveData = null;
    currentWaves = null;
    currentWaveIndex = 0;
  }

  /// <summary>
  /// 敵Waveのプロパティ群を設定する
  /// </summary>
  public void SetEnemyWavePropertySet(Dictionary<int, List<EnemyWaveProperty>> propertySet)
  {
    // SetEnemyWavePropertySet()はManagerがIdle状態でのみ呼び出すことができる
    if (!IsIdle) {
      Logger.Error("[WaveManager] SetEnemyWavePropertySet can only called in state of Idle.");
      return;
    }

    waveData = new Dictionary<int, List<EnemyWave>>();

    enemyWavePropertySet = propertySet;

    foreach (var pair in enemyWavePropertySet)
    {
      foreach (var property in pair.Value)
      {
        var wave = new EnemyWave();
        wave.Init(property);
        Add(pair.Key, wave);
      }
    }
  }

  /// <summary>
  /// Waveを登録する
  /// </summary>
  public void Add(int no, EnemyWave wave)
  {
    if (!waveData.TryGetValue(no, out var waveList)) {
      waveData.Add(no, waveList = new List<EnemyWave>() { wave });
    }
    else {
      waveList.Add(wave);
    }
  }

  /// <summary>
  /// 登録されているWaveDataに基づいて、Waveを実行する。
  /// </summary>
  public void Run()
  {
    // Run()はManagerがIdle状態でのみ呼び出すことができる
    if (!IsIdle) {
      Logger.Error("[WaveManager] Run can only called in state of Idle.");
      return;
    }

#if _DEBUG
    // Run()実行時、全てのWaveはIdle状態でなければならない
    if (!AllWavesAreIdle) {
      Logger.Error("[WaveManager] All WaveData must be state with Idle when called Run method.");
      return;
    }
#endif

    // Waveデータが登録されていない
    if (waveData == null) {
      Logger.Warn("[WaveManager] The Run method is called, but WaveData aren't regist.");
      return;
    }

    state.SetState(State.Running);
  }

  /// <summary>
  /// Wave終了要請
  /// </summary>
  public void Terminate()
  {
    // Idle状態であれば何もしない
    if (IsIdle) {
      return;
    }

    // 終了フラグを立てる
    isTerminating = true;

    // 現在のWaveを終了
    if (currentWaves != null) {
      TerminateWaves(currentWaves);
    }
  }

  //----------------------------------------------------------------------------
  // Life Cycle
  //----------------------------------------------------------------------------
  protected override void MyAwake()
  {
    base.MyAwake();

    state = new StateMachine<State>();
    state.Add(State.Idle);
    state.Add(State.Running, EnterRunning, UpdateRunning);
    state.SetState(State.Idle);
  }

  // Update is called once per frame
  void Update()
  {
    state.Update();
  }

  //----------------------------------------------------------------------------
  // for Update
  //----------------------------------------------------------------------------

  //----------------------------------------------------------------------------
  // for Idle
  private void EnterIdle()
  {
    currentWaveIndex = 0;
    currentWaves     = null;
    isTerminating    = false;
  }

  //----------------------------------------------------------------------------
  // for Running

  private void EnterRunning()
  {
    currentWaveIndex = 0;

    if (!waveData.TryGetValue(currentWaveIndex, out currentWaves)) {
      Logger.Error($"[WaveManager] WavesData[{currentWaveIndex}] is nothing.");
      return;
    }

    FireWaves(currentWaves);
  }

  private void UpdateRunning()
  {
    foreach (var wave in currentWaves) {
      wave.Update();
    }

    // 現在のWaveがIdle
    if (CurrentWavesAreIdle) 
    {
      // 次のWaveがある場合は次のWaveへ
      if (HasNextWave) {
        currentWaveIndex++;
        currentWaves = waveData[currentWaveIndex];
        FireWaves(currentWaves);
      }

      // 最終WaveだったならばIdle状態へ遷移
      else {
        state.SetState(State.Idle);
        Debug.Log("[WaveManager] ChangeState: Running --> Idle.");
        return;
      }
    }
  }

  //----------------------------------------------------------------------------
  // For me
  //----------------------------------------------------------------------------

  /// <summary>
  /// 指定されたWaveを全て実行する
  /// </summary>
  private void FireWaves(List<EnemyWave> waves)
  {
    foreach (var wave in waves) {
      wave.Run();
    }
  }

  /// <summary>
  /// 指定されたWaveを全て終了する
  /// </summary>
  private void TerminateWaves(List<EnemyWave> waves)
  {
    foreach (var wave in waves) {
      wave.Terminate();
    }
  }

  //private void OnGUI()
  //{
  //  using (new EditorGUILayout.HorizontalScope()) {
  //    GUILayout.Label($"Manager State = {state.StateKey.ToString()}");
  //  }


  //  using (new EditorGUILayout.HorizontalScope()) 
  //  {
  //    if (GUILayout.Button("Run")) {
  //      Run();
  //    }

  //    if (GUILayout.Button("Terminate")) {
  //      Terminate();
  //    }
  //  }  
  //}
}
