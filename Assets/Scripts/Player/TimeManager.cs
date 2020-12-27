using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private static TimeManager _instance;

    public static TimeManager Instance
    {
        get
        {
            return _instance;
        }
    }

    public float slowTime;
    public float timeToBack = 2f;

    public float timeBack = 0f;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    private void Update()
    {
        if (Time.unscaledTime < timeBack)
            return;

        Time.timeScale += (1f / slowTime) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
    }

    public void SlowDown(float factor, float time)
    {
        slowTime = time;
        Time.timeScale = factor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        timeBack = Time.unscaledTime + timeToBack;
    }
}
