using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CornController : MonoBehaviour
{
    [Header("参照")]
    public Image cornImage;
    public Sprite cornRaw;
    public Sprite cornCooked;
    public Sprite cornBurned;
    public Sprite cornSauced;

    [Header("焼き加減設定")]
    public float cookSpeed = 1.0f;
    public float perfectTime = 3.0f;
    public float tolerance = 0.7f;

    [Header("タレ塗り設定")]
    [SerializeField] private float sauceNeeded = 200f; // 塗り切り目安（なぞった距離）

    float cookTimer = 0f;
    bool waitingSauce = false;

    // タレ塗り用
    private float sauceProgress = 0f;
    private Vector3 lastMousePos;
    private bool isSaucing = false;

    void Start()
    {
        ResetCorn();
    }

    void Update()
    {
        if (!CornGameManager.Instance.IsPlaying) return;

        // スペースで焼き
        if (Input.GetKey(KeyCode.Space) && !waitingSauce)
        {
            cookTimer += Time.deltaTime * cookSpeed;
            UpdateCornImage();
        }

        if (Input.GetKeyUp(KeyCode.Space) && !waitingSauce)
        {
            JudgeCook();
        }

        // タレ塗りフェーズ
        if (waitingSauce)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isSaucing = true;
                lastMousePos = Input.mousePosition;
            }

            if (Input.GetMouseButton(0) && isSaucing)
            {
                Vector3 current = Input.mousePosition;
                float dist = Vector3.Distance(current, lastMousePos);
                sauceProgress += dist;
                lastMousePos = current;

                // デバッグ表示
                Debug.Log("タレ進捗: " + sauceProgress);
            }

            if (Input.GetMouseButtonUp(0) && isSaucing)
            {
                isSaucing = false;
                JudgeSauce();
            }
        }
    }

    void UpdateCornImage()
    {
        if (cornImage == null) return;

        if (cookTimer < perfectTime * 0.7f)
            cornImage.sprite = cornRaw;
        else if (cookTimer < perfectTime * 1.3f)
            cornImage.sprite = cornCooked;
        else
            cornImage.sprite = cornBurned;
    }

    void JudgeCook()
    {
        int score;
        string message;
        bool success;

        if (Mathf.Abs(cookTimer - perfectTime) <= tolerance)
        {
            score = 50; message = "焼き加減バッチリ！"; success = true;
        }
        else if (cookTimer < perfectTime)
        {
            score = 20; message = "生焼け！"; success = false;
        }
        else
        {
            score = 20; message = "焦げすぎ！"; success = false;
        }

        CornGameManager.Instance.OnCornFinished(score, message, success);

        // タレ塗りフェーズへ
        waitingSauce = true;
        sauceProgress = 0f;
    }

    void JudgeSauce()
    {
        int score;
        string message;
        bool success;

        if (sauceProgress < sauceNeeded * 0.4f)
        {
            score = 20; message = "タレ少ない！"; success = false;
        }
        else if (sauceProgress < sauceNeeded * 1.2f)
        {
            score = 50; message = "タレちょうど！"; success = true;
        }
        else
        {
            score = 20; message = "タレだくだく！"; success = false;
        }

        if (cornImage != null && cornSauced != null)
            cornImage.sprite = cornSauced;

        FinishCorn(score, message, success);
    }

    void FinishCorn(int addScore, string message, bool success)
    {
        CornGameManager.Instance.OnCornFinished(addScore, message, success);
        ResetCorn();
    }

    void ResetCorn()
    {
        cookTimer = 0f;
        waitingSauce = false;
        sauceProgress = 0f;

        if (cornImage != null && cornRaw != null)
            cornImage.sprite = cornRaw;
    }
}
