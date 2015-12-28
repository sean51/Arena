using UnityEngine;
using System;
using System.Collections;

using UnityEditor;

public class NetworkManager : MonoBehaviour 
{
	private const string roomName = "RoomName";
	private RoomInfo[] roomsList;

    [Header("Testing")]
    [Tooltip("This is for testing only. Place the character you want to test in here.")]
    public string character_To_Play_As;

	void Start()
	{
        if (character_To_Play_As == "")
            character_To_Play_As = "Player";
        //PhotonNetwork.sendRate = 500;
        //PhotonNetwork.sendRateOnSerialize = 500;
        PhotonNetwork.ConnectUsingSettings ("0.1");
	}

	void OnGUI()
	{
		if (!PhotonNetwork.connected) 
		{
			GUILayout.Label (PhotonNetwork.connectionStateDetailed.ToString ());
		} 
		else if (PhotonNetwork.room == null) {
			// Create Room
			if (GUI.Button (new Rect (100, 100, 250, 100), "Start Server")) {
				TypedLobby typed_lobby = new TypedLobby ();
				RoomOptions room_options = new RoomOptions () {
					isVisible = true,
					isOpen = true,
					cleanupCacheOnLeave = true,
					maxPlayers = 5
				};
				PhotonNetwork.CreateRoom (roomName + Guid.NewGuid ().ToString ("N"), room_options, typed_lobby);
			}

			// Join Room
			if (roomsList != null) {
				for (int i = 0; i < roomsList.Length; i++) {
					if (GUI.Button (new Rect (100, 250 + (110 * i), 250, 100), "Join " + roomsList [i].name)) {
						PhotonNetwork.JoinRoom (roomsList [i].name);
					}
				}
			}
		} 
		else 
		{
			GUI.Label (new Rect (Screen.width - 200, Screen.height - 24, 200, 80), "" + PhotonNetwork.GetPing ());
		}
	}

	void OnReceivedRoomListUpdate()
	{
		roomsList = PhotonNetwork.GetRoomList();
	}

	void OnJoinedRoom()
	{
		GameObject player = PhotonNetwork.Instantiate (character_To_Play_As, new Vector3 (0, 1, 0), Quaternion.identity, 0);
		//player.transform.GetComponent<Player> ().enabled = true;
		player.transform.GetComponentInChildren<CameraRotate> ().enabled = true;
		player.transform.GetComponentInChildren<Camera> ().enabled = true;
	}

}