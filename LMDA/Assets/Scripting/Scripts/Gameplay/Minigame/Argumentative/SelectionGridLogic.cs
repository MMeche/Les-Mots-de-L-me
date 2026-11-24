using System;
using System.Collections.Generic;
using Ink.Parsed;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SelectionGridLogic : MonoBehaviour
{
    int _fullSlot = 0;
    int _nextEmptyIndx=0;

    List<ArgWordCardLogic> _loadedCards;
    List<ArgCardType> _loadedCombo;
    BattleManager _battleManager;
    [SerializeField] GameObject _validateButton;
    [SerializeField] GameObject _comboInfo;

    void OnEnable()
    {
        _battleManager = ServiceProvider.Instance.ServiceLocator.Get<BattleManager>();
        _loadedCombo = new();
        _loadedCards = new(transform.childCount);
        _nextEmptyIndx = 0;
        _fullSlot = 0;
        for (int i = 0; i < transform.childCount ; i++)
        {
            _loadedCards.Add(null);
        }
    }

    public void AddCard(Transform newCard)
    {
        ArgWordCardLogic argCard = newCard.GetComponent<ArgWordCardLogic>();

        if (transform.childCount - TrueCount() > 0)
        {
            if (TrueCount() == 0)
            {
                _comboInfo.SetActive(true);
            }
            if (argCard.GetArgCardType() != ArgCardType.neutral)
            {
                _loadedCards[_nextEmptyIndx] = argCard;
                newCard.SetParent(transform.GetChild(_nextEmptyIndx), false);
                argCard.BorderWiden();
            }
            else
            {
                _loadedCards[_nextEmptyIndx] = Instantiate(argCard, transform.GetChild(_nextEmptyIndx));
                _loadedCards[_nextEmptyIndx].BorderWiden();
            }

                _nextEmptyIndx = FindNextEmptyIndex();
            if (transform.childCount - TrueCount() == 0)
            {
                _validateButton.SetActive(true);
                
            }
            UpdateCombo();
        }
    }
    int FindNextEmptyIndex()
    {
        return _loadedCards.FindIndex(x => x == null);
    }

    int TrueCount()
    {
        int count = 0;
        foreach(var c in _loadedCards)
        {
            if (c != null) count++;
        }
        return count;
    }

    private void UpdateCombo()
    {
        _loadedCombo.Clear();
        foreach (var card in _loadedCards)
        {
            if (card != null) _loadedCombo.Add(card.GetArgCardType());
        }
        _battleManager.BattleManagerEventBus.ComboUpdate(new() { LoadedCombo = _loadedCombo });
        //SendMessageUpwards("ComboUpdate", _loadedCombo);
    }

    public void ValidateCombo()
    {
        _loadedCombo.Clear();
        foreach(var card in _loadedCards)
        {
            if (card != null) _loadedCombo.Add(card.GetArgCardType());
        }
        _validateButton.SetActive(false);
        _battleManager.BattleManagerEventBus.ComboAchieved(new() { LoadedCombo = _loadedCombo });
        //SendMessageUpwards("ComboAchieved", _loadedCombo);
        
    }

    public void UpdateEmptySlots(Transform card)
    {
        //_loadedCards.FindIndex(x => x==card.GetComponent<ArgWordCardLogic>());
        if (card != null)_loadedCards[_loadedCards.FindIndex(x => x == card.GetComponent<ArgWordCardLogic>())] = null;
        _nextEmptyIndx = FindNextEmptyIndex();
        if (transform.childCount - TrueCount() != 0)
        {
            _validateButton.SetActive(false);
        }
        UpdateCombo();
        if (TrueCount() == 0)
        {
            _comboInfo.SetActive(false);
        }
    }

    public List<ArgWordCardLogic> GetArgCardList()
    {
        return _loadedCards;
    }

    private void OnDisable()
    {
        _validateButton.SetActive(false);
        _loadedCombo.Clear();
        _loadedCards.Clear();
    }
}
