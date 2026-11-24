using TMPro;
using UnityEngine;
using System.IO;
using System;
public class DeleteConfirmLogic : MonoBehaviour
{
    [SerializeField] LoadSectionManager _loadSection;
    [SerializeField] TMP_Text textZone;

    private void OnEnable()
    {
        string saveFile = _loadSection.GetSaveFile();
        DateTime date = File.GetCreationTime(saveFile);
        textZone.text = "Vous allez supprimer la sauvegarde " + Path.GetFileNameWithoutExtension(saveFile) + " du " + date.Day + "/" + date.Month + "/" + date.Year;
    }
}
