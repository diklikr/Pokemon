using JetBrains.Annotations;
using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class Pokemoninformation
{
    public Pokemoninformation(Pokemon definition)
    {
       m_Definition = definition;
       m_Name = definition.PokemonName;
        m_currentHealth = maxHealth;
        m_Moves = new List<PokemonMove>(4);
        foreach (PokemonMove move in definition.Moves)
        {
            if (move == null) { continue; }
                m_Moves.Add(move);
        }
    }
    public string Name => m_Name;  
    public string description => m_Definition.Description;
    public int maxHealth => m_Definition.Health + m_Level;
    public int Attack => m_Definition.Attack + m_Level;
    public int SpecialAttack => m_Definition.SpecialAttack + m_Level;
    public int Health => m_Definition.Health + m_Level;
    public GameObject Mode1 => m_Definition.Mode1;
    public Sprite Sprite => m_Definition.Sprite;
    public int Defense => m_Definition.Defense + m_Level;
    public int SpecialDefense => m_Definition.SpecialDefense + m_Level;
    public int Speed => m_Definition.Speed + m_Level;
    
    public PokemonTypes.TypeList MainType => m_Definition.MainType;
    public PokemonTypes.TypeList SecondaryType => m_Definition.SecondaryType;

    public PokemonMove[] Moves => m_Moves.ToArray();


    private Pokemon m_Definition;
    private string m_Name;
    private int m_Xp;
    private int m_Level = 1; // Default level, can be set or modified later
    private int m_currentHealth;

    private List<PokemonMove> m_Moves;

    public void GetDamaged(int damage)
    {
        m_currentHealth -= damage;
        if (m_currentHealth < 0)
        {
            m_currentHealth = 0; // Prevent health from going below zero
        }
    }

    public void Heal(int amount)
    {
        m_currentHealth += amount;
        if (m_currentHealth > maxHealth)
        {
            m_currentHealth = maxHealth; // Prevent health from exceeding max health
        }
    }

    public void GainXp(int amouunt)
    {
        m_Xp += amouunt;
        if(m_Xp >= GetXpForNextLevel())
        {
            m_Xp -= GetXpForNextLevel();
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
        m_currentHealth = maxHealth;
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
        if (m_Moves.Count >= 4)
        {
            Debug.LogWarning("A Pokémon can only have 4 moves. Cannot learn new move: " + newMove.MoveName);
            return;
        }
        m_Moves.Add(newMove);
    }
    public void ForgetMove(PokemonMove moveToForget)
    {
        if (m_Moves.Contains(moveToForget))
        {
            m_Moves.Remove(moveToForget);
        }
        else
        {
            Debug.LogWarning("Move not found in Pokémon's move list: " + moveToForget.MoveName);
        }
    }
    public GameObject SpawnMode1(Transform p_Parent)
    {
        if (m_Definition.Mode1 == null)
        {
            Debug.LogWarning("Mode1 prefab is not assigned for " + m_Name);
            return null;
        }
        
            GameObject t_Mode1 = UnityEngine.Object.Instantiate(m_Definition.Mode1, p_Parent);
        t_Mode1.transform.localPosition = Vector3.zero; // Reset position to parent
        t_Mode1.transform.localRotation = Quaternion.identity; // Reset rotation to parent
        t_Mode1.transform.localScale = Vector3.one; // Reset scale to parent
        t_Mode1.name = "Mode1";
        return t_Mode1;

    }
}
