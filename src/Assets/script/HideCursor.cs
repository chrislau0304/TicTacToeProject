using UnityEngine;

public class HideCursor : MonoBehaviour
{
    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined; // 限制滑鼠在遊戲視窗內，仍然可以自由點擊
    }

    private void Start()
    {
        // 隱藏滑鼠指標並允許點擊
        
    }

    private void OnDestroy()
    {
        // 當場景結束或物件被摧毀時，恢復滑鼠的可見性
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None; // 釋放滑鼠
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None; // 釋放滑鼠
        }
    }

}
