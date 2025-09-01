using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandyRotator : MonoBehaviour
{
    [Header("��]")]
    public float rotateSpeed = 120f;     // �񂷑����i�x/�b�j

    [Header("�Ȃ���")]
    public Transform cottonCandy;        // �_��[�̖Ȃ���
    public float minScale = 0.5f;        // �����T�C�Y
    public float maxScale = 5.0f;        // ����T�C�Y�i�����ڂ̍ő�j
    public float growPerSecond = 0.6f;   // ���͒��̐������x

    [Header("����")]
    public float perfectSize = 3.0f;     // �ڕW�T�C�Y
    public float tolerance = 0.5f;      // ���e�͈́i�}�Ŕ���j

    public bool IsPlaying { get; set; } = true;  // GameManager����ON/OFF

    float currentScale;

    void Start()
    {
        currentScale = minScale;
        if (cottonCandy != null) cottonCandy.localScale = Vector3.one * currentScale;
    }

    void Update()
    {
        if (!IsPlaying) return;

        // �����L�[���͂ŉ�]
        float h = Input.GetAxis("Horizontal"); // A/D, ����
        if (Mathf.Abs(h) > 0.01f)
        {
            transform.Rotate(Vector3.forward, -h * rotateSpeed * Time.deltaTime);

            // ���͂�����Ԃ����Ȃ��߂����
            currentScale = Mathf.Min(currentScale + growPerSecond * Time.deltaTime, maxScale);
            if (cottonCandy != null) cottonCandy.localScale = Vector3.one * currentScale;
        }

        // �X�y�[�X�Ŋ�������
        if (Input.GetKeyDown(KeyCode.Space))
        {
            JudgeAndFinish();
        }
    }

    void JudgeAndFinish()
    {
        // ����iPerfect/Too Small/Too Big�j
        int score;
        string message;
        bool success;

        if (Mathf.Abs(currentScale - perfectSize) <= tolerance)
        {
            score = 100; message = "�p�[�t�F�N�g!"; success = true;
        }
        else if (currentScale < perfectSize)
        {
            score = -50; message = "���������I"; success = false;
        }
        else
        {
            score = 50; message = "�傫�����I"; success = false;
        }

        // GameManager �֒ʒm
        CandyGameManager.Instance.OnCandyFinished(score, message, success);

        // ���̖Ȃ��߂ɔ����ă��Z�b�g
        currentScale = minScale;
        if (cottonCandy != null) cottonCandy.localScale = Vector3.one * currentScale;
    }
}
