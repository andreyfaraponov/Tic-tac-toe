using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu_mover : MonoBehaviour {

    RectTransform my;
	// Use this for initialization
	void Start () {
        my = gameObject.GetComponent<RectTransform>();
	}
	
	// Update is called once per frame
	void Update () {
        //my.transform.Translate(new Vector3(0, 0, 0) * Time.deltaTime);
        //my.transform.Translate(Vector3.forward * Time.deltaTime);
        if (my.position.y < 150)
            my.transform.Translate(my.up * Time.deltaTime * 75, Space.World);
    }
}
