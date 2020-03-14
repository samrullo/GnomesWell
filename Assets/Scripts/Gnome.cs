using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gnome : MonoBehaviour
{
    public Transform cameraFollowTarget;

    public Rigidbody2D ropeBody;

    public Sprite armHoldingEmpty;
    public Sprite armHoldingTreasure;

    public SpriteRenderer holdingArm;

    public GameObject deathPrefab;
    public GameObject flameDeathPrefab;
    public GameObject ghostPrefab;

    public float delayBeforeRemoving = 3.0f;
    public float delayBeforeReleasingGhost = 0.25f;

    public GameObject bloodFountainPrefab;

    bool dead = false;

    bool _holdingTreasure = false;

    public bool holdingTreasure
    {
        get
        {
            return _holdingTreasure;
        }

        set
        {
            if (dead == true)
            {
                return;
            }
            _holdingTreasure = value;
            if (holdingArm != null)
            {
                if (_holdingTreasure)
                {
                    holdingArm.sprite = armHoldingTreasure;
                }
                else
                {
                    holdingArm.sprite = armHoldingEmpty;
                }
            }
        }
    }

    public enum DamageType
    {
        Burning,
        Slicing
    }

    public void ShowDamageEffect(DamageType damageType)
    {
        switch (damageType)
        {
            case DamageType.Burning:
                if (flameDeathPrefab != null)
                {
                    Instantiate(flameDeathPrefab, cameraFollowTarget.position, cameraFollowTarget.rotation);
                }
                break;
            case DamageType.Slicing:
                if (deathPrefab != null)
                {
                    Instantiate(deathPrefab,cameraFollowTarget.position,cameraFollowTarget.rotation);
                }
                break;
        }
    }


    public void DestroyGnome(DamageType damageType)
    {
        holdingTreasure = false;
        dead = true;

        //find all child objects and randomly disconnect their joints
        foreach(BodyPart bodyPart in GetComponentsInChildren<BodyPart>())
        {
            switch (damageType)
            {
                case DamageType.Burning:
                    // 1 in 3 chance of burning
                    bool shouldBurn = Random.Range(0, 2) == 0;
                    if (shouldBurn)
                    {
                        bodyPart.ApplyDamageSprite(damageType);
                    }
                    break;
                case DamageType.Slicing:
                    // always slice
                    bodyPart.ApplyDamageSprite(damageType);
                    break;
            }

            // 1 in 3 chance of seperating from body
            bool shouldDetach = Random.Range(0, 2) == 0;

            if (shouldDetach)
            {
                // remove this bodyPart's rigidbody and collider after it comes to rest
                bodyPart.Detach();

                // add bloodFountain if seperating and damageType is Slicing
                if (damageType == DamageType.Slicing)
                {
                    if(bodyPart.bloodFountainOrigin!=null && bloodFountainPrefab != null)
                    {
                        GameObject fountain = Instantiate(bloodFountainPrefab, bodyPart.bloodFountainOrigin.position, bodyPart.bloodFountainOrigin.rotation) as GameObject;
                        fountain.transform.SetParent(this.cameraFollowTarget,false);
                    }
                }

            }

            // destroy all joints of this bodyPart
            var allJoints = bodyPart.GetComponentsInChildren<Joint2D>();

            foreach(Joint2D joint in allJoints)
            {
                Destroy(joint);
            }
        }

        // add RemoveAfterDelay component
        var remove = gameObject.AddComponent<RemoveAfterDelay>();
        remove.delay = delayBeforeRemoving;

        StartCoroutine(ReleaseGhost());
        
    }

    IEnumerator ReleaseGhost()
    {
        if (ghostPrefab == null)
        {
            yield break;
        }

        // wait for delayBeforeRelease seconds
        yield return new WaitForSeconds(delayBeforeReleasingGhost);

        // add the ghost
        Instantiate(ghostPrefab, transform.position, Quaternion.identity);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
