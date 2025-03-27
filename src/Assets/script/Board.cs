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
    [SerializeField] private AudioClip clickSound;    // �I���Ѯ�ɪ�����

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
        // �p�G��l�w�g�аO�L�F�A������^�]��������ơ^
        if (box.isMarked)
            return;

        // �Y��l�٥��Q�аO�A����s��l���A�εe��
        marks[box.index] = currentMark;
        box.SetAsMarked(GetSprite(), currentMark, GetColor());

        // �����I�����ġ]�u�b���\�аO�ɼ���^
        if (clickSound != null)
        {
            AudioSource.PlayClipAtPoint(clickSound, box.transform.position);
        }

        // �O���аO�ƶq
        marksCount++;

        // �ˬd�O�_�����a���
        bool won = CheckIfWin();
        if (won)
        {
            OnWinAction?.Invoke(currentMark, GetColor());
            Debug.Log($"{currentMark} Wins.");
            canPlay = false;
            return;
        }

        // ��Ҧ���l���Q�аO�ɡ]�����^�A���𭫱ҹC��
        if (marksCount == 9)
        {
            OnWinAction?.Invoke(Mark.None, Color.white);
            Debug.Log("Nobody Wins.");
            canPlay = false;
            StartCoroutine(RestartGameAfterDelay(3f));
            return;
        }

        // �������a
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

        // �X�i�u������
        Vector3 direction = (end - start).normalized;
        float lengthExtension = 1.05f;

        start -= direction * lengthExtension;
        end += direction * lengthExtension;

        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);

        // �]�m�u�����p��
        lineRenderer.startWidth = 0.5f;
        lineRenderer.endWidth = 0.5f;

        Color color = GetColor();
        color.a = 0.95f;
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;

        lineRenderer.enabled = true;

        // �ӧQ�᩵�𭫱ҹC��
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
