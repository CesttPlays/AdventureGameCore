using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Movement : MonoBehaviour {

	public Sockets socketScript;

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
			Person person = new Person ();
			person.UID = socketScript.UID;
			person.name = socketScript.name;
			person.data = new Data () {
				position = new Position(){
					x = goToMove.transform.position.x,
					y = goToMove.transform.position.y,
					z = goToMove.transform.position.z,
					angle = goToMove.transform.eulerAngles.z
				}
			};
			socketScript.Send ("Movement",person);
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
