using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [Header("Bullet Prefab")]
    [SerializeField] private GameObject bullet;
    private bool notEnoughBulletsInPool = true;
    private List<GameObject> bullets = new List<GameObject>();

    [SerializeField] private AttackPattern[] attackPatterns;
    private int attackPatternIndex = 0;

    private int angleIndex = 0;
    private int colorIndex = 0;

    private bool isAttacking;

    private Color startBulletColor;
    private Sprite startSprite;

    private int bulletsSpawned = 0;

    private IEnumerator fireBullets;

    private AttackPattern currentAttackPattern;

    private void Start()
    {
        fireBullets = FireBullets();
    }
    public void StartFiringBullets()
    {
        if(!isAttacking)
        {
            startBulletColor = bullet.GetComponent<SpriteRenderer>().color;
            startSprite = bullet.GetComponent<SpriteRenderer>().sprite;
            attackPatternIndex = 0;
            angleIndex = 0;
            colorIndex = 0;
            bulletsSpawned = 0;
            isAttacking = true;
            currentAttackPattern = attackPatterns[0];
            StartCoroutine(StartNextPattern());
        }
    }
    private IEnumerator FireBullets()
    {
        //Used for offsetting the rotation
        angleIndex++;

        //Even spacing for the bullet spawnpoints
        float bulletAngle = 360 / currentAttackPattern.GetNumberOfBullets();

        for (int i = 0; i < currentAttackPattern.GetNumberOfBullets(); i++)
        {
            //Set the position of the bulles with correct rotation
            float currentAngle = (i * bulletAngle) + (currentAttackPattern.GetAngleOffset() * angleIndex);

            Vector3 currentRotation = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, currentAngle);
            GameObject currentBullet = GetBullet();

            currentBullet.transform.position = transform.position;
            currentBullet.transform.rotation = Quaternion.Euler(currentRotation);
            currentBullet.transform.localScale = new Vector2(currentAttackPattern.GetBulletSize(), currentAttackPattern.GetBulletSize());

            if(currentAttackPattern.GetBulletSprite() != null)
            {
                currentBullet.GetComponent<SpriteRenderer>().sprite = currentAttackPattern.GetBulletSprite();
            } else
            {
                currentBullet.GetComponent<SpriteRenderer>().sprite = startSprite;
            }

            ChangeBulletColor(currentBullet);

            //Activate bullet
            currentBullet.SetActive(true);

            //Set bullet velocity
            currentBullet.GetComponent<Rigidbody2D>().velocity = currentBullet.transform.up * currentAttackPattern.GetBulletSpeed();

            //Deactivate bullet
            StartCoroutine(DeactivateBullet(currentBullet));
        }
        yield return new WaitForSeconds(currentAttackPattern.GetAttackDelay());
        RestartFireBullets();
    }
    private void ChangeBulletColor(GameObject currentBullet)
    {
        if(currentAttackPattern.GetBulletsOfColor() > 0)
        {
            //Increases the color array index when the correct number of bullets have spawned
            if (bulletsSpawned == currentAttackPattern.GetBulletsOfColor())
            {
                if (attackPatternIndex < attackPatterns.Length)
                {
                    if (colorIndex == currentAttackPattern.GetColorList().Length - 1)
                    {
                        colorIndex = 0;
                    }
                    else
                    {
                        colorIndex++;
                    }
                }
                bulletsSpawned = 0;
            }

            if (bulletsSpawned < currentAttackPattern.GetBulletsOfColor())
            {
                bulletsSpawned++;
            }
        }

        //Sets the color of the bullets
        if (attackPatterns[attackPatternIndex].GetRandomColor())
        {
            int randomNum = Random.Range(0, currentAttackPattern.GetColorList().Length);
            currentBullet.GetComponent<SpriteRenderer>().color = currentAttackPattern.GetColorList()[randomNum];
        }
        else if (currentAttackPattern.GetColorList().Length > 0)
        {
            currentBullet.GetComponent<SpriteRenderer>().color = currentAttackPattern.GetColorList()[colorIndex];
        }

        else
        {
            currentBullet.GetComponent<SpriteRenderer>().color = startBulletColor;
            Debug.Log("Color Array Empty");
        }
    }
    private void RestartFireBullets()
    {
        if (fireBullets != null)
        {
            StopCoroutine(fireBullets);
        }
        fireBullets = FireBullets();
        StartCoroutine(fireBullets);
    }
    private IEnumerator DeactivateBullet(GameObject currentBullet)
    {
        yield return new WaitForSeconds(currentAttackPattern.GetBulleLifespan());
        currentBullet.SetActive(false);
    }

    //Get Bullet From Object Pool
    private GameObject GetBullet()
    {
        //Checks if there are bullets in the bullets array and returns one if it is active
        if(bullets.Count > 0)
        {
            for(int i = 0; i < bullets.Count; i++)
            {
                if(!bullets[i].activeInHierarchy)
                {
                    return bullets[i];
                }
            }
        }

        //Spawns a new bullet if there are not enough in the pool and returns it
        if(notEnoughBulletsInPool)
        {
            GameObject currentBullet = Instantiate(bullet);
            currentBullet.SetActive(false);
            bullets.Add(currentBullet);
            return currentBullet;
        }
        return null;
    }
    private IEnumerator StartNextPattern()
    {
        StopCoroutine(fireBullets);

        //Wait to start pattern
        yield return new WaitForSeconds(currentAttackPattern.GetTimeUntilPatternStart());

        RestartFireBullets();

        //Wait until current pattern is finished
        yield return new WaitForSeconds(currentAttackPattern.GetPatternLength());

        attackPatternIndex++;

        //Check if all patterns are finished
        if (attackPatternIndex == attackPatterns.Length)
        {
            StopAttacking();
            Debug.Log("All patterns complete");
        }
        else
        {
            colorIndex = 0;
            angleIndex = 0;
            bulletsSpawned = 0;
            currentAttackPattern = attackPatterns[attackPatternIndex];
            StartCoroutine(StartNextPattern());
        }
    }
    public void StopAttacking()
    {
        StopAllCoroutines();
        isAttacking = false;
    }
}