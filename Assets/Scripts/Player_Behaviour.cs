using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Behaviour : MonoBehaviour {
	public float SpeedMulti = 3;
	private Rigidbody rb; 
	void Start()
	{
		rb = GetComponent<Rigidbody>();
	}
	void FixedUpdate () {
		float moveX = Input.GetAxis("Horizontal");
		float moveZ = Input.GetAxis("Vertical");
	
		Vector3 movement = new Vector3(moveX , 0.0f ,moveZ);
		
		rb.AddForce(movement*100); 
	}
}
