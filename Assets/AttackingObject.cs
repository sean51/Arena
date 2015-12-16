using UnityEngine;
using System.Collections;

public class AttackingObject : MonoBehaviour {

    Player player;
	// Use this for initialization
	void Start () {
        player = transform.root.GetComponent<Player>();
	}

    void SetPlayer(Player p)
    {
        player = p;
    }

	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider c) {
        if (c.tag == "Enemy") {
            if (player != null) player.DealDamage(c.transform.GetComponent<Player>());
            else Debug.LogWarning("AttackObject Player not found");
        }
    }
}
