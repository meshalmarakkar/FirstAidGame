using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartBleedingGame : MonoBehaviour {

    public GameObject FirstQuestion;
    public GameObject SecondQuestion;
    public GameObject ThirdQuestion;
    public GameObject FourthQuestion;
    public GameObject EndingGameScreen;

    public void ResetGame()
    {
        FirstQuestion.SetActive(true);
        SecondQuestion.SetActive(false);
        ThirdQuestion.SetActive(false);
        FourthQuestion.SetActive(false);
        EndingGameScreen.SetActive(false);
    }
}
