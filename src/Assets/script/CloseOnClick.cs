using UnityEngine;

public class CloseOnClick : MonoBehaviour
{
    void Update()
    {
        // 當畫面上一有滑鼠左鍵點擊，就關閉（銷毀）此物件
        if (Input.GetMouseButtonDown(0))
        {
            Destroy(gameObject);
        }
    }
}
