using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using SimpleJSON;

public class TrophyController : MonoBehaviour {

    public Texture trophyImage;
	
	// Update is called once per frame
	void Update () {
        // get name of the badge represented
        string badgeName = this.name.ToString();

        // if the badge is in EngAGe returned list, use the active image 
        foreach (JSONNode b in EngAGe.E.getBadges())
        {
            if (string.Equals(b["name"], badgeName) && b["earned"].AsBool)
            {
                this.GetComponent<RawImage>().texture = trophyImage;
            }
        }
    }
}
