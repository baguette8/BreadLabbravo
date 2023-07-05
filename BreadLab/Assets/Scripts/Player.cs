using UnityEngine;
using UnityEngine.UI;

public enum PlayerState
{
    Alive,
    Dead
}

public class Player : MonoBehaviour
{
    public float speed = 1.0f;
    public float health = 100.0f;
    public float energy = 100.0f;
    public PlayerState state = PlayerState.Alive;

    [Tooltip("Drag the Health Slider here")]
    public Slider healthBar;
    [Tooltip("Drag the Energy Slider here")]
    public Slider energyBar;

    void Start()
    {
        // Find the Sliders in the scene if they are not assigned
        if (healthBar == null)
        {
            healthBar = GameObject.Find("HealthBar").GetComponent<Slider>();
        }
        if (energyBar == null)
        {
            energyBar = GameObject.Find("EnergyBar").GetComponent<Slider>();
        }

        UpdateHealthEnergyBars();
    }

    void Update()
    {
        // You might want to limit energy not to go beyond 100
        energy = Mathf.Clamp(energy, 0, 100);

        if (health <= 0)
        {
            state = PlayerState.Dead;
            // Notify GameManager about game over
            GameManager.Instance.GameOver();
        }
        
        // Update health and energy bars
        UpdateHealthEnergyBars();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Food"))
        {
            energy += 50.0f;
            health += 10.0f; // Gaining health from eating food
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Predator"))
        {
            health -= 50.0f; // Losing health when encountering a predator
            Destroy(collision.gameObject);
        }
        else if (collision.gameObject.CompareTag("Critter"))
        {
            energy += 20.0f;
            health += 20.0f; // Gaining more health from eating a critter
            Destroy(collision.gameObject);
        }

        // Update health and energy bars
        UpdateHealthEnergyBars();
    }

    void UpdateHealthEnergyBars()
    {
        healthBar.value = health / 100.0f;
        energyBar.value = energy / 100.0f;
    }
}
