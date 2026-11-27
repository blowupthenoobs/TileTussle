using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using MapStuff;

public class TileManagerScript : MonoBehaviour
{
    public static TileManagerScript Instance;
    // [SerializeField] public int width, height;

    [SerializeField] private GameObject tilePrefab;
    [SerializeField] GameObject cameraObject;
    [SerializeField] MapItem mapData;
    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    [SerializeField] private GameObject tree;
    [SerializeField] private GameObject rock;


    [SerializeField] private GameObject testMarker;

    
    public List<List<GameObject>> tiles = new List<List<GameObject>>();

    public int shoots;

    void Awake()
    {
        if(Instance == null)
            Instance=this;
        else
            Destroy(gameObject);

        GenerateGrid();
        RandomlyGenerateObstical(3, rock);
    }

    void GenerateGrid()
    {
        for(int x = 0; x < mapData.map.matrix.Count; x++)
        {
            tiles.Add(new List<GameObject>());

            for(int y = 0; y < mapData.map.matrix[x].tiles.Count; y++)
            {
                var newTile = Instantiate(tilePrefab, new Vector3(x, y), transform.rotation);
                tiles[x].Add(newTile);
                
                newTile.transform.SetParent(transform);
                newTile.name = "(" + x.ToString() + ", " + y.ToString() + ")";
                newTile.GetComponent<TileScript>().Spawn(mapData.map.getTile(x, mapData.map.matrix[x].tiles.Count - (y + 1)));
            }
        }

        SetCameraPosition((float)mapData.map.matrix.Count / 2, (float)mapData.map.matrix[0].tiles.Count / 2);
        CalculateBounds();
    }

    void SetCameraPosition(float x, float y)
    {
        var newPos = new Vector3(x -.5f, y -.5f, -10);
        cameraObject.transform.position = newPos;
    }

    void CalculateBounds()
    {
        CameraControls.upperBounds = new Vector2(tiles.Count - 0.5f, tiles[tiles.Count - 1].Count - .5f);
        CameraControls.lowerBounds = new Vector2(-.5f, -.5f);
        Instantiate(testMarker, CameraControls.upperBounds, transform.rotation);
        Instantiate(testMarker, CameraControls.lowerBounds, transform.rotation);

        CameraControls.UpdateBounds();
    }

    public GameObject GetTile(int x, int y)
    {
        if(x < 0 || x >= tiles.Count)
            return null;
        if(y < 0 || y >= tiles[x].Count)
            return null;
        
        return tiles[x][y];
    }

    void RandomlyGenerateObstical(int spawns, GameObject item)
    {
        for(int z = 0; z < spawns; z++)
        {
            bool foundSpot = false;
            for(int i = 0; i < shoots; i++)
            {
                if(!foundSpot)
                {
                    var x = Random.Range(0, mapData.map.matrix.Count);
                    var y = Random.Range(0, mapData.map.matrix[x].tiles.Count);

                    if(tiles[x][y].GetComponent<TileScript>().heldItem == null && tiles[x][y].GetComponent<TileScript>().tileType != TileType.empty)
                    {
                        GameObject newItem = Instantiate(item, new Vector3(x, y), transform.rotation);
                        tiles[x][y].GetComponent<TileScript>().heldItem = newItem;
                        foundSpot = true;

                        newItem.transform.SetParent(gameObject.transform);
                    }
                }
            }
        }
    }

    void RandomlyGenerateBigObstical(int spawns, GameObject item)
    {
        for(int z = 0; z < spawns; z++)
        {
            bool foundSpot = false;
            for(int i = 0; i < shoots; i++)
            {
                bool tilesUnoccupied = true;

                if(!foundSpot)
                {
                    Vector2 size = item.transform.localScale;

                    int x = Random.Range(0, (int)(mapData.map.matrix.Count - size.x));
                    int y = Random.Range(0, (int)(mapData.map.matrix[x].tiles.Count - size.y));

                    for(int a = 0; a < size.x; a++)
                    {
                        for(int b = 0; b < size.y; b++)
                        {
                            if(tiles[x + a][y + b].GetComponent<TileScript>().heldItem != null && tiles[x + a][y + b].GetComponent<TileScript>().tileType != TileType.empty)
                            {
                                tilesUnoccupied = false;
                            }
                        }
                    }

                    if(tilesUnoccupied)
                    {
                        GameObject newItem = Instantiate(item, new Vector3(x + (size.x - 1)/2, y + (size.y - 1)/2), transform.rotation);

                        for(int a = 0; a < size.x; a++)
                        {
                            for(int b = 0; b < size.y; b++)
                                tiles[x + a][y + b].GetComponent<TileScript>().heldItem = newItem;
                        }

                        foundSpot = true;

                        newItem.transform.SetParent(gameObject.transform);
                    }
                }
            }
        }
    }
}
