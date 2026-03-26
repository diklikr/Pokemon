// name=PokemonComponent.cs
using UnityEngine;

public class PokemonComponent : MonoBehaviour
{
    public Pokemoninformation m_PokemonInfo;
    Animator m_Animator;

    // Initialize with the ScriptableObject definition (creates Mode1 under this transform)
    public void Initialize(Pokemon p_Definition)
    {
        if (p_Definition == null)
        {
            Debug.LogError("PokemonComponent.Initialize: passed null definition.");
            return;
        }

        m_PokemonInfo = new Pokemoninformation(p_Definition);

        GameObject mode = m_PokemonInfo.SpawnMode1(transform);
        if (mode != null)
        {
            m_Animator = mode.GetComponent<Animator>();
            if (m_Animator == null)
                Debug.LogWarning($"PokemonComponent.Initialize: Mode1 for {m_PokemonInfo.Name} has no Animator component.");

            // Log renderer counts
            var meshRenderers = mode.GetComponentsInChildren<MeshRenderer>(true);
            var skinned = mode.GetComponentsInChildren<SkinnedMeshRenderer>(true);
            var sprites = mode.GetComponentsInChildren<SpriteRenderer>(true);
            Debug.Log($"PokemonComponent.Initialize: {m_PokemonInfo.Name} mode childCount={mode.transform.childCount} mesh={meshRenderers.Length} skinned={skinned.Length} sprites={sprites.Length}");
        }
        else
        {
            Debug.LogWarning($"PokemonComponent.Initialize: Mode1 spawn failed for {m_PokemonInfo.Name}. Spawning debug cube to show location.");
            // Fallback debug cube so you can see spawn position
            GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.SetParent(transform, false);
            cube.transform.localPosition = Vector3.zero;
            cube.transform.localScale = Vector3.one * 1.0f;
            cube.name = "DebugCube_ModeMissing";
            var mr = cube.GetComponent<MeshRenderer>();
            mr.material = new Material(Shader.Find("Standard"));
            mr.material.color = Color.magenta;
        }

        gameObject.name = m_PokemonInfo.Name;
        Debug.Log($"PokemonComponent.Initialize: Spawned {gameObject.name} at {transform.position} (mode present: {mode != null}).");
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