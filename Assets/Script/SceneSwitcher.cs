using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    [SerializeField] private string sceneName; // 遷移先のシーン名
    [SerializeField] private AudioClip buttonClickSound; // ボタンを押したときの音声ファイル
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
    }

    // このメソッドをボタンのOnClickイベントにアタッチします
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
        yield return new WaitForSeconds(buttonClickSound.length); // 音が再生されるまで待つ
        LoadScene();
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(sceneName);
    }
}