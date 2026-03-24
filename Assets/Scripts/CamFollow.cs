using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CamFollow : MonoBehaviour
{


    public Transform treeTop;
    Vector3 offset = new Vector3(0, -6, -10);
    bool isDragging = false;
    Vector3 mousePos;
    Vector3 clickedPos;
    float timer;


    public CanvasGroup mainMenuGroup;
    public CanvasGroup interactiveUIGroup;
    public CanvasGroup panUIGroup;

    void ShowMainMenu(bool _default)
    {
        if (_default)
        {
            mainMenuGroup.alpha = 1;
            interactiveUIGroup.alpha = 1;
            panUIGroup.alpha = 0;
        }else
        {
            mainMenuGroup.alpha = 0;
            interactiveUIGroup.alpha = 0;
            panUIGroup.alpha = 0.1f;

        }

    }



    // Update is called once per frame
    void Update()
    {

        if (!isDragging)
        {
            timer += Time.deltaTime;
            if(timer > 2)
            {

                transform.position -= (transform.position - (treeTop.position + offset))/1.1f * Time.deltaTime;
            }
        }



        if (Input.GetMouseButtonDown(1))
        {
            clickedPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            isDragging = true;
            ShowMainMenu(false);

        }
        if (Input.GetMouseButtonUp(1))
        {
            isDragging = false;
            ShowMainMenu(true);

        }

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (isDragging)
        {
            timer = 0;
            transform.position -= (mousePos - clickedPos);
        }


    }
}
