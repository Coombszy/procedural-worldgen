using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(World_Controller))]
public class customButton : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        World_Controller myScript = (World_Controller)target;
        if (GUILayout.Button("ReGenerate"))
        {
            myScript.ReGenerate();
        }
    }
}

public class World_Controller : MonoBehaviour {

	public GameObject player;
	public GameObject ChunkPrefab;
	public int Load_In_Radius = 250;
	public int Load_Out_Radius = 300;
	public int ChunkSize = 50;
	public Vector3 ReloadDistance = new Vector3(35,0,35);

	private List<GameObject> Chunks = new List<GameObject>();	
	private Vector3 LastLocation;
	private List<Vector3> ChunksToRender = new List<Vector3>();
	private List<Vector3> AlreadyRendered = new List<Vector3>();

	public void ReGenerate()
	{
		foreach(GameObject Chunk in Chunks)
		{
			Destroy(Chunk);
		}
		Chunks = new List<GameObject>();
		ChunksToRender = new List<Vector3>();
		AlreadyRendered = new List<Vector3>();
		OneUpdate();
	}

	void Update () {
		if(MovedEnough())
		{
			OneUpdate();
		}
	}

	void OneUpdate()
	{
		ChunksToRender = GetChunksToRender();
			foreach(Vector3 position in ChunksToRender)
			{
				if(!(AlreadyRendered.Contains(position)))
				{
					GameObject NewChunk = (Instantiate(ChunkPrefab, position, Quaternion.identity) as GameObject);
					NewChunk.transform.parent = gameObject.transform;
					Chunks.Add(NewChunk);
					AlreadyRendered.Add(position);
				}
			}
			for(int i = 0; i < Chunks.Count; i++)
			{
				if((Mathf.Abs(LastLocation.x - Chunks[i].transform.position.x) > Load_Out_Radius) || (Mathf.Abs(LastLocation.z - Chunks[i].transform.position.z) > Load_Out_Radius) )
				{
					AlreadyRendered.Remove(Chunks[i].transform.position);
					Destroy(Chunks[i]);
					Chunks.RemoveAt(i);
				}
			}
	}

	void Start() {
		LastLocation = player.transform.position;
		OneUpdate();
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
		//Vector3 RoundedPlayerVector = new Vector3((player.transform.position.x-LocationInChunkX),(player.transform.position.y),(player.transform.position.z-LocationInChunkZ));		
		
		for(int z = ToRenderZNegative; z < ToRenderZPositive; z++)
		{
			for(int x = ToRenderXNegative; x < ToRenderXPositive; x++)
			{
				Vector3 NewChunk = new Vector3(ChunkSize*x,0,ChunkSize*z)+RoundedPlayerVector;
				if(!(AlreadyRendered.Contains(NewChunk)))
				{
					NewChunksToRender.Add(NewChunk);
				}
				else {
				}
			}
		}
		return NewChunksToRender;
	}
}
