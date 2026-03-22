
using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{

    public Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponent<Slider>();
    }

    
    public void GoTo(float _values)
    {
        slider.value = _values;
    }


    public void Reset()
    {
        slider.value = 0;
    }
}
