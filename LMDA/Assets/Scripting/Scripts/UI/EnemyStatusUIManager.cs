using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EnemyStatusUIManager : MonoBehaviour
{
    BattleUIManager _battleUIManager;
    BattleManager _battleManager;
    EnemyStatus _enemyStatus;
    TMP_Text _nameTextField;
    Image _healthBar;
    Image _incomingDamage;
    float _currentHealth;
    [SerializeField] GameObject _damageDisplayOnHealthBar;

    void OnEnable()
    {
        _nameTextField = GetComponentInChildren<TMP_Text>();
        _healthBar = GetComponentsInChildren<Image>()[1];
        _incomingDamage = GetComponentsInChildren<Image>()[2];

        _battleUIManager = ServiceProvider.Instance.ServiceLocator.Get<BattleUIManager>();
        _battleManager = ServiceProvider.Instance.ServiceLocator.Get<BattleManager>();

        _battleUIManager.BattleUIEventBus.OnSetBaseValue += SetBaseValue;
        _battleUIManager.BattleUIEventBus.OnEnemyStatusUpdate += EnemyAttacked;
        _battleUIManager.BattleUIEventBus.OnEnemyIncomingDamageUpdate += IncomingDamage;
    }

    private void SetBaseValue(BattleUIEventContext obj)
    {
        if (obj.EnemyStatus != null)
        {
            try
            {
                _enemyStatus = obj.EnemyStatus;
                SetUI();
            }
            catch
            {
                Debug.LogWarning($"No enemy status was sent on battle start");
            }
        }
    }

    private void SetUI()
    {
        _healthBar.fillAmount = _battleManager.GetEnemyCurrentHealth() / _enemyStatus.MaxHealth;
        _nameTextField.text = _enemyStatus.CharName;

        _currentHealth = _healthBar.fillAmount;
    }

    private void EnemyAttacked(BattleUIEventContext obj)
    {
        float tmp =  _currentHealth - obj.NewHealth;

        _incomingDamage.fillAmount = 0;
        _currentHealth = obj.NewHealth;
        _healthBar.fillAmount = _currentHealth;

        RectTransform damageDisplayRect = Instantiate(_damageDisplayOnHealthBar, this.transform).GetComponent<RectTransform>();
        RectTransform incomingDamageRect = _incomingDamage.GetComponent<RectTransform>();
        RectTransform healthBarRect = _healthBar.GetComponent<RectTransform>();

        damageDisplayRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, healthBarRect.rect.width* tmp);
        damageDisplayRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, healthBarRect.rect.height);
        damageDisplayRect.anchoredPosition = new Vector2(healthBarRect.anchoredPosition.x - healthBarRect.rect.width/2 + healthBarRect.rect.width*(1-_currentHealth) - damageDisplayRect.rect.width/2, healthBarRect.anchoredPosition.y);
        

        incomingDamageRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _healthBar.GetComponent<RectTransform>().rect.width * _healthBar.fillAmount);
        incomingDamageRect.anchoredPosition = new Vector2(incomingDamageRect.anchoredPosition.x + tmp*healthBarRect.rect.width/ 2, incomingDamageRect.anchoredPosition.y);
        
        



    }

    private void IncomingDamage(BattleUIEventContext obj)
    {
        if(obj.StackDamage)_incomingDamage.fillAmount += obj.IncomingPlayerDamage / (_currentHealth*_enemyStatus.MaxHealth);
        else _incomingDamage.fillAmount = obj.IncomingPlayerDamage / (_currentHealth * _enemyStatus.MaxHealth);
    }
}
