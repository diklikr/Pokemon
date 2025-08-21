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
               GameManager.StartCombat();
            print("Grass Encounter Triggered!");
            }
            else
            {
                print("No encounter this time.");
            }
        }
    }
}
