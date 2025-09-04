using TMPro;
using UnityEngine;

public class CombatUI : MonoBehaviour
{
    [SerializeField] private GameObject m_ButtonPrefab;
    [SerializeField] Transform m_Button;
    [SerializeField] private TextMeshProUGUI m_TextBox;
    [SerializeField] private TextMeshProUGUI m_Playerpokemon;
    public void Initialize(Pokemoninformation pokemoninformation)
    {
        if (pokemoninformation.Moves == null)
        {
            Debug.LogError("Pokemon has no moves");
            return;
        }
        foreach (var move in pokemoninformation.Moves)
        {
            MoveButton button = Instantiate(m_ButtonPrefab, m_Button).GetComponent<MoveButton>();
            button.Initialize(move);
        }
    }

    public void SetTextBox(string text)
    {
        m_TextBox.text = text;
        m_Playerpokemon.text = "Pengin";
    }
}
