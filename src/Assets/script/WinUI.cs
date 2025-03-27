using UnityEngine;

public class WinUI : MonoBehaviour
{
    [Header("UI References :")]
    [SerializeField] private GameObject redText;    // X �ӧQ����ܪ�����奻
    [SerializeField] private GameObject greenText;  // O �ӧQ����ܪ����奻
    [SerializeField] private GameObject Panel;
    [SerializeField] private GameObject red;
    [SerializeField] private GameObject green;
    [SerializeField] private GameObject Draw;       // ��������ܪ� Draw ����

    [Header("Board Reference :")]
    [SerializeField] private Board board;           // �ѽL�޿�}���Ѧ�

    private void Start()
    {
        // ���U Board ���ӧQ�ƥ�
        board.OnWinAction += OnWinEvent;

        // �C���}�l�����éҦ��ӧQ/������ UI
        redText.SetActive(false);
        greenText.SetActive(false);
        Panel.SetActive(false);
        Draw.SetActive(false);
    }

    private void OnWinEvent(Mark mark, Color color)
    {
        // �����éҦ� UI ����A�קK���ƥX�{
        redText.SetActive(false);
        greenText.SetActive(false);
        Draw.SetActive(false);

        if (mark == Mark.X)
        {
            // �� X �ӧQ�A��ܬ���奻
            Panel.SetActive(true);
            red.SetActive(false);
            green.SetActive(false);
            redText.SetActive(true);
        }
        else if (mark == Mark.O)
        {
            // �� O �ӧQ�A��ܺ��奻
            Panel.SetActive(true);
            red.SetActive(false);
            green.SetActive(false);
            greenText.SetActive(true);
        }
        else if (mark == Mark.None)
        {
            // �����ɡA��� Draw ����
            Panel.SetActive(true);
            red.SetActive(false);
            green.SetActive(false);
            Draw.SetActive(true);
        }
    }

    private void OnDestroy()
    {
        // �Ѱ��ƥ�q�\
        board.OnWinAction -= OnWinEvent;
    }
}
