using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMovement : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float xPos = Random.Range(-7, 7);
        float yVel = Random.Range(9, 14);
        float xVel = Random.Range(-4, 4);


        if (transform.position.y < -6)
        {
            xPos = Random.Range(-7, 7);
            if (xPos < -3)
                xVel = Random.Range(-1, 6);
            if (xPos > 3)
                xVel = Random.Range(-6, 1);

            transform.position = new Vector2(xPos, -6);

            GetComponent<Rigidbody2D>().velocity = new Vector2(xVel, yVel);
        }


    }
}
