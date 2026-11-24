using System;
using TMPro;
using UnityEngine;

public class ComboInfoDisplay : MonoBehaviour
{
    [SerializeField] Animator _controller;
    [SerializeField] TMP_Text _atkText;
    [SerializeField] TMP_Text _defText;
    
    public void UpdateDisplay(int atk, int def)
    {
        
        _atkText.text = (atk>0?"+ ":"- ") + Math.Abs(atk).ToString() + " ATK";
        _defText.text = (def<0?"+ ":"- ") + Math.Abs(def).ToString() + " DEF";
        _controller.SetTrigger("Update");
    }

    public void UpdateEnemyDisplay(int atk)
    {
        _atkText.text =  atk.ToString() + " Dégâts";
        _controller.SetTrigger("Update");
    }
}
