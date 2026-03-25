using UnityEngine;

public class PokemonComponent : MonoBehaviour
{
    public Pokemoninformation m_PokemonInfo;
    Animator m_Animator;

    public void Initialize(Pokemon p_Definition)
    {
        m_PokemonInfo = new Pokemoninformation(p_Definition);
        GameObject mode = m_PokemonInfo.SpawnMode1(transform);
        if (mode != null)
            m_Animator = mode.GetComponent<Animator>();
        gameObject.name = m_PokemonInfo.Name;
    }

    public void PlayAnimation(string animationName)
    {
        m_Animator?.CrossFadeInFixedTime(animationName, 0.2f);
    }

    public PokemonMove UseMove(string moveName)
    {
        if (m_PokemonInfo == null) return null;
        foreach (PokemonMove move in m_PokemonInfo.Moves)
        {
            if (move != null && move.MoveName == moveName)
            {
                return move;
            }
        }
        Debug.LogWarning($"Move {moveName} not found in {m_PokemonInfo.Name}'s moves.");
        return null;
    }

    public PokemonMove UseRandomMove()
    {
        var moves = m_PokemonInfo.Moves;
        if (moves == null || moves.Length == 0) return null;
        return moves[Random.Range(0, moves.Length)];
    }
}
