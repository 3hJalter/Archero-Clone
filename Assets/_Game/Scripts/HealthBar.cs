using UnityEngine;
using UnityEngine.UI;

public class HealthBar : HMonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private Entity entity;
    [SerializeField] private HealthText healthTextPrefab;
    private readonly MiniPool<HealthText> _healthTextPool = new();
    private float _maxHealth;
    private bool _hasPool;
    private void Start()
    {
        _maxHealth = entity.GetMaxHealth();
        _hasPool = healthTextPrefab != null;
        if (_hasPool) _healthTextPool.OnInit(healthTextPrefab, 2, Tf);
    }

    public void OnChangeHealthBar(int damageHit = 0)
    {
        healthBar.fillAmount = entity.GetHealth() / _maxHealth;
        if (_hasPool) _healthTextPool.Spawn().OnInit(damageHit, this);
    }

    public void DespawnText(HealthText healthText)
    {
        _healthTextPool.Despawn(healthText);
    }

}
