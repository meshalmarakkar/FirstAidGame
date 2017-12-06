using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class TEST_Click2 : MonoBehaviour {

    public MenuController gameController;

    void OnMouseOver()
    {

        if (Input.GetMouseButtonDown(0))
        {
            JSONNode vals = JSON.Parse("{\"correct\" : \"" + "subtract_score" + "\" }"); //"others" compared to add_score so -25
            // ask EngAGe to assess the action based on the config file
            StartCoroutine(EngAGe.E.assess("bleeding_correctAnswer", vals, gameController.ActionAssessed));
            Destroy(gameObject);
            print("INCORRECT");
        }

    }
}
