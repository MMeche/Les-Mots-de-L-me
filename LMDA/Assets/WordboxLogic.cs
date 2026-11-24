using Ink.Parsed;
using UnityEngine;
using System.Collections.Generic;
using TMPro;
using System.Collections;

public class WordboxLogic : MonoBehaviour
{
    [SerializeField] List<string> _words;
    [SerializeField] GameObject _wordObject;
    [SerializeField] bool _oneTime;
    [SerializeField] float _delay =0.0f;


    SphereCollider _collider;
    bool _playing = false;
    private void OnEnable()
    {
        _collider = GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_playing)
        {
            StartCoroutine(SpawnWords(other.transform));

        }
    }
    IEnumerator SpawnWords(Transform target)
    {
        _playing = true;
        foreach(string word in _words)
        {
            TMP_Text textEntry = Instantiate(_wordObject, target).GetComponent<TMP_Text>();
            textEntry.text = word;
            yield return new WaitForSeconds(1.5f);
        }
        
        if (_oneTime)
        {
            gameObject.SetActive(false);
        }
        Invoke("ReactivateWordBox", _delay);
    }

    private void ReactivateWordBox()
    {
        _playing = false;
    }
}
