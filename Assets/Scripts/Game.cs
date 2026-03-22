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

    [Header("Animations")]
    public UIAnimations animations;

    [Header("Score UI")]
    public ScoreList scoreList;

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

    void OnPlay()
    {

                shakeTimer = 0;
                timeOutSlider.value = 0;
                title.color = defaultColour;
                rain.Stop();
                bigWin.Stop();
                smallWin.Stop();

                animations.StartGame();
        //reset delta + FPS
        avgDelta = 0f;
        frames = 0;
        canResolveScore = true;

    }
    void OnStop()
    {
         animations.StopGame();
         timer = 0;

        if (Mathf.Abs(score - 3f) <= tolerance) //score >= 2.9995f && score <= 3.0005f
        {
            
            score = 3.000f; // can edit this later (for now I make score 3 and remove it in the line below)
            title.text = ( score - 3f).ToString("0.000");// force 3.000 becuase of input tolorance!!!
            scoreList.AddPanel(score,true);
           
        }else
        {
            if (score < 3) { // rain
                rain.Play();
            }
            else // thuinder needs to be ADDED!!!!
            {
                rain.Play();
            }
            scoreList.AddPanel(score,false);
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
        if (score > 0f && score < 1f) ScreenShake(15);
        else if (score >= 1f && score < 3f - tolerance) ScreenShake(5);
        else if (score > 3f + tolerance) ScreenShake(150);
        

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

       

    }


    public void OnTap()
    {
        tap = true;
    }
}
