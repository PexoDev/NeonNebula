using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class UIController : MonoBehaviour {

    //Publics
    public GameObject GameOverMenu, PauseMenu, GameScripts, BlackScreen, CheatMenu;
    public Button[] GameOverButtons, PauseMenuButtons; // 0TryAgain;2ExitToMenu | 0Resume;1TryAgain;2ExitToMenu
    public Text GameOverScoreText, OnScreenText, ScoreText, StarsCount;
    public GameObject[] MissilesCount1, MissilesCount2, RaygunTimers1, RaygunTimers2;

    //Privates
    GameController GCS;
    Renderer BlackScreenRenderer;
    List<string> CheatKey;
    List<char> CheatPassword;
    void Start () {
        GCS = GameScripts.GetComponent<GameController>();
        BlackScreenRenderer = BlackScreen.GetComponent<Renderer>();
        ButtonListeners();
        CheatMenuStart();
    }
    void CheatMenuStart()
    {
        CheatMenu.SetActive(false);
        CheatPassword = new List<char>
        {
            'y',
            'o',
            'u',
            'r',
            'p',
            'a',
            's',
            's',
            'w',
            'd'
        };
        CheatKey = new List<string>();
    }
    void CheatMenuActivation()
    {
        if (!CheatMenu.activeSelf && GCS.GamePaused)
        {
            if (Input.inputString!="")
                CheatKey.Add(Input.inputString);
            if (CheatKey.Count > 1)
                if (CheatKey[CheatKey.Count - 1][0] == CheatPassword[CheatPassword.Count - 1])
                {
                    for (int i = 0; i < CheatPassword.Count; i++)
                    {
                        if (CheatPassword[CheatPassword.Count - (i + 1)] == CheatKey[CheatKey.Count - (i + 1)][0])
                        {
                            if (i == CheatPassword.Count - 1)
                            {
                                GCS.Score = -99999;
                                CheatMenu.SetActive(true);
                                break;
                            }
                        }
                        else
                            break;
                    }
                    CheatKey.Clear();
                }
        }
    }
    private void Update()
    {
        CheatMenuActivation();
    }

    public void CountMissiles(int MissileCountNumb, bool isPlayerOne)
    {
        if (!isPlayerOne)
        {
            for (int i = 0; i < MissileCountNumb; i++)
            {
                MissilesCount1[i].SetActive(true);
            }
            for (int j = MissileCountNumb; j < 5; j++)
            {
                MissilesCount1[j].SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < MissileCountNumb; i++)
            {
                MissilesCount2[i].SetActive(true);
            }
            for (int j = MissileCountNumb; j < 5; j++)
            {
                MissilesCount2[j].SetActive(false);
            }
        }
    }

    public void CountRaygun(int RaygunCount, bool isPlayerOne)
    {
        if (!isPlayerOne)
        {
            for (int i = 0; i < RaygunCount; i++)
            {
                RaygunTimers1[i].SetActive(true);
            }
            for (int j = RaygunCount; j < 5; j++)
            {
                RaygunTimers1[j].SetActive(false);
            }
        }
        else
        {
            for (int i = 0; i < RaygunCount; i++)
            {
                RaygunTimers2[i].SetActive(true);
            }
            for (int j = RaygunCount; j < 5; j++)
            {
                RaygunTimers2[j].SetActive(false);
            }
        }
    }
    public void StarsCountTextChange(int starscount)
    {
        StarsCount.text = "Stars: " + starscount + "/4";
    }

    public void ScoreTextChange(string text)
    {
        ScoreText.text ="Score: "+ text;
    }

    public void MainText(string text)
    {
        OnScreenText.text = text;
    }

    public IEnumerator GameOverMenuFunction(bool Highest)
        {
        BlackScreen.SetActive(true);
        while (BlackScreenRenderer.material.color.a < 1)
        {
            BlackScreenRenderer.material.color = new Color(0, 0, 0, BlackScreenRenderer.material.color.a + 0.05f);
            yield return new WaitForSeconds(0.025f);
        }
        GameOverScoreText.text ="Your Score: "+ GCS.Score.ToString();
        if (Highest)
            GameOverScoreText.text +="\n New Highest Score!";
        EventSystem.current.SetSelectedGameObject(GameOverButtons[0].gameObject);
        GameOverButtons[0].OnSelect(null);
        GameOverMenu.SetActive(true);
    }

    public void PauseMenuFunction(bool pause)
    {
        if (pause)
        {
            CheatKey.Clear();
            PauseMenu.SetActive(false);
            GCS.GamePaused = false;
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(PauseMenuButtons[0].gameObject);
            PauseMenuButtons[0].OnSelect(null);
            GCS.GamePaused = true;
            PauseMenu.SetActive(true);
        }
    }

    void ButtonListeners()
    {
        GameOverButtons[0].onClick.AddListener(delegate { SceneManager.LoadScene(SceneManager.GetActiveScene().name); });
        GameOverButtons[1].onClick.AddListener(delegate { StartCoroutine("ExitToMenu"); });

        PauseMenuButtons[0].onClick.AddListener(delegate { PauseMenuFunction(true);  });
        PauseMenuButtons[1].onClick.AddListener(delegate { SceneManager.LoadScene(SceneManager.GetActiveScene().name); });
        PauseMenuButtons[2].onClick.AddListener(delegate { StartCoroutine("ExitToMenu"); });

    }

    IEnumerator ExitToMenu()
    {
        GameOverMenu.SetActive(false);

        BlackScreen.SetActive(true);
        while (BlackScreenRenderer.material.color.a < 1)
        {
            BlackScreenRenderer.material.color = new Color(0, 0, 0, BlackScreenRenderer.material.color.a + 0.05f);
            yield return new WaitForSeconds(0.01f);
        }
        SceneManager.LoadScene(0);
    }
}
