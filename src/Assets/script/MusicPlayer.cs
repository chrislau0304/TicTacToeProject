using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private static MusicPlayer instance;

    private void Awake()
    {
        // 如果已有一個 MusicPlayer 實例存在，則銷毀當前物件
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        // 讓這個 GameObject 在切換場景時不被銷毀
        DontDestroyOnLoad(gameObject);

        // 取得 AudioSource，如果存在，設置為循環播放
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.loop = true;
        }
    }
}
