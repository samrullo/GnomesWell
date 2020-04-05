using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSwapper : MonoBehaviour
{
    public Sprite spriteToUse;
    private Sprite originalSprite;

    public SpriteRenderer spriteRenderer;

    // Start is called before the first frame update
    public void SwapSprite()
    {
        //swap sprites if spriteToUse and originalSprite are not the same
        if (originalSprite != spriteToUse)
        {
            originalSprite = spriteRenderer.sprite;
            spriteRenderer.sprite = spriteToUse;
        }
        
    }

    // resets the sprite to the original sprite
    public void ResetSprite() {
        if (originalSprite != null)
        {
            spriteRenderer.sprite = originalSprite;
        }
    }
}
