using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PopupManager : MonoBehaviour
{
    [SerializeField] GameObject popupPanel;          // ポップアップUIパネル
    [SerializeField] TextMeshProUGUI questionText;   // 質問文（TMP）
    private string nextSceneName;

    [SerializeField] AudioClip clickSound;           // ボタン効果音
    private AudioSource audioSource;                 // 再生用AudioSourc


    void Start()
    {
        popupPanel.SetActive(false);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // ゲーム開始時はカーソル非表示＆ロック
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /// <summary>
    /// ポップアップを表示
    /// </summary>
    public void ShowPopup(string question, string sceneName)
    {


        questionText.text = question;
        nextSceneName = sceneName;
        popupPanel.SetActive(true);

        // ポップアップ表示時にカーソルを表示してロック解除
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    /// <summary>
    /// 「はい」ボタン押下時
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
    /// 「いいえ」ボタン押下時
    /// </summary>
    public void OnNo()
    {
        PlayClickSound();

        popupPanel.SetActive(false);

        // ポップアップを閉じたらカーソルを元の状態に戻す
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    /// <summary>
    /// 効果音を再生
    /// </summary>
    private void PlayClickSound()
    {
        if (clickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(clickSound);
        }
    }
}
