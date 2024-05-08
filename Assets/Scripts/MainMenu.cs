using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;
    private void Start()
    {
        playButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.SampleScene);
        });
        quitButton.onClick.AddListener(() =>
        {
            Application.Quit();
        });
        Time.timeScale = 1.0f;
    }
}
