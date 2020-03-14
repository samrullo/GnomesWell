using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class BodyPart : MonoBehaviour
{
    //sprite to render when the gnome takes a damage of type burning
    public Sprite burningSprite;

    // .... damage of type detached
    public Sprite detachedSprite;

    // is detached?
    bool detached = false;

    // point from which blood fountain emits when the body part is sliced
    public Transform bloodFountainOrigin;

    // decouple this object from its parent and flag it as needing physics removal
    public void Detach()
    {
        detached = true;
        this.tag = "Untagged";
        transform.SetParent(null, true);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    // if the body part is detached we will check if it stopped moving
    // and remove everythin from it except its Sprite
    void Update()
    {
        if (detached == false)
        {
            return;
        }

        Rigidbody2D rigidBody = GetComponent<Rigidbody2D>();

        if (rigidBody.IsSleeping())
        {
            foreach(Joint2D joint in GetComponentsInChildren<Joint2D>())
            {
                Destroy(joint);
            }

            foreach (Rigidbody2D body in GetComponentsInChildren<Rigidbody2D>())
            {
                Destroy(body);
            }

            foreach (Collider2D collider in GetComponentsInChildren<Collider2D>())
            {
                Destroy(collider);
            }

            Destroy(this);
        }
        
    }

    // swaps out the sprite for this part based on what kind of damage was received
    public void ApplyDamageSprite(Gnome.DamageType damageType)
    {
        Sprite spriteToUse = null;
        switch (damageType)
        {
            case Gnome.DamageType.Burning:
                spriteToUse = burningSprite;
                break;
            case Gnome.DamageType.Slicing:
                spriteToUse = detachedSprite;
                break;

        }
        if (spriteToUse != null)
        {
            GetComponent<SpriteRenderer>().sprite = spriteToUse;
        }
    }
}
