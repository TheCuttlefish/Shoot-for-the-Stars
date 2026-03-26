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
    public UIAnimations animations;

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

    #region private variable
    private bool playing = false;
    private float timer;
    private float score;
    private float shakeTimer = 10;// set it to something so you don't lose on start ?


    #endregion
    #region notes

    //-7.821 - angle of the UI bg
    #endregion

    void OnPlay() // is this a single frame ???
    {
                seedPanel.UpdateSeed();
                shakeTimer = 0;
                timeOutSlider.value = 0;
                title.color = defaultColour;
                rain.Stop();
                bigWin.Stop();
                smallWin.Stop();
                seedUI.StopSpin();
                animations.StartGame();
       // winPoint.SetActive(true);
        winPoint.GetComponent<WinPoint>().Show();
        //reset delta + FPS
        avgDelta = 0f;
        frames = 0;
        canResolveScore = true;

    }
    void OnStop()
    {
         animations.StopGame();
         timer = 0;



        if (Mathf.Abs(score - 3f) <= tolerance)
        {
            score = 3.000f;
            title.text = (score - 3f).ToString("0.000");
            scoreList.AddPanel(score, true);
        }
        else if (score < 3f - tolerance)
        {
            // BEFORE target  too early
            rain.Play();
            winPoint.GetComponent<WinPoint>().Hide();
            failSlow.Play();

            scoreList.AddPanel(score, false);
        }
        else
        {
            // AFTER target too late
            rain.Play();

            scoreList.AddPanel(score, false);
        }

    }
    void InPlayMode()
    {
            timer += Time.deltaTime ; // main timer
            timerSlider.GoTo( timer);// - play slider
            if(timer > 3) timeOutSlider.value = timer - 3; // time out slider
            
            score = timer;
        // font size needs to be imporved later / maybe??
            //title.fontSize = 122;// set font larger when game starts
            title.text = (timer - 3f).ToString("0.000");
            bg.anchoredPosition = new Vector2(0, 0);

        //keep chekcing FPS
        avgDelta += Time.deltaTime;
        frames++;
    }
    bool canResolveScore = true;


    bool Lost()
    {

        averageFrame = avgDelta / Mathf.Max(frames, 1);
        tolerance = Mathf.Clamp(averageFrame * difficulty, 0.005f, 0.03f);

        if (Mathf.Abs(score - 3f) <= tolerance)
            return false;

        return true;

    }


    void IsStopped()
    {


        averageFrame = avgDelta / Mathf.Max(frames, 1);
        tolerance = Mathf.Clamp(averageFrame * difficulty, 0.005f, 0.03f); //-possible time step to get a perfect hit on different divecices (frame rates)
                                                                           //----- 0.002 hard
                                                                           //----- 0.003 medium
                                                                           //----- 0.005 easy

        //when pressed stop!!!
        if (shakeTimer < 1)  shakeTimer += Time.deltaTime * 3;
        //screenshakes
        // TOO EARLY | LOSE 
        if (score > 0f && score < 2f)  ScreenShake(15);
        else if (score >= 2f && score < 3f - tolerance) ScreenShake(5);
        else if (score > 3f + tolerance) ScreenShake(15);
        

        if (canResolveScore)
        {
            
            if (Mathf.Abs(score - 3f) <= tolerance) //score >= 2.9995f && score <= 3.0005f
            {
                win.Invoke();// call win event!!
                title.color = WinColour;
                animations.Win();
                bigWin.Play();
                borderEffect.PlayEffect(); // border highlight!
                timeOutSlider.value = 0; // force 0
                timerSlider.GoTo(3.000f);// focce 3 seconds!!
                //winPoint.SetActive(true);// show win point glow just in case
                //winPoint.GetComponent<WinPoint>().Show(); // this can go away??? I think bug is fixed, but I am keeping it here for now just in case it does not register then win properlly
                winPoint.GetComponent<WinPoint>().Win();
            }
            else if (score > 3f + tolerance)
            {
                
                title.color = LoseColour;
                
            }
            canResolveScore = false;

        }
        

    }
    void ScreenShake(float _intensity)
    {
        bg.anchoredPosition = new Vector2(shake.Evaluate(shakeTimer) * _intensity, 0);
    }
   

   public void ResetGame()
    {
         SceneManager.LoadScene(0);
    }

    

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) ResetGame(); /// remove with alpha version!!




        // check if failed
        if (timer > 3 && winPoint.GetComponent<WinPoint>().Visible() && Lost())
        {
            winPoint.GetComponent<WinPoint>().Hide();
            //winPoint.SetActive(false);
            failFast.Play();
        }
        // --- end effect




        // input!!
        if ( Input.GetKeyDown(KeyCode.Space) || tap)
        {
            playing = !playing; //-- toggle play on input
            if (playing) OnPlay();else OnStop();
            tap = false;
        }

        //game loop
        if(playing) InPlayMode();
        else  IsStopped();



        //multitouch check
        if (Input.touchCount > 1)
        {
            multiTouchUsed = true;
        }

        if (Input.touchCount == 0)
        {
            multiTouchUsed = false;
        }





    }




    bool multiTouchUsed = false;

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
