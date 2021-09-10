using UnityEngine;
using UnityEditor;

//THIS IS AN EXAMPLE AND IS USED FOR REFERENCE ONLY: https://www.youtube.com/watch?v=34736DHWzaI
public class BasicObjectSpawner : EditorWindow
{
    enum Shape
    {
        DISK,
        SPHERE
    }
    Shape spawnerShape = Shape.DISK;
    string objectBaseName = "";
    int objectID = 1;
    GameObject objectToSpawn;
    float objectScale;
    float spawnRadius = 5f;
    float spawnHeight = 2f;


    [MenuItem("Window/Custom Tools/Basic Object Spawner")]
    public static void ShowWindow()
    {
        GetWindow(typeof(BasicObjectSpawner));
    }

    private void OnGUI()
    {
        GUILayout.Label("Spawn New Object", EditorStyles.boldLabel);

        spawnerShape = (Shape) EditorGUILayout.EnumPopup("Spawner Shape", spawnerShape);
        objectToSpawn = EditorGUILayout.ObjectField("Prefab to Spawn", objectToSpawn, typeof(GameObject), false) as GameObject;
        objectBaseName = EditorGUILayout.TextField("Base Name", objectBaseName);
        objectID = EditorGUILayout.IntField("Object ID", objectID);
        objectScale = EditorGUILayout.Slider("Object Scale", objectScale, 0.5f, 3f);
        spawnRadius = EditorGUILayout.FloatField("Spawn Radius", spawnRadius);
        //if (spawnerShape == Shape.DISK)
        //{
            spawnHeight = EditorGUILayout.FloatField("Spawn Height", spawnHeight);
        //}

        
        if (GUILayout.Button("Spawn Object"))
        {
            SpawnObject();
        }
    }

    private void SpawnObject()
    {
        if (objectToSpawn == null)
        {
            Debug.LogError("Please assign an object to be spawned. :)");
            return;
        }
        if (objectBaseName == string.Empty)
        {
            Debug.LogError("Please assign a name to the object. :/");
            return;
        }

        Vector3 spawnPos = Vector3.zero;
        switch (spawnerShape)
        {
            case Shape.DISK:
                Vector2 spawnCircle = Random.insideUnitCircle * spawnRadius;
                spawnPos = new Vector3(spawnCircle.x, spawnHeight, spawnCircle.y);
                break;
            case Shape.SPHERE:
                Vector3 spawnSphere = Random.insideUnitSphere * spawnRadius;
                spawnPos = new Vector3(spawnSphere.x, spawnSphere.y+spawnHeight, spawnSphere.z);
                break;
        }


        GameObject newObject = Instantiate(objectToSpawn, spawnPos, Quaternion.identity);
        newObject.name = objectBaseName + objectID;
        newObject.transform.localScale = Vector3.one * objectScale;

        objectID++;
    }
}
