using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public AudioMixer audioMixer;

    public GameObject menu;

    public Text score;
    public GameObject settingsMenu;

    public GameObject gameplay;

    private void Awake()
    {
        settingsMenu.SetActive(false);
        gameplay = GameObject.Find("Gameplay");
        score.text = PlayerPrefs.GetString("highScore", "High score: " + PlayerPrefs.GetInt("maxScore", 0));
    }

    private void Start()
    {
        Cursor.visible = true;
        SetVolume(PlayerPrefs.GetFloat("volume"));
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
        PlayerPrefs.SetFloat("volume", volume);
    }

    public void Fullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SettingsButton()
    {
        settingsMenu.SetActive(true);
    }

    public void CloseButton()
    {
        settingsMenu.SetActive(false);
    }

    public void PlayPressed()
    {
        menu.SetActive(false);
        gameplay.GetComponent<Gameplay>().enabled = true;
    }

    public void ExitPressed()
    {
        Application.Quit();
    }

}
