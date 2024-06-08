using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    private float enemyForwardSpeed;
    private Vector3 direction;
    private CharacterController characterController;
    [SerializeField]
    private GameStatesSO gameStates;
    private bool currentlyAttacks = false;

    [SerializeField]
    private InGameInfoSO inGameInfo;

    private Animator animator;

    private bool isDead;
    private bool isWalkOn;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        isDead = false;
        isWalkOn = false;

        if (SceneManager.GetActiveScene().name.Equals("Menu"))
        {
            animator.Play("ZombieIdle");
            animator.CrossFade("ZombieIdle", Random.Range(0f, 2f), 0, Random.Range(0f, 2f));
        }

    }

    private void FixedUpdate()
    {
        if (gameStates.isGameStarded && !isWalkOn)
        {
            animator.Play("ZombieWalk");
            isWalkOn = true;
        }

        if (gameStates.isGameStarded && !isDead)
        {
            direction.z = -enemyForwardSpeed;
            characterController.Move(direction * Time.fixedDeltaTime);
        }

        if (currentlyAttacks && transform.position.z > inGameInfo.playerPositionZ)
        {
            animator.Play("ZombieAttack");

            if (!AudioManager.Instance.zombieSfxSource.isPlaying)
            {
                AudioManager.Instance.PlayZombieSFX("ZombieAttack");
            }
            currentlyAttacks = false;
        }

    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Obstacle"))
        {
            currentlyAttacks = true;
            GameObject objectToDestroy = hit.gameObject;
            StartCoroutine(DestroyObstacleCoroutine(objectToDestroy));
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            AudioManager.Instance.PlayZombieSFX("ZombieDeath");
            animator.Play("ZombieDeathA");
            inGameInfo.enemiesScore++;
            isDead = true;
            characterController.enabled = false;
            Destroy(collision.gameObject);

            if (gameStates.isThisTutorial)
            {
                inGameInfo.tutorialCounter++;
            }
        }
    }

    private IEnumerator DestroyObstacleCoroutine(GameObject objectToDestroy)
    {
        yield return new WaitForSeconds(3);
        Destroy(objectToDestroy);
        if (AudioManager.Instance.zombieSfxSource.isPlaying)
        {
            AudioManager.Instance.zombieSfxSource.Stop(); //TODO: Bug, czasem sfx wy³¹cza siê na inne dŸwiêki
        }
    }

}
