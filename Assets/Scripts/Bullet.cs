using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {


    public GameObject firedFrom;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision) {
        if (collision.rigidbody && collision.rigidbody.gameObject == firedFrom) {
            Debug.Log("We hit the gun!");
        }

        else {

            if (collision.gameObject.GetComponent<Soldier>()) {
                collision.gameObject.GetComponent<Soldier>().Kill();
            }
        }

        Destroy(gameObject);
    }
}
