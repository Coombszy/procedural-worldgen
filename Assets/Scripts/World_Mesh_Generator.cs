using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World_Mesh_Generator : MonoBehaviour {
	Mesh mesh;
	
	Vector3[] vertices;
	int[] triangles;
	public int xSize = 100;
	public int zSize = 100;

	// public float WorldNoiseMulti = 4f;
	// public float WorldNoiseScale=0.05f;
	// public float textureNoiseOffset = 0.5f;
	// public float textureNoiseScale = 0.2f;

	public int octaves = 4;
	public float lacunarity = 3f;
	public float persistance = 1.4f;
	public float Perlin_scale = 300f;
	public int seed = 3300;
	public Vector3 octave_offset = new Vector3(100,0,100);

	private Vector3 StartLoc;
	private int COORD_OFFSET= 5908870;
	

	void Start () {
		mesh = new Mesh();
		StartLoc = GetComponent<MeshFilter>().transform.position + new Vector3(COORD_OFFSET, 0, COORD_OFFSET);
		CreateShape();
		UpdateMesh();
		GetComponent<MeshFilter>().mesh = mesh;
		GetComponent<MeshCollider>().sharedMesh = mesh;
		
	}
	// void Update () {
	// 	StartLoc = GetComponent<MeshFilter>().transform.position + new Vector3(COORD_OFFSET, 0, COORD_OFFSET);
	// 	CreateShape();
	// 	UpdateMesh();
	// 	GetComponent<MeshFilter>().mesh = mesh;
	// 	GetComponent<MeshCollider>().sharedMesh = mesh;
		
	// }
	
	void CreateShape () {
		vertices = new Vector3[(xSize+1) * (zSize+1)];
		int i = 0;


		System.Random prng = new System.Random (seed);
		Vector2[] octaveOffsets = new Vector2[octaves];
		for (int x = 0; x < octaves; x++) {
			float offsetX = prng.Next (-100000, 100000) + octave_offset.x;
			float offsetY = prng.Next (-100000, 100000) + octave_offset.y;
			octaveOffsets [x] = new Vector2 (offsetX, offsetY);
		}

		if (Perlin_scale <= 0) {
			Perlin_scale = 0.0001f;
		}

		float maxNoiseHeight = float.MinValue;
		float minNoiseHeight = float.MaxValue;

		for (int z = (int)Mathf.Floor(StartLoc.z); z <= zSize + (int)Mathf.Floor(StartLoc.z); z ++)
		{
			for (int x = (int)Mathf.Floor(StartLoc.x); x <= xSize + (int)Mathf.Floor(StartLoc.x); x++)
			{


				float amplitude = 1;
				float frequency = 1;
				float noiseHeight = 0;

				for (int n = 0; n < octaves; n++) {
					float sampleX = (x-(xSize/2)) / Perlin_scale * frequency + octaveOffsets[n].x;
					float sampleY = (z-(zSize/2)) / Perlin_scale * frequency + octaveOffsets[n].y;

					float perlinValue = Mathf.PerlinNoise (sampleX, sampleY) * 2 - 1;
					noiseHeight += perlinValue * amplitude;

					amplitude *= persistance;
					frequency *= lacunarity;
					}
				if (noiseHeight > maxNoiseHeight) {
					maxNoiseHeight = noiseHeight;
				} else if (noiseHeight < minNoiseHeight) {
					minNoiseHeight = noiseHeight;
				}
				vertices[i] = new Vector3(x-((int)Mathf.Floor(StartLoc.x)+xSize/2),noiseHeight,z-((int)Mathf.Floor(StartLoc.z)+zSize/2));
				i++;

			}
		}
		triangles = new int[zSize * xSize* 6];
		int vert = 0, tris = 0;
		for (int z = 0; z < zSize; z++)
		{		
			for (int x = 0; x < xSize; x++)
			{
				triangles[tris + 0] = vert + 0;
				triangles[tris + 1] = vert + 1 + xSize;
				triangles[tris + 2] = vert + 1;
				triangles[tris + 3] = vert + 1;
				triangles[tris + 4] = vert + 1 + xSize;
				triangles[tris + 5] = vert + 2 + xSize;
				vert++;tris+=6;
			}
			vert++;
			}
	}

	void UpdateMesh()
	{
		mesh.Clear();
		
		mesh.vertices = vertices;
		mesh.triangles = triangles;

		mesh.RecalculateNormals();
	}


	// void CreateShapeOLD () {
	// 	vertices = new Vector3[(xSize+1) * (zSize+1)];
	// 	int i = 0;
	// 	for (int z = (int)Mathf.Floor(StartLoc.z); z <= zSize + (int)Mathf.Floor(StartLoc.z); z ++)
	// 	{
	// 		for (int x = (int)Mathf.Floor(StartLoc.x); x <= xSize + (int)Mathf.Floor(StartLoc.x); x++)
	// 		{
	// 			float WorldNoise = Mathf.PerlinNoise(x*WorldNoiseScale, z*WorldNoiseScale)*WorldNoiseMulti;
	// 			float TextureNoise = (Mathf.PerlinNoise(x*textureNoiseScale,z*textureNoiseScale)*textureNoiseOffset*2)-(textureNoiseOffset);
	// 			float y = WorldNoise+TextureNoise;
	// 			vertices[i] = new Vector3(x-((int)Mathf.Floor(StartLoc.x)+xSize/2),y,z-((int)Mathf.Floor(StartLoc.z)+zSize/2));
	// 			i++;
	// 		}
	// 	}
	// 	triangles = new int[zSize * xSize* 6];
	// 	int vert = 0, tris = 0;
	// 	for (int z = 0; z < zSize; z++)
	// 	{
				
		
	// 	for (int x = 0; x < xSize; x++)
	// 	{
	// 		triangles[tris + 0] = vert + 0;
	// 		triangles[tris + 1] = vert + 1 + xSize;
	// 		triangles[tris + 2] = vert + 1;
	// 		triangles[tris + 3] = vert + 1;
	// 		triangles[tris + 4] = vert + 1 + xSize;
	// 		triangles[tris + 5] = vert + 2 + xSize;
	// 		vert++;tris+=6;
	// 	}
	// 	vert++;
	// 	}
	// }
}
