using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinPoint : MonoBehaviour
{

    public Material outerMat;
    public Material innerMat;
    Color original = (Color)new Color32(255, 246, 144, 111);
    Color originalInner = (Color)new Color32(255, 246, 144, 58);
    [Range(0f, 1f)]
    public float alphaControl = 0.0f;
    bool isVisible = true;

    float timer;

    public bool Visible()
    {
        return isVisible;
    }

    private void Awake()
    {
        SetAlpha(1);
    }

    public void Hide()
    {
        SetAlpha(0);
        isVisible = false;
        timer = 0f;
    }

    public void Show()
    {
        Hide();
        SetAlpha(1);
        isVisible = true;
    }

    float winTimer = 0.0f;

    public void Win()
    {
        winTimer = 1f;
    }


    Vector2 outerOffset;
    Vector2 innerOffset;

    float outerSpeed;
    float innerSpeed;

    void Update()
    {

        if (winTimer > 0)
        {
            winTimer -= Time.deltaTime;

            SetAlpha(winTimer);

        }
        

        // appear animation
    
        if (isVisible)
        {
            if (timer < 1)
            {
                timer += Time.deltaTime;
                SetAlpha(timer);


                float t = Mathf.Clamp01(timer);

                // optional smoothing (recommended)
                float eased = Mathf.SmoothStep(0f, 1f, t);

                SetAlpha(eased);




                // speeds (same idea, but now just values)
                outerSpeed = Mathf.Lerp(1.6f, -(0.6f ), eased);
                innerSpeed = Mathf.Lerp(1.6f, -(1f ), eased);


            }
        }


                // accumulate movement over time
                outerOffset.y += (outerSpeed - winTimer) * Time.deltaTime;
                innerOffset.y += (innerSpeed - winTimer) *Time.deltaTime ;

                // send offset (NOT speed anymore)
                outerMat.SetVector("_ScrollOffset", outerOffset);
                innerMat.SetVector("_ScrollOffset", innerOffset);
        
    }


    void SetAlpha(float _alpha = 0)
    {

        float winBoost = 1f + winTimer * 3; // winTimer 

        Color c = original;
        c.a = original.a * _alpha * winBoost;
        outerMat.color = c;

        Color c2 = originalInner;
        c2.a = originalInner.a * _alpha * winBoost;
        innerMat.color = c2;
    }



}
