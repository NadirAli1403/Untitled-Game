using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    public List<Image> heartContainers;
    public Sprite fullHeartSprite;
    public Sprite threeQuarterHeartSprite;
    public Sprite halfHeartSprite;
    public Sprite quarterHeartSprite;
    public Sprite emptyHeartSprite;



    public void UpdateHealthUI(int currentHealth, int maxHealth)
    {
        Debug.Log(heartContainers.Count);

        for (int i = currentHealth; i < maxHealth; i++)
        {
            heartContainers[i].sprite = emptyHeartSprite;
        }

        for (int i=0; i<currentHealth; i++)
        {
            heartContainers[i].sprite = fullHeartSprite;
        }

    }
}
