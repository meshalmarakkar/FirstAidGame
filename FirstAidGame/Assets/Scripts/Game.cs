using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{

    static float timer = 0f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        timer = timer + Time.deltaTime;

        if (timer >= 2f && timer <= 2.5f)
        {
           

        }
    }
}
