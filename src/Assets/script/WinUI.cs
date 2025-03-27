using UnityEngine;

public class WinUI : MonoBehaviour
{
    [Header("UI References :")]
    [SerializeField] private GameObject redText;    // X 勝利時顯示的紅色文本
    [SerializeField] private GameObject greenText;  // O 勝利時顯示的綠色文本
    [SerializeField] private GameObject Panel;
    [SerializeField] private GameObject red;
    [SerializeField] private GameObject green;
    [SerializeField] private GameObject Draw;       // 平局時顯示的 Draw 物件

    [Header("Board Reference :")]
    [SerializeField] private Board board;           // 棋盤邏輯腳本參考

    private void Start()
    {
        // 註冊 Board 的勝利事件
        board.OnWinAction += OnWinEvent;

        // 遊戲開始時隱藏所有勝利/平局的 UI
        redText.SetActive(false);
        greenText.SetActive(false);
        Panel.SetActive(false);
        Draw.SetActive(false);
    }

    private void OnWinEvent(Mark mark, Color color)
    {
        // 先隱藏所有 UI 物件，避免重複出現
        redText.SetActive(false);
        greenText.SetActive(false);
        Draw.SetActive(false);

        if (mark == Mark.X)
        {
            // 當 X 勝利，顯示紅色文本
            Panel.SetActive(true);
            red.SetActive(false);
            green.SetActive(false);
            redText.SetActive(true);
        }
        else if (mark == Mark.O)
        {
            // 當 O 勝利，顯示綠色文本
            Panel.SetActive(true);
            red.SetActive(false);
            green.SetActive(false);
            greenText.SetActive(true);
        }
        else if (mark == Mark.None)
        {
            // 當平局時，顯示 Draw 物件
            Panel.SetActive(true);
            red.SetActive(false);
            green.SetActive(false);
            Draw.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        // 解除事件訂閱
        board.OnWinAction -= OnWinEvent;
    }
}
