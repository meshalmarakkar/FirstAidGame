using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableController : MonoBehaviour {

    public GameObject step1;
    public GameObject step2;
    public GameObject step3;

    public void CreateCollectables()
    {
        for (int i = 0; i < 30; i++)
        {
            float randX = Random.Range(-3.0f, 3.0f);
            float randZ = Random.Range(3.0f, 290.0f);
            Instantiate(step1, new Vector3(randX, 0.75f, randZ), Quaternion.identity);

            randX = Random.Range(-3.0f, 3.0f);
            randZ = Random.Range(3.0f, 290.0f);
            Instantiate(step2, new Vector3(randX, 0.75f, randZ), Quaternion.identity);

            randX = Random.Range(-3.0f, 3.0f);
            randZ = Random.Range(3.0f, 290.0f);
            Instantiate(step3, new Vector3(randX, 0.75f, randZ), Quaternion.identity);
        }
    }

    public void ResetGame()
    {
        //get an array of GameObjects that share the same tag.Then you can iterate the array and delete them.
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Recovery_PickUpStep1");
        for (int i = 0; i < objects.Length; i++)
        {
            Destroy(objects[i]);
        }
        objects = GameObject.FindGameObjectsWithTag("Recovery_PickUpStep2");
        for (int i = 0; i < objects.Length; i++)
        {
            Destroy(objects[i]);
        }
       objects = GameObject.FindGameObjectsWithTag("Recovery_PickUpStep3");
        for (int i = 0; i < objects.Length; i++)
        {
            Destroy(objects[i]);
        }
    }
}
