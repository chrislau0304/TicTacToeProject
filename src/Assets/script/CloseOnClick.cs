using UnityEngine;

public class CloseOnClick : MonoBehaviour
{
    void Update()
    {
        // ��e���W�@���ƹ������I���A�N�����]�P���^������
        if (Input.GetMouseButtonDown(0))
        {
            Destroy(gameObject);
        }
    }
}
