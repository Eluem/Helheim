using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputTesting : MonoBehaviour
{
	public Vector2 StickAxis = Vector2.zero;

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
        StickAxis.x = Input.GetAxis("Horizontal_test");


        float val = 0;
		val = Input.GetAxis("Horizontal_test");

        transform.position = new Vector3(transform.position.x + val * Time.deltaTime, transform.position.y, transform.position.z);
    }
}
