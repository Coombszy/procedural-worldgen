using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class World_Controller : MonoBehaviour {

	public GameObject player;
	public int Radius = 200;
	public Vector3 ReloadDistance = new Vector3(10,0,10);

	private List<GameObject> Chunks;	
	private Vector3 LastLocation;

	void Update () {
		if(MovedEnough())
		{
			List<Vector3> ToRender = GetChunksToRender();
		}
	}
	
	void Start() {
		LastLocation = player.transform.position;
	}
	
	bool MovedEnough()
	{
		if((Mathf.Abs((player.transform.position - LastLocation).x) > ReloadDistance.x || Mathf.Abs((player.transform.position - LastLocation).z) > ReloadDistance.z ))
		{
			LastLocation = player.transform.position;
			return true;
		}
		else{
			return false;
		}
	}

	List<Vector3> GetChunksToRender()
	{
		return new List<Vector3>();
	}
}
