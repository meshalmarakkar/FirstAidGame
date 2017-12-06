using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using SimpleJSON;

public class MenuController : MonoBehaviour {

    public Text recoveryPositionScore;
    public Text compressionScore;

    public GameObject camera_main;
    public GameObject camera_bleeding;
    public GameObject camera_burns;
    public GameObject mainMenu;
    public GameObject lv_RecoveryPosition;
    public GameObject lv_Compression;
    public GameObject lv_Bleeding;
    public GameObject lv_Burns;

    public GameObject ui_mainMenu;
    public GameObject ui_lv_RecoveryPosition;
    public GameObject ui_lv_Compression;

    private const int idSG = 361; //game id from website

    public Text txtFeedback;
    public GameObject ui_PlayerDetails;
    public GameObject ui_MiniGameOptions;
    public Button Button_Trophy;
    public Button Button_Leaderboard;
    public InputField input_playerName;
    public InputField input_playerAge;
    public Toggle toggle_prevExperience;
    public Button button_startGame;
    public GameObject text_incompleteInput;

    public GameObject ui_trophies;
    public GameObject ui_display_info;
    public Text text_info;

    private string playerName;
    private string playerAge;
    private bool prevExperience;

    private bool firstTime_infoSeen;
    private string gameInfo;

    private bool offlineMode;
    void Start()
    {
        deactivateLevels();
        activate_menu(true);
        ui_MiniGameOptions.SetActive(false);
        ui_PlayerDetails.SetActive(true);
        firstTime_infoSeen = false;

        StartCoroutine(EngAGe.E.getGameDesc(idSG));
    }

    public void Click_StartGame()
    {
        if (input_playerName.text == "" || input_playerAge.text == "")
        {
            text_incompleteInput.SetActive(true);
        }
        else
        {
            offlineMode = false;
            // get the seriousGame object from engage
            JSONNode SGdesc = EngAGe.E.getSG()["seriousGame"];

            gameInfo = "Game Info\n" +
                             SGdesc["name"] + " - " + SGdesc["description"];

            StartCoroutine(EngAGe.E.getLeaderboard(idSG));
            StartCoroutine(EngAGe.E.getBadgesWon(idSG));
            ui_PlayerDetails.SetActive(false);
            Click_Info();

            StartCoroutine(EngAGe.E.guestLogin(idSG, "LoginScene", "ParametersScene"));
        }
    }

    public void Click_PlayOffline()
    {
        offlineMode = true;
        Button_Trophy.interactable = false;
        Button_Leaderboard.interactable = false;
        gameInfo = "Game Info\n" +
                   "You are currently playing an offline version of First Aid Game by B00269701, B00275050, B00287064, and B00253493";
 
        ui_PlayerDetails.SetActive(false);
        Click_Info();
    }

    public void Click_Info()
    {
        ui_MiniGameOptions.SetActive(false);
        ui_display_info.SetActive(true);

        text_info.text = gameInfo;
    }

    public void Click_Trophy()
    {
        ui_MiniGameOptions.SetActive(false);
        ui_trophies.SetActive(true);
    }

    public void Click_BackFromInfo()
    {
        if (offlineMode == false)
        {
            if (firstTime_infoSeen == false)
            {
                playerName = input_playerName.text;
                playerAge = input_playerAge.text;
                prevExperience = toggle_prevExperience.isOn;

                int i = 0;
                foreach (JSONNode param in EngAGe.E.getParameters())
                {
                    if (i == 0)
                        param.Add("value", playerName);
                    else if (i == 1)
                        param.Add("value", playerAge);
                    else if (i == 2)
                        param.Add("value", prevExperience == true ? "yes" : "no");
                    i++;
                }

                //StartCoroutine(EngAGe.E.startGameplay(idSG, "GameScene"));
                firstTime_infoSeen = true;
            }
        }
        ui_display_info.SetActive(false);
        ui_trophies.SetActive(false);
        ui_MiniGameOptions.SetActive(true);
        text_info.text = "";
    }

    public void Click_Leaderboard()
    {
        ui_MiniGameOptions.SetActive(false);
        ui_display_info.SetActive(true);

        // get the leaderboard object from engage
        JSONNode leaderboard = EngAGe.E.getLeaderboardList();
        // look only at the eu_score 
        JSONArray recoveryScorePerf = leaderboard["recovery_score"].AsArray;
        JSONArray compressionScorePerf = leaderboard["compression_score"].AsArray;
        JSONArray bleedingScorePerf = leaderboard["bleeding_score"].AsArray;
        JSONArray burnsScorePerf = leaderboard["burns_score"].AsArray;

        // display up to 10 best gameplays
        int max = 5;
   
        text_info.text = "LEADERBOARD\n";
        text_info.text += "\nRecovery Position\n";
        foreach (JSONNode gameplay in recoveryScorePerf)
        {
            if (max-- > 0)
            {
                // each gameplay has a "name" and a "score"
                float score = gameplay["score"].AsFloat;
                text_info.text += score + " - " +
                    gameplay["name"] + "\n";
            }
        }
        text_info.text += "\nCompression\n";
        foreach (JSONNode gameplay in compressionScorePerf)
        {
            if (max-- > 0)
            {
                // each gameplay has a "name" and a "score"
                float score = gameplay["score"].AsFloat;
                text_info.text += score + " - " +
                    gameplay["name"] + "\n";
            }
        }
        text_info.text += "\nBleeding\n";
        foreach (JSONNode gameplay in bleedingScorePerf)
        {
            if (max-- > 0)
            {
                // each gameplay has a "name" and a "score"
                float score = gameplay["score"].AsFloat;
                text_info.text += score + " - " +
                    gameplay["name"] + "\n";
            }
        }
        text_info.text += "\nBurns\n";
        foreach (JSONNode gameplay in burnsScorePerf)
        {
            if (max-- > 0)
            {
                // each gameplay has a "name" and a "score"
                float score = gameplay["score"].AsFloat;
                text_info.text += score + " - " +
                    gameplay["name"] + "\n";
            }
        }

    }

    private void activate_menu(bool activation)
    {
        ui_mainMenu.SetActive(activation);
       // mainMenu.SetActive(activation);
    }
    private void activate_lv_RecoveryPosition(bool activation)
    {
        ui_lv_RecoveryPosition.SetActive(activation);
        lv_RecoveryPosition.SetActive(activation);
    }
    private void activate_lv_Compression(bool activation)
    {
        ui_lv_Compression.SetActive(activation);
        lv_Compression.SetActive(activation);
    }
    private void activate_lv_Bleeding(bool activation)
    {
        lv_Bleeding.SetActive(activation);
    }
    private void activate_lv_Burns(bool activation)
    {
        lv_Burns.SetActive(activation);
    }

    private void deactivateLevels()
    {
        activate_menu(false);
        activate_lv_RecoveryPosition(false);
        activate_lv_Compression(false);
        activate_lv_Bleeding(false);
        activate_lv_Burns(false);
    }

    public void Play_RecoveryPosition()
    {
        deactivateLevels();
        activate_lv_RecoveryPosition(true);
        if (!offlineMode)
            StartCoroutine(EngAGe.E.startGameplay(idSG, "GameScene"));
    }
    public void Play_Compression()
    {
        deactivateLevels();
        activate_lv_Compression(true);
        if (!offlineMode)
            StartCoroutine(EngAGe.E.startGameplay(idSG, "GameScene"));
    }
    public void Play_Bleeding()
    {
        deactivateLevels();
        activate_lv_Bleeding(true);
        camera_main.SetActive(false);
        camera_bleeding.SetActive(true);
        if (!offlineMode)
            StartCoroutine(EngAGe.E.startGameplay(idSG, "GameScene"));
    }
    public void Play_Burns()
    {
        deactivateLevels();
        activate_lv_Burns(true);
        camera_main.SetActive(false);
        camera_burns.SetActive(true);
    }
    public void MainMenu()
    {
        if (lv_Bleeding.activeSelf == true)
        {
            camera_main.SetActive(true);
            camera_bleeding.SetActive(false);
        }
        if (lv_Burns.activeSelf == true)
        {
            camera_main.SetActive(true);
            camera_burns.SetActive(false);
        }
        deactivateLevels();
        activate_menu(true);
        if (offlineMode == false)
        {
            StartCoroutine(EngAGe.E.getLeaderboard(idSG));
            StartCoroutine(EngAGe.E.getBadgesWon(idSG));
        }
    }
    public void Quit()
    {
        Application.Quit();
    }

    public bool GetIfOffline()
    {
        return offlineMode;
    }

    public void ActionAssessed(JSONNode jsonReturned)
    {
        UpdateScores();
    }

    public void UpdateScores()
    {
        foreach (JSONNode score in EngAGe.E.getScores())
        {
            string scoreName = score["name"];
            string scoreValue = score["value"];
            if (string.Equals(scoreName, "recovery_score"))
            {
                recoveryPositionScore.text = "Score:" + float.Parse(scoreValue).ToString();
            }
            else if (string.Equals(scoreName, "compression_score"))
            {
                compressionScore.text = "Score:" + float.Parse(scoreValue).ToString();
            }
        }
    }
}
