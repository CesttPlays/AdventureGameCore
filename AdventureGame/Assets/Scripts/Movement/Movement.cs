using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Movement : MonoBehaviour {

	public Sockets socketScript; 
	string data;

	void Start(){
		socketScript = this.gameObject.GetComponent<Sockets> ();
	}

	public void Move(string dir, GameObject goToMove , float speed){
		
		switch (dir) {

		case "UP":
			{
				goToMove.transform.eulerAngles = new Vector3 (goToMove.transform.eulerAngles.x,goToMove.transform.eulerAngles.y,0);
				goToMove.transform.position += goToMove.transform.up * Time.deltaTime * speed;
				break;
			}
		case "DOWN":
			{
				goToMove.transform.eulerAngles = new Vector3 (goToMove.transform.eulerAngles.x,goToMove.transform.eulerAngles.y,180);
				goToMove.transform.position += goToMove.transform.up * Time.deltaTime * speed;

				break;
			}
		case "LEFT":
			{
				goToMove.transform.eulerAngles = new Vector3 (goToMove.transform.eulerAngles.x,goToMove.transform.eulerAngles.y,90);
				goToMove.transform.position += goToMove.transform.up * Time.deltaTime * speed;

				break;
			}
		case "RIGHT":
			{
				goToMove.transform.eulerAngles = new Vector3 (goToMove.transform.eulerAngles.x,goToMove.transform.eulerAngles.y,270);
				goToMove.transform.position += goToMove.transform.up * Time.deltaTime * speed;

				break;
			}
		default : {
				System.Console.WriteLine ("Input incorrecto");
				break;
			}

		}
		if (socketScript != null) {
			data = goToMove.transform.position.ToString ().Split('(', ')')[1];
			socketScript.Send ("Movement",data );
			Debug.Log ("Sending Movement");
		}
			
		


	}

	public static void Move(string dir, GameObject goToMove , float speed, Vector3 angle){

		switch (dir) {

		case "FORWARD":
			{
				goToMove.transform.eulerAngles = angle;
				goToMove.transform.position += goToMove.transform.up * Time.deltaTime * speed;

				break;
			}
		default : {
				System.Console.WriteLine ("Input incorrecto");
				break;
			}

		}

	}


}
