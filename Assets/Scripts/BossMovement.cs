using UnityEngine;
using System.Collections;


public class BossMovement : MonoBehaviour
{
	public GameController gameController;
	public GameObject playerExplosion;

	public float hp;
	public bool isDying=false;

	public float speed;
	public float tilt;
	public Boundary boundary;

	float moveHorizontal;
	float moveVertical;
	public float waitBetweenMoving;
	public bool moveRight;
	
	public GameObject shot;
	public Transform shotSpawn;
	public GameObject fakeShot;
	public Transform fakeShotSpawn;
	public float fireRate;

	public GameObject mainEngine;
	public GameObject frontLeftEngine;
	public GameObject frontRightEngine;
	public GameObject frontLeftEnergy;
	public GameObject frontRightEnergy;
	public GameObject explode;
	public AudioClip playOnDealth;


	void Start(){
		StartCoroutine (changeDirection());
	}

	IEnumerator startFiring (){
		frontLeftEnergy.SetActive (true);
		frontRightEnergy.SetActive (true);
		yield return new WaitForSeconds (.75f);
		while (true) {
			Instantiate(fakeShot, fakeShotSpawn.position, shotSpawn.rotation);
			audio.Play ();
			yield return new WaitForSeconds (.25f);
			Instantiate(shot, shotSpawn.position, shotSpawn.rotation);

			yield return new WaitForSeconds (fireRate-.25f);
			if (gameController.gameOver == true) {
				break;
			}
			if (isDying == true) {
				break;
			}
		}
	}

	IEnumerator changeDirection (){
		//intro to the scene
		float tempZ = boundary.zMax;
		boundary.zMax = 23;
		yield return new WaitForSeconds (20);
		frontLeftEngine.SetActive (false);
		frontRightEngine.SetActive (false);
		moveVertical = -1;

		yield return new WaitForSeconds (5);
		boundary.zMax = tempZ;
		StartCoroutine (startFiring());
		frontLeftEngine.SetActive (true);
		frontRightEngine.SetActive (true);

		mainEngine.SetActive(false);

		//movement

		while(true){
			if(moveRight==true ){
				moveHorizontal = Random.Range(.4f,1);
			}
			if(moveRight==false ){
				moveHorizontal = Random.Range(-1,.4f);
			}
			if(this.transform.position.x>4.5){
				moveRight=false;
			}
			if(this.transform.position.x<-4.5){
				moveRight=true;
			}
			 moveVertical = Random.Range(-.75f,.5f);

			yield return new WaitForSeconds (waitBetweenMoving);
			if(gameController.gameOver == true){
				moveVertical = -2;
				boundary.zMin=-12;
				boundary.xMin=boundary.xMin-5;
				boundary.xMax=boundary.xMax+5;
				frontLeftEngine.SetActive (false);
				frontRightEngine.SetActive (false);
				frontLeftEnergy.SetActive (false);
				frontRightEnergy.SetActive (false);
				mainEngine.SetActive(true);
				break;
			}
			if (isDying == true) {
				explode.SetActive(true);
				audio.clip=playOnDealth;
				audio.loop=true;
				audio.Play();
				yield return new WaitForSeconds (2);
				rigidbody.angularVelocity = new Vector3(200,0f,200);
				moveVertical = -2;
				frontLeftEngine.SetActive (false);
				frontRightEngine.SetActive (false);
				frontLeftEnergy.SetActive (false);
				frontRightEnergy.SetActive (false);
				mainEngine.SetActive(true);
				boundary.zMin=-30;
				boundary.xMin=boundary.xMin-5;
				boundary.xMax=boundary.xMax+5;
				yield return new WaitForSeconds (4);
				audio.loop=false;
				yield return new WaitForSeconds (1);
				GameObject.Destroy(this.gameObject);

				break;
			}
		}

	}

	void FixedUpdate ()
	{
		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		rigidbody.velocity = movement * speed;
		
		rigidbody.position = new Vector3
			(
				Mathf.Clamp (rigidbody.position.x, boundary.xMin, boundary.xMax), 
				0f, 
				Mathf.Clamp (rigidbody.position.z, boundary.zMin, boundary.zMax)
				);
		if (isDying == false) {
			rigidbody.rotation = Quaternion.Euler (0.0f, 180, rigidbody.velocity.x * -tilt);
		}
	}
	void OnTriggerEnter(Collider other) {

		if (other.tag == "Player")
		{
			Destroy(other.gameObject);
			Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
			gameController.GameOver ();
		}

		if (other.tag == "Player_Weapon") {
			hp=hp-1;
			if(hp==0){
				gameController.AddScore(100);
				isDying=true;
				//instatiate boss dead
			}
			Destroy(other.gameObject);
		}
	}
}
