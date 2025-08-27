using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InfoBar : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_PokemonName;
    [SerializeField] private Slider m_HealthBar;
    [SerializeField] private TextMeshProUGUI m_Level;
    [SerializeField] private TextMeshProUGUI m_HP;

    private float m_TargetHealth;

    public void Initialize(Pokemoninformation p_Info)
    {
        m_PokemonName.text = p_Info.Name;
        m_HealthBar.maxValue = p_Info.maxHealth;
        m_Level.text = "Lvl: " + p_Info.Level.ToString();
        m_HealthBar.value = p_Info.CurrentHP;
        m_TargetHealth = p_Info.Health;
    }

    private void Update()
    {
        //transform.LookAt(transform.position - Camera.main.transform);
    }

    public void SetHealth(float health) => m_TargetHealth = health;
}
