using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour {

    public GameObject Shoot;

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = this.GetComponent<Camera>().ScreenToWorldPoint(Input.mousePosition);
        var target = GameObject.Find("Crosshair");

        target.transform.position = new Vector3(pos.x, pos.y, -9);

        Cursor.visible = false;

        if (Input.GetMouseButtonDown(0))
        {
            Shoot.GetComponent<AudioSource>().Play();
        }
    }
}
