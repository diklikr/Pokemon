using UnityEngine;
using System.Collections.Generic;
using System;

public class Pokemoninformation
{
    public Pokemoninformation(Pokemon definition)
    {
        m_Definition = definition;
        m_Name = definition.PokemonName;
        m_Level = 1;
        m_Moves = new List<PokemonMove>(2);
        if (definition.Moves != null)
        {
            for (int i = 0; i < Mathf.Min(2, definition.Moves.Length); i++)
            {
                var move = definition.Moves[i];
                if (move != null)
                    m_Moves.Add(move);
            }
        }
        m_currentHealth = MaxHealth;
    }

    // Event signature: currentHP, maxHP
    public event Action<int, int> OnHPChanged;

    public string Name => m_Name;
    public string Description => m_Definition.Description;

    public int MaxHealth => m_Definition.Health + m_Level;
    public int CurrentHP => m_currentHealth;
    public int Attack => m_Definition.Attack + m_Level;
    public int SpecialAttack => m_Definition.SpecialAttack + m_Level;
    public int Defense => m_Definition.Defense + m_Level;
    public int SpecialDefense => m_Definition.SpecialDefense + m_Level;
    public int Speed => m_Definition.Speed + m_Level;

    public GameObject Mode1 => m_Definition.Mode1;
    public Sprite Sprite => m_Definition.Sprite;

    public bool IsFainted => m_currentHealth <= 0;

    public PokemonTypes.TypeList MainType => m_Definition.MainType;
    public PokemonTypes.TypeList SecondaryType => m_Definition.SecondaryType;

    public PokemonMove[] Moves => m_Moves.ToArray();

    private Pokemon m_Definition;
    private string m_Name;
    private int m_Xp;
    private int m_Level = 1;
    private int m_currentHealth;

    private List<PokemonMove> m_Moves;

    public void GetDamaged(int damage)
    {
        m_currentHealth -= damage;
        if (m_currentHealth < 0)
            m_currentHealth = 0;

        // Notify listeners
        OnHPChanged?.Invoke(m_currentHealth, MaxHealth);
    }

    public void Heal(int amount)
    {
        m_currentHealth += amount;
        if (m_currentHealth > MaxHealth)
            m_currentHealth = MaxHealth;

        // Notify listeners
        OnHPChanged?.Invoke(m_currentHealth, MaxHealth);
    }

    public void GainXp(int amount)
    {
        m_Xp += amount;
        int xpForNext = GetXpForNextLevel();
        if (m_Xp >= xpForNext)
        {
            m_Xp -= xpForNext;
            LevelUp();
        }
    }

    private int GetXpForNextLevel()
    {
        return (int)(Mathf.Pow(m_Level + 1, 1.25f) * 100);
    }

    private void LevelUp()
    {
        m_Level++;
        m_currentHealth = MaxHealth;
        // Notify listeners of full heal on level up
        OnHPChanged?.Invoke(m_currentHealth, MaxHealth);
    }

    public void Rename(string newName)
    {
        if (string.IsNullOrEmpty(newName))
        {
            Debug.LogWarning("New name cannot be null or empty.");
            return;
        }
        m_Name = newName;
    }

    public void LearnMove(PokemonMove newMove)
    {
        if (m_Moves.Count >= 2)
        {
            Debug.LogWarning("A Pokémon can only have 2 moves. Cannot learn: " + (newMove != null ? newMove.MoveName : "null"));
            return;
        }
        m_Moves.Add(newMove);
    }

    public void ForgetMove(PokemonMove moveToForget)
    {
        if (m_Moves.Contains(moveToForget))
            m_Moves.Remove(moveToForget);
        else
            Debug.LogWarning("Move not found in Pokémon's move list: " + (moveToForget != null ? moveToForget.MoveName : "null"));
    }

    public GameObject SpawnMode1(Transform p_Parent)
    {
        if (m_Definition.Mode1 == null)
        {
            Debug.LogWarning($"SpawnMode1: Mode1 prefab is not assigned for {m_Name}");
            return null;
        }

        GameObject t_Mode1 = UnityEngine.Object.Instantiate(m_Definition.Mode1, p_Parent);
        if (t_Mode1 == null)
        {
            Debug.LogWarning($"SpawnMode1: Instantiate returned null for {m_Name}'s Mode1.");
            return null;
        }

        // Reset transform relative to parent.
        t_Mode1.transform.localPosition = Vector3.zero;
        t_Mode1.transform.localRotation = Quaternion.identity;
        t_Mode1.transform.localScale = Vector3.one;
        t_Mode1.name = "Mode1";

        // Ensure all children active, scales normalized and set layer to Default
        void NormalizeTransforms(Transform root)
        {
            foreach (Transform child in root)
            {
                child.gameObject.SetActive(true);
                child.localScale = Vector3.one;
                child.gameObject.layer = LayerMask.NameToLayer("Default");
                NormalizeTransforms(child);
            }
        }
        t_Mode1.SetActive(true);
        t_Mode1.layer = LayerMask.NameToLayer("Default");
        NormalizeTransforms(t_Mode1.transform);

        // Enable renderer components
        var meshRenderers = t_Mode1.GetComponentsInChildren<MeshRenderer>(true);
        var skinned = t_Mode1.GetComponentsInChildren<SkinnedMeshRenderer>(true);
        var sprites = t_Mode1.GetComponentsInChildren<SpriteRenderer>(true);

        int totalRenderers = 0;
        foreach (var r in meshRenderers) { r.enabled = true; r.gameObject.layer = LayerMask.NameToLayer("Default"); totalRenderers++; }
        foreach (var r in skinned) { r.enabled = true; r.gameObject.layer = LayerMask.NameToLayer("Default"); totalRenderers++; }
        foreach (var r in sprites) { r.enabled = true; r.gameObject.layer = LayerMask.NameToLayer("Default"); totalRenderers++; }

        if (totalRenderers == 0)
        {
            Debug.LogWarning($"SpawnMode1: Instantiated Mode1 for {m_Name} has no Mesh/SkinnedMesh/SpriteRenderer (childCount={t_Mode1.transform.childCount}).");
        }
        else
        {
            Debug.Log($"SpawnMode1: Spawned Mode1 for {m_Name}. Renderers found: {totalRenderers}.");
        }

        Debug.Log($"SpawnMode1: Spawned Mode1 for {m_Name} under {p_Parent.name}. localPos={t_Mode1.transform.localPosition} localScale={t_Mode1.transform.localScale}");

        return t_Mode1;
    }
}