using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class World_Controller : MonoBehaviour {

	public GameObject player;
	public int Load_In_Radius = 120;
	public int Load_Out_Radius = 150;
	public Vector3 ReloadDistance = new Vector3(35,0,35);
	public int ChunkSize = 50;
	public GameObject ChunkPrefab;

	private List<GameObject> Chunks;	
	private Vector3 LastLocation;
	private List<Vector3> ChunksToRender = new List<Vector3>();

	void Update () {
		if(MovedEnough())
		{
			ChunksToRender = GetChunksToRender();
			foreach(Vector3 position in ChunksToRender)
			{
				Instantiate(ChunkPrefab, position, Quaternion.identity);
			}
			for(int i = 0; i < Chunks.Count; i++)
			{
				if((Mathf.Abs(LastLocation.x - Chunks[i].transform.position.x) > Load_Out_Radius) && (Mathf.Abs(LastLocation.z - Chunks[i].transform.position.z) > Load_Out_Radius) )
				{
					ChunksToRender.Remove(Chunks[i].transform.position);
					Destroy(Chunks[i]);
					Chunks.RemoveAt(i);
				}
			}
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
		List<Vector3> NewChunksToRender = new List<Vector3>();
		float LocationInChunkX = player.transform.position.x%ChunkSize;
		float LocationInChunkZ = player.transform.position.z%ChunkSize;
		int ToRenderXPositive = (int)(Load_In_Radius+LocationInChunkX-((Load_In_Radius+LocationInChunkX)%ChunkSize))/ChunkSize;
		int ToRenderXNegative = 0-(int)(Load_In_Radius-LocationInChunkX-((Load_In_Radius-LocationInChunkX)%ChunkSize))/ChunkSize;
		int ToRenderZPositive = (int)(Load_In_Radius+LocationInChunkZ-((Load_In_Radius+LocationInChunkZ)%ChunkSize))/ChunkSize;
		int ToRenderZNegative = 0-(int)(Load_In_Radius-LocationInChunkZ-((Load_In_Radius-LocationInChunkZ)%ChunkSize))/ChunkSize;
		Vector3 RoundedPlayerVector = new Vector3((player.transform.position.x-LocationInChunkX),0,(player.transform.position.z-LocationInChunkZ));
		for(int z = ToRenderZNegative; z < ToRenderZPositive; z++)
		{
			for(int x = ToRenderXNegative; x < ToRenderXPositive; x++)
			{
				Vector3 NewChunk = new Vector3(ChunkSize*x,0,ChunkSize*z)+RoundedPlayerVector;
				if(!(ChunksToRender.Contains(NewChunk)))
				{
					NewChunksToRender.Add(NewChunk);
				}
			}
		}
		return NewChunksToRender;
	}
}
