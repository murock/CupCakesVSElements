using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {

    //used to hold objects so they don't need to be created and destroyed all the time
    [SerializeField]
    private GameObject[] objectPrefabs;

    private List<GameObject> pooledObjects = new List<GameObject>();

    public GameObject GetObject(string type)
    {

        foreach (GameObject go in pooledObjects)
        {
            if (go.name == type && !go.activeInHierarchy)    //if name matches type we want to spawn and is inactive then re-use
            {
                go.SetActive(true);
                return go;
            }
        }

        //creates a new object if one isn't in the object pool
        for (int i = 0; i < objectPrefabs.Length; i++)
        {
            if (objectPrefabs[i].name == type)
            {
                GameObject newObject = Instantiate(objectPrefabs[i]);
                pooledObjects.Add(newObject);
                newObject.name = type;
                return newObject;
            }
        }
        return null;
    }


    public void ReleaseObject(GameObject gameObject)
    {
        gameObject.SetActive(false);
    }
}
