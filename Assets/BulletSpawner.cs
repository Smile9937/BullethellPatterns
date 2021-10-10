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
    private int attackPatternIndex;

    private int angleIndex;
    private int colorIndex;

    private bool isAttacking;

    private Color startBulletColor;

    #region ChangeCoroutine
    private Coroutine currentCoroutine;
    private void ChangeCoroutine(IEnumerator coroutine)
    {
        //Replaces the instance of a coroutine with a new one
        if(currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(coroutine);
    }
    #endregion
    public void StartFireBulletCourutine()
    {
        if(!isAttacking)
        {
            startBulletColor = bullet.GetComponent<SpriteRenderer>().color;
            attackPatternIndex = 0;
            angleIndex = 0;
            colorIndex = 0;
            isAttacking = true;
            StartCoroutine(FireBullets());
            StartCoroutine(StartNextPattern());
            StartCoroutine(IncreaseColorIndex());
        }
    }
    private IEnumerator FireBullets()
    {
        yield return new WaitForSeconds(attackPatterns[attackPatternIndex].GetAttackSpeed());
        //Used for offsetting the rotation
        angleIndex++;

        //Even spacing for the bullet spawnpoints
        float bulletAngle = 360 / attackPatterns[attackPatternIndex].GetNumberOfBullets();

        //Set the position of the bulles with correct rotation
        for(int i = 0 ; i < attackPatterns[attackPatternIndex].GetNumberOfBullets(); i++)
        {
            float currentAngle = (i * bulletAngle) + (attackPatterns[attackPatternIndex].GetAngleOffset() * angleIndex);
            Vector3 currentRotation = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, currentAngle);

            GameObject currentBullet = GetBullet();
            currentBullet.transform.position = transform.position;
            currentBullet.transform.rotation = Quaternion.Euler(currentRotation);
            currentBullet.transform.localScale = new Vector2(attackPatterns[attackPatternIndex].GetBulletSize(), attackPatterns[attackPatternIndex].GetBulletSize());

            //Activate bullet
            currentBullet.SetActive(true);

            //Set bullet velocity
            currentBullet.GetComponent<Rigidbody2D>().velocity = currentBullet.transform.up * attackPatterns[attackPatternIndex].GetBulletSpeed();

            //Set bullet color
            if (attackPatterns[attackPatternIndex].GetRandomColor())
            {
                currentBullet.GetComponent<SpriteRenderer>().color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
            }
            else if (attackPatterns[attackPatternIndex].GetColorList().Length > 0)
            {
                currentBullet.GetComponent<SpriteRenderer>().color = attackPatterns[attackPatternIndex].GetColorList()[colorIndex];
            }
            else
            {
                currentBullet.GetComponent<SpriteRenderer>().color = startBulletColor;
            }

            //Deactivate bullet
            StartCoroutine(DeactivateBullet(currentBullet));
        }
        //Restart this courutine based on attack speed
        StartCoroutine(FireBullets());
    }
    private IEnumerator DeactivateBullet(GameObject currentBullet)
    {
        yield return new WaitForSeconds(attackPatterns[attackPatternIndex].GetBulleLifespan());
        currentBullet.SetActive(false);
    }
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
    private IEnumerator IncreaseColorIndex()
    {
        yield return new WaitForSeconds(attackPatterns[attackPatternIndex].GetTimeBetweenColor());
        Debug.Log(colorIndex);
        if (attackPatternIndex < attackPatterns.Length)
        {
            if (colorIndex >= attackPatterns[attackPatternIndex].GetColorList().Length-1)
            {
                colorIndex = 0;
            } else
            {
                colorIndex++;
            }
             

            Debug.Log("Test");
            ChangeCoroutine(IncreaseColorIndex());
        }
    }
    private IEnumerator StartNextPattern()
    {
        //Starts a new attack pattern
        if (attackPatternIndex < attackPatterns.Length)
        {
            //StopCoroutine(IncreaseColorIndex());
            colorIndex = 0;

            yield return new WaitForSeconds(attackPatterns[attackPatternIndex].GetPatternLength());

            attackPatternIndex++;
            //ChangeCoroutine(IncreaseColorIndex());
            StartCoroutine(StartNextPattern());
        } else
        {
            StopAttacking();
            Debug.Log("All patterns complete");
        }
    }
    public void StopAttacking()
    {
        StopAllCoroutines();
        isAttacking = false;
    }
}