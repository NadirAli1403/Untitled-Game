using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public Image[] heartContainers;
    public Sprite fullHeartSprite;
    public Sprite threeQuarterHeartSprite;
    public Sprite halfHeartSprite;
    public Sprite quarterHeartSprite;
    public Sprite emptyHeartSprite;

    private Player player; // Reference to the Player script

    void Start()
    {
        // Get the Player component from the same GameObject
        if (!TryGetComponent<Player>(out player))
        {
            Debug.LogError("Player component not found!");
            return;
        }
        else
        {
            Debug.Log("Player component found on " + gameObject.name);
        }

        // Ensure heartContainers array length matches player's max health
        if (heartContainers.Length != player.maxHealth)
        {
            Debug.LogError("Number of heart containers must match player's max health!");
            return;
        }
    }

    private void Update()
    {
        // Update the health UI based on the player's current health
        UpdateHealthUI(player.hitPoints, player.maxHealth);
    }

    private void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        float fractionPerHeart = 1f / heartContainers.Length;

        for (int i = 0; i < heartContainers.Length; i++)
        {
            float heartFraction = (i + 1) * fractionPerHeart;

            if (heartFraction <= currentHealth / (float)maxHealth)
            {
                heartContainers[i].sprite = fullHeartSprite;
            }
            else if (heartFraction <= (currentHealth - 0.25f) / (float)maxHealth)
            {
                heartContainers[i].sprite = threeQuarterHeartSprite;
            }
            else if (heartFraction <= (currentHealth - 0.5f) / (float)maxHealth)
            {
                heartContainers[i].sprite = halfHeartSprite;
            }
            else if (heartFraction <= (currentHealth - 0.75f) / (float)maxHealth)
            {
                heartContainers[i].sprite = quarterHeartSprite;
            }
            else
            {
                heartContainers[i].sprite = emptyHeartSprite;
            }
        }
    }
}
