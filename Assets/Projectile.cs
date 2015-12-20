using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour 
{
	float time_to_destruction = 5.0f;
	float speed = 10.0f;
	Rigidbody rigid;

	// Use this for initialization
	void Start () 
	{
		rigid = GetComponent<Rigidbody> ();
		rigid.velocity = speed * transform.forward;
	}
	
	// Update is called once per frame
	void Update () 
	{
		time_to_destruction -= Time.deltaTime;
		if (time_to_destruction <= 0.0f)
		{
			PhotonNetwork.Destroy (gameObject);
		}
	}
}
