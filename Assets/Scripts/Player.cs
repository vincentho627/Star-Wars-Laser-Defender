using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{

    [Header("Player")] 
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float padding = 1f;
    [SerializeField] float health = 500f;
    HeathBar healthBar;
    [SerializeField] GameObject particleExplosion;
    [SerializeField] float deathTime = 1f;

    [Header("Shooting")]
    [SerializeField] GameObject laserPrefab;
    [SerializeField] float projectileSpeed = 20f;
    [SerializeField] float projectShootPeriod = 0f;

    [Header("Sound Effects")]
    [SerializeField] AudioClip shootVFX;
    [SerializeField] [Range(0, 1)] float shootSoundVolume = 0.5f;
    [SerializeField] AudioClip deathVFX;
    [SerializeField] [Range(0, 1)] float deathSoundVolume = 0.7f;
    [SerializeField] AudioClip damagedVFX;
    [SerializeField] [Range(0, 1)] float damagedSoundVolumee = 0.5f;

    [Header("Boosts")]
    [SerializeField] GameObject shieldPrefab;
    [SerializeField] AudioClip boostDown;
    [SerializeField] [Range(0, 1)] float boostDownVolume; 
    GameObject shielding;

    float xMin;
    float xMax;
    float yMin;
    float yMax;
    bool linked = false;
   
    bool shield = false;

    Coroutine shootCoroutine;

    private void Awake()
    {
        if (FindObjectsOfType(GetType()).Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SetUpMoveBoundaries();
        laserPrefab.GetComponent<DamageDealer>().SetDamage(100);
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            Link();

        }
        Move();
        Shoot();
        Boosts();
    }

    private void Boosts()
    {
        if (shield)
        {
            shielding.transform.position = this.transform.position;
        }
    }

    private void Link()
    {
        if (!linked)
        {
            healthBar = FindObjectOfType<HeathBar>();
            healthBar.ResetHealthSlider();
            linked = true;
        } 
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (other.gameObject.GetComponent<Boost>() != null)
        {
            Boost boost = other.gameObject.GetComponent<Boost>();
            string boostType = boost.GetEffect();
            BoostPlayer(boostType);

            Destroy(other.gameObject);
            return;
        }

        if (shield)
        {
            shield = false;
            Destroy(shielding);
            AudioSource.PlayClipAtPoint(boostDown, Camera.main.transform.position, boostDownVolume);
        }
        else
        {
            health -= damageDealer.GetDamage();
            healthBar.SetHealthSlider(health);
        }

        if (!damageDealer)
        {
            return;
        }

        if (other.gameObject.GetComponent<Enemy>() != null) {
            if (!other.gameObject.GetComponent<Enemy>().IsBoss())
            {
                damageDealer.Hit();
            }
        } else
        {
            damageDealer.Hit();
        }

        if (health < 0)
        {
            Die();
        } else
        {
            if (!shield)
            {
                AudioSource.PlayClipAtPoint(damagedVFX, Camera.main.transform.position, damagedSoundVolumee);
            }
        }

       
    }

    private void BoostPlayer(string boostType)
    {
        if (!shield)
        {
            if (boostType == "Shield")
            {
                shield = true;
                shielding = Instantiate(shieldPrefab, transform.position, Quaternion.identity);
            }
        }

        if (boostType == "Health")
        {
            if (health < healthBar.GetMaxHealthSlider())
            {
                if (health < healthBar.GetMaxHealthSlider() - 100)
                {
                    health += 100;

                }
                else
                {
                    health = healthBar.GetMaxHealthSlider();
                }

                healthBar.SetHealthSlider(health);
            }
        }

        if (boostType == "Energy")
        {
            DamageDealer playerDamage = laserPrefab.GetComponent<DamageDealer>();
            playerDamage.SetDamage(playerDamage.GetDamage() * 1.2f);
            FindObjectOfType<BoostStats>().AddEnergyStat();
        }
    }

    private void Die()
    {
        FindObjectOfType<Level>().LoadGameOver();
        Destroy(gameObject);
        GameObject explosion = Instantiate(particleExplosion, transform.position, transform.rotation);
        Destroy(explosion, deathTime);
        AudioSource.PlayClipAtPoint(deathVFX, Camera.main.transform.position, deathSoundVolume);
        
    }

    IEnumerator ShootContinously()
    {
        while (true)
        {
            GameObject laser = Instantiate(laserPrefab, transform.position, Quaternion.identity) as GameObject;
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, projectileSpeed);
            AudioSource.PlayClipAtPoint(shootVFX, Camera.main.transform.position, shootSoundVolume);
            yield return new WaitForSeconds(projectShootPeriod);
        }
       
    }

    private void Shoot()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            shootCoroutine = StartCoroutine((ShootContinously()));
        }
        if (Input.GetButtonUp("Fire1"))
        {
            StopCoroutine(shootCoroutine);
        }
    }

    private void Move()
    {
        var deltaX = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        var deltaY = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;

        var newXPos = Mathf.Clamp(transform.position.x + deltaX, xMin, xMax);
        var newYPos = Mathf.Clamp(transform.position.y + deltaY, yMin, yMax);

        transform.position = new Vector2(newXPos, newYPos);
    }

    private void SetUpMoveBoundaries()
    {
        Camera gameCamera = Camera.main;
        Vector3 minVector = new Vector3(0, 0, 0);
        xMin = gameCamera.ViewportToWorldPoint(minVector).x + padding;
        yMin = gameCamera.ViewportToWorldPoint(minVector).y + padding;
        xMax = gameCamera.ViewportToWorldPoint(new Vector3(1, 0, 0)).x - padding;
        yMax = gameCamera.ViewportToWorldPoint(new Vector3(0, 1, 0)).y - padding;
    }

    public float GetHealth()
    {
        return health;
    }

    public bool HasShield()
    {
        return shield;
    }

}
