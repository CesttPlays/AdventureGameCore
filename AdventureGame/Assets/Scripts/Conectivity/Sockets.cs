using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Quobject.SocketIoClientDotNet.Client;

public class Sockets : MonoBehaviour {

	private string sUrl = "http://localhost:3000";

	protected Socket m_socket = null;
	Queue<string[]> sQeue = new Queue<string[]>();
	string name = "";
	GameObject nameTextGO;
	Text nameText;
	public GameObject player;
	// Use this for initialization
	void Start () {
		nameTextGO = GameObject.Find ("nameText");
		nameText = nameTextGO.GetComponent<Text> ();

	}

	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Return)) {
			name = nameText.text.ToString ();
			player.SetActive (true);
			nameTextGO.transform.parent.gameObject.SetActive (false);
			Listen ();
		}
		if (sQeue.Count > 0) {
			string[] datas = sQeue.Dequeue (); 
			player.transform.position = new Vector3 (float.Parse(datas [0]), float.Parse(datas [1]), float.Parse(datas [2]));
		}
	}

	void Listen(){
		if (m_socket == null) {
			m_socket = IO.Socket (sUrl);

			m_socket.On (Socket.EVENT_CONNECT, OnConnect);

			m_socket.On (Socket.EVENT_CONNECT_ERROR, () => {
				Debug.Log ("Error Connecting");
			});	

			m_socket.On("Movement",(data) =>{
				OnMovement(data.ToString());
			});
		}

	}

	public void Send(string message, string data){
		m_socket.Emit (message, data);
	}
	void OnConnect(){
		Debug.Log ("Connected");
		m_socket.Emit("Hello",name);
	}
	void OnMovement(string data){
		Debug.Log ("Moving");
		string[] datas = data.Split (',');
		sQeue.Enqueue (datas);
		
	}

	public Socket getSocket(){
		return m_socket;
	}

}
