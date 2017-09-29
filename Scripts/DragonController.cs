using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragonController : MonoBehaviour {

    public GameObject target;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.RotateAround(target.transform.position, new Vector3(0, -10, 0), 30 * Time.deltaTime);
    }
}
