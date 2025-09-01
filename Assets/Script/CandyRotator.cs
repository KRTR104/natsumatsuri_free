using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyRotator : MonoBehaviour
{
    [Header("回転")]
    public float rotateSpeed = 120f;     // 回す速さ（度/秒）

    [Header("綿あめ")]
    public Transform cottonCandy;        // 棒先端の綿あめ
    public float minScale = 0.5f;        // 初期サイズ
    public float maxScale = 5.0f;        // 上限サイズ（見た目の最大）
    public float growPerSecond = 0.6f;   // 入力中の成長速度

    [Header("判定")]
    public float perfectSize = 3.0f;     // 目標サイズ
    public float tolerance = 0.5f;      // 許容範囲（±で判定）

    public bool IsPlaying { get; set; } = true;  // GameManagerからON/OFF

    float currentScale;

    void Start()
    {
        currentScale = minScale;
        if (cottonCandy != null) cottonCandy.localScale = Vector3.one * currentScale;
    }

    void Update()
    {
        if (!IsPlaying) return;

        // ←→キー入力で回転
        float h = Input.GetAxis("Horizontal"); // A/D, ←→
        if (Mathf.Abs(h) > 0.01f)
        {
            transform.Rotate(Vector3.forward, -h * rotateSpeed * Time.deltaTime);

            // 入力がある間だけ綿あめが育つ
            currentScale = Mathf.Min(currentScale + growPerSecond * Time.deltaTime, maxScale);
            if (cottonCandy != null) cottonCandy.localScale = Vector3.one * currentScale;
        }

        // スペースで完成判定
        if (Input.GetKeyDown(KeyCode.Space))
        {
            JudgeAndFinish();
        }
    }

    void JudgeAndFinish()
    {
        // 判定（Perfect/Too Small/Too Big）
        int score;
        string message;
        bool success;

        if (Mathf.Abs(currentScale - perfectSize) <= tolerance)
        {
            score = 100; message = "パーフェクト!"; success = true;
        }
        else if (currentScale < perfectSize)
        {
            score = -50; message = "小さすぎ！"; success = false;
        }
        else
        {
            score = 50; message = "大きすぎ！"; success = false;
        }

        // GameManager へ通知
        CandyGameManager.Instance.OnCandyFinished(score, message, success);

        // 次の綿あめに備えてリセット
        currentScale = minScale;
        if (cottonCandy != null) cottonCandy.localScale = Vector3.one * currentScale;
    }
}
