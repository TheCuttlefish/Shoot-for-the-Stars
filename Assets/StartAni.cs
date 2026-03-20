
using UnityEngine;
using UnityEngine.UI;

public class StartAni : MonoBehaviour
{


    public CanvasGroup canvasGroup;
    // Start is called before the first frame update
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        canvasGroup.alpha = (Mathf.Cos( Time.time * 10 ) + 1 )/ 2 ;
    }
}
