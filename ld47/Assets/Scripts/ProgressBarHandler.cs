using UnityEngine;
public class ProgressBarHandler : MonoBehaviour
{  

    public LoopHandler loopHandler;
    public AnimatedProgressbar bar;

    private float cachedSpeed = 0;

    private void Start() {
        cachedSpeed = bar.m_speed;
    }

    private int MinTimeSpan
    {
        get
        {
            return loopHandler.minReplayTimespan * (loopHandler.ReplayCount + 1); 
        }
    }

    private void Update() 
    {
        var delta = Time.time - loopHandler.LastTime;
        var progress = 1 - (MinTimeSpan - delta) / MinTimeSpan;
        progress = Mathf.Clamp(progress, 0, 1);
        bar.FillAmount = progress;
        if(progress == 1)
            bar.m_speed = 0;
        else
            bar.m_speed = cachedSpeed;
    }
}