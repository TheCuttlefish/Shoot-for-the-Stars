using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Loading : MonoBehaviour
{
    public float loadDelay = 2f;

    float timer = 0f;
    bool startedLoading = false;
    AsyncOperation loadOp;

    void Update()
    {
        timer += Time.deltaTime;

        // wait 2 seconds before starting load
        if (timer >= loadDelay && !startedLoading)
        {
            startedLoading = true;
            loadOp = SceneManager.LoadSceneAsync("game", LoadSceneMode.Additive);
        }

        // once game is loaded --> remove loading scene
        if (startedLoading && loadOp != null && loadOp.isDone)
        {
            SceneManager.UnloadSceneAsync("loading");
        }
    }
}