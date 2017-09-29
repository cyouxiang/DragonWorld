using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float moveSpeed = 10f;
    public float turnSpeed = 50f;
    private const int keyCount = 450;           // Max KeyCode
    //private KeyCode mButtonKeyCode;

    void Start () {
        //mButtonKeyCode = (KeyCode)System.Enum.Parse(typeof(KeyCode), mButtonName);
    }
	
	void Update () {
		if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0) {
            transform.Translate(Vector3.forward * Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime);
            if (Input.GetKey(KeyCode.JoystickButton2)) {
                //print("2");
                transform.Rotate(Vector3.up, Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime);
            } else {
                transform.Translate(Vector3.right * Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime);
            }
            
        }
            
        //if (Input.anyKey) {
        //    for (int i = 1; i < keyCount; i++) {
        //        if (Input.GetKeyDown((KeyCode)i)) {
        //            print(((KeyCode)i));
        //        }
        //    }
        //}

        if (Input.GetKey(KeyCode.JoystickButton4)) {
            transform.Translate(Vector3.up * moveSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.JoystickButton5)) {
            transform.Translate(Vector3.down * moveSpeed * Time.deltaTime);
        }
	}
}
