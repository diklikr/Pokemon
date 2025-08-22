using UnityEngine;

public class Pasto : MonoBehaviour
{
    public float chance = 1f;
    // 20% chance to spawn grass
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (Random.value <= chance)
            {
                // Get the GameManager instance and a random Pokemon for the encounter
                GameManager gm = GameManager.GetInstance();
                Pokemon playerPoke = gm.poke3; 
                Pokemon wildPoke = gm.GetRandomPokemon();
                GameManager.StartCombat(playerPoke, wildPoke);
                print("Grass Encounter Triggered!");
            }
            else
            {
                print("No encounter this time.");
            }
        }
    }
}
