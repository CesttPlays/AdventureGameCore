using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Quobject.SocketIoClientDotNet.Client;
using Newtonsoft.Json;

public class Sockets : MonoBehaviour {

	private string sUrl = "http://localhost:3000";

	protected Socket m_socket = null;

	Queue<qeueData> aQueue = new Queue<qeueData>();

	GameObject nameTextGO;
	Text nameText;
	public GameObject player;
	public GameObject playerPF;
	public Dictionary<string,GameObject> players = new Dictionary<string,GameObject> ();
	Vector3 tempPosition;
	float angle;
	List<Person> people = new List<Person>();
	public string UID;
	public string name = "";
	// Use this for initialization
	void Start () {
		nameTextGO = GameObject.Find ("nameText");
		nameText = nameTextGO.GetComponent<Text> ();

	}

	// Update is called once per frame
	void Update () {
		
		if (aQueue.Count > 0) {
			qeueData tempData = aQueue.Dequeue ();
			StartCoroutine (tempData.func, tempData.person);
		}
	}

	public void Listen(){
		if (m_socket == null) {
			m_socket = IO.Socket (sUrl);
			tempPosition = player.transform.position;
			angle = player.transform.rotation.z;
			m_socket.On("Connected",(data) =>{
				OnConnected(data.ToString());
			});
			m_socket.On (Socket.EVENT_CONNECT_ERROR, () => {
				Debug.Log ("Error Connecting");
			});	

			m_socket.On("Movement",(data) =>{
				OnMovement(data.ToString());
			});
			m_socket.On ("New Player", (data) =>{
				OnNewPlayer(data.ToString());
			});
			m_socket.On ("Players Connected", (data) =>{
				OnCreatePlayersConnected(data.ToString());
			});
		}

	}

	public void Send(string message, Person data){
		m_socket.Emit(message, JsonConvert.SerializeObject(data));
	}

	void OnConnected(string _UID){
		Debug.Log ("Connected");

		aQueue.Enqueue (new qeueData{
			func = "OnConnectedIEnumerator",
			person = null
		});
		name = nameText.text.ToString ();
		UID = _UID;
		Debug.Log ("UID: " + UID);
		Person tempPerson = new Person ();
		tempPerson.name = name;
		tempPerson.UID = _UID;
		tempPerson.data = new Data () {
			position = new Position(){
				x = tempPosition.x,
				y = tempPosition.y,
				z = tempPosition.z,
				angle = this.angle
			}
		};
		m_socket.Emit("Hello",JsonConvert.SerializeObject(tempPerson));
	}
		

	public IEnumerator OnConnectedIEnumerator(){
		Debug.Log ("connected");
		player.SetActive (true);
		player.GetComponent<PlayerMovement> ().own = true;
		nameTextGO.transform.parent.gameObject.SetActive (false);
		yield return new WaitForEndOfFrame();
	}

	void OnMovement(string data){
		
		try{
			Person tempPerson = JsonConvert.DeserializeObject<Person> (data);
			aQueue.Enqueue(new qeueData{
				func = "OnMovementIenumerator",
				person = tempPerson
			});
		}catch(Exception e){
			Debug.LogError (e);
		}


	}

	IEnumerator OnMovementIenumerator(Person person){
		Debug.Log ("Moving");
		GameObject tempGO = players[person.UID];
		tempGO.transform.position = new Vector3 (person.data.position.x, person.data.position.y, person.data.position.z);
		tempGO.transform.rotation = Quaternion.Euler(0f, 0f, person.data.position.angle);
		yield return new WaitForEndOfFrame();
	}

	void OnNewPlayer(string data){
		Debug.Log ("New player");

		aQueue.Enqueue (new qeueData{
			func = "CreatePlayer",
			person = JsonConvert.DeserializeObject<Person>(data)
		});
	}
	IEnumerator CreatePlayer(Person person){
		GameObject tempGO = Instantiate (playerPF, new Vector3(person.data.position.x , person.data.position.y , person.data.position.z), new Quaternion(0f,0f,person.data.position.angle,0f)) as GameObject;
		tempGO.name = person.name;
		players[person.UID] = tempGO;

		yield return new WaitForEndOfFrame();
	}

	void OnCreatePlayersConnected(string data){
		try{
			List<Person> tempPersons = JsonConvert.DeserializeObject<List<Person>> (data);
			for (int i = 0; i < tempPersons.Count; i++) {
				if(tempPersons[i] != null){
					print(tempPersons[i].name);
					aQueue.Enqueue (new qeueData{
						func = "CreatePlayer",
						person = tempPersons[i]
					});
				}
			}
		}catch(Exception e){
			Debug.LogError(e);
		}

	}
		
}



public struct qeueData{
	public string func;
	public Person person;
}

public class Person{
	public string name { get; set;}
	public string UID { get; set;}
	public Data data { get; set;}
	
}

public class Data{
	public Position position { get; set;}
}


public class Position{
	public float x { get; set;}
	public float y { get; set;}
	public float z { get; set;}
	public float angle { get; set;}
}
