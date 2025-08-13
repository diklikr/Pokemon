using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] public Pokemon pokeDefinition;
    void Start()
    {
        GameManager.SpawnPokemon(pokeDefinition, transform.position);
        GameManager.SpawnPokemon(pokeDefinition, transform.position + Vector3.right);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
