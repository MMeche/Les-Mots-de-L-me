using UnityEngine;
using TMPro;
using UnityEngine.UI;
using static Unity.Burst.Intrinsics.X86.Avx;

public class PlayerStatusUIManager : MonoBehaviour
{
    BattleUIManager _battleUIManager;
    TMP_Text _nameTextField;
    Image _healthBar;
    Image _incomingDamage;
    float _currentHealth;

    [SerializeField] GameObject _damageDisplayOnHealthBar;
    [SerializeField] PlayerStatus _playerStatus;

    void OnEnable()
    {
        _nameTextField = GetComponentInChildren<TMP_Text>();
        _healthBar = GetComponentsInChildren<Image>()[0];
        _incomingDamage = GetComponentsInChildren<Image>()[1];
        

        _battleUIManager = ServiceProvider.Instance.ServiceLocator.Get<BattleUIManager>();

        _battleUIManager.BattleUIEventBus.OnSetBaseValue += SetBaseValue;
        _battleUIManager.BattleUIEventBus.OnPlayerStatusUpdate += UpdateUI;
        _battleUIManager.BattleUIEventBus.OnPlayerIncomingDamageUpdate += IncomingDamage;
    }

    private void SetBaseValue(BattleUIEventContext obj)
    {
        if (obj != null)
        {
            if(obj.PlayerStatus != null)
            {
                //_playerStatus = obj.PlayerStatus;
                SetUI();
            }
            
            //catch 
            //{
            //    Debug.LogWarning($"No player status was sent on battle start");
            //}
        }
    }

    private void SetUI()
    {
        _healthBar.fillAmount = _playerStatus.Health / _playerStatus.MaxHealth;
        //_nameTextField.text = _playerStatus.CharName;
        _currentHealth = _playerStatus.Health / _playerStatus.MaxHealth;
        float tmp = 1 - _currentHealth;
        

        RectTransform healthBarRect = _healthBar.GetComponent<RectTransform>();
        RectTransform incomingDamageRect = _incomingDamage.GetComponent<RectTransform>();
        incomingDamageRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _healthBar.GetComponent<RectTransform>().rect.width * _healthBar.fillAmount);
        incomingDamageRect.anchoredPosition = new Vector2(healthBarRect.anchoredPosition.x - tmp * healthBarRect.rect.width / 2, incomingDamageRect.anchoredPosition.y);
    }

    private void UpdateUI(BattleUIEventContext obj)
    {
        float tmp = _currentHealth - obj.NewHealth;


        _currentHealth = obj.NewHealth;
        _incomingDamage.fillAmount = 0;

        RectTransform damageDisplayRect = Instantiate(_damageDisplayOnHealthBar, this.transform).GetComponent<RectTransform>();
        RectTransform incomingDamageRect = _incomingDamage.GetComponent<RectTransform>();
        RectTransform healthBarRect = _healthBar.GetComponent<RectTransform>();



        damageDisplayRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, healthBarRect.rect.width * _currentHealth * tmp);
        damageDisplayRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, healthBarRect.rect.height);
        damageDisplayRect.anchoredPosition = new Vector2(healthBarRect.anchoredPosition.x + healthBarRect.rect.width / 2 - healthBarRect.rect.width * (1 - _currentHealth) + damageDisplayRect.rect.width / 2, healthBarRect.anchoredPosition.y);
        _healthBar.fillAmount = _currentHealth;

        incomingDamageRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _healthBar.GetComponent<RectTransform>().rect.width * _healthBar.fillAmount);
        incomingDamageRect.anchoredPosition = new Vector2(incomingDamageRect.anchoredPosition.x - tmp * healthBarRect.rect.width / 2, incomingDamageRect.anchoredPosition.y);
    }

    private void PlayerAttacked(BattleUIEventContext obj)
    {
        float tmp = Mathf.Clamp01(_currentHealth) / Mathf.Clamp01(_playerStatus.MaxHealth) - obj.NewHealth;
        

        _currentHealth = obj.NewHealth;


        RectTransform damageDisplayRect = Instantiate(_damageDisplayOnHealthBar, this.transform).GetComponent<RectTransform>();
        RectTransform healthBarRect = _healthBar.GetComponent<RectTransform>();



        damageDisplayRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, healthBarRect.rect.width * _currentHealth * tmp);
        damageDisplayRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, healthBarRect.rect.height);
        damageDisplayRect.anchoredPosition = new Vector2(healthBarRect.anchoredPosition.x - healthBarRect.rect.width / 2 + healthBarRect.rect.width * (1 - _currentHealth) - damageDisplayRect.rect.width / 2, healthBarRect.anchoredPosition.y);
        _healthBar.fillAmount = _currentHealth;




    }

    private void IncomingDamage(BattleUIEventContext obj)
    {
        if(obj.StackDamage)_incomingDamage.fillAmount += obj.IncomingEnemyDamage / (_playerStatus.MaxHealth*_currentHealth);
        else _incomingDamage.fillAmount = obj.IncomingEnemyDamage / (_playerStatus.MaxHealth * _currentHealth );
    }
    public PlayerStatus GetStatus()
    {
        return _playerStatus;
    }

    private void OnDisable()
    {
        _battleUIManager.BattleUIEventBus.OnSetBaseValue -= SetBaseValue;
        _battleUIManager.BattleUIEventBus.OnPlayerStatusUpdate -= UpdateUI;
        _battleUIManager.BattleUIEventBus.OnPlayerIncomingDamageUpdate -= IncomingDamage;
    }
}
