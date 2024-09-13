using System;
using UnityEngine;

/// <summary>
/// 時間経過を管理するクラス
/// </summary>
public class Timer
{
  //============================================================================
  // Variables
  //============================================================================
  /// <summary>
  /// 制限時間、この時間に0より大きい値がセットされるとTimerは稼働扱いになる。
  /// </summary>
  private float timeLimit = 0f;

  /// <summary>
  /// 経過時間
  /// </summary>
  private float timer = 0f;

  /// <summary>
  /// Timer開始時のコールバック
  /// </summary>
  private Action OnStart = null;

  /// <summary>
  /// タイマー停止時のコールバック
  /// </summary>
  private Action OnStop = null;

  /// <summary>
  /// タイマー更新時のコールバック
  /// </summary>
  private Action<float> OnUpdate = null;

  //============================================================================
  // Properties
  //============================================================================
  /// <summary>
  /// タイマー稼働中
  /// </summary>
  public bool IsRunning {
    get { return 0 < timeLimit; }
  }

  /// <summary>
  /// 経過割合
  /// </summary>
  public float Rate {
    get {
      // 0除算対策
      if (timeLimit <= 0) {
        return 0;
      }

      return Mathf.Clamp(timer / timeLimit, 0, 1f);
    }
  }

  //============================================================================
  // Methods
  //============================================================================

  //----------------------------------------------------------------------------
  // Public
  //----------------------------------------------------------------------------

  /// <summary>
  /// コールバックをセット
  /// </summary>
  /// <param name="onStart"></param>
  /// <param name="onUpdate"></param>
  /// <param name="onStop"></param>
  public void Setup(Action onStart, Action<float> onUpdate = null, Action onStop = null)
  {
    OnStart  = onStart;
    OnUpdate = onUpdate;
    OnStop   = onStop;
  }

  /// <summary>
  /// タイマーを開始する
  /// </summary>
  public void Start(float time)
  {
    this.timeLimit = time;
    timer = 0f;

    OnStart?.Invoke();
  }

  /// <summary>
  /// タイマーを更新する
  /// </summary>
  public void Update(float deltaTime)
  {
    if (!IsRunning) {
      return;
    }

    OnUpdate?.Invoke(Rate);

    if (timeLimit < timer) {
      Stop();
      return;
    }

    timer += deltaTime;
  }

  /// <summary>
  /// タイマーを停止する
  /// </summary>
  public void Stop()
  {
    timeLimit = 0;
    timer     = 0;

    OnStop?.Invoke();
  }
}
