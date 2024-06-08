using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private GameObject player;

    [SerializeField]
    private InGameInfoSO inGameInfo;

    [SerializeField]
    private float playerForwardSpeed;
    [SerializeField]
    private float playerMaxSpeed;
    [SerializeField]
    private float playerIncreaseSpeed;
    private Vector3 direction;
    private CharacterController characterController;

    [SerializeField]
    private float distanceBetweenTracks;
    [SerializeField]
    private float changeTrackSpeed;
    private int desiredTrack = 1; // 0 -> left, 1 -> middle, 2 -> right
    private Vector3 newPosition = Vector3.zero;

    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float gravity = -20;
    private bool isAbleToJump = false;
    private bool isAbleToSlide = false;
    private bool justJumped = false;
    private bool isCurrentlySliding = false;

    [SerializeField]
    private SwipeMenager swipeMenager;

    [SerializeField]
    private GameStatesSO gameStates;

    private Animator animator;

    private PlayerMenager playerMenager;

    private bool maxSpeed = false;

    private bool runPlayed = false;
    private bool runMaxSpeedPlayed = false;
    private bool runPausedJump = false;
    private bool runPausedSlide = false;

    private GameObject playerModel;

    private TutorialManager tutorialManager;

    void Start()
    {
        FindPlayerAnimator();
        characterController = GetComponent<CharacterController>();
        playerMenager = FindObjectOfType<PlayerMenager>();
        gameStates.isGameOver = false;
        gameStates.isGameStarded = false;
        animator.SetBool("maxSpeed", false);
        if (gameStates.isThisTutorial)
        {
            tutorialManager = FindObjectOfType<TutorialManager>();
        }
    }

    void Update()
    {
        if (playerModel == null)
        {
            foreach (Transform child in transform)
            {
                if (child.CompareTag("PlayerSkin"))
                {
                    playerModel = child.gameObject;
                }
            }
        }

        if (gameStates.isGameStarded)
        {

            ChangeTrack();

            if (swipeMenager.SwipeUp && characterController.isGrounded)
            {
                isAbleToJump = true;
            }

            if (swipeMenager.SwipeDown && !isCurrentlySliding)
            {
                isAbleToSlide = true;
            }
        }

        PlayRunSound();

    }

    void FixedUpdate()
    {
        if (gameStates.isGameStarded)
        {
            IncreaseSpeed();

            characterController.Move(direction * Time.fixedDeltaTime);

            transform.position = Vector3.Lerp(transform.position, newPosition, changeTrackSpeed * Time.fixedDeltaTime);

            Jump();

            Slide();
        }

    }

    private void PlayRunSound()
    {
        if (!runPlayed && gameStates.isGameStarded && !gameStates.isGameOver)
        {
            AudioManager.Instance.PlayCharacterSFX("Run");
            runPlayed = true;
        }
        else if (runPlayed && !runMaxSpeedPlayed && maxSpeed)
        {
            AudioManager.Instance.PlayCharacterSFX("Run2");
            runMaxSpeedPlayed = true;
        }
        else if (!runPlayed && gameStates.isThisTutorial)
        {
            AudioManager.Instance.PlayCharacterSFX("Run");
            runPlayed = true;
        }

        if (AudioManager.Instance.characterSfxSource.isPlaying && !characterController.isGrounded)
        {
            AudioManager.Instance.characterSfxSource.Pause();
            runPausedJump = true;
        }

        if (AudioManager.Instance.characterSfxSource.isPlaying && isCurrentlySliding)
        {
            AudioManager.Instance.characterSfxSource.Pause();
            runPausedSlide = true;
        }

        if (runPausedJump && characterController.isGrounded)
        {
            AudioManager.Instance.characterSfxSource.UnPause();
            runPausedJump = false;
        }

        if (runPausedSlide && !isCurrentlySliding)
        {
            AudioManager.Instance.characterSfxSource.UnPause();
            runPausedSlide = false;
        }

        if (gameStates.isGameOver)
        {
            AudioManager.Instance.characterSfxSource.Stop();
            runPlayed = false;
        }
    }

    private void FindPlayerAnimator()
    {
        foreach (Transform child in player.transform)
        {
            if (child.CompareTag("PlayerSkin"))
            {
                animator = child.gameObject.GetComponent<Animator>();
            }
        }
    }

    private void ChangeTrack()
    {
        if (swipeMenager.SwipeRight && desiredTrack != 2)
        {
            desiredTrack++;

            if (gameStates.isThisTutorial && tutorialManager.CurrentMap == 0)
            {
                inGameInfo.tutorialCounter++;
            }
        }
        else if (swipeMenager.SwipeLeft && desiredTrack != 0)
        {
            desiredTrack--;

            if (gameStates.isThisTutorial && tutorialManager.CurrentMap == 0)
            {
                inGameInfo.tutorialCounter++;
            }
        }

        newPosition = transform.position.z * transform.forward + transform.position.y * transform.up;

        switch (desiredTrack)
        {
            case 0:
                newPosition += Vector3.left * distanceBetweenTracks;
                break;
            case 2:
                newPosition += Vector3.right * distanceBetweenTracks;
                break;
        }

    }

    private void Jump()
    {
        if (isAbleToJump)
        {
            animator.SetLayerWeight(animator.GetLayerIndex("FullBody"), 1f);
            animator.SetLayerWeight(animator.GetLayerIndex("UpperBody"), 0f);
            animator.SetLayerWeight(animator.GetLayerIndex("LowerBody"), 0f);
            animator.Play("jump");

            AudioManager.Instance.PlaySFX("JumpA");
            direction.y = jumpForce;

            isAbleToJump = false;
            justJumped = true;
        }
        else
        {
            direction.y += gravity * Time.fixedDeltaTime;

            if (justJumped && characterController.isGrounded)
            {
                AudioManager.Instance.PlaySFX("JumpB");
                justJumped = false;
                playerModel.transform.rotation = Quaternion.identity;
            }

        }
    }

    private void Slide()
    {
        if (isAbleToSlide)
        {
            AudioManager.Instance.PlaySFX("Slide");
            StartCoroutine(SlideCoroutine());
            isAbleToSlide = false;
        }
    }

    private IEnumerator SlideCoroutine()
    {
        isCurrentlySliding = true;
        var oldCenter = characterController.center;
        var oldHeight = characterController.height;
        characterController.center = new Vector3(0, 0.5f, 0);
        characterController.height = 1;
        animator.SetLayerWeight(animator.GetLayerIndex("FullBody"), 1f);
        animator.SetLayerWeight(animator.GetLayerIndex("UpperBody"), 0f);
        animator.SetLayerWeight(animator.GetLayerIndex("LowerBody"), 0f);
        animator.Play("slide");
        yield return new WaitForSeconds(1);
        playerModel.transform.rotation = Quaternion.identity;
        characterController.center = oldCenter;
        characterController.height = oldHeight;
        isCurrentlySliding = false;
    }

    private IEnumerator HitShieldCoroutine(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        playerMenager.ShieldActive = false;
        inGameInfo.tutorialCounter = 1;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!hit.transform.CompareTag("Ground") && playerMenager.GhostMode)
        {
            hit.collider.enabled = false;
            return;
        }

        if (hit.transform.CompareTag("Obstacle") && playerMenager.StimpackTime > 0)
        {
            if (playerMenager.mainPlayerData.achievementValue[8] != 1 && hit.transform.name.Contains("car"))
            {
                playerMenager.mainPlayerData.achievementValue[8] = 1;
            }
            Destroy(hit.gameObject);
        }
        else if (hit.transform.CompareTag("Obstacle") || hit.transform.CompareTag("Enemy"))
        {
            if (playerMenager.ShieldActive)
            {
                Destroy(hit.gameObject);
                StartCoroutine(HitShieldCoroutine(3f));
            }
            else
            {
                AudioManager.Instance.PlaySFX("PlayerDeath");
                gameStates.isGameOver = true;
            }
        }
    }

    private void IncreaseSpeed()
    {
        if (playerForwardSpeed < playerMaxSpeed && !gameStates.isThisTutorial)
        {
            playerForwardSpeed += playerIncreaseSpeed * Time.fixedDeltaTime;

        }
        direction.z = playerForwardSpeed;

        if (playerForwardSpeed >= playerMaxSpeed && !maxSpeed)
        {
            maxSpeed = true;
            animator.SetBool("maxSpeed", true);
            playerMenager.mainPlayerData.achievementValue[4]++;
            Debug.Log("Max Speed!");
        }
    }

    public void SetPlayerPositionAfterFirstAid()
    {
        desiredTrack = 1;
    }

    public float GetPlayerPositionZ()
    {
        return player.transform.position.z;
    }

    public void TutorialSetPlayerOnStartStage()
    {
        SetPlayerPositionAfterFirstAid();
        animator.applyRootMotion = false;
        playerModel.transform.localRotation = Quaternion.identity;
        playerModel.transform.localPosition = Vector3.zero;

        animator.Play("run");
    }

}
