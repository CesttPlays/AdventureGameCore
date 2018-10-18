using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {

	[Range(0.1f,100f)]
	public float speed = 1f;

	public GameObject bullet;
	Movement move; 
	void Start () {
		move = GameObject.Find ("GameManager").GetComponent<Movement> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.W)){
			move.Move ("UP", this.gameObject, speed);
		}else if(Input.GetKey(KeyCode.S)){
			move.Move ("DOWN", this.gameObject, speed);
		}else if(Input.GetKey(KeyCode.A)){
			move.Move ("LEFT", this.gameObject, speed);
		}else if(Input.GetKey(KeyCode.D)){
			move.Move ("RIGHT", this.gameObject, speed);
		}else if(Input.GetKey(KeyCode.Space)){
			GameObject tempBullet = Instantiate (bullet, this.transform.position, this.transform.rotation) as GameObject;
		}
	}
}
