using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] private GameObject piece, chick, asteroid;
    [SerializeField] private float pieceSpawnRate, chickSpawnRate, asterSpawnRate, powerSpawnRate;

    // List to hold pool objects
    [SerializeField] private List<PoolObject> poolList = new List<PoolObject>();

    private bool spawnPause;
    private float timer, startTime, minute = 60;
    private int level = 1, random;
    private GameObject poolObjects;

    // Array of spawn points
    private Transform[] spawnPts;
    private Transform targetSpawnPt;

    public static PoolManager instance;

    // Delegate and event for speed increase
    public delegate void SpeedIncreaser();
    public event SpeedIncreaser IncreaseSpeed;


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        spawnPts = GetComponentsInChildren<Transform>();
        CreatePoolParents();
    }

    void Start()
    {
        StartSpawning();
        startTime = Time.time;
    }

    private void Update()
    {
        LevelUpgrader();

        // Pause spawning when game is paused or over
        if (GameManager.instance.gameState == GameState.Paused || GameManager.instance.gameState == GameState.GameOver)
        {
            StopAllCoroutines();
            spawnPause = true;
        }
        else if (GameManager.instance.gameState == GameState.Running && spawnPause)
        {
            StartSpawning();
        }
    }

    // Upgrade level and increase spawn rates
    void LevelUpgrader()
    {
        timer = Time.time - startTime;
        if (timer / minute >= 1)
        {
            if (level < 4)
            {
                minute *= 1.6f;
                level++;
                IncreaseSpeed?.Invoke();
                IncreaseSpawnRate();
                Debug.Log("level" + level);
            }
        }
    }

    // Increase spawn rates based on level
    void IncreaseSpawnRate()
    {
        pieceSpawnRate -= 0.15f;
        chickSpawnRate -= 0.5f;
    }

    // Start spawning objects
    void StartSpawning()
    {
        spawnPause = false;
        targetSpawnPt = spawnPts[Random.Range(0, spawnPts.Length)];
        if (piece != null)
            StartCoroutine(GeneratePieces());
        if (chick != null)
            StartCoroutine(GenerateChicks());
        if (asteroid != null)
            StartCoroutine(GenerateAsteroid());
        if (GameManager.instance.magnet && GameManager.instance.shield)
        {
            StartCoroutine(GeneratePowerPickup());
        }
    }

    // Coroutine to generate pieces
    IEnumerator GeneratePieces()
    {
        while (!spawnPause)
        {
            yield return new WaitForSeconds(pieceSpawnRate);
            PoolInstantiateObj(piece, targetSpawnPt.position, Quaternion.identity, ObjType.Piece);
            targetSpawnPt = spawnPts[Random.Range(0, spawnPts.Length)];
        }
    }

    // Coroutine to generate chicks
    IEnumerator GenerateChicks()
    {
        while (!spawnPause)
        {
            yield return new WaitForSeconds(chickSpawnRate);
            PoolInstantiateObj(chick, targetSpawnPt.position, Quaternion.identity, ObjType.Chick);
            targetSpawnPt = spawnPts[Random.Range(0, spawnPts.Length)];
        }
    }

    // Coroutine to generate asteroids
    IEnumerator GenerateAsteroid()
    {
        while (!spawnPause)
        {
            yield return new WaitForSeconds(asterSpawnRate);
            PoolInstantiateObj(asteroid, targetSpawnPt.position, Quaternion.identity, ObjType.Asteroid);
            targetSpawnPt = spawnPts[Random.Range(0, spawnPts.Length)];
        }
    }

    // Coroutine to generate power pickups
    IEnumerator GeneratePowerPickup()
    {
        while (!spawnPause)
        {
            yield return new WaitForSeconds(powerSpawnRate);
            random = Random.Range(0, 1);
            GameObject objToInstantiate = random == 0 ? GameManager.instance.magnet : GameManager.instance.shield;
            PoolInstantiateObj(objToInstantiate, targetSpawnPt.position, Quaternion.identity, ObjType.Asteroid);
            targetSpawnPt = spawnPts[Random.Range(0, spawnPts.Length)];
        }
    }

    // Reward coroutine
    public IEnumerator Reward(Vector2 pos)
    {
        if (piece != null)
        {
            for (int i = 0; i < 4; i++)
            {
                PoolInstantiateObj(piece, pos, Quaternion.identity, ObjType.Piece);
                yield return new WaitForSeconds(0.1f);
                if (i % 2 == 0)
                    pos.x += 0.4f;
                else
                    pos.x -= 0.6f;
            }
        }
    }

    // Endgame method to stop all coroutines
    public void Endgame()
    {
        StopAllCoroutines();
    }

    // Method to remove object from pool
    GameObject QueueRemove(ref List<GameObject> gList)
    {
        if (gList.Count == 0)
        {
            return null;
        }
        GameObject removedObj = gList[0];
        for (int i = 1; i < gList.Count; i++)
        {
            gList[i - 1] = gList[i];
        }
        gList.RemoveAt(gList.Count - 1);
        return removedObj;
    }

    // Create parent game objects for object pools
    void CreatePoolParents()
    {
        poolObjects = new GameObject("GameObjects Pool");
        GameObject bulletPool = new GameObject("Bullet Pool");
        GameObject chickPool = new GameObject("Chick Pool");
        GameObject asteroidPool = new GameObject("Asteroid Pool");
        GameObject piecePool = new GameObject("Piece Pool");
        GameObject vfxPool = new GameObject("VFX Pool");
        GameObject unknownPool = new GameObject("Unknown Pool");

        bulletPool.transform.parent = poolObjects.transform;
        chickPool.transform.parent = poolObjects.transform;
        asteroidPool.transform.parent = poolObjects.transform;
        piecePool.transform.parent = poolObjects.transform;
        vfxPool.transform.parent = poolObjects.transform;
        unknownPool.transform.parent = poolObjects.transform;
    }

    // Set parent for pooled object
    public void SetPoolParent(GameObject gObject, ObjType type)
    {
        string parentName = type.ToString() + " Pool";
        GameObject parent = poolObjects.transform.Find(parentName).gameObject;
        gObject.transform.parent = parent.transform;
    }

    // Instantiate object from pool
    public GameObject PoolInstantiateObj(GameObject gObject, Vector2 position, Quaternion rotation, ObjType type = ObjType.Default)
    {
        GameObject newObj;
        foreach (PoolObject p in poolList)
        {
            if (p.poolName == gObject.name)
            {
                if (p.inActiveObjects.Count > 0)
                {
                    newObj = QueueRemove(ref p.inActiveObjects);
                    newObj.SetActive(true);
                    newObj.transform.position = position;
                    newObj.transform.rotation = rotation;
                }
                else
                {
                    newObj = Instantiate(gObject, position, rotation);
                    p.recyclCount++;
                }
                SetPoolParent(newObj, type);
                return newObj;
            }
        }
        PoolObject poolObject = new PoolObject();
        poolObject.poolName = gObject.name;
        poolList.Add(poolObject);
        newObj = Instantiate(gObject, position, rotation);
        poolObject.recyclCount++;
        SetPoolParent(newObj, type);
        return newObj;
    }

    // Method to return object to pool
    public void PoolDestroyObj(GameObject gObj)
    {
        string gObjName = gObj.name.Substring(0, gObj.name.Length - 7);
        foreach (PoolObject p in poolList)
        {
            if (p.poolName == gObjName)
            {
                gObj.SetActive(false);
                p.inActiveObjects.Add(gObj);
                return;
            }
        }
        Debug.Log("You are trying to poolDestroy object where it doesn't get created from PoolInstantiate");
    }
}

[System.Serializable]
public class PoolObject
{
    public string poolName;
    public List<GameObject> inActiveObjects = new List<GameObject>();
    public int recyclCount = 0;
}

public enum ObjType
{
    Bullet,
    VFX,
    Chick,
    Piece,
    Asteroid,
    Default
}
