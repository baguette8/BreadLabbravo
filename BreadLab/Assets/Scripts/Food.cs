using UnityEngine;

public class Food : MonoBehaviour
{
    // Amount of energy that the food gives when eaten
    public float energyValue = 50f;

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the colliding object is a critter or player
        if (collision.gameObject.CompareTag("Critter") || collision.gameObject.CompareTag("Player"))
        {
            // Attempt to get the critter or player script from the colliding object
            Critter critter = collision.gameObject.GetComponent<Critter>();
            Player player = collision.gameObject.GetComponent<Player>();

            // Increase energy of the critter or player, if applicable
            if (critter != null)
            {
                critter.energy += energyValue;
            }
            else if (player != null)
            {
                player.energy += energyValue;
            }

            // Destroy the food object
            Destroy(gameObject);
        }
    }
}
