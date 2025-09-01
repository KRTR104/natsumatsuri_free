using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StallInteraction : MonoBehaviour
{
    [TextArea]
    public string question = "○○を作る"; // 質問文
    public string sceneToLoad = "NextScene";      // 遷移先シーン名
    public int playerLayer = 3;                   // プレイヤーのLayer番号（例: PlayerLayerを8に設定）

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
