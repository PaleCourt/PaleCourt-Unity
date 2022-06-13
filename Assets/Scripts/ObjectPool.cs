using System;
using System.Collections.Generic;
using Modding;
using UnityEngine;

public sealed class ObjectPool : MonoBehaviour
{
  private static List<GameObject> tempList = new List<GameObject>();
  private static List<GameObject> destroyList = new List<GameObject>();
  private static Vector2 activeStashLocation = new Vector2(-20f, -20f);
  private Dictionary<GameObject, List<GameObject>> pooledObjects = new Dictionary<GameObject, List<GameObject>>();
  private Dictionary<GameObject, GameObject> spawnedObjects = new Dictionary<GameObject, GameObject>();
  public StartupPool[] startupPools;
  private bool startupPoolsCreated;
  private static bool isRecycling;
  private static ObjectPool _instance;

  public static ObjectPool instance
  {
    get
    {
      if (_instance == null)
      {
        _instance = FindObjectOfType<ObjectPool>();
        if (_instance == null)
          Debug.LogError("Couldn't find an Object Pool, make sure a Game Manager exists in the scene.");
        else
          DontDestroyOnLoad(_instance.gameObject);
      }
      return _instance;
    }
  }

  private void Awake()
  {
    if (_instance == null)
    {
      _instance = this;
      DontDestroyOnLoad(this);
    }
    else
    {
      if (!(this != _instance))
        return;
      Debug.LogErrorFormat("An extra Global Object Pool has been created by {0} please remove this script. Master Object Pool: {1} (Scene: {2} at time: {3})", transform.parent.name, _instance.name, Application.loadedLevelName, Time.realtimeSinceStartup);
      if (transform.parent.name == "_GameManager")
      {
        Debug.Log(("Object Pool instance is no longer set to master object pool, another Object Pool exists in this scene. Instance currently set to : " + _instance.name));
        _instance = this;
      }
      else
        Destroy(gameObject);
    }
  }

  private void Start()
  {
    if (!instance.startupPoolsCreated)
    {
      CreateStartupPools();
    }
    else
    {
      for (int index = 0; index < startupPools.Length; ++index)
        startupPools[index].prefab.CreatePool(startupPools[index].size);
    }
  }

  public static void CreateStartupPools()
  {
    if (instance.startupPoolsCreated)
      return;
    instance.startupPoolsCreated = true;
    StartupPool[] startupPools = instance.startupPools;
    if (startupPools == null || startupPools.Length <= 0)
      return;
    for (int index = 0; index < startupPools.Length; ++index)
      CreatePool(startupPools[index].prefab, startupPools[index].size);
  }

  public static void CreatePool<T>(T prefab, int initialPoolSize) where T : Component
  {
    CreatePool(prefab.gameObject, initialPoolSize);
  }

  public static void CreatePool(GameObject prefab, int initialPoolSize)
  {
    ObjectPoolAuditor.RecordPoolCreated(prefab, initialPoolSize);
    if (prefab != null)
    {
      List<GameObject> gameObjectList;
      if (!instance.pooledObjects.ContainsKey(prefab))
      {
        gameObjectList = new List<GameObject>();
        instance.pooledObjects.Add(prefab, gameObjectList);
        if (initialPoolSize > 0)
        {
          bool activeSelf = prefab.activeSelf;
          bool flag;
          if (prefab.GetComponent<ActiveRecycler>() != null)
          {
            flag = true;
            prefab.SetActive(true);
          }
          else
          {
            flag = false;
            prefab.SetActive(false);
          }
          Transform transform = instance.transform;
          while (gameObjectList.Count < initialPoolSize)
          {
            GameObject gameObject = Instantiate<GameObject>(prefab);
            gameObject.transform.parent = transform;
            if (flag)
              gameObject.transform.position = activeStashLocation;
            gameObjectList.Add(gameObject);
          }
          prefab.SetActive(activeSelf);
        }
      }
      else
      {
        gameObjectList = instance.pooledObjects[prefab];
        if (initialPoolSize > 0)
        {
          int num = gameObjectList.Count + initialPoolSize;
          bool activeSelf = prefab.activeSelf;
          bool flag;
          if (prefab.GetComponent<ActiveRecycler>() != null)
          {
            flag = true;
            prefab.SetActive(true);
          }
          else
          {
            flag = false;
            prefab.SetActive(false);
          }
          Transform transform = instance.transform;
          while (gameObjectList.Count < num)
          {
            GameObject gameObject = Instantiate(prefab);
            gameObject.transform.parent = transform;
            if (flag)
              gameObject.transform.position = activeStashLocation;
            gameObjectList.Add(gameObject);
          }
          prefab.SetActive(activeSelf);
        }
      }
      if (gameObjectList == null)
        return;
      foreach (GameObject gameObject in gameObjectList)
      {
        foreach (tk2dBaseSprite componentsInChild in gameObject.GetComponentsInChildren<tk2dSprite>(true))
          componentsInChild.ForceBuild();
      }
    }
    else
    {
      if (!(prefab == null))
        return;
      Debug.LogError("Trying to create an Object Pool for a prefab that is null.");
    }
  }

  public void RevertToStartState()
  {
    RecycleAll();
    List<GameObject> gameObjectList1 = new List<GameObject>();
    foreach (KeyValuePair<GameObject, List<GameObject>> pooledObject in pooledObjects)
    {
      GameObject key = pooledObject.Key;
      List<GameObject> gameObjectList2 = pooledObject.Value;
      int num = 0;
      for (int index = 0; index < startupPools.Length; ++index)
      {
        StartupPool startupPool = startupPools[index];
        if (startupPool.prefab == key)
        {
          num = startupPool.size;
          break;
        }
      }
      while (gameObjectList2.Count > num)
      {
        Destroy(gameObjectList2[0]);
        gameObjectList2.RemoveAt(0);
      }
      if (gameObjectList2.Count < num)
        CreatePool(key, num - gameObjectList2.Count);
      else if (num == 0)
        gameObjectList1.Add(key);
    }
    foreach (GameObject key in gameObjectList1)
      pooledObjects.Remove(key);
  }

  public static T Spawn<T>(T prefab, Transform parent, Vector3 position, Quaternion rotation) where T : Component
  {
    return Spawn(prefab.gameObject, parent, position, rotation).GetComponent<T>();
  }

  public static T Spawn<T>(T prefab, Vector3 position, Quaternion rotation) where T : Component
  {
    return Spawn(prefab.gameObject, null, position, rotation).GetComponent<T>();
  }

  public static T Spawn<T>(T prefab, Transform parent, Vector3 position) where T : Component
  {
    return Spawn(prefab.gameObject, parent, position, Quaternion.identity).GetComponent<T>();
  }

  public static T Spawn<T>(T prefab, Vector3 position) where T : Component
  {
    return Spawn(prefab.gameObject, null, position, Quaternion.identity).GetComponent<T>();
  }

  public static T Spawn<T>(T prefab, Transform parent) where T : Component
  {
    return Spawn(prefab.gameObject, parent, Vector3.zero, Quaternion.identity).GetComponent<T>();
  }

  public static T Spawn<T>(T prefab) where T : Component
  {
    return Spawn(prefab.gameObject, null, Vector3.zero, Quaternion.identity).GetComponent<T>();
  }

  public static GameObject Spawn(
    GameObject prefab,
    Transform parent,
    Vector3 position,
    Quaternion rotation)
  {
    return ModHooks.Instance.OnObjectPoolSpawn(orig_Spawn(prefab, parent, position, rotation));
  }

  public static GameObject Spawn(GameObject prefab, Transform parent, Vector3 position)
  {
    return Spawn(prefab, parent, position, Quaternion.identity);
  }

  public static GameObject Spawn(
    GameObject prefab,
    Vector3 position,
    Quaternion rotation)
  {
    return Spawn(prefab, null, position, rotation);
  }

  public static GameObject Spawn(GameObject prefab, Transform parent)
  {
    return Spawn(prefab, parent, Vector3.zero, Quaternion.identity);
  }

  public static GameObject Spawn(GameObject prefab, Vector3 position)
  {
    return Spawn(prefab, null, position, Quaternion.identity);
  }

  public static GameObject Spawn(GameObject prefab)
  {
    return Spawn(prefab, null, Vector3.zero, Quaternion.identity);
  }

  public static void Recycle<T>(T obj) where T : Component
  {
    Recycle(obj.gameObject);
  }

  public static void Recycle(GameObject obj)
  {
    GameObject prefab;
    if (instance != null && instance.spawnedObjects.TryGetValue(obj, out prefab))
    {
      Recycle(obj, prefab);
    }
    else
    {
      ObjectPoolAuditor.RecordDespawned(obj, false);
      Destroy(obj);
    }
  }

  private static void Recycle(GameObject obj, GameObject prefab)
  {
    isRecycling = true;
    if (obj != null && prefab != null)
    {
      instance.pooledObjects[prefab].Add(obj);
      instance.spawnedObjects.Remove(obj);
      obj.transform.parent = instance.transform;
      if (obj.GetComponent<ActiveRecycler>() != null)
      {
        obj.transform.position = activeStashLocation;
        FSMUtility.SendEventToGameObject(obj, "A RECYCLE", false);
      }
      else
        obj.SetActive(false);
      ObjectPoolAuditor.RecordDespawned(obj, true);
    }
    isRecycling = false;
  }

  public static void RecycleAll<T>(T prefab) where T : Component
  {
    RecycleAll(prefab.gameObject);
  }

  public static void RecycleAll(GameObject prefab)
  {
    foreach (KeyValuePair<GameObject, GameObject> spawnedObject in instance.spawnedObjects)
    {
      if (spawnedObject.Value == prefab)
        tempList.Add(spawnedObject.Key);
    }
    for (int index = 0; index < tempList.Count; ++index)
      Recycle(tempList[index]);
    tempList.Clear();
  }

  public static void RecycleAll()
  {
    tempList.AddRange((IEnumerable<GameObject>) instance.spawnedObjects.Keys);
    for (int index = 0; index < tempList.Count; ++index)
      Recycle(tempList[index]);
    tempList.Clear();
  }

  public static bool IsSpawned(GameObject obj)
  {
    return instance.spawnedObjects.ContainsKey(obj);
  }

  public static int CountPooled<T>(T prefab) where T : Component
  {
    return CountPooled(prefab.gameObject);
  }

  public static int CountPooled(GameObject prefab)
  {
    List<GameObject> gameObjectList;
    if (instance.pooledObjects.TryGetValue(prefab, out gameObjectList))
      return gameObjectList.Count;
    return 0;
  }

  public static int CountSpawned<T>(T prefab) where T : Component
  {
    return CountSpawned(prefab.gameObject);
  }

  public static int CountSpawned(GameObject prefab)
  {
    int num = 0;
    foreach (GameObject gameObject in instance.spawnedObjects.Values)
    {
      if (prefab == gameObject)
        ++num;
    }
    return num;
  }

  public static int CountAllPooled()
  {
    int num = 0;
    foreach (List<GameObject> gameObjectList in instance.pooledObjects.Values)
      num += gameObjectList.Count;
    return num;
  }

  public static List<GameObject> GetPooled(
    GameObject prefab,
    List<GameObject> list,
    bool appendList)
  {
    if (list == null)
      list = new List<GameObject>();
    if (!appendList)
      list.Clear();
    List<GameObject> gameObjectList;
    if (instance.pooledObjects.TryGetValue(prefab, out gameObjectList))
      list.AddRange(gameObjectList);
    return list;
  }

  public static List<T> GetPooled<T>(T prefab, List<T> list, bool appendList) where T : Component
  {
    if (list == null)
      list = new List<T>();
    if (!appendList)
      list.Clear();
    List<GameObject> gameObjectList;
    if (instance.pooledObjects.TryGetValue(prefab.gameObject, out gameObjectList))
    {
      for (int index = 0; index < gameObjectList.Count; ++index)
        list.Add(gameObjectList[index].GetComponent<T>());
    }
    return list;
  }

  public static List<GameObject> GetSpawned(
    GameObject prefab,
    List<GameObject> list,
    bool appendList)
  {
    if (list == null)
      list = new List<GameObject>();
    if (!appendList)
      list.Clear();
    foreach (KeyValuePair<GameObject, GameObject> spawnedObject in instance.spawnedObjects)
    {
      if (spawnedObject.Value == prefab)
        list.Add(spawnedObject.Key);
    }
    return list;
  }

  public static List<T> GetSpawned<T>(T prefab, List<T> list, bool appendList) where T : Component
  {
    if (list == null)
      list = new List<T>();
    if (!appendList)
      list.Clear();
    GameObject gameObject = prefab.gameObject;
    foreach (KeyValuePair<GameObject, GameObject> spawnedObject in instance.spawnedObjects)
    {
      if (spawnedObject.Value == gameObject)
        list.Add(spawnedObject.Key.GetComponent<T>());
    }
    return list;
  }

  public static void DestroyPooled(GameObject prefab)
  {
    List<GameObject> gameObjectList;
    if (!instance.pooledObjects.TryGetValue(prefab, out gameObjectList))
      return;
    for (int index = 0; index < gameObjectList.Count; ++index)
      Destroy(gameObjectList[index]);
    gameObjectList.Clear();
  }

  public static void DestroyPooled<T>(T prefab) where T : Component
  {
    DestroyPooled(prefab.gameObject);
  }

  public static void DestroyPooled(GameObject prefab, int amountToRemove)
  {
    RecycleAll(prefab);
    List<GameObject> gameObjectList;
    if (!instance.pooledObjects.TryGetValue(prefab, out gameObjectList))
      return;
    for (int index = 0; index < amountToRemove && gameObjectList.Count > 0; ++index)
    {
      Destroy(gameObjectList[0]);
      gameObjectList.RemoveAt(0);
    }
  }

  public static void DestroyPooled<T>(T prefab, int amount) where T : Component
  {
    DestroyPooled(prefab.gameObject, amount);
  }

  public static void DestroyAll(GameObject prefab)
  {
    RecycleAll(prefab);
    DestroyPooled(prefab);
  }

  public static void DestroyAll<T>(T prefab) where T : Component
  {
    DestroyAll(prefab.gameObject);
  }

  public static GameObject orig_Spawn(
    GameObject prefab,
    Transform parent,
    Vector3 position,
    Quaternion rotation)
  {
    bool flag = prefab.GetComponent<ActiveRecycler>() != null;
    List<GameObject> gameObjectList;
    if (instance.pooledObjects.TryGetValue(prefab, out gameObjectList))
    {
      GameObject gameObject1 = null;
      if (gameObjectList.Count > 0)
      {
        while (gameObject1 == null && gameObjectList.Count > 0)
        {
          gameObject1 = gameObjectList[0];
          gameObjectList.RemoveAt(0);
        }
        if (gameObject1 != null)
        {
          Transform transform = gameObject1.transform;
          transform.parent = parent;
          transform.localPosition = position;
          transform.localRotation = rotation;
          if (flag)
            FSMUtility.SendEventToGameObject(gameObject1, "A SPAWN", false);
          else
            gameObject1.SetActive(true);
          instance.spawnedObjects.Add(gameObject1, prefab);
          ObjectPoolAuditor.RecordSpawned(prefab, false);
          return gameObject1;
        }
      }
      Debug.LogWarningFormat("Object Pool attached to {0} has run out of {1} prefabs, Instantiating an additional one.", instance.name, prefab.name);
      GameObject gameObject2 = Instantiate<GameObject>(prefab);
      Transform transform1 = gameObject2.transform;
      transform1.parent = parent;
      transform1.localPosition = position;
      transform1.localRotation = rotation;
      if (flag)
        FSMUtility.SendEventToGameObject(gameObject2, "A SPAWN", false);
      instance.spawnedObjects.Add(gameObject2, prefab);
      ObjectPoolAuditor.RecordSpawned(prefab, true);
      return gameObject2;
    }
    if (prefab == null)
    {
      Debug.LogErrorFormat("Object Pool attached to {0} was asked for a NULL prefab.", instance.name);
      return null;
    }
    Debug.LogWarningFormat("Object Pool attached to {0} could not find a copy of {1}, Instantiating a new one.", instance.name, prefab.name);
    CreatePool(prefab.gameObject, 1);
    return Spawn(prefab);
  }

  [Serializable]
  public class StartupPool
  {
    public int size;
    public GameObject prefab;
  }
}
