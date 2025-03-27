using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject objectPrefab;  // 指定你要召喚的Prefab
    private GameObject spawnedObject;

    // 這個方法可以連結到按鈕的OnClick事件
    public void SpawnObject()
    {
        // 若目前尚未有該物件存在才進行生成
        if (spawnedObject == null)
        {
            spawnedObject = Instantiate(objectPrefab);
        }
    }
}
