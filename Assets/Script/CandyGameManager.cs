using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CandyGameManager : MonoBehaviour
{
    public static CandyGameManager Instance { get; private set; }

    [Header("参照")]
    public CandyRotator rotator;               // CandyPivot のスクリプト
    public Transform stick;                    // 棒
    public Transform candy;                    // 綿あめ

    [Header("UI")]
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public GameObject resultPopup;             // 吹き出し等
    public TextMeshProUGUI resultPopupText;

    [Header("効果音（任意）")]
    public AudioSource audioSource;
    public AudioClip successClip;
    public AudioClip failClip;

    [Header("ルール")]
    public float timeLimit = 60f;

    int score;
    float timeLeft;
    bool playing;

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    void Start()
    {
        StartGame();
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

    public void StartGame()
    {
        score = 0;
        timeLeft = timeLimit;
        playing = true;
        if (rotator != null) rotator.IsPlaying = true;
        UpdateUI();

        // 初期の綿あめ
        if (candy != null) candy.localScale = Vector3.one * rotator.minScale;
    }

    void EndGame()
    {
        playing = false;
        if (rotator != null) rotator.IsPlaying = false;

        ShowPopup($"終了！スコア: {score}");
        // ここでリザルト画面表示やハイスコア保存など
        // PlayerPrefs.SetInt("HighScore_Cotton", Mathf.Max(score, PlayerPrefs.GetInt("HighScore_Cotton", 0)));
    }

    public void OnCandyFinished(int addScore, string message, bool success)
    {
        if (!playing) return;

        score += addScore;
        UpdateUI();

        // 演出
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

