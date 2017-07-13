using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuControllerScript : MonoBehaviour {

    //Publics
    public Camera MainCam;
    public Text HighestScoreText;
    public GameObject BlackScreen;
    public Canvas[] Menus; //0Main;1Author;2HighestScore;3Options;4HowToPlay
    public Button[] Buttons; //0StartGame;1HighestScore;2Author;3ExitGame;4BackToMenu;5BackToMenu;6ResetHighestScore;7OptionsBackToMenu;8SaveOptions;9RestoreDefaultOptions;10Options
    public Slider MusicVolumeSlider;
    public Toggle[] QualityLevels; //0Excellent;1Decent;2Low
    //Privates
    int actualmenu = 0;
    HighestScoreScript HSS;
    Renderer BlackScreenRenderer;
    Coroutine ActiveCoroutine;

	void Start () {
        ButtonListeners();
        HSS = gameObject.GetComponent<HighestScoreScript>();
        BlackScreen.SetActive(true);
        BlackScreenRenderer = BlackScreen.GetComponent<Renderer>();
        BlackScreenRenderer.material.color = Color.clear;
        HighestScoreText.text = HSS.LoadHighestScore().ToString();
        Menus[0].gameObject.SetActive(true);
        Menus[1].gameObject.SetActive(false);
        Menus[2].gameObject.SetActive(false);
        Menus[3].gameObject.SetActive(false);
        Menus[4].gameObject.SetActive(false);
        ActiveCoroutine = StartCoroutine("OpenMenu");
        EventSystem.current.SetSelectedGameObject(Buttons[0].gameObject);
        Buttons[0].OnSelect(null);
        LoadSettings();
    }
	
    void LoadSettings()
    {
        GlobalSettings.LoadSettings();
        QualityLevels[GlobalSettings.GraphicsQuality].isOn = true;
        MusicVolumeSlider.value = GlobalSettings.MusicVolume;
    }

	void Update () {

        if (Input.GetKeyDown(KeyCode.Escape) && actualmenu!=0 && ActiveCoroutine == null)
        {
            ActiveCoroutine = StartCoroutine(FadeToBlack(0));
            EventSystem.current.SetSelectedGameObject(Buttons[0].gameObject);
            Buttons[0].OnSelect(null);
        }
    }

    void ButtonListeners()
    {
        Buttons[0].onClick.AddListener(delegate { ActiveCoroutine = StartCoroutine(StartGame(1)); });
        Buttons[1].onClick.AddListener(delegate { ActiveCoroutine = StartCoroutine(FadeToBlack(1)); EventSystem.current.SetSelectedGameObject(Buttons[4].gameObject); Buttons[4].OnSelect(null); });
        Buttons[2].onClick.AddListener(delegate { ActiveCoroutine = StartCoroutine(FadeToBlack(2)); EventSystem.current.SetSelectedGameObject(Buttons[5].gameObject); Buttons[5].OnSelect(null); });
        Buttons[3].onClick.AddListener(delegate { Application.Quit(); });
        Buttons[4].onClick.AddListener(delegate { ActiveCoroutine = StartCoroutine(FadeToBlack(0)); EventSystem.current.SetSelectedGameObject(Buttons[0].gameObject); Buttons[0].OnSelect(null); });
        Buttons[5].onClick.AddListener(delegate { ActiveCoroutine = StartCoroutine(FadeToBlack(0)); EventSystem.current.SetSelectedGameObject(Buttons[0].gameObject); Buttons[0].OnSelect(null); });
        Buttons[6].onClick.AddListener(delegate { HSS.ResetHighestScore(); HighestScoreText.text = "0"; });
        Buttons[7].onClick.AddListener(delegate { ActiveCoroutine = StartCoroutine(FadeToBlack(0)); EventSystem.current.SetSelectedGameObject(Buttons[0].gameObject); Buttons[0].OnSelect(null); });
        Buttons[8].onClick.AddListener(delegate 
        {
            /*SAVING SETTINGS*/
            if (QualityLevels[0].isOn)
                GlobalSettings.GraphicsQuality = 0;
            if (QualityLevels[1].isOn)
                GlobalSettings.GraphicsQuality = 1;
            if (QualityLevels[2].isOn)
                GlobalSettings.GraphicsQuality = 2;

            GlobalSettings.MusicVolume = MusicVolumeSlider.value;
            print(GlobalSettings.GraphicsQuality + " " + GlobalSettings.MusicVolume);

            GlobalSettings.SaveSettings();
        });
        Buttons[9].onClick.AddListener(delegate {
            GlobalSettings.ResetSettings();
            QualityLevels[0].isOn = true;
            MusicVolumeSlider.value = 1f;
        });
        Buttons[10].onClick.AddListener(delegate { ActiveCoroutine = StartCoroutine(FadeToBlack(3)); EventSystem.current.SetSelectedGameObject(Buttons[7].gameObject); Buttons[7].OnSelect(null); });
        Buttons[11].onClick.AddListener(delegate { ActiveCoroutine = StartCoroutine(StartGame(2)); });
        Buttons[12].onClick.AddListener(delegate { ActiveCoroutine = StartCoroutine(FadeToBlack(0)); EventSystem.current.SetSelectedGameObject(Buttons[0].gameObject); Buttons[0].OnSelect(null); });
        Buttons[13].onClick.AddListener(delegate { ActiveCoroutine = StartCoroutine(FadeToBlack(4)); EventSystem.current.SetSelectedGameObject(Buttons[12].gameObject); Buttons[12].OnSelect(null); });
    }

    IEnumerator StartGame(int Scene)
    {

        while (BlackScreenRenderer.material.color.a < 1)
        {
            BlackScreenRenderer.material.color = new Color(0, 0, 0, BlackScreenRenderer.material.color.a + 0.05f);
            yield return new WaitForSeconds(0.005f);
        }
        SceneManager.LoadScene(Scene);
        ActiveCoroutine = null;
    }

    IEnumerator OpenMenu()
    {
        BlackScreenRenderer.material.color = Color.black;
        while (BlackScreenRenderer.material.color.a > 0)
        {
            BlackScreenRenderer.material.color = new Color(0, 0, 0, BlackScreenRenderer.material.color.a - 0.02f);
            yield return new WaitForSeconds(0.0025f);
        }

        StopCoroutine(ActiveCoroutine);
        ActiveCoroutine = null;
    }

    IEnumerator FadeToBlack(int menu)
    {
        
        while (BlackScreenRenderer.material.color.a < 1)
        {
            BlackScreenRenderer.material.color = new Color(0, 0, 0, BlackScreenRenderer.material.color.a + 0.05f);
            yield return new WaitForSeconds(0.01f);
        }
        Menus[actualmenu].gameObject.SetActive(false);
        Menus[menu].gameObject.SetActive(true);
        actualmenu = menu;
        while (BlackScreenRenderer.material.color.a > 0)
        {
            BlackScreenRenderer.material.color = new Color(0, 0, 0, BlackScreenRenderer.material.color.a - 0.05f);
            yield return new WaitForSeconds(0.01f);
        }
        StopCoroutine(ActiveCoroutine);
        ActiveCoroutine = null;
    }
}
