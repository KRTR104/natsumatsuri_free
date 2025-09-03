using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PopupManager : MonoBehaviour
{
    [SerializeField] GameObject popupPanel;          // �|�b�v�A�b�vUI�p�l��
    [SerializeField] TextMeshProUGUI questionText;   // ���╶�iTMP�j
    private string nextSceneName;

    [SerializeField] AudioClip clickSound;           // �{�^�����ʉ�
    private AudioSource audioSource;                 // �Đ��pAudioSourc


    void Start()
    {
        popupPanel.SetActive(false);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // �Q�[���J�n���̓J�[�\����\�������b�N
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /// <summary>
    /// �|�b�v�A�b�v��\��
    /// </summary>
    public void ShowPopup(string question, string sceneName)
    {


        questionText.text = question;
        nextSceneName = sceneName;
        popupPanel.SetActive(true);

        // �|�b�v�A�b�v�\�����ɃJ�[�\����\�����ă��b�N����
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    /// <summary>
    /// �u�͂��v�{�^��������
    /// </summary>
    public void OnYes()
    {
        PlayClickSound();

        if (!string.IsNullOrEmpty(nextSceneName))
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }

    /// <summary>
    /// �u�������v�{�^��������
    /// </summary>
    public void OnNo()
    {
        PlayClickSound();

        popupPanel.SetActive(false);

        // �|�b�v�A�b�v�������J�[�\�������̏�Ԃɖ߂�
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /// <summary>
    /// ���ʉ����Đ�
    /// </summary>
    private void PlayClickSound()
    {
        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }
}
