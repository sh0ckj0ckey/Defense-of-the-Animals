using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenuController : MonoBehaviour
{
    public GameObject IntergradeImage;
    public float FadeSpeed = 0.05f;

    public void OnStartGame()
    {
        StartCoroutine(FadeImage());

        StartCoroutine(DelayInvoker.DelayToInvoke(() =>
        {
            SceneManager.LoadScene(1);
            SceneManager.sceneLoaded += StartSceneLoaded;
        }, 1));
    }

    private void StartSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "StartScene")
        {
            ResetIntergradeImage();
        }
        else if (scene.name == "MainScene")
        {
            ResetIntergradeImage();
        }
    }

    public void OnExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void OnClickBack2Home()
    {
        StartCoroutine(FadeImage());

        StartCoroutine(DelayInvoker.DelayToInvoke(() =>
        {
            SceneManager.LoadScene(0);
            SceneManager.sceneLoaded += StartSceneLoaded;
        }, 1));
    }

    private IEnumerator FadeImage()
    {
        var img = IntergradeImage.GetComponent<Image>();
        float r = img.color.r;
        float g = img.color.g;
        float b = img.color.b;
        while (img.color.a < 1)
        {
            img.color = new Color(r, g, b, img.color.a + FadeSpeed);
            yield return new WaitForFixedUpdate();  //��Ϊupdate���ȶ�,fixedupdate�ܱ�֤ÿ����ɫ�任����һ��ʱ���ȶ�������֡���Ϳ��Ծ�׼���ơ�
        }
    }

    private void ResetIntergradeImage()
    {
        var img = IntergradeImage.GetComponent<Image>();
        var color = img.color;
        color.a = 0;
        img.color = color;
    }
}
