using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class SettingsScene : MonoBehaviour
{
    public AudioMixer audioMixer;
    public static bool subtitles = true;
    public static float musicVolume = 100;
    public static float dialogueVolume = 100;
    public static float soundEffectsVolume = 100;
    public static bool fullscreen = true;
    Resolution[] resolutions;
    public Dropdown resolutionDropdown;

    void Start()
    {
        if (SceneManager.GetActiveScene().name.Equals("Settings"))
        {
            resolutions = Screen.resolutions;
            resolutionDropdown.ClearOptions();
            List<string> options = new List<string>();
            int currResolutionIndex = 0;
            for (int i = 0; i < resolutions.Length; i++)
            {
                string option = resolutions[i].width + " x " + resolutions[i].height + " @ " + resolutions[i].refreshRateRatio + "hz";
                options.Add(option);
                if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
                {
                    currResolutionIndex = i;
                }
            }
            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = currResolutionIndex;
            resolutionDropdown.RefreshShownValue();
        }
        
    }

    public void setResolution(int resolutionIndex)
    {
        Screen.SetResolution(resolutions[resolutionIndex].width, resolutions[resolutionIndex].height, Screen.fullScreen);
    }
    public void LoadSettings()
    {
        SceneManager.LoadScene("Settings");
    }

    public void ChangeMusicVolume(float v)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(v) * 20);
        musicVolume = v;
    }
    public void ChangeDialogueVolume(float v)
    {
        audioMixer.SetFloat("DialogueVolume", Mathf.Log10(v) * 20);
        dialogueVolume = v;
    }
    public void ChangeSoundEffectsVolume(float v)
    {
        audioMixer.SetFloat("SoundEffectsVolume", Mathf.Log10(v) * 20);
        soundEffectsVolume = v;
    }

    public void setFullscreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }
}
