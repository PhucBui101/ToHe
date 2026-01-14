using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour
{
    private IEnumerator Start()
        {
            Application.targetFrameRate = 60;

            yield return new WaitForEndOfFrame();
            
            SceneManager.LoadScene("SampleScene");
        }
}
