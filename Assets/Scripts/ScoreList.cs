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
        if (Input.GetKeyDown(KeyCode.Alpha1)) AddPanel(0, "win");
        
    }

    private void Start()
    {
        defaultColour = GameObject.Find("Canvas").GetComponent<Game>().defaultColour;
        LoseColour = GameObject.Find("Canvas").GetComponent<Game>().LoseColour;
        WinColour = GameObject.Find("Canvas").GetComponent<Game>().WinColour;
       
    }

    public void AddPanel(float _score, string _gameState)
    {
        //show cell in the progress box
        GameObject _result;
        _result = Instantiate(cell, history.transform);
        _result.transform.SetAsFirstSibling();
        _result.GetComponent<Image>().color = defaultColour;
        
        GameObject _p = Instantiate(panel,transform);
        string scoreAsString = (_score - 3f).ToString("0.000");// not sure if this used anymore
         _p.transform.Find("score").GetComponent<Text>().text = (_score - 3f).ToString("0.000");
         _p.transform.Find("miss").GetComponent<Text>().text = (3.000f - _score).ToString("-0.000;+0.000");

        //remove too many!
        if (transform.childCount > 9)Destroy(transform.GetChild(0).gameObject);

        //FULL WIN
        if (_gameState == "win")
        {
            OnWin.Invoke();
            _p.transform.Find("score").GetComponent<Text>().text = "SUPER";
            _p.GetComponent<Image>().color = new Color(0, 0, 0, 0);
            _result.GetComponent<Image>().color = WinColour;
            currentSteak++;
            _result.GetComponent<Result>().ShowIconAndMult("perfect", currentSteak);
            _p.transform.Find("colour").GetComponent<Image>().color = WinColour;

        }
        else if(_gameState == "early")
        {
            _p.transform.Find("score").GetComponent<Text>().text = "EARLY";
            currentSteak = 0;
             // SMALL WIN range
             if (_score >= 2.900f && _score <= 3.100f)
                OnSmallWin.Invoke(); // this is small growth I think

                _result.GetComponent<Image>().color = defaultColour;
                _result.GetComponent<Result>().ShowIconAndMult("early", currentSteak);
                _p.transform.Find("colour").GetComponent<Image>().color = defaultColour;
        }
        else if (_gameState == "late")
        {
            _p.transform.Find("score").GetComponent<Text>().text = "LATE";
            currentSteak = 0;
            if (_score >= 2.900f && _score <= 3.100f)
                OnSmallWin.Invoke(); // this is small growth I think

            _result.GetComponent<Image>().color = LoseColour;
                _result.GetComponent<Result>().ShowIconAndMult("late", currentSteak);
                _p.transform.Find("colour").GetComponent<Image>().color = LoseColour;

         }

    }


}
