using UnityEngine;
using System.Collections;

public class Interpolation : MonoBehaviour 
{
	private PhotonView PV;

	private Vector3 correctPlayerPos = Vector3.zero; // We lerp towards this
	private Quaternion correctPlayerRot = Quaternion.identity; // We lerp towards this
	private Quaternion correctShootingRot = Quaternion.identity; // We lerp towards this

	public Animator anim;
	public Transform mesh;

	Quaternion realRotation = Quaternion.identity;

	void Awake ()
	{
		anim = GetComponent<Animator>();
	}

	void Start () 
	{
		PV = GetComponent<PhotonView> ();
	}

	void Update () 
	{
		if (!PV.isMine) 
		{
			SyncedMovement ();
		}
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			stream.SendNext(transform.position);
			stream.SendNext (mesh.rotation);
			stream.SendNext (anim.GetBool ("Moving"));
		}
		else
		{
			correctPlayerPos = (Vector3)stream.ReceiveNext();
			realRotation = (Quaternion)stream.ReceiveNext ();
			anim.SetBool ("Moving", (bool)stream.ReceiveNext ());
		}
	}

	private void SyncedMovement()
	{
		transform.position = Vector3.Lerp(transform.position, correctPlayerPos, Time.deltaTime * 5);
		mesh.rotation = Quaternion.Lerp (mesh.rotation, realRotation, 0.1f);
	}
}
