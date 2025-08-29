using TMPro;
using UnityEngine;

public class MoveButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_Move_Type;
    [SerializeField] private TextMeshProUGUI m_Move_Name;
    [SerializeField] private TextMeshProUGUI m_Move_Name2;

    private PokemonMove m_MovetoChoose;
  public void Initialize(PokemonMove p_Move)
    {
        m_MovetoChoose = p_Move;
        //m_Move_Type.text = p_Move.Type.ToString();
        m_Move_Name.text = p_Move.name;

    }

    public void OnClick()
    {
        if(m_MovetoChoose == null)  return;
        //CombatManager.SetPlayerMove(m_MovetoChoose);
    }
}
