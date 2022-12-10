using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

/// <summary>
/// Òì²½Ìø×ª³¡¾°
/// </summary>
public class GameSceneUtils : Singleton<GameSceneUtils>
{
    public void LoadSceneAsync(string sceneName, Action call)
    {
        AsyncOperation ao = SceneManager.LoadSceneAsync(sceneName);
        ao.completed += (_ao) => {
            call?.Invoke();
        };
    }
}