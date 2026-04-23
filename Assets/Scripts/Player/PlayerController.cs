using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public PlayerData playerData;

    private float currentHP;
    private float speed;

    private PlayerInput playerInput;
    private Vector2 moveInput;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();

        currentHP = playerData.maxHP;
        speed = playerData.moveSpeed;
    }

    void Update()
{
    
    if (GameManager.Instance.currentState != GameState.Playing) return;

    if (playerInput == null) return;

    moveInput = playerInput.actions["Move"].ReadValue<Vector2>();
    transform.Translate(new Vector3(moveInput.x, moveInput.y, 0) * speed * Time.deltaTime);
}

   void OnCollisionStay2D(Collision2D collision)
{
    if (collision.gameObject.CompareTag("Wall"))
    {
        
        TakeDamage(2.0f * Time.deltaTime); 
    }
}

   void TakeDamage(float dmg)
{
   
    if (GameManager.Instance.currentState == GameState.GameOver) return;

    currentHP -= dmg;
    Debug.Log("Player HP: " + currentHP);

    if (currentHP <= 0)
    {
        currentHP = 0; 
        GameManager.Instance.GameOver();
    }
}


}