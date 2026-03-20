using UnityEngine;
using UnityEngine.UI;

public class FPScounter : MonoBehaviour
{
    public Text fpsText;

    float timer;
    int frames;

    void Update()
    {
        frames++;
        timer += Time.unscaledDeltaTime;   // Don't scale with time scale!

        if (timer >= 1f)
        {
            int fps = Mathf.RoundToInt(frames / timer);
            fpsText.text = " FPS: " +fps;

            frames = 0;
            timer = 0;
        }
    }
}
