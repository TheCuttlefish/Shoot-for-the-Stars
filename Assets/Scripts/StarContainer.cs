using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarContainer : MonoBehaviour
{

    public List<CollectedStar> stars = new List<CollectedStar> ();
    int index = 0;
    int mult = 0;
    int tempMult = 0;
    bool spawnStars = false;

    public void ResetMult()
    {
        mult = 0;
    }


    public void UpdateMult()
    {
        mult++;
        tempMult = mult;
        spawnStars = true;

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) UpdateMult();


        if(spawnStars )
        {
            if (tempMult > 0)
            {
                stars[index].ShowStar();
                index++;
                if (index > stars.Count - 1) index = 0;
                tempMult--;
            }else
            {
                spawnStars = false;
            }

        }

    }


}
