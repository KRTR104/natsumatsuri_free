using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IkayakiController : MonoBehaviour
{
    [Header("参照")]
    public Image ikaImage;                 // 表示するイカのImage
    public Sprite ikaRaw;                  // 生
    public Sprite ikaCooked;               // ちょうど
    public Sprite ikaBurned;               // 焦げ

    [Header("焼き加減設定")]
    public float cookSpeed = 1.0f;         // 焼ける速さ
    public float perfectTime = 3.0f;       // ちょうど良い時間
    public float tolerance = 0.7f;         // 許容範囲

    float cookTimer = 0f;

    void Start()
    {
        ResetIkayaki();
    }

    void Update()
    {
        if (IkayakiGameManager.Instance == null || !IkayakiGameManager.Instance.IsPlaying) return;

        // スペース押しで焼き時間を進める
        if (Input.GetKey(KeyCode.Space))
        {
            cookTimer += Time.deltaTime * cookSpeed;
            UpdateIkayakiImage();
        }

        // スペース離したら判定
        if (Input.GetKeyUp(KeyCode.Space))
        {
            JudgeAndFinish();
        }
    }

    void UpdateIkayakiImage()
    {
        if (ikaImage == null) return;

        if (cookTimer < perfectTime * 0.7f)
            ikaImage.sprite = ikaRaw;     // 生
        else if (cookTimer < perfectTime * 1.3f)
            ikaImage.sprite = ikaCooked;  // ちょうど
        else
            ikaImage.sprite = ikaBurned;  // 焦げ
    }

    void JudgeAndFinish()
    {
        int score;
        string message;
        bool success;

        if (Mathf.Abs(cookTimer - perfectTime) <= tolerance)
        {
            score = 100; message = "パーフェクト！"; success = true;
        }
        else if (cookTimer < perfectTime)
        {
            score = 50; message = "生焼け！"; success = false;
        }
        else
        {
            score = 50; message = "焦げすぎ！"; success = false;
        }

        IkayakiGameManager.Instance.OnIkayakiFinished(score, message, success);

        ResetIkayaki();
    }


    void ResetIkayaki()
    {
        cookTimer = 0f;
        if (ikaImage != null && ikaRaw != null)
            ikaImage.sprite = ikaRaw;
    }
}
