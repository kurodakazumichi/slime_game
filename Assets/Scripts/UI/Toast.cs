using UnityEngine;
using UnityEngine.UI;

public class Toast : MyMonoBehaviour
{
  //============================================================================
  // Enum
  //============================================================================
  private enum State
  {
    Idle,
    FadeIn,
    Indicated,
    FadeOut,
  }

  //============================================================================
  // Inspector Variables
  //============================================================================
  [SerializeField]
  private Text uiText;

  [SerializeField]
  private float fadeInTime = 1.0f;

  [SerializeField]
  private float indicatedTime = 1.0f;

  [SerializeField]
  private float fadeOutTime = 1.0f;

  //============================================================================
  // Variables
  //============================================================================

  /// <summary>
  /// 開始位置
  /// </summary>
  private Vector2 startPosition = Vector3.zero;

  /// <summary>
  /// 目標位置
  /// </summary>
  private Vector2 targetPosition = Vector3.zero;

  /// <summary>
  /// 汎用タイマー
  /// </summary>
  private float timer = 0;

  /// <summary>
  /// テキスト色
  /// </summary>
  private Color textColor;

  /// <summary>
  /// ステートマシン
  /// </summary>
  private readonly StateMachine<State> state = new();

  //============================================================================
  // Properities
  //============================================================================

  /// <summary>
  /// アイドル状態
  /// </summary>
  public bool IsIdle => state.StateKey == State.Idle;

  //============================================================================
  // Methods
  //============================================================================

  //----------------------------------------------------------------------------
  // Public
  //----------------------------------------------------------------------------
  public void Show(string msg, Vector2 start, Vector2 target)
  {
    uiText.text      = msg;
    startPosition  = start;
    targetPosition = target;

    state.SetState(State.FadeIn);
  }

  //----------------------------------------------------------------------------
  // Life Cycle
  //----------------------------------------------------------------------------

  protected override void MyAwake()
  {
    textColor = uiText.color;
    state.Add(State.Idle, EnterIdle);
    state.Add(State.FadeIn, EnterFadeIn, UpdateFadeIn);
    state.Add(State.Indicated, EnterIndicated, UpdateIndicated);
    state.Add(State.FadeOut, EnterFadeOut, UpdateFadeOut);
    state.SetState(State.Idle);
  }

  void Update()
  {
    state.Update(); 
  }

  //----------------------------------------------------------------------------
  // State
  //----------------------------------------------------------------------------

  //----------------------------------------------------------------------------
  // State Idle

  private void EnterIdle()
  {
    SetActive(false);
  }

  //----------------------------------------------------------------------------
  // State FadeIn
  private void EnterFadeIn()
  {
    timer = 0;
    textColor.a = 0;
    CachedRectTransform.anchoredPosition = startPosition;
    SetActive(true);
  }

  private void UpdateFadeIn()
  {
    var rate = timer / fadeInTime;
    rate = Mathf.Pow(rate, 0.3f);

    textColor.a = Mathf.Lerp(0f, 1f, rate);
    uiText.color = textColor;

    CachedRectTransform.anchoredPosition 
      = Vector3.Lerp(startPosition, targetPosition, rate);

    if (1f <= timer) {
      state.SetState(State.Indicated);
    }

    timer += TimeSystem.UI.DeltaTime;
  }

  //----------------------------------------------------------------------------
  // State Indicated
  private void EnterIndicated()
  {
    timer = 0;
  }

  private void UpdateIndicated()
  {
    if (indicatedTime < timer) {
      state.SetState(State.FadeOut);
    }

    timer += TimeSystem.UI.DeltaTime;
  }

  //----------------------------------------------------------------------------
  // State Indicated
  private void EnterFadeOut()
  {
    timer = 0;
    textColor.a = 1f;
    uiText.color = textColor;
  }

  private void UpdateFadeOut()
  {
    textColor.a = Mathf.Lerp(1f, 0f, timer/fadeOutTime);
    uiText.color = textColor;

    if (fadeOutTime < timer) {
      state.SetState(State.Idle);
    }

    timer += TimeSystem.UI.DeltaTime;
  }
}
