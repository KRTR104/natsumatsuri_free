using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StallInteraction : MonoBehaviour
{
    [TextArea]
    [SerializeField] string question = "○○を作る"; // 質問文
    [SerializeField] string sceneToLoad = "NextScene";      // 遷移先シーン名
    [SerializeField] int playerLayer = 3;                   // プレイヤーのLayer番号

    private PopupManager popupManager;
    private bool playerInRange = false;

    void Start()
    {
        popupManager = FindObjectOfType<PopupManager>();
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            popupManager.ShowPopup(question, sceneToLoad);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == playerLayer)
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == playerLayer)
        {
            playerInRange = false;
        }
    }
}
