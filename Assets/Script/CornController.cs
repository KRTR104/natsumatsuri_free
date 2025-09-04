using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CornController : MonoBehaviour
{
    [Header("�Q��")]
    public Image cornImage;
    public Sprite cornRaw;
    public Sprite cornCooked;
    public Sprite cornBurned;
    public Sprite cornSauced;

    [Header("�Ă������ݒ�")]
    public float cookSpeed = 1.0f;
    public float perfectTime = 3.0f;
    public float tolerance = 0.7f;

    [Header("�^���h��ݒ�")]
    [SerializeField] private float sauceNeeded = 200f; // �h��؂�ڈ��i�Ȃ����������j

    float cookTimer = 0f;
    bool waitingSauce = false;

    // �^���h��p
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

        // �X�y�[�X�ŏĂ�
        if (Input.GetKey(KeyCode.Space) && !waitingSauce)
        {
            cookTimer += Time.deltaTime * cookSpeed;
            UpdateCornImage();
        }

        if (Input.GetKeyUp(KeyCode.Space) && !waitingSauce)
        {
            JudgeCook();
        }

        // �^���h��t�F�[�Y
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

                // �f�o�b�O�\��
                Debug.Log("�^���i��: " + sauceProgress);
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
            score = 50; message = "�Ă������o�b�`���I"; success = true;
        }
        else if (cookTimer < perfectTime)
        {
            score = 20; message = "���Ă��I"; success = false;
        }
        else
        {
            score = 20; message = "�ł������I"; success = false;
        }

        CornGameManager.Instance.OnCornFinished(score, message, success);

        // �^���h��t�F�[�Y��
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
            score = 20; message = "�^�����Ȃ��I"; success = false;
        }
        else if (sauceProgress < sauceNeeded * 1.2f)
        {
            score = 50; message = "�^�����傤�ǁI"; success = true;
        }
        else
        {
            score = 20; message = "�^�����������I"; success = false;
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
