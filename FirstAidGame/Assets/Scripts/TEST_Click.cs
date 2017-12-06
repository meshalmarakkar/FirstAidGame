using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SimpleJSON;

public class TEST_Click : MonoBehaviour {

    public MenuController gameController;
    public GameObject FirstQuestion;
    public GameObject SecondQuestion;
    public GameObject ThirdQuestion;
    public GameObject FourthQuestion;
    public GameObject EndingGameScreen;
    public GameObject BackButton; 

    void OnMouseOver()
    {
        
        if (Input.GetMouseButtonDown(0))
        {
            if (gameController.GetIfOffline() == false)
            {
                JSONNode vals = JSON.Parse("{\"correct\" : \"" + "add_score" + "\" }");
                // ask EngAGe to assess the action based on the config file
                StartCoroutine(EngAGe.E.assess("bleeding_correctAnswer", vals, gameController.ActionAssessed));
            }
            if (FirstQuestion.activeSelf == true)
            {
                FirstQuestion.SetActive(false);
                SecondQuestion.SetActive(true);
            } else if (SecondQuestion.activeSelf == true)
            {
                SecondQuestion.SetActive(false);
                ThirdQuestion.SetActive(true);
            } else if (ThirdQuestion.activeSelf == true)
            {
                ThirdQuestion.SetActive(false);
                FourthQuestion.SetActive(true);
            } else if (FourthQuestion.activeSelf == true)
            {
                FourthQuestion.SetActive(false);
                EndingGameScreen.SetActive(true);
                BackButton.SetActive(false);
            }
        }

    }
}
