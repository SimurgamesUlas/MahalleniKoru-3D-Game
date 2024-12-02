using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class AnaMenuKontrol : MonoBehaviour
{
    public GameObject LoadingPanel;
    public Slider LoadingSlider;
    public GameObject CikisPanel;
    public void OyunaBasla()
    {
        GameKontrolcu.OyunDurdumu = false;
        Time.timeScale = 1;
        StartCoroutine(SahneYuklemeLoading());
    }
    IEnumerator SahneYuklemeLoading()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(1);
        LoadingPanel.SetActive(true);
        while (!operation.isDone)
        {
            float ilerleme = Mathf.Clamp01(operation.progress / .9f);
            LoadingSlider.value = ilerleme;
            yield return null;
        }
      
    }
    public void OyundanCik()
    {
        CikisPanel.SetActive(true);
       
    }
    public void evet()
    {
       Application.Quit();
    }
    public void hayir()
    {
        CikisPanel.SetActive(false);
    }
}
