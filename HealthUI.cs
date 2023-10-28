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
        int heartFraction = currentHealth % 4;
        int numOfContainers = heartContainers.Count;
        int currentContainers = numOfContainers-1;
        Sprite[] heartSprites = { emptyHeartSprite, quarterHeartSprite, halfHeartSprite, threeQuarterHeartSprite, fullHeartSprite };
        int spriteIndex = 0;
        Debug.Log(heartFraction + " " + currentContainers + " " + spriteIndex);
        // Determine the correct sprite index based on heartFraction
        if (numOfContainers > currentContainers+1 && currentContainers > 0)
        {
            switch (heartFraction)
            {
                case 0:
                    spriteIndex = 0; // Empty Heart
                    currentContainers--;
                    break;
                case 1:
                    spriteIndex = 1; // Quarter Heart
                    break;
                case 2:
                    spriteIndex = 2; // Half Heart
                    break;
                case 3:
                    spriteIndex = 3; // Three-Quarter Heart
                    break;
                default:
                    spriteIndex = 4; // Full Heart
                    break;
            }
        }

        else
        {
            spriteIndex = 4;
        }

        if(!(numOfContainers==currentContainers+1))
        {
            //IDK ANYMORE
        }
        // Set the sprite based on emptyContainers and spriteIndex
        heartContainers[currentContainers].sprite = heartSprites[spriteIndex];
    }
}
