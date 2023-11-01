using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    //scene transition
    public Image fader;

    void Awake()
    {
        if (FindObjectsOfType<GameManager>().Length > 1 )
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    void Start()
    {
        FadeIn();
    }

    void Update()
    {
        
    }

    //scene transition
    public void LoadLevel(string sceneName)
    {
        StartCoroutine(FadeOut(sceneName));
    } 
    IEnumerator FadeOut(string sceneName)
    {
        fader.CrossFadeAlpha(1,.75f,true);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(sceneName);
        yield return null;
        FadeIn();
    }
    void FadeIn()
    {
        fader.CrossFadeAlpha(0,2,true);
    }
}
