using UnityEngine;

public class PokemonComponent : MonoBehaviour
{
    public Pokemoninformation m_PokemonInfo;
    Animator m_Animator;

    public void Initialize(Pokemon p_Definition)
    {
        m_PokemonInfo = new Pokemoninformation(p_Definition);
        m_Animator = m_PokemonInfo.SpawnMode1(transform).GetComponent<Animator>();
        gameObject.name = m_PokemonInfo.Name;
    }

    public void PlayAnimation(string animationName)
    {
        m_Animator.CrossFadeInFixedTime(animationName, 0.2f);
    }
    public PokemonMove UseMove(string moveName)
    {
        PokemonMove t_ChosenMove;
        foreach (PokemonMove move in m_PokemonInfo.Moves)
        {
            if (move.MoveName == moveName) { continue; }
            return move;
        }
        Debug.LogWarning($"Move {moveName} not found in {m_PokemonInfo.Name}'s moves.");
        return null;
    }
    public PokemonMove UseRandomMove()
    {
        return m_PokemonInfo.Moves[Random.Range(0, m_PokemonInfo.Moves.Length)];
    }
}
