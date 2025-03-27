using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    private static MusicPlayer instance;

    private void Awake()
    {
        // �p�G�w���@�� MusicPlayer ��Ҧs�b�A�h�P����e����
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        // ���o�� GameObject �b���������ɤ��Q�P��
        DontDestroyOnLoad(gameObject);

        // ���o AudioSource�A�p�G�s�b�A�]�m���`������
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.loop = true;
        }
    }
}
