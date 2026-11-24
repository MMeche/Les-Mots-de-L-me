using UnityEngine;
using System.IO;

public class PlaySection : MonoBehaviour
{
    [SerializeField] Transform _content;
    private void OnEnable()
    {
        string [] files = Directory.GetFiles(Application.persistentDataPath, "*.json");
        if (files.Length != 0)
        {
            _content.GetChild(0).gameObject.SetActive(true); //Continuer
            _content.GetChild(1).gameObject.SetActive(true); //Charger
        }
        else
        {
            _content.GetChild(0).gameObject.SetActive(false); //Continuer
            _content.GetChild(1).gameObject.SetActive(false); //Charger
        }
    }

    

}
