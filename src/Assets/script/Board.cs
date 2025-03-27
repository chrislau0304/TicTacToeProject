using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Board : MonoBehaviour
{
    [Header("Input Settings : ")]
    [SerializeField] private LayerMask boxesLayerMask;
    [SerializeField] private float touchRadius;

    [Header("Mark Sprites : ")]
    [SerializeField] private Sprite spriteX;
    [SerializeField] private Sprite spriteO;

    [Header("Mark Colors : ")]
    [SerializeField] private Color colorX;
    [SerializeField] private Color colorO;

    [Header("Player Indicators : ")]
    public GameObject playerXIndicator;
    public GameObject playerXtext;
    public GameObject playerOIndicator;
    public GameObject playerOtext;

    [Header("Sound Effects :")]
    [SerializeField] private AudioClip clickSound;    // 點擊棋格時的音效

    public UnityAction<Mark, Color> OnWinAction;

    public Mark[] marks;

    private Camera cam;
    private Mark currentMark;
    private bool canPlay;
    private LineRenderer lineRenderer;
    private int marksCount = 0;

    private void Start()
    {
        cam = Camera.main;
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;

        currentMark = Mark.X;
        marks = new Mark[9];
        canPlay = true;

        UpdatePlayerIndicator();
    }

    private void Update()
    {
        if (canPlay && Input.GetMouseButtonUp(0))
        {
            Vector2 touchPosition = cam.ScreenToWorldPoint(Input.mousePosition);
            Collider2D hit = Physics2D.OverlapCircle(touchPosition, touchRadius, boxesLayerMask);

            if (hit) // box is touched
                HitBox(hit.GetComponent<Box>());
        }
    }

    private void HitBox(Box box)
    {
        // 如果格子已經標記過了，直接返回（不做任何事）
        if (box.isMarked)
            return;

        // 若格子還未被標記，先更新格子狀態及畫面
        marks[box.index] = currentMark;
        box.SetAsMarked(GetSprite(), currentMark, GetColor());

        // 播放點擊音效（只在成功標記時播放）
        if (clickSound != null)
        {
            AudioSource.PlayClipAtPoint(clickSound, box.transform.position);
        }

        // 記錄標記數量
        marksCount++;

        // 檢查是否有玩家獲勝
        bool won = CheckIfWin();
        if (won)
        {
            OnWinAction?.Invoke(currentMark, GetColor());
            Debug.Log($"{currentMark} Wins.");
            canPlay = false;
            return;
        }

        // 當所有格子都被標記時（平局），延遲重啟遊戲
        if (marksCount == 9)
        {
            OnWinAction?.Invoke(Mark.None, Color.white);
            Debug.Log("Nobody Wins.");
            canPlay = false;
            StartCoroutine(RestartGameAfterDelay(3f));
            return;
        }

        // 切換玩家
        SwitchPlayer();
    }



    private bool CheckIfWin()
    {
        return AreBoxesMatched(0, 1, 2) ||
               AreBoxesMatched(3, 4, 5) ||
               AreBoxesMatched(6, 7, 8) ||
               AreBoxesMatched(0, 3, 6) ||
               AreBoxesMatched(1, 4, 7) ||
               AreBoxesMatched(2, 5, 8) ||
               AreBoxesMatched(0, 4, 8) ||
               AreBoxesMatched(2, 4, 6);
    }

    private bool AreBoxesMatched(int i, int j, int k)
    {
        Mark m = currentMark;
        bool matched = (marks[i] == m && marks[j] == m && marks[k] == m);

        if (matched)
            DrawLine(i, k);

        return matched;
    }

    private void DrawLine(int i, int k)
    {
        Vector3 start = transform.GetChild(i).position;
        Vector3 end = transform.GetChild(k).position;

        // 擴展線的長度
        Vector3 direction = (end - start).normalized;
        float lengthExtension = 1.05f;

        start -= direction * lengthExtension;
        end += direction * lengthExtension;

        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);

        // 設置線條的厚度
        lineRenderer.startWidth = 0.5f;
        lineRenderer.endWidth = 0.5f;

        Color color = GetColor();
        color.a = 0.95f;
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;

        lineRenderer.enabled = true;

        // 勝利後延遲重啟遊戲
        StartCoroutine(RestartGameAfterDelay(3f));
    }

    private IEnumerator RestartGameAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void SwitchPlayer()
    {
        currentMark = (currentMark == Mark.X) ? Mark.O : Mark.X;
        UpdatePlayerIndicator();
    }

    private Color GetColor()
    {
        return (currentMark == Mark.X) ? colorX : colorO;
    }

    private Sprite GetSprite()
    {
        return (currentMark == Mark.X) ? spriteX : spriteO;
    }

    private void UpdatePlayerIndicator()
    {
        playerXIndicator.SetActive(currentMark == Mark.X);
        playerXtext.SetActive(currentMark == Mark.X);

        playerOIndicator.SetActive(currentMark == Mark.O);
        playerOtext.SetActive(currentMark == Mark.O);
    }
}
