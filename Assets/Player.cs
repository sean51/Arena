using UnityEngine;
using System.Collections;

public enum PlayerState { run, idle, attacking, dead, takeDamage }

public abstract class Player : MonoBehaviour {

    private int health = 1;
    private int maxHealth = 1;
    private float speed = 1;
    private int defense = 1;
    private int damage = 1;

    public float jumpAmount = 1000;

    Transform attackingObject1;
    Transform attackingObject2;
    Transform attackingObject3;

    GameObject createdObject1;
    GameObject createdObject2;
    GameObject createdObject3;

    public Animator anim;
    CapsuleCollider col;
    Rigidbody body;
    public Transform mesh;

    Quaternion destRot;
    bool left, right, forward, backward, moving;
    public Transform cam;

    PlayerState currentState = PlayerState.idle;

	//Nextworking variables//
	private PhotonView PV;
	Vector3 realPosition = Vector3.zero;
	Quaternion realRotation = Quaternion.identity;
	bool other_moving = false;

    void Start() 
	{
		PV = GetComponent<PhotonView> ();
        col = GetComponent<CapsuleCollider>();
        body = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
		anim.SetBool ("Moving", false);
        StopMovement();
    }


    void FixedUpdate()
    {
		if (PV.isMine) {
			if (currentState != PlayerState.dead && (Input.GetAxis ("Horizontal") != 0 || Input.GetAxis ("Vertical") != 0)) {
				transform.Translate (Input.GetAxis ("Horizontal") * speed / 10, 0, Input.GetAxis ("Vertical") * speed / 10);
			}
		} 
		else 
		{
			//transform.position = Vector3.Lerp (transform.position, realPosition, 0.1f);
			transform.rotation = Quaternion.Lerp (transform.rotation, realRotation, 0.1f);
			//anim.SetBool("Moving", true);
		}
    }

	// Update is called once per frame
	void Update () {
        
		if (PV.isMine) 
		{
			if (health > maxHealth)
				health = maxHealth;


			if (currentState != PlayerState.dead) {
				//Movement
				if (Input.GetAxis ("Horizontal") != 0 || Input.GetAxis ("Vertical") != 0) {
					moving = true;
					anim.SetBool ("Moving", true);

					//Setting rotation states
					if (Input.GetAxis ("Horizontal") < 0)
						left = true;
					else
						left = false;
					if (Input.GetAxis ("Horizontal") > 0)
						right = true;
					else
						right = false;
					if (Input.GetAxis ("Vertical") < 0)
						backward = true;
					else
						backward = false;
					if (Input.GetAxis ("Vertical") > 0)
						forward = true;
					else
						forward = false;

				} else if (moving) {
					anim.SetBool ("Moving", false);
					StopMovement ();
				}
            

				//Rotations
				if (left)
					Rotate (270);
				if (right)
					Rotate (90);
				if (forward)
					Rotate (0);
				if (backward)
					Rotate (180);

				if (left && forward)
					Rotate (315);
				if (left && backward)
					Rotate (225);
				if (right && forward)
					Rotate (45);
				if (right && backward)
					Rotate (135);

				mesh.rotation = Quaternion.Slerp (mesh.rotation, destRot, .25f);
				if (Input.GetMouseButton (0)) {
					transform.eulerAngles = new Vector3 (transform.eulerAngles.x, cam.eulerAngles.y, transform.eulerAngles.z);
					destRot = transform.rotation;
				}


				//Jump
				if (Input.GetButtonDown ("Jump")) {
					Ray ray = new Ray (col.bounds.center, -transform.up);
					RaycastHit hit;
					if (Physics.Raycast (ray, out hit, col.height / 2 + .05f)) {
						if (!hit.collider.isTrigger) {
							body.AddForce (0, jumpAmount, 0);
						}
					}
				}


				//Attack
				if (Input.GetButtonDown ("Ability1")) {
					currentState = PlayerState.attacking;
					Ability1 ();
				}
				if (Input.GetButtonDown ("Ability2")) {
					currentState = PlayerState.attacking;
					Ability2 ();
				}
				if (Input.GetButtonDown ("Ability3")) {
					currentState = PlayerState.attacking;
					Ability3 ();
				}
			}
		}
    }

    void StopMovement() {
        left = false;
        right = false;
        forward = false;
        backward = false;
    }


    void Rotate(int degree)
    {
        destRot = transform.rotation;
        destRot *= Quaternion.Euler(Vector3.up * degree);
    }

    private void TakeDamage(int dmg) {
        if(dmg - defense < 0) health -= dmg - defense;
        if (health <= 0) Death();
    }

    public void DealDamage(Player p) {
        p.TakeDamage(damage);
    }

    private void Death() {
        currentState = PlayerState.dead;
    }

    protected abstract void Ability1();

    protected abstract void Ability2();

    protected abstract void Ability3();

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting) 
		{
			//stream.SendNext (anim.GetFloat ("Moving"));
			//stream.SendNext (transform.position);
			stream.SendNext (mesh.rotation);
			stream.SendNext (anim.GetBool ("Moving"));
		} 
		else 
		{
			//anim.SetFloat ("Moving", (float)stream.ReceiveNext ());
			//realPosition = (Vector3)stream.ReceiveNext();
			realRotation = (Quaternion)stream.ReceiveNext ();
			anim.SetBool ("Moving", (bool)stream.ReceiveNext ());
		}	
	}
}
