using UnityEngine;
using System.Collections;

public enum PlayerState { run, idle, attacking, dead, takeDamage }

public abstract class Player : MonoBehaviour {

    private int health = 1;
    private int maxHealth = 1;
    private int speed = 1;
    private int defense = 1;
    private int damage = 1;

    public float jumpAmount = 1000;

    Transform attackingObject1;
    Transform attackingObject2;
    Transform attackingObject3;

    GameObject createdObject1;
    GameObject createdObject2;
    GameObject createdObject3;

    Animator anim;
    CapsuleCollider col;
    Rigidbody body;

    PlayerState currentState = PlayerState.idle;

    void Start() {
        col = GetComponent<CapsuleCollider>();
        body = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }
    
	// Update is called once per frame
	void Update () {
        if (health > maxHealth) health = maxHealth;


        if (currentState != PlayerState.dead)
        {
            //Movement
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                anim.SetBool("Moving", true);
                transform.Translate(Input.GetAxis("Horizontal") * speed, 0, Input.GetAxis("Vertical") * speed);

            }
            else
            {
                anim.SetBool("Moving", false);
            }

            if (Input.GetButtonDown("Jump"))
            {
                Ray ray = new Ray(col.bounds.center, -transform.up);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, col.height / 2 + .05f)) {
                    if (!hit.collider.isTrigger)
                    {
                        Debug.Log("Jump Ready");
                        body.AddForce(0, jumpAmount, 0);
                    }
                }
            }


            //Attack
            if (Input.GetButtonDown("Ability1"))
            {
                currentState = PlayerState.attacking;
                Ability1();
            }
            if (Input.GetButtonDown("Ability2"))
            {
                currentState = PlayerState.attacking;
                Ability2();
            }
            if (Input.GetButtonDown("Ability3"))
            {
                currentState = PlayerState.attacking;
                Ability3();
            }
        }
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





}
