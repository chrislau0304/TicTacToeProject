using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject objectPrefab;  // ���w�A�n�l�ꪺPrefab
    private GameObject spawnedObject;

    // �o�Ӥ�k�i�H�s������s��OnClick�ƥ�
    public void SpawnObject()
    {
        // �Y�ثe�|�����Ӫ���s�b�~�i��ͦ�
        if (spawnedObject == null)
        {
            spawnedObject = Instantiate(objectPrefab);
        }
    }
}
