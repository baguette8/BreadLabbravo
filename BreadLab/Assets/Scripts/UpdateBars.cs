using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Player))]
public class UpdateBars : MonoBehaviour
{
    private Player player;

    private Slider healthSlider;
    private Slider energySlider;

    private void Start()
    {
        player = GetComponent<Player>();

        // Create Health Slider
        GameObject healthSliderObject = new GameObject("HealthSlider");
        healthSlider = healthSliderObject.AddComponent<Slider>();
        healthSliderObject.transform.SetParent(this.transform);
        healthSliderObject.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
        healthSliderObject.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
        healthSliderObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 50);
        
        // Set the color of the Health Slider to green
        healthSliderObject.GetComponent<Slider>().targetGraphic.color = Color.green;
        healthSlider.fillRect.GetComponent<Image>().color = Color.green;

        // Create Energy Slider
        GameObject energySliderObject = new GameObject("EnergySlider");
        energySlider = energySliderObject.AddComponent<Slider>();
        energySliderObject.transform.SetParent(this.transform);
        energySliderObject.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.5f);
        energySliderObject.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.5f);
        energySliderObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0);
        
        // Set the color of the Energy Slider to blue
        energySliderObject.GetComponent<Slider>().targetGraphic.color = Color.blue;
        energySlider.fillRect.GetComponent<Image>().color = Color.blue;
    }

    private void Update()
    {
        if (player != null)
        {
            healthSlider.value = player.health;
            energySlider.value = player.energy;
        }
    }
}