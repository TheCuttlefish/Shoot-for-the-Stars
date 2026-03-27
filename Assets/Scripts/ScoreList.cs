using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class ScoreList : MonoBehaviour
{

    public GameObject panel;
    private Color defaultColour;
    private Color LoseColour;
    private Color WinColour;
   // private Color crownColour;

    public UnityEvent OnWin;
    public UnityEvent OnSmallWin;
    public GameObject history;
    public GameObject cell;
 

    public int currentSteak = 0;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) AddPanel(0, true);
        
    }


    private void Start()
    {
        defaultColour = GameObject.Find("Canvas").GetComponent<Game>().defaultColour;
        LoseColour = GameObject.Find("Canvas").GetComponent<Game>().LoseColour;
        WinColour = GameObject.Find("Canvas").GetComponent<Game>().WinColour;
       
    }

    public void AddPanel(float _score, bool _win = true)
    {
        //show cell in the progress box
        GameObject _result;
        _result = Instantiate(cell, history.transform);
        _result.transform.SetAsFirstSibling();
        _result.GetComponent<Image>().color = defaultColour;
        
        _result.GetComponent<Result>().ShowIconAndMult("early");// set default to rain???? 
        // just set default result icon when it spawns

        //show panel
        GameObject _p = Instantiate(panel,transform);
        string scoreAsString = (_score - 3f).ToString("0.000");// not sure if this used anymore
         _p.transform.Find("score").GetComponent<Text>().text = (_score - 3f).ToString("0.000");
        
         _p.transform.Find("miss").GetComponent<Text>().text = (3.000f - _score).ToString("-0.000;+0.000");


        if(3.000f - _score > 0)
        _p.transform.Find("score").GetComponent<Text>().text = "EARLY";
        else
        {
            _p.transform.Find("score").GetComponent<Text>().text = "LATE";
        }
        //remove too many!
        if (transform.childCount > 9)Destroy(transform.GetChild(0).gameObject);



        // known bug!!!
        // currectly score does not always line up with timer in game, as they are calculated differently!!!



        //FULL WIN
        if (_win)
        {
            _p.transform.Find("score").GetComponent<Text>().text = "SUPER";
            OnWin.Invoke();
            _p.GetComponent<Image>().color = new Color(0, 0, 0, 0);

                _result.GetComponent<Image>().color = WinColour;
            // _cell.GetComponent<Image>().rectTransform.localScale = new Vector3(1, 1, 1) ;
            currentSteak++;
            _result.GetComponent<Result>().ShowIconAndMult("perfect", currentSteak);
                _p.transform.Find("colour").GetComponent<Image>().color = WinColour;

        }
        else
        {

                currentSteak = 0;






            // SMALL WIN range
            if (_score >= 2.900f && _score <= 3.100f)
            {
                
               OnSmallWin.Invoke(); // this is small growth I think
            }

            // TOO EARLY
            else if (_score < 2.900f)
            {
                _result.GetComponent<Image>().color = defaultColour;
                _result.GetComponent<Result>().ShowIconAndMult("late");
                _p.transform.Find("colour").GetComponent<Image>().color = defaultColour;
            }

            // TOO LATE
            else
            {
                _result.GetComponent<Image>().color = LoseColour;
                
                _result.GetComponent<Result>().ShowIconAndMult("late");
                _p.transform.Find("colour").GetComponent<Image>().color = LoseColour;
            }

            

        }

    }


}
