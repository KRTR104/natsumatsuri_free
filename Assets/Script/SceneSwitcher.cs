using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    [SerializeField] private string sceneName; // �J�ڐ�̃V�[����
    [SerializeField] private AudioClip buttonClickSound; // �{�^�����������Ƃ��̉����t�@�C��
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    // ���̃��\�b�h���{�^����OnClick�C�x���g�ɃA�^�b�`���܂�
    public void SwitchScene()
    {
        if (buttonClickSound != null)
        {
            StartCoroutine(PlaySoundAndSwitchScene());
        }
        else
        {
            LoadScene();
        }
    }

    private IEnumerator PlaySoundAndSwitchScene()
    {
        audioSource.PlayOneShot(buttonClickSound);
        yield return new WaitForSeconds(buttonClickSound.length); // �����Đ������܂ő҂�
        LoadScene();
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}