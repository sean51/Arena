using UnityEngine;
using System.Collections;

public class Prediction : MonoBehaviour 
{
	private float lastSynchronizationTime = 0f;
	private float syncDelay = 0f;
	private float syncTime = 0f;
	private Vector3 syncStartPosition = Vector3.zero;
	private Vector3 syncEndPosition = Vector3.zero;

	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			stream.SendNext (GetComponent<Rigidbody>().position);
			stream.SendNext (GetComponent<Rigidbody>().velocity);
		}
		else
		{
			Vector3 syncPosition = (Vector3)stream.ReceiveNext ();
			Vector3 syncVelocity = (Vector3)stream.ReceiveNext ();

			syncTime = 0f;
			syncDelay = Time.time - lastSynchronizationTime;
			lastSynchronizationTime = Time.time;

			syncEndPosition = syncPosition + syncVelocity * syncDelay;
			syncStartPosition = GetComponent<Rigidbody>().position;
		}
	}
}
