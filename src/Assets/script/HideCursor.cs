using UnityEngine;

public class HideCursor : MonoBehaviour
{
    private void Awake()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined; // ����ƹ��b�C���������A���M�i�H�ۥ��I��
    }

    private void Start()
    {
        // ���÷ƹ����Шä��\�I��
        
    }

    private void OnDestroy()
    {
        // ����������Ϊ���Q�R���ɡA��_�ƹ����i����
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None; // ����ƹ�
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None; // ����ƹ�
        }
    }

}
