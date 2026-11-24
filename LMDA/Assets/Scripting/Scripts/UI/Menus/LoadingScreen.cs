using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class LoadingScreen : MonoBehaviour
{
    
    public GameObject loadingPanel;
    public Slider progressBar;
    public TextMeshProUGUI progressText;
    public Animator _animator;

    bool _wasPlaying = true;

    public void LoadScene(string sceneName)
    {
        loadingPanel.SetActive(true);
        StartCoroutine(LoadSceneAsync(sceneName));
    }


    private IEnumerator LoadSceneAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        while (!operation.isDone && _animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1f)
        {
            if (operation.progress <= 0.1f) 
            {
                progressText.text = "0%";
            }


            if(!operation.isDone && (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f && !_animator.IsInTransition(0)))
            {
                _animator.Play("Loading",0,0f);
            }
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            progressBar.value = progress;

            if (progressText != null)
                progressText.text = $"{progress * 100f:F0}%";

            // Quand le chargement est presque terminé
            if (operation.progress >= 0.9f &&_animator.GetCurrentAnimatorStateInfo(0).normalizedTime <= 1f)
            {
                progressText.text = "90%";
                Debug.Log(operation.progress);


            }
            
            yield return null;
        }
        operation.allowSceneActivation = true;
        
    }
}
