using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadSceneAsync(string sceneName, Action onComplete)
    {
        StartCoroutine(LoadSceneRoutine(sceneName, onComplete));
    }

    private IEnumerator LoadSceneRoutine(string sceneName, Action onComplete)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        asyncLoad.allowSceneActivation = false; // 자동 활성화를 막음

        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {
                Debug.Log($"Scene Load {asyncLoad.progress * 100}%");

                asyncLoad.allowSceneActivation = true;

                break;
            }
            yield return null; // 다음 프레임까지 대기
        }

        onComplete?.Invoke();

    }
}
