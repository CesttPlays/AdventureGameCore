using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using Quobject.SocketIoClientDotNet.Client;

public class Sockets : MonoBehaviour {

	private string sUrl = "http://localhost:3000";

	protected Socket m_socket = null;
	Queue<string> aQueue = new Queue<string>();
	string name = "";
	GameObject nameTextGO;
	Text nameText;
	public GameObject player;
	public Dictionary<float,GameObject> players = new Dictionary<float,GameObject> ();
	System.Random rnd = new System.Random();
	// Use this for initialization
	void Start () {
		nameTextGO = GameObject.Find ("nameText");
		nameText = nameTextGO.GetComponent<Text> ();

	}

	// Update is called once per frame
	void Update () {
		

		if (aQueue.Count > 0) {
			String tempAction = aQueue.Dequeue ();
			string[] datas = tempAction.Split (';');
			if (datas.Length > 1) {
				StartCoroutine (datas [0], datas [1]);
			} else {
				StartCoroutine (datas [0]);
			}


		}
	}

	public void Listen(){
		if (m_socket == null) {
			m_socket = IO.Socket (sUrl);

			m_socket.On (Socket.EVENT_CONNECT, OnConnect);

			m_socket.On (Socket.EVENT_CONNECT_ERROR, () => {
				Debug.Log ("Error Connecting");
			});	

			m_socket.On("Movement",(data) =>{
				OnMovement(data.ToString());
			});
			m_socket.On ("New Player", (data) =>{
				OnNewPlayer(data.ToString());
			});
		}

	}

	public void Send(string message, string data){
		m_socket.Emit (message, data);
	}
	void OnConnect(){
		Debug.Log ("Connected");
		aQueue.Enqueue ("OnLogin");
		name = nameText.text.ToString ();
		string tempString = name +","+ rnd.NextDouble().ToString();
		m_socket.Emit("Hello",tempString);
	}


	public Socket getSocket(){
		return m_socket;
	}

	public IEnumerator OnLogin(){
		Debug.Log ("Hola");
		player.SetActive (true);
		nameTextGO.transform.parent.gameObject.SetActive (false);
		yield return new WaitForEndOfFrame();
	}
	void OnMovement(string data){
		Debug.Log ("Moving");
		string[] datas = data.Split (',');


	}

	void OnNewPlayer(string data){
		Debug.Log ("New player");
		aQueue.Enqueue ("CreatePlayer" + ";" + data);
	}
	IEnumerator CreatePlayer(string data){
		Debug.Log ("Create Player");
		string[] datas = data.Split (',');
		GameObject tempGO = Instantiate (player, Vector3.zero, Quaternion.identity) as GameObject;
		tempGO.name = datas [0];
		players.Add (float.Parse(datas [1]), tempGO);
		yield return new WaitForEndOfFrame();
	}



}
