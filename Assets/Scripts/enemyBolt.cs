using UnityEngine;
using System.Collections;

public class enemyBolt : MonoBehaviour {

	public GameObject playerExplosion;
	public GameController gameController;

	void Start(){
		gameController =(GameController) GameObject.FindObjectOfType (typeof(GameController));
	}

	void OnTriggerEnter(Collider other) 
	{
		if (other.tag == "Player")
		{
			Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
			gameController.GameOver ();
			Destroy(other.gameObject);
			Destroy(gameObject);
		}

	}
}
