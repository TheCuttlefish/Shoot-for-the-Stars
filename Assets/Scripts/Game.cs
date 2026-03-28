using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Game : MonoBehaviour
{


    [Header("Main UI")]
    public CanvasGroup startButton;
    public Text title;
    public RectTransform bg;
    public SeedPanelUI seedPanel;

    [Header("Animations")]
    public HintUI hint;

    [Header("Score UI")]
    public ScoreList scoreList;

    [Header("Seed UI")]
    public SeedUI seedUI;

    [Header("Timer UI")]
    public Slider timeOutSlider;
    public TimerUI timerSlider;

    [Header("Colours")]
    public Color defaultColour;
    public Color LoseColour;
    public Color WinColour;
    public Color CrownColour;

    [Header("Effects")]
    public ParticleSystem bigWin;
    public ParticleSystem smallWin;
    public ParticleSystem rain;
    public AnimationCurve shake;
    public BorderEffect borderEffect;

    public GameObject winPoint;
    public FailEffect failFast;
    public FailEffect failSlow;
    [Range(0.8f, 2f)]
    public float difficulty = 1.5f;
    public const float visualOffset = 0.05f;

    public UnityEvent win;
    public void SetDiffilcuty(float _d)
    {
            difficulty = _d;
    }

    float avgDelta = 0f;
    int frames = 0;
    float averageFrame;
    float tolerance;


    bool tap = false;
    public StarContainer starContainer;
    #region private variable
    private bool playing = false;
    private float timer;
    private float score;
    private float shakeTimer = 10;// set it to something so you don't lose on start ?
    bool canResolveScore = true;
    bool multiTouchUsed = false;
    bool tooLate = false;
    #endregion
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0)) ResetGame(); /// remove with alpha version!!
        if ( Input.GetKeyDown(KeyCode.Space) || tap)
        {
            playing = !playing; //-- toggle play on input
            if (playing) OnPlay();else OnStop();
            tap = false;
        }

        //game loop
        if (playing) InPlayMode();
        else   inWaitingToStart();
        
        //multitouch check
        if (Input.touchCount > 1) multiTouchUsed = true;
        if (Input.touchCount == 0) multiTouchUsed = false;
    }

    void OnPlay() // is this a single frame ???
    {
        tooLate = false;
        timer = 0;
        seedPanel.UpdateSeed();
        shakeTimer = 0;
        timeOutSlider.value = 0;
        title.color = defaultColour;
        rain.Stop();
        bigWin.Stop();
        smallWin.Stop();
        seedUI.StopSpin();
        hint.StartGame();
        winPoint.GetComponent<WinPoint>().Show();
        //reset delta + FPS
        avgDelta = 0f;
        frames = 0;
        canResolveScore = true;

        //reset background after shake
        bg.anchoredPosition = new Vector2(0, 0);
    }


    void ResolveResult()
    {
        string result;

        // --- Decide result
        if (tooLate) result = "late";
        else if (Mathf.Abs(score - 3f) <= tolerance) result = "win";
        else if (score < 3f - tolerance) result = "early";
        else result = "late";

        ApplyResult(result);
    }

    void ApplyResult(string result)
    {
        switch (result)
        {
            case "win":
                timerSlider.GoTo(3.000f);
                seedUI.StartSpin();
                score = 3.000f;
                title.text = (score - 3f).ToString("0.000");
                scoreList.AddPanel(score, "win");
                starContainer.UpdateMult();
                winPoint.GetComponent<WinPoint>().Win();
                hint.Hide();
                break;

            case "early":
                if( score > (3 - visualOffset )) timerSlider.GoTo((3 - visualOffset));
                rain.Play();
                starContainer.ResetMult();
                winPoint.GetComponent<WinPoint>().Hide();
                failSlow.Play();
                scoreList.AddPanel(score, "early");
                hint.Show();
                break;

            case "late":
                if (score < (3 + visualOffset)) timerSlider.GoTo((3 + visualOffset));
                rain.Play();
                starContainer.ResetMult();
                scoreList.AddPanel(score, "late");
                hint.Show();
                break;
        }
    }

    void OnStop()
    {
        ResolveResult();


    }
    void InPlayMode()
    {
        timer += Time.deltaTime ; // main timer
        timerSlider.GoTo( timer);// - play slider
        if(timer > 3) timeOutSlider.value = timer - 3; // time out slider
        score = timer;
        title.text = (timer - 3f).ToString("0.000");
        bg.anchoredPosition = new Vector2(0, 0);

        //keep chekcing FPS
        avgDelta += Time.deltaTime;
        frames++;

        //check tolorace
        averageFrame = avgDelta / Mathf.Max(frames, 1);
        tolerance = Mathf.Clamp(averageFrame * difficulty, 0.005f, 0.03f); // 0.0001 hard, 0,01 easy etc

        TooLate();

    }


    bool Lost()
    {
        if (Mathf.Abs(score - 3f) <= tolerance)return false;
        else return true;
        
    }


    void inWaitingToStart()
    {
        //when pressed stop!!!
        if (shakeTimer < 1)  shakeTimer += Time.deltaTime * 3;
        //screenshakes
        // TOO EARLY | LOSE 
        if (score > 0f && score < 2f)  ScreenShake(15);
        else if (score >= 2f && score < 3f - tolerance) ScreenShake(5);
        else if (score > 3f + tolerance) ScreenShake(15);
    }
    void ScreenShake(float _intensity)
    {
        bg.anchoredPosition = new Vector2(shake.Evaluate(shakeTimer) * _intensity, 0);
    }
   

   public void ResetGame()
   {
         SceneManager.LoadScene("menu");
   }


   
   void TooLate()
    {
        if (tooLate) return; // prevent spam

        if (timer > 3f + tolerance && winPoint.GetComponent<WinPoint>().Visible())
        {
                tooLate = true;
                winPoint.GetComponent<WinPoint>().Hide();
                failFast.Play();
        }
    }
    


    public void OnTap()
    {
        if (multiTouchUsed)
            return;

        if (Input.touchCount > 1)
            return;

        if (Input.touchCount == 1 && Input.GetTouch(0).phase != TouchPhase.Ended)
            return;

        tap = true;
    }
}
