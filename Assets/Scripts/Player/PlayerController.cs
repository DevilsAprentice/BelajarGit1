using UnityEngine;
using UnityEngine.InputSystem; // Pastikan ini ada untuk Mouse.current

public class PlayerController : MonoBehaviour
{
    public PlayerData playerData;

    private float currentHP;
    private float speed;
    
    public GameObject bulletPrefab;
    public Transform bulletSpawnPoint; // Variabel untuk menentukan posisi spawn peluru
    
    private float previousAttackInput; // Cukup tulis satu kali saja
    private float attackInput;

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
        attackInput = playerInput.actions["Attack"].ReadValue<float>();
        transform.Translate(new Vector3(moveInput.x, moveInput.y, 0) * speed * Time.deltaTime);

        // Cek agar hanya menembak sekali per ketukan (bukan nembak terus tiap frame)
        if (attackInput > 0 && previousAttackInput <= 0)
        {
            Shoot();
        }

        // Simpan input saat ini untuk dicek di frame berikutnya
        previousAttackInput = attackInput;
    }

    void Shoot()
    {
        Debug.Log("Player is shooting!");
        
        if (bulletPrefab == null)
        {
            Debug.LogWarning("Bullet prefab not assigned!");
            return;
        }

        // Determine spawn position
        Vector3 spawnPos = bulletSpawnPoint != null ? bulletSpawnPoint.position : transform.position;

        // Memperbaiki pembacaan mouse agar konsisten dengan New Input System
        Vector2 mouseScreenPos = Mouse.current.position.ReadValue();
        Vector3 mousePosWithZ = new Vector3(mouseScreenPos.x, mouseScreenPos.y, Mathf.Abs(Camera.main.transform.position.z));
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePosWithZ);
        mouseWorldPos.z = 0; // Ensure Z is 0 for 2D
        
        // Calculate direction from player to mouse
        Vector3 shootDirection = (mouseWorldPos - spawnPos).normalized;
        
        Debug.Log($"Spawn Pos: {spawnPos}, Mouse World Pos: {mouseWorldPos}, Direction: {shootDirection}");

        // Instantiate bullet
        GameObject bulletObj = Instantiate(bulletPrefab, spawnPos, Quaternion.identity);
        
        // Set bullet direction
        Bullet bullet = bulletObj.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.SetDirection(shootDirection);
            Debug.Log($"Bullet direction set to: {shootDirection}");
        }
        else
        {
            Debug.LogError("Bullet component not found on prefab!");
        }

        Debug.Log("Bullet spawned!");
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