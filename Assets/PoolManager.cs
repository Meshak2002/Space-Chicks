using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    [SerializeField] private GameObject piece, chick, asteroid;
    [SerializeField] private float pieceSpawnRate, chickSpawnRate, asterSpawnRate, magnetSpawnRate, shieldSpawnRate;
    [SerializeField] public List<PoolObject> poolList = new List<PoolObject>();
    private bool piecePause, chickPause, asterPause, magnetPause, shieldPause, spawnPause;

    private Transform[] spawnPts;
    private Transform targetSpawnPt;
    public static PoolManager instance;

    private GameObject bulletPool, chickPool, asteroidPool, piecePool, vfxPool, poolObjects;

    private void Awake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
            return;
        }    
        instance = this;
        spawnPts = GetComponentsInChildren<Transform>();

        createPoolParents();
    }

    void Start()
    {
        StartSpawning();
    }

    private void Update()
    {
        if (GameManager.instance.gameState == GameState.Paused || GameManager.instance.gameState == GameState.GameOver)
        {
            StopAllCoroutines();
            spawnPause = true;
        }
        else if(GameManager.instance.gameState == GameState.Running && spawnPause)
        {
            StartSpawning();
        }
    }

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
        if (GameManager.instance.magnet)
            StartCoroutine(GenerateMagnet());
        if (GameManager.instance.shield) 
            StartCoroutine (GenerateShield());
    }

    void createPoolParents()
    {
        poolObjects = new GameObject("GameObjects Pool");

        bulletPool = new GameObject("Bullet Pool");
        bulletPool.transform.parent = poolObjects.transform;
        chickPool = new GameObject("Chick Pool");
        chickPool.transform.parent = poolObjects.transform;
        asteroidPool = new GameObject("Asteroid Pool");
        asteroidPool.transform.parent = poolObjects.transform;
        piecePool = new GameObject("Piece Pool");
        piecePool.transform.parent = poolObjects.transform;
        vfxPool = new GameObject("VFX Pool");
        vfxPool.transform.parent = poolObjects.transform;
    }

    IEnumerator GeneratePieces()
    {
        while (piecePause == false)
        {
            yield return new WaitForSeconds(pieceSpawnRate);
            poolInstantiateObj(piece, targetSpawnPt.position, Quaternion.identity,ObjType.Piece);
            targetSpawnPt = spawnPts[Random.Range(0, spawnPts.Length)];
        }
    }

    IEnumerator GenerateChicks()
    {
        while (chickPause == false)
        {
            yield return new WaitForSeconds(chickSpawnRate);
            poolInstantiateObj(chick, targetSpawnPt.position, Quaternion.identity, ObjType.Chick);
            targetSpawnPt = spawnPts[Random.Range(0, spawnPts.Length)];
        }
    }

    IEnumerator GenerateAsteroid()
    {
        while (asterPause == false)
        {
            yield return new WaitForSeconds(asterSpawnRate);
            poolInstantiateObj(asteroid, targetSpawnPt.position, Quaternion.identity, ObjType.Asteroid);
            targetSpawnPt = spawnPts[Random.Range(0, spawnPts.Length)];
        }
    }

    IEnumerator GenerateMagnet()
    {
        while (magnetPause == false)
        {
            yield return new WaitForSeconds(magnetSpawnRate);
            poolInstantiateObj(GameManager.instance.magnet, targetSpawnPt.position, Quaternion.identity, ObjType.Asteroid);
            targetSpawnPt = spawnPts[Random.Range(0, spawnPts.Length)];
        }
    }

    IEnumerator GenerateShield()
    {
        while (shieldPause == false)
        {
            yield return new WaitForSeconds(shieldSpawnRate);
            poolInstantiateObj(GameManager.instance.shield, targetSpawnPt.position, Quaternion.identity, ObjType.Asteroid);
            targetSpawnPt = spawnPts[Random.Range(0, spawnPts.Length)];
        }
    }

    public IEnumerator Reward(Vector2 pos)
    {
        if (piece != null)
        {
            for (int i = 0; i < 6; i++)
            {
                poolInstantiateObj(piece,pos, Quaternion.identity, ObjType.Piece);
                yield return new WaitForSeconds(.1f);
                if (i % 2 == 0)
                    pos.x += .4f;
                else
                    pos.x -= .6f;
            }
        }
    }

    public void Endgame()
    {
        StopAllCoroutines();
    }

    GameObject queueRemove(ref List<GameObject> gList)
    {
        if(gList.Count == 0)
        {
            return null;
        }
        GameObject removedObj=gList[0];
        for(int i = 1;i< gList.Count; i++)
        {
            gList[i - 1] = gList[i];
        }
        gList.RemoveAt(gList.Count - 1);
        return removedObj;
    }
    
    public void setPoolParent(GameObject gObject, ObjType type)
    {
        if (type == ObjType.Bullet)
        {
            gObject.transform.parent = bulletPool.transform;
        }
        else if (type == ObjType.Piece)
        {
            gObject.transform.parent = piecePool.transform;
        }
        else if (type == ObjType.Chick)
        {
            gObject.transform.parent = chickPool.transform;
        }
        else if (type == ObjType.Asteroid)
        {
            gObject.transform.parent = asteroidPool.transform;
        }
        else
        {
            gObject.transform.parent = vfxPool.transform;
        }
                
    }

    public GameObject poolInstantiateObj(GameObject gObject, Vector2 position, Quaternion rotation, ObjType type = ObjType.Default)
    {
        GameObject newObj;
        foreach (PoolObject p in poolList)
        {
            if (p.poolName == gObject.name)
            {
                if (p.inActiveObjects.Count > 0)
                {
                    // newObj = p.inActiveObjects.Dequeue();
                    newObj = queueRemove(ref p.inActiveObjects);
                    newObj.SetActive(true);
                    newObj.transform.position = position;
                    newObj.transform.rotation = rotation;
                }
                else
                {
                    newObj = Instantiate(gObject, position, rotation);
                    p.recyclCount++;
                   // Debug.Log(p.poolName+": Extras");
                }
                setPoolParent(newObj, type);
                return newObj;
            }
        }
        PoolObject poolObject = new PoolObject();
        poolObject.poolName = gObject.name;
        //Debug.Log(poolObject.poolName + ": Initial");
        poolList.Add(poolObject);
        newObj = Instantiate(gObject, position, rotation);
        poolObject.recyclCount++;
        setPoolParent(newObj, type);
        return newObj;
    }

    public void poolDestroyObj(GameObject gObj)
    {
        string gObjName = gObj.name.Substring(0,gObj.name.Length-7);
        foreach(PoolObject p in poolList)
        {
            if(p.poolName == gObjName)
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
    public List<GameObject> inActiveObjects=new List<GameObject>();
    public int recyclCount =0;
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