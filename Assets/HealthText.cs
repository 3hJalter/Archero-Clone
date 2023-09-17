using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class HealthText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshProUGUI;
    [SerializeField] private Animation animationFly;

    private HealthBar _healthBar;
    private float _timeHidden;
    public void OnInit(int health, HealthBar pHealthBar)
    {
        _healthBar = pHealthBar;
        _timeHidden = 1f;
        textMeshProUGUI.text = health.ToString();
        animationFly.Play();
    }

    private void Update()
    {
        if (_timeHidden > 0) _timeHidden -= Time.deltaTime;
        else
        {
            _healthBar.DespawnText(this);
        }
    }
}
