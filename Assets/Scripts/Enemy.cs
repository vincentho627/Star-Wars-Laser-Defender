using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [Header("Stats")]
    [SerializeField] float currentHealth = 100f;
    [SerializeField] int points = 50;

    [Header("Shooting")]
    [SerializeField] float shotCounter;
    [SerializeField] float minTimeBetweenShots = 0.2f;
    [SerializeField] float maxTimeBetweenShots = 3f;
    [SerializeField] float projectileSpeed = 20f;   
    [SerializeField] GameObject laserPrefab;
    [SerializeField] GameObject particleExplosion;
    [SerializeField] int shootingDamage;

    [Header("Sound Effects")]
    [SerializeField] float deathTime = 1f;
    [SerializeField] AudioClip shootVFX;
    [SerializeField] [Range(0, 1)] float shootingSoundVolume = 0.7f;
    [SerializeField] AudioClip deathVFX;
    [SerializeField] [Range(0,1)] float deathSoundVolume = 0.7f;
    [SerializeField] bool isBoss = false;

    [Header("Boosts")]
    [SerializeField] List<GameObject> boostPrefabs;
    [SerializeField] float chanceOfSuccess = 0.9f;
    [SerializeField] float boostFallSpeed = 20f;


    // Start is called before the first frame update
    void Start()
    {
        shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        laserPrefab.GetComponent<DamageDealer>().SetDamage(shootingDamage * FindObjectOfType<Level>().GetLevel());
    }

    // Update is called once per frame
    void Update()
    {
        CountDownToShoot();
    }

    public bool IsBoss()
    {
        return isBoss;
    }

    private void CountDownToShoot()
    {
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0)
        {
            Fire();
            shotCounter = Random.Range(minTimeBetweenShots, maxTimeBetweenShots);
        }
    }

    private void Fire()
    {
        if (isBoss)
        {
            Vector2 laserPositionRightYellow = new Vector2(transform.position.x + 1f, transform.position.y - 1f);
            GameObject laser = Instantiate(laserPrefab, laserPositionRightYellow, Quaternion.identity) as GameObject;
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
            AudioSource.PlayClipAtPoint(shootVFX, Camera.main.transform.position, shootingSoundVolume);

            Vector2 laserPositionLeftYellow = new Vector2(transform.position.x - 1f, transform.position.y - 1f);
            laser = Instantiate(laserPrefab, laserPositionLeftYellow, Quaternion.identity);
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
            AudioSource.PlayClipAtPoint(shootVFX, Camera.main.transform.position, shootingSoundVolume);

            Vector2 laserPositionLeftWing = new Vector2(transform.position.x - 2f, transform.position.y + 2f);
            laser = Instantiate(laserPrefab, laserPositionLeftWing, Quaternion.identity);
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
            AudioSource.PlayClipAtPoint(shootVFX, Camera.main.transform.position, shootingSoundVolume);

            Vector2 laserPositionRightWing = new Vector2(transform.position.x + 2f, transform.position.y + 2f);
            laser = Instantiate(laserPrefab, laserPositionRightWing, Quaternion.identity);
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
            AudioSource.PlayClipAtPoint(shootVFX, Camera.main.transform.position, shootingSoundVolume);

            //Vector2 cannonOne = new Vector2(transform.position.x - 1f, transform.position.y - 0.1f);
            //laser = Instantiate(laserPrefab, cannonOne, Quaternion.Euler(10, -projectileSpeed, 0));
            //laser.GetComponent<Rigidbody2D>().velocity = new Vector2(10, -projectileSpeed);
            //AudioSource.PlayClipAtPoint(shootVFX, Camera.main.transform.position, shootingSoundVolume);

            //Vector2 cannonTwo = new Vector2(transform.position.x + 1f, transform.position.y + 0.1f);
            //laser = Instantiate(laserPrefab, cannonTwo, Quaternion.identity);
            //laser.GetComponent<Rigidbody2D>().velocity = new Vector2(10, -projectileSpeed);
            //AudioSource.PlayClipAtPoint(shootVFX, Camera.main.transform.position, shootingSoundVolume);
        }
        else
        {
            Vector2 laserPosition = new Vector2(transform.position.x, transform.position.y - 1f);
            GameObject laser = Instantiate(laserPrefab, laserPosition, Quaternion.identity) as GameObject;
            laser.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
            AudioSource.PlayClipAtPoint(shootVFX, Camera.main.transform.position, shootingSoundVolume);
        }
      
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DamageDealer damageDealer = other.gameObject.GetComponent<DamageDealer>();
        if (!damageDealer)
        {
            return;
        }
        currentHealth -= damageDealer.GetDamage();

        if (other.gameObject.GetComponent<Player>() == null)
        {
            damageDealer.Hit();

        } else
        {
            if (!other.gameObject.GetComponent<Player>().HasShield())
            {
                damageDealer.Hit();
            }
        }

   


        if (currentHealth <= 0)
        {
            Die();
            
            if (isBoss)
            {
                FindObjectOfType<GameSession>().HyperSpace();
                FindObjectOfType<Level>().NextLevel();
            }
        }
    }



    public void Hit(int damage)
    {
        currentHealth -= damage;
        if (currentHealth < 0)
        {
            Destroy(gameObject);
        } 
    }

    private void Die()
    {
        FindObjectOfType<GameSession>().AddToScore(points);
        Destroy(gameObject);
        GameObject explosion = Instantiate(particleExplosion, transform.position, transform.rotation);
        Destroy(explosion, deathTime);
        AudioSource.PlayClipAtPoint(deathVFX, Camera.main.transform.position, deathSoundVolume);
        FindObjectOfType<EnemySpawner>().EnemyDown();

        float randomNumber = Random.Range(0f, 1f);
        if (randomNumber < chanceOfSuccess)
        {
            float randomBoost = Random.Range(0f, boostPrefabs.Count);

            int index = (int) randomBoost;

            GameObject boost = Instantiate(boostPrefabs[index], transform.position, Quaternion.identity);
            boost.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -boostFallSpeed);
        }
    }
}
