using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PopupManager : MonoBehaviour
{
    [SerializeField] GameObject popupPanel;          // �|�b�v�A�b�vUI�p�l��
    [SerializeField] TextMeshProUGUI questionText;   // ���╶�iTMP�j
    private string nextSceneName;


    void Start()
    {
        popupPanel.SetActive(false);

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
        popupPanel.SetActive(false);

        // �|�b�v�A�b�v�������J�[�\�������̏�Ԃɖ߂�
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
