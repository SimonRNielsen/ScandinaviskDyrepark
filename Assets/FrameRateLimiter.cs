using UnityEngine;

public class FrameRateLimiter : MonoBehaviour
{
    public int fps_limit = 60;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        QualitySettings.vSyncCount = 0;

        Application.targetFrameRate = fps_limit;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
