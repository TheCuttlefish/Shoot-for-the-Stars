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
    public GameObject progressBox;
    public GameObject cell;
 

    private void Start()
    {
        defaultColour = GameObject.Find("Canvas").GetComponent<Game>().defaultColour;
        LoseColour = GameObject.Find("Canvas").GetComponent<Game>().LoseColour;
        WinColour = GameObject.Find("Canvas").GetComponent<Game>().WinColour;
       
    }

    public void AddPanel(float _score, bool _win = true)
    {
        //show cell in the progress box
        GameObject _cell;
        _cell = Instantiate(cell, progressBox.transform);
        _cell.transform.SetAsFirstSibling();
        _cell.GetComponent<Image>().color = defaultColour;
        _cell.GetComponent<Weather>().Rain();// set default to rain???? 


        //show panel
        GameObject _p = Instantiate(panel,transform);
        string scoreAsString = _score.ToString("0.000");
        _p.transform.Find("score").GetComponent<Text>().text = _score.ToString("0.000");
        _p.transform.Find("miss").GetComponent<Text>().text = (3.000f - _score).ToString("-0.000;+0.000");

        //remove too many!
        if (transform.childCount > 20)Destroy(transform.GetChild(0).gameObject);



        // known bug!!!
        // currectly score does not always line up with timer in game, as they are calculated differently!!!



        //FULL WIN
        if (_win)
        {

            OnWin.Invoke();
            _p.GetComponent<Image>().color = new Color(0, 0, 0, 0);

                _cell.GetComponent<Image>().color = WinColour;
               // _cell.GetComponent<Image>().rectTransform.localScale = new Vector3(1, 1, 1) ;
                 _cell.GetComponent<Weather>().Sun();
                _p.transform.Find("colour").GetComponent<Image>().color = WinColour;

        }
        else
        {







            // SMALL WIN range
            if (_score >= 2.900f && _score <= 3.000f)
            {
                OnSmallWin.Invoke();
            }

            // TOO EARLY
            else if (_score < 2.900f)
            {
                _cell.GetComponent<Image>().color = defaultColour;
                _cell.GetComponent<Weather>().Rain();
                _p.transform.Find("colour").GetComponent<Image>().color = defaultColour;
            }

            // TOO LATE
            else
            {
                _cell.GetComponent<Image>().color = LoseColour;
                _cell.GetComponent<Weather>().Rain();
                _p.transform.Find("colour").GetComponent<Image>().color = LoseColour;
            }

            

        }

    }


}
