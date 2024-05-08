using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Loader
{
    public enum Scene
    {
        Start,
        Loading,
        SampleScene
    }
    public static Scene targetScene;
    public static void Load(Scene scene)
    {
        targetScene = scene;
        SceneManager.LoadScene(Loader.Scene.Loading.ToString());
    }
    public static void LoaderCallBack()
    {
        SceneManager.LoadScene(targetScene.ToString());
    }

}
