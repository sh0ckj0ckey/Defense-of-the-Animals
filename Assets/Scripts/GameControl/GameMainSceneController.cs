using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMainSceneController : MonoBehaviour
{
    public GameObject IntergradeImage;
    public float FadeSpeed = 0.05f;

    private void StartSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainScene")
        {
            ResetIntergradeImage();
            EnemySpawner.Instance.StartSpawningEnemy();
        }
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
        if (IntergradeImage != null)
        {
            var img = IntergradeImage.GetComponent<Image>();
            var color = img.color;
            color.a = 0;
            img.color = color;
        }
    }
}
