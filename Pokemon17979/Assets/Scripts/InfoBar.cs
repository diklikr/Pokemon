using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoBar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_PokemonName;
    [SerializeField] private Slider m_HealthBar;
    [SerializeField] private Slider m_EnemyHP;
    [SerializeField] private TextMeshProUGUI m_HP;

    private float m_TargetHealth;
    private Pokemoninformation pokemoninformation;

    public void Initialize(Pokemoninformation p_Info)
    {
        m_PokemonName.text = p_Info.Name;
        m_HealthBar.maxValue = p_Info.maxHealth;
        m_EnemyHP.maxValue = p_Info.maxHealth;
        m_TargetHealth = p_Info.Health;
    }

    public void SetHealth(Pokemoninformation playerInfo,Pokemoninformation enemyInfo)
    {
        pokemoninformation = playerInfo;
        m_HealthBar.value = playerInfo.CurrentHP;
        m_EnemyHP.value = enemyInfo.CurrentHP;
        m_HP.text = $"{playerInfo.CurrentHP}/{playerInfo.maxHealth}";
    }
}
