using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CandyGameManager : MonoBehaviour
{
    public static CandyGameManager Instance { get; private set; }

    [Header("�Q��")]
    [SerializeField] CandyRotator rotator;                // CandyPivot �̃X�N���v�g
    [SerializeField] Transform stick;                    // �_
    [SerializeField] Transform candy;                    // �Ȃ���

    [Header("UI")]
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] GameObject resultPopup;
    [SerializeField] TextMeshProUGUI resultPopupText;
    [SerializeField] TextMeshProUGUI countdownText;

    [Header("���ʉ��i�C�Ӂj")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip successClip;
    [SerializeField] AudioClip failClip;

    [Header("��������")]
    [SerializeField] float timeLimit = 60f;

    int score;
    float timeLeft;
    bool playing;

    // �ǉ�: �_�̏����ʒu��ۑ�
    Vector3 stickStartPos;
    Quaternion stickStartRot;

    void Awake()
    {
        if (Instance != null && Instance != this) Destroy(gameObject);
        else Instance = this;
    }

    void Start()
    {
        // �_�̏����ʒu�E��]���L�^
        if (stick != null)
        {
            stickStartPos = stick.position;
            stickStartRot = stick.rotation;
        }

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
        if (rotator != null) rotator.IsPlaying = false;

        int count = 3;
        while (count > 0)
        {
            if (countdownText != null)
                countdownText.text = count.ToString();
            yield return new WaitForSeconds(1f);
            count--;
        }

        if (countdownText != null)
            countdownText.text = "�X�^�[�g!";
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
        if (rotator != null) rotator.IsPlaying = true;
        UpdateUI();

        ResetStickAndCandy();
    }

    void EndGame()
    {
        playing = false;
        if (rotator != null) rotator.IsPlaying = false;

        ShowPopup($"�I���I�X�R�A: {score}");

        // 3�b��ɃV�[���J��
        StartCoroutine(EndRoutine());
    }

    IEnumerator EndRoutine()
    {
        yield return new WaitForSeconds(3f);

        // �V�[���J��
        SceneManager.LoadScene("SelectScene");
    }

    public void OnCandyFinished(int addScore, string message, bool success)
    {
        if (!playing) return;

        score += addScore;
        UpdateUI();

        ShowPopup(message);
        PlaySE(success);

        // ���������̂Ŗ_�ƖȂ��߂����Z�b�g
        ResetStickAndCandy();
    }

    void ResetStickAndCandy()
    {
        if (stick != null)
        {
            stick.position = stickStartPos;
            stick.rotation = stickStartRot;
        }
        if (candy != null)
            candy.localScale = Vector3.one * (rotator != null ? rotator.minScale : 0.5f);
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
