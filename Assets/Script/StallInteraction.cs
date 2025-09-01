using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StallInteraction : MonoBehaviour
{
    [TextArea]
    public string question = "���������"; // ���╶
    public string sceneToLoad = "NextScene";      // �J�ڐ�V�[����
    public int playerLayer = 3;                   // �v���C���[��Layer�ԍ��i��: PlayerLayer��8�ɐݒ�j

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
