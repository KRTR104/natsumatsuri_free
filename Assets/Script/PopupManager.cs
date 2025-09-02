using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PopupManager : MonoBehaviour
{
    [SerializeField] GameObject popupPanel;          // ポップアップUIパネル
    [SerializeField] TextMeshProUGUI questionText;   // 質問文（TMP）
    private string nextSceneName;


    void Start()
    {
        popupPanel.SetActive(false);

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
        popupPanel.SetActive(false);

        // ポップアップを閉じたらカーソルを元の状態に戻す
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
