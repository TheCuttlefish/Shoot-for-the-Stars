using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectedStar : MonoBehaviour
{
    public RectTransform startPos;
    public RectTransform endPos;

    float duration = 1.0f;
    float t = 1f;

    float arcHeight = 220f;
    float wiggleAmount = 10;

    float initialDelay = 0.5f;
    float initialDelayT = 1f;

    float delay = 0.6f;
    float delayT = 1f;
    CanvasGroup myCanvas;
    RectTransform rect;

    Vector3 savedPos;


    public void ResetStar()
    {
        if (rect == null)
            rect = GetComponent<RectTransform>();

        // reset timers
        initialDelayT = 0f;
        delayT = 0f;
        t = 0f;

        // reset transform
        savedPos = startPos.position;
        rect.position = savedPos;
        rect.localScale = Vector3.zero;
    }



    void Start()
    {
        myCanvas = GetComponent<CanvasGroup>();
        rect = GetComponent<RectTransform>();
        rect.position = startPos.position;
        rect.localScale = Vector3.zero;
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.S)) ResetStar();

        myCanvas.alpha = 1-t;

        float rotation = Mathf.Lerp(160f, 0f, t); // 3 spins
        rect.rotation = Quaternion.Euler(0f, 0f, rotation);
        // Phase 1: initial wait (do nothing)
        if (initialDelayT < initialDelay)
        {
            initialDelayT += Time.deltaTime;
            return;
        }

        // Phase 2: scale pop
        if (delayT < delay)
        {
            delayT += Time.deltaTime;

            float s = Mathf.Clamp01(delayT / delay);
            float easedS = s * s;

            float pop = Mathf.Sin(easedS * Mathf.PI);
            float scale = Mathf.Lerp(0f, 1.5f, pop);
            scale = Mathf.Lerp(scale, 0.5f, easedS);

            rect.localScale = Vector3.one * scale;

            return;
        }

        // Phase 3: movement (unchanged)
        t += Time.deltaTime / duration;

        float easedT = Mathf.SmoothStep(0, 1, t);

        Vector3 pos = Vector3.Lerp(savedPos, endPos.position, easedT);

        float arc = Mathf.Sin(easedT * Mathf.PI) * arcHeight;
        float wiggle = Mathf.Sin(easedT * Mathf.PI * 4) * wiggleAmount * Mathf.Cos(Time.time * 0.5f) * 10;

        rect.position = pos + Vector3.up * arc + Vector3.up * wiggle;

        Debug.DrawLine(savedPos, endPos.position);
    }
}