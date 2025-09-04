using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CornGameManager : MonoBehaviour
{
    public static CornGameManager Instance { get; private set; }

    [Header("UI")]
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] GameObject resultPopup;
    [SerializeField] TextMeshProUGUI resultPopupText;
    [SerializeField] TextMeshProUGUI countdownText;

    [Header("効果音（任意）")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip successClip;
    [SerializeField] AudioClip failClip;

    [Header("制限時間")]
    [SerializeField] float timeLimit = 60f;

    int score;
    float timeLeft;
    bool playing;
    public bool IsPlaying => playing;

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    void Start()
    {
        StartCoroutine(GameStartRoutine());
    }

    void Update()
    {
        if (!playing) return;

        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0f)
        {
            timeLeft = 0f;
            EndGame();
        }
        UpdateUI();
    }

    IEnumerator GameStartRoutine()
    {
        playing = false;

        int count = 3;
        while (count > 0)
        {
            if (countdownText != null)
                countdownText.text = count.ToString();
            yield return new WaitForSeconds(1f);
            count--;
        }

        if (countdownText != null)
            countdownText.text = "スタート!";
        yield return new WaitForSeconds(0.5f);

        if (countdownText != null)
            countdownText.text = "";

        StartGame();
    }

    public void StartGame()
    {
        score = 0;
        timeLeft = timeLimit;
        playing = true;
        UpdateUI();
    }

    void EndGame()
    {
        playing = false;
        ShowPopup($"終了！スコア: {score}");
        StartCoroutine(EndRoutine());
    }

    IEnumerator EndRoutine()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("SelectScene"); // 終了後の遷移先
    }

    public void OnCornFinished(int addScore, string message, bool success)
    {
        if (!playing) return;

        score += addScore;
        UpdateUI();

        ShowPopup(message);
        PlaySE(success);
    }

    void UpdateUI()
    {
        if (scoreText != null) scoreText.text = $"{score}";
        if (timerText != null) timerText.text = $"{Mathf.CeilToInt(timeLeft)}";
    }

    void ShowPopup(string msg)
    {
        if (resultPopup == null || resultPopupText == null) return;
        resultPopup.SetActive(true);
        resultPopupText.text = msg;
        CancelInvoke(nameof(HidePopup));
        Invoke(nameof(HidePopup), 0.9f);
    }

    void HidePopup()
    {
        if (resultPopup != null) resultPopup.SetActive(false);
    }

    void PlaySE(bool success)
    {
        if (audioSource == null) return;
        var clip = success ? successClip : failClip;
        if (clip != null) audioSource.PlayOneShot(clip);
    }
}
