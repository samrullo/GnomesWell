using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject startingPoint;

    public Rope rope;

    public CameraFollow cameraFollow;

    // current Gnome as opposed to all dead ones
    Gnome currentGnome;

    //gnome prefab to instantiate a new gnome
    public GameObject gnomePrefab;

    //UI component that contains the buttons
    public RectTransform mainMenu;

    //UI component that contains menu items
    public RectTransform gameplayMenu;

    //UI component that contains you win screen
    public RectTransform gameOverMenu;

    // if true all damage will be ignored(but damage effects will still be shown)
    // make this a property to make it show up in list of methods in the Inspector for UnityEvent
    public bool gnomeInvincible { get; set; }

    // how long to wait after dying before creating a new gnome
    public float delayAfterDeath;

    // the sound to play when the gnome dies
    public AudioClip gnomeDieSound;

    // gnome wins sound
    public AudioClip gameOverSound;



    // Start is called before the first frame update
    void Start()
    {
        // when the game starts call Reset
        Reset();
        Debug.Log("I finished reset...");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Reset the entire game
    public void Reset()
    {
        if (gameOverMenu)
            gameOverMenu.gameObject.SetActive(false);

        if (mainMenu)
            mainMenu.gameObject.SetActive(false);

        if (gameplayMenu)
            gameplayMenu.gameObject.SetActive(true);

        // Find all resettable objects and tell them to reset
        var resettableObjects = FindObjectsOfType<Resettable>();

        foreach(Resettable r in resettableObjects)
        {
            r.Reset();
        }

        // make a new gnome
        CreateNewGnome();

        // unpause the game
        Time.timeScale = 1.0f;
    }

    void CreateNewGnome()
    {
        // remove the current gnome if there is one
        RemoveGnome();

        // create a new gnome gameObject and set it to be currentGnome
        GameObject newGnome = (GameObject)Instantiate(gnomePrefab, startingPoint.transform.position, Quaternion.identity);

        currentGnome = newGnome.GetComponent<Gnome>();        

        // make the rope visible
        rope.gameObject.SetActive(true);

        // connect the rope's trailing end to the gnome's foot
        rope.gnomeRightLeg = currentGnome.ropeBody;

        // reset rope's length to default
        rope.ResetRope();

        // tell the cameraFollow to start tracking this gnome
        cameraFollow.target = currentGnome.cameraFollowTarget;
    }


    void RemoveGnome()
    {
        // don't actually do anything if the gnome is invincible
        if (gnomeInvincible)
            return;

        // hide the rope
        rope.gameObject.SetActive(false);

        // stop tracking the gnome
        cameraFollow.target = null;

        // if we have a current gnome make that no longer to be player
        if (currentGnome != null)
        {
            // this gnome is no longer holding treasure
            currentGnome.holdingTreasure = false;

            // make this object no longer the player so that colliders don't detect it anymore
            currentGnome.gameObject.tag = "Untagged";

            // find every object that is currently tagged as Player and remove that tag
            foreach(Transform child in currentGnome.transform)
            {
                child.gameObject.tag = "Untagged";
            }

            currentGnome = null;
        }
    }

    void KillGnome(Gnome.DamageType damageType)
    {
        // if we have AudioSource play gnomeDiedSound
        var audio = GetComponent<AudioSource>();
        if (audio != null)
        {
            audio.PlayOneShot(gnomeDieSound);
        }

        // show damage effects
        currentGnome.ShowDamageEffect(damageType);

        // if we're not invincible then reset the gnome and make this gnome no more current
        if (gnomeInvincible == false)
        {
            // tell the Gnome that it died
            currentGnome.DestroyGnome(damageType);
            RemoveGnome();
            StartCoroutine(ResetAfterDelay());
        }
    }

    IEnumerator ResetAfterDelay()
    {
        // wait for delayAfterDeath seconds and then call Reset
        yield return new WaitForSeconds(delayAfterDeath);
        Reset();
    }

    public void TrapTouched()
    {
        KillGnome(Gnome.DamageType.Slicing);
    }

    public void FireTrapTouched()
    {
        KillGnome(Gnome.DamageType.Burning);
    }

    public void TreasureCollected()
    {
        currentGnome.holdingTreasure = true;
    }

    public void ExitReached()
    {
        // if we have a player and that player is holding treasure then game is over
        if(currentGnome!=null && currentGnome.holdingTreasure)
        {
            // if we have AudioSource play gameOverSound
            var audio = GetComponent<AudioSource>();
            if (audio != null)
            {
                audio.PlayOneShot(gameOverSound);
            }

            // pause the game
            Time.timeScale = 0.0f;

            if (gameOverMenu)
                gameOverMenu.gameObject.SetActive(true);
            
            if (gameplayMenu)
                gameplayMenu.gameObject.SetActive(false);

        }
    }

    // called when menu button or Resume Game button is tapped
    public void SetPaused(bool paused)
    {
        // if we're paused stop time and enable the menu and disable the game overlay
        if (paused)
        {
            Time.timeScale = 0.0f;
            mainMenu.gameObject.SetActive(true);
            gameplayMenu.gameObject.SetActive(false);
        }
        else
        {
            Time.timeScale = 1.0f;
            mainMenu.gameObject.SetActive(false);
            gameplayMenu.gameObject.SetActive(true);
        }
    }

    // called when restart button is pressed
    public void RestartGame()
    {
        // Immediately remove the gnome instead of killing
        Destroy(currentGnome.gameObject);
        currentGnome = null;
        Reset();
    }

}
