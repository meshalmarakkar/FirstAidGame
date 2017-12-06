using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sprite_Back_Button_Win : MonoBehaviour
{
    public MenuController gameController;

    void OnMouseOver()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (gameController.GetIfOffline() == false)
                StartCoroutine(EngAGe.E.endGameplay(true));
            Cursor.visible = true;
            gameController.MainMenu();
        }
    }
}