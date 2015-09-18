using UnityEngine;
using System.Collections;

public class DestroyByContact : MonoBehaviour
{
	public GameObject explosion;
	public GameObject playerExplosion;
	public int scoreValue;
	private GameController gameController;
	
	void Start ()
	{
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");
		if (gameControllerObject != null)
		{
			gameController = gameControllerObject.GetComponent <GameController>();
		}
		if (gameController == null)
		{
			Debug.Log ("Cannot find 'GameController' script");
		}
	}
	
	void OnTriggerEnter(Collider other) 
	{
		if (other.tag == "Boundary")
		{
			return;
		}
		if(other.tag == "Asteroid"){
			this.rigidbody.velocity= new Vector3(this.rigidbody.velocity.x * -1 ,0.0f,this.rigidbody.velocity.z);
			return;
		}
	
	Instantiate(explosion, transform.position, transform.rotation);
		if (other.tag == "Boss") {
			Destroy(this.gameObject);
			return;
		}
		if (other.tag == "Player")
		{
			Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
			gameController.GameOver ();
		}
		if (other.tag == "Player_Weapon") {
			gameController.AddScore (scoreValue);
		}
		Destroy(other.gameObject);
		Destroy(gameObject);
	}
}

