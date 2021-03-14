using UnityEngine;
using UnityEngine.UI;

public class HealthSlider : MonoBehaviour
{
    [SerializeField] Slider slider;
    [SerializeField] Image healthIndicator;

    [SerializeField] Color32 fullHealthColor;
    [SerializeField] Color32 lowHealthColor;

    private Gradient _gradient;

    public float Health { get; private set; }
    public float MaxHealth { get; private set; }

    public void Initialize(float health, bool showFullHealth)
    {
        _gradient = new Gradient()
        {
            colorKeys = new GradientColorKey[]
            {
                new GradientColorKey(lowHealthColor, 0f),
                new GradientColorKey(fullHealthColor, 1f)
            }
        };

        Health = health;
        MaxHealth = health;

        healthIndicator.color = _gradient.Evaluate(1);

        slider.gameObject.SetActive(showFullHealth);
    }

    public void SubtractHealth(float amount)
    {
        Health -= amount;

        if (Health < 0)
            Health = 0;

        VisualiseDamage();
    }

    private void VisualiseDamage()
    {
        float value = Health / MaxHealth;

        if (slider.gameObject.activeSelf == false)
            slider.gameObject.SetActive(true);

        slider.value = value;
        healthIndicator.color = _gradient.Evaluate(value);
    }
}
