using UnityEngine;

public class MapDecoration : MonoBehaviour
{
  private float timer = 0;

  // Start is called once before the first execution of Update after the MonoBehaviour is created
  void Start()
  {

  }

  // Update is called once per frame
  void Update()
  {
    var x = Mathf.Lerp(0, 2f, Mathf.Abs(Mathf.Sin(timer)));
    var y = Mathf.Lerp(-2f, 2, Mathf.Abs(Mathf.Sin(timer*1.5f)));


    transform.rotation = Quaternion.Euler(x, y, 0);

    timer += TimeSystem.DeltaTime;
  }
}
