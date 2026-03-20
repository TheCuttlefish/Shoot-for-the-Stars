using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineFollower : MonoBehaviour
{



    public LineRenderer lr;
    public PointGen gen;
    Vector3 wind;
    private void Start()
    {
        lr = GetComponent<LineRenderer>();
    }



    private void Update()
    {

        if (gen.points.Count > 40)
        {
            for (int i = 0; i < 40; i++)
            {
                float heightDiff = gen.points[i].y - Camera.main.transform.position.y;

                // 6 is your offset; adjust 6f and 1f however you like
                float windByHeight = Mathf.Clamp((heightDiff/4 + 0f), 0f, 0.3f);
                
                    wind = new Vector3(Mathf.Sin((Time.time + (gen.points[i].y * 0.01f) * i/100) ) * 0.5f * windByHeight, 0, 0);
                

                //--------------------------------------------------------------------------follow speed is the last parameter
                    lr.SetPosition(i, Vector3.Lerp(lr.GetPosition(i), gen.points[i] + wind, 0.2f)); ///0.05 - origianl. 1.05 -- low fps
                //0.6
            }
        }

    }
}
