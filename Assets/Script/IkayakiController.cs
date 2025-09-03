using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IkayakiController : MonoBehaviour
{
    [Header("�Q��")]
    public Image ikaImage;                 // �\������C�J��Image
    public Sprite ikaRaw;                  // ��
    public Sprite ikaCooked;               // ���傤��
    public Sprite ikaBurned;               // �ł�

    [Header("�Ă������ݒ�")]
    public float cookSpeed = 1.0f;         // �Ă��鑬��
    public float perfectTime = 3.0f;       // ���傤�Ǘǂ�����
    public float tolerance = 0.7f;         // ���e�͈�

    float cookTimer = 0f;

    void Start()
    {
        ResetIkayaki();
    }

    void Update()
    {
        if (IkayakiGameManager.Instance == null || !IkayakiGameManager.Instance.IsPlaying) return;

        // �X�y�[�X�����ŏĂ����Ԃ�i�߂�
        if (Input.GetKey(KeyCode.Space))
        {
            cookTimer += Time.deltaTime * cookSpeed;
            UpdateIkayakiImage();
        }

        // �X�y�[�X�������画��
        if (Input.GetKeyUp(KeyCode.Space))
        {
            JudgeAndFinish();
        }
    }

    void UpdateIkayakiImage()
    {
        if (ikaImage == null) return;

        if (cookTimer < perfectTime * 0.7f)
            ikaImage.sprite = ikaRaw;     // ��
        else if (cookTimer < perfectTime * 1.3f)
            ikaImage.sprite = ikaCooked;  // ���傤��
        else
            ikaImage.sprite = ikaBurned;  // �ł�
    }

    void JudgeAndFinish()
    {
        int score;
        string message;
        bool success;

        if (Mathf.Abs(cookTimer - perfectTime) <= tolerance)
        {
            score = 100; message = "�p�[�t�F�N�g�I"; success = true;
        }
        else if (cookTimer < perfectTime)
        {
            score = 50; message = "���Ă��I"; success = false;
        }
        else
        {
            score = 50; message = "�ł������I"; success = false;
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
