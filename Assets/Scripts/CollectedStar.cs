using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectedStar : MonoBehaviour
{
    public RectTransform startPos;
    public RectTransform endPos;

    public StarPanel starPanel;

    RectTransform rect;
    CanvasGroup myCanvas;

    // timings
    float duration = 1.0f;
    float t = 0f;

    float initialDelay;
    float initialDelayT = 0f;

    float delay;
    float delayT = 0f;

    // motion
    float arcHeight;
    float wiggleAmount;

    // variation
    float phaseOffset;
    float localTime;

    Vector3 savedPos;

    bool isActivated = false;
    bool collected = false;

    void Start()
    {
        rect = GetComponent<RectTransform>();
        myCanvas = GetComponent<CanvasGroup>();

        rect.position = startPos.position;
        rect.localScale = Vector3.zero;
    }

    public void ShowStar()
    {
        if (rect == null)
            rect = GetComponent<RectTransform>();

        isActivated = true;
        collected = false;

        // reset timers
        t = 0f;
        initialDelayT = 0f;
        delayT = 0f;
        localTime = 0f;

        // RANDOM VARIATION 🔥
        phaseOffset = Random.Range(0f, 100f);
        initialDelay = Random.Range(0.6f, 0.6f);
        delay = Random.Range(0.6f, 0.9f);
        arcHeight = Random.Range(-120f, 280f);
        wiggleAmount = Random.Range(-100f, 100f);

        // reset transform
        savedPos = startPos.position;
        rect.position = savedPos;
        rect.localScale = Vector3.zero;
    }

    void Update()
    {
        if (!isActivated) return;

        localTime += Time.deltaTime;

        // collect trigger
        if (t >= 0.9f && !collected)
        {
            collected = true;
            starPanel.Shake();
        }

        if (t > 1f)
        {
            isActivated = false;
            return;
        }

        // fade out
        myCanvas.alpha = 1f - t;

        // rotation
        float rotation = Mathf.Lerp(160f, 0f, t);
        rect.rotation = Quaternion.Euler(0f, 0f, rotation);

        // -------- Phase 1: initial delay --------
        if (initialDelayT < initialDelay)
        {
            initialDelayT += Time.deltaTime;
            return;
        }

        // -------- Phase 2: pop --------
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

        // -------- Phase 3: movement --------
        t += Time.deltaTime / duration;

        float easedT = Mathf.SmoothStep(0f, 1f, t);

        Vector3 pos = Vector3.Lerp(savedPos, endPos.position, easedT);

        // arc
        float arc = Mathf.Sin(easedT * Mathf.PI) * arcHeight;

        // wiggle (phase offset applied here 🔥)
        float wiggle =
            Mathf.Sin(easedT * Mathf.PI * 4f + phaseOffset) *
            wiggleAmount *
            Mathf.Cos((localTime + phaseOffset) * 2f);

        rect.position = pos + Vector3.up * (arc + wiggle);

        Debug.DrawLine(savedPos, endPos.position);
    }
}