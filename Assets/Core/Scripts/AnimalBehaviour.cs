using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class AnimalBehaviour : MonoBehaviour
{
    #region Field

    //Scene names
    private const string quizScene = "QuizScene";
    private const string gameOverScene = "GameOverScene";
    private const string hudScene = "HUD";
    /*[SerializeField, Tooltip("Set same as total closing time for Quiz"), Range(1, 15)] */private float closeTime = 0.0001f;
    private bool animalSceneLoaded = false;

    //The jumping heigth - is public so it can bechanged in Unity
    [SerializeField, Tooltip("Jump heigth or fly heigth for birds")]
    public float heigth = 5f;

    //Moving speed
    [SerializeField, Tooltip("Moving speed")]
    public float speed = 5f;

    //Player loop around the background
    [SerializeField, Tooltip("The player will be teleportet back to the start position when it gets to the end")]
    public bool loopBackground = false;

    //End of background if the Player needs to respawn at the start position again
    [SerializeField, Tooltip("The end coordinat of the X axis")]
    public float backgroundEndX = 5f;

    //Input System Asset
    public InputActionAsset inputActions;

    //Animal rigidbody
    protected Rigidbody2D rb;

    //Animal animator
    protected Animator animator;

    //Jump values
    private InputAction jumpInput;

    //Bool to tjek if the Player is jumping
    private bool isJumping = false;

    //The Player can dobbel jump, to make sure it is max 2 times
    protected int dobbelJump = 0;

    //Player needs to be with in the camera wiewpoint
    private Camera mainCamera;
    private float topBound;

    //Player sprite heigth
    private float playerHeight;

    //Player start position
    private Vector2 startPos;

    //Animalsoundeffects
    private AudioSource animalSoundEffect;

    //Soundeffects timer
    private float lastTimeAudio = 0f;
    [SerializeField, Tooltip("The sound will be repaetet")]
    public bool repeatSounds = true;
    [SerializeField, Tooltip("The time the soundeffect is starting")]
    public float soundStart = 5f;
    [SerializeField, Tooltip("The length in secons of the sound effect")]
    public float soundEffectLength = 0f;


    /// ///////////////////////////////HUD/////////////////////////////////

    //The HUDManager object, that shows the HUD in the HUD scene. 
    private HUDManager hud;

    //The time lift before the gme stops
    [SerializeField, Tooltip("The default time remaining when the player starts a level")]
    protected float timeRemaining = 60;

    //A counter to use ofr things happening every second
    protected float secondsCounter = 0;

    //The number of items picked up by the player in the current level
    //[SerializeField, Tooltip("The default starting number of items picked up")]
    //protected int pickUps;

    //Bool to show wether a HUDManager has ben sucesfully added/defined
    private bool hudAdded = false;

    [SerializeField, Tooltip("Map that this animal belongs to")] private MapAssociation associatedMap;

    #endregion


    #region Properties

    #endregion


    #region Method
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Start()
    {

    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (!hudAdded)
        {
            AddHUD();
        }

        if (timeRemaining < 0 && animalSceneLoaded)
        {
            StartCoroutine(LoadScene(gameOverScene, true));
        }

        secondsCounter += Time.deltaTime;
        if (secondsCounter > 1)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= 1;
                if (hud != null)
                {
                    hud.SetTime(timeRemaining);
                }
            }
            else
            {
                StartCoroutine(LoadScene(quizScene));
            }
            secondsCounter = 0;
        }
    }

    protected virtual void Awake()
    {

        //Rigidbody
        rb = GetComponent<Rigidbody2D>();

        //Animator
        animator = GetComponent<Animator>();

        //Start position
        startPos = rb.position;

        //Jumping input under the action "Player/Jump"
        //Tap on touch screen and "Space" on keyboard
        jumpInput = inputActions.FindActionMap("Player").FindAction("Jump");

        //Camera bounds
        CameraBounds();

        //Soundeffect
        animalSoundEffect = GetComponent<AudioSource>();
    }

    protected virtual void OnEnable()
    {

        QuizMemory quizMemory = Resources.Load<QuizMemory>("QuizMemory_SO");

        quizMemory.CurrentMap = associatedMap;
        quizMemory.previousQuestions.Clear();

        animalSceneLoaded = true;

        quizMemory.CorrectAnswer += AddTime;

        inputActions.FindActionMap("Player").Enable();

        //Jump action
        jumpInput.Enable();

        //Staring jump
        jumpInput.performed += ctx => isJumping = true;

    }

    protected virtual void OnDisable()
    {

        QuizMemory memory = Resources.Load<QuizMemory>("QuizMemory_SO");
        memory.CorrectAnswer -= AddTime;

        inputActions.FindActionMap("Player").Disable();

        //Ending jump
        jumpInput.performed -= ctx => isJumping = false;

        jumpInput.Disable();
    }

    private void Jumping()
    {
        //The Rigidbodys velocity
        rb.linearVelocity = new Vector2(rb.linearVelocityX, 0f);

        //Transitions to "Jumping" animation
        animator.SetTrigger("Jump");
        animator.SetBool("canJump", false);

        //Adding force to make the jump
        rb.AddForce(Vector2.up * heigth, ForceMode2D.Impulse);

    }

    protected virtual void FixedUpdate()
    {

        //Tells animator if animal is moving up or down
        animator.SetFloat("velocityY", rb.linearVelocityY);

        //The player can only jump if the isJumping is true and haven't jet dobbeljumped
        if (isJumping == true && dobbelJump < 2)
        {
            Jumping();

            //Resetting isJumping to false
            isJumping = false;

            //Adding a jump to dobbelJump
            dobbelJump++;
        }

        //The players temporary position
        Vector2 pos = rb.position;

        //The player can't move out of the top of the screen
        //The players position-Y is rigth under the top of the screen (minus the player heigth) it will limets the position-Y
        if (rb.position.y > (topBound - playerHeight))
        {
            pos.y = topBound - playerHeight;
            rb.position = new Vector2(pos.x, pos.y);
        }

        //Moving horizontal with fixed speed
        Vector2 movePos = Vector2.right * speed * Time.fixedDeltaTime;

        //To respawn back to start position 
        if (rb.position.x > backgroundEndX && loopBackground == true)
        {
            pos.x = startPos.x;
        }

        //Players new position 
        rb.position = pos + movePos;

        //Sound prut
        lastTimeAudio = lastTimeAudio + Time.fixedDeltaTime;

        if (repeatSounds == true)
        {
            if (lastTimeAudio > soundStart && animalSoundEffect.playOnAwake == true) //First time the sound starts after the input soundStart
            {
                animalSoundEffect.Play();

                lastTimeAudio = 0;
            }
            else if (lastTimeAudio > soundStart + soundEffectLength) //Play nest time after the soundStart and the length of the length of the sound input
            {
                animalSoundEffect.Play();

                lastTimeAudio = 0;
            }
        }
    }


    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        //Resetting to 0 so the player can start dobbel jumping again
        if (collision.gameObject.tag != "Collectible")
        {
            dobbelJump = 0;

            //Enables jumping animation precondition
            animator.SetBool("canJump", true);
            animator.ResetTrigger("Jump");
        }
    }

    /// <summary>
    /// Getting the screenBounds and SpriteRenderer size
    /// </summary>
    private void CameraBounds()
    {
        //Findung the camera scrren bounds
        mainCamera = Camera.main;

        Vector2 screenBounds = mainCamera.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        //The top bound 
        topBound = screenBounds.y;

        //Getting the player heigth with the SpriteRenderer
        SpriteRenderer sr = GetComponent<SpriteRenderer>();

        if (sr != null)
        {
            playerHeight = sr.bounds.extents.y;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //When player collect a collectible it will be no longer be active
        if (collision.gameObject.CompareTag("Collectible"))
        {
            collision.gameObject.SetActive(false);
        }
    }




    /// <summary>
    /// Tries to find a gameobject from the hierarchy with the HUD tag, and set the Animals hud field to the GameObject's HUDManager component. 
    /// </summary>
    private void AddHUD()
    {
        if (!hudAdded)
        {
            GameObject gameObject = GameObject.FindGameObjectWithTag("HUD");
            if (gameObject != null)
            {
                hud = gameObject.GetComponent<HUDManager>();
                if (hud != null)
                {
                    hudAdded = true;
                    hud.SetTime(timeRemaining);
                }
            }
        }
    }

    /// <summary>
    /// Method to load Quiz
    /// </summary>
    /// <param name="sceneName">String of scene to be loadeds name</param>
    /// <param name="unloadScene">Set true if current scene should be unloaded</param>
    /// <returns>Quiz Scene (un)loaded additive</returns>
    public IEnumerator LoadScene(string sceneName, bool unloadScene = false)
    {

        if (unloadScene)
        {
            animalSceneLoaded = false;
            yield return new WaitForSeconds(closeTime);
        }

        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        if (unloadScene)
        {

            hudAdded = false;
            yield return SceneManager.UnloadSceneAsync(hudScene);

            yield return SceneManager.UnloadSceneAsync(gameObject.scene);

        }

    }

    /// <summary>
    /// Method to add more game-time
    /// </summary>
    /// <param name="time">Time added with trigger</param>
    public void AddTime(float time)
    {

        timeRemaining += time;
        if (hud != null && timeRemaining >= 0)
            hud.SetTime(timeRemaining);

    }

    #endregion


}

