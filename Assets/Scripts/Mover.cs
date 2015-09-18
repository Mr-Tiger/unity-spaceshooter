using UnityEngine;
using System.Collections;

public class Mover : MonoBehaviour
{
	public float minSpeed;
	public float maxSpeed;

	public float minAngle;
	public float maxAngle;

	void Start ()
	{
		rigidbody.velocity = transform.forward *Random.Range (minSpeed, maxSpeed);
		rigidbody.velocity = rigidbody.velocity +( transform.right *Random.Range (minAngle, maxAngle));
	}
}
