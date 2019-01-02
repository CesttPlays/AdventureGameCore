using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour {


	[Range(0.1f,100f)]
	public float speed = 1f;

	Renderer m_Renderer;

	void Awake(){
		m_Renderer = GetComponent<Renderer> ();
	}

	void Update () {
		Movement.Move ("FORWARD", this.gameObject, speed, gameObject.transform.eulerAngles);
		if (!m_Renderer.isVisible) {
			Destroy (this.gameObject);
		}
	}

}
