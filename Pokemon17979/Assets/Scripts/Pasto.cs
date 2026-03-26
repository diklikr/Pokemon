using UnityEngine;

public class Pasto : MonoBehaviour
{
    public float chance = 1f;
    // 20% chance to spawn grass
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Player>(out Player player))
        {
            if (Random.value <= chance)
            {
             GameManager.StartCombat(player.poke);
                print("Grass Encounter Triggered!");
            }
            else
            {
                print("No encounter this time.");
            }
        }
    }
}
