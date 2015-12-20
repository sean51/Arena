using UnityEngine;
using System.Collections;

public class Ethanb : Player 
{
	protected override void Start ()
	{
		base.Start();
		attackingObject1 = Get_Child("Ethan_Light");
		attackingObject2 = Get_Child ("EthanBody");
	}

    protected override void Ability1() 
	{
		attackingObject1.GetComponent<Light> ().enabled = !attackingObject1.GetComponent<Light> ().enabled;
    }

    protected override void Ability2()
    {
		GameObject projectile_object = PhotonNetwork.Instantiate ("Ethan_Projectile", attackingObject2.position + mesh.forward, mesh.rotation, 0);
    }

    protected override void Ability3()
    {

    }

}
