using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletSpawner : MonoBehaviour
{
    [Header("Edit Spawner")]
    public float attackSpeed = 3;
    public float numberOfBullets = 8;
    public float angleOffset;

    [Header("Edit Bullets")]
    public float bulletSpeed = 3;
    public float bulletLifespan = 5;
    public bool randomColor;
    public Color bulletColor;
    public Color[] colorList;

    private int angleIndex;

    [Header("Bullet Prefab")]
    public GameObject bullet;
    private bool notEnoughBulletsInPool = true;
    private List<GameObject> bullets = new List<GameObject>();
    public void StartFireBulletCourutine()
    {
        StartCoroutine(FireBullets());
    }
    private IEnumerator FireBullets()
    {
        angleIndex++;
        float bulletAngle = 360 / numberOfBullets;

        for(int i = 0 ; i < numberOfBullets ; i++)
        {
            float currentAngle = (i * bulletAngle) + (angleOffset * angleIndex);
            Vector3 currentRotation = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, currentAngle);

            GameObject currentBullet = GetBullet();
            currentBullet.transform.position = transform.position;
            currentBullet.transform.rotation = Quaternion.Euler(currentRotation);
            currentBullet.SetActive(true);

        currentBullet.GetComponent<Rigidbody2D>().velocity = currentBullet.transform.up * bulletSpeed;
        if (randomColor)
            {
                currentBullet.GetComponent<SpriteRenderer>().color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
            }
            else
            {
                currentBullet.GetComponent<SpriteRenderer>().color = bulletColor;
            }
            StartCoroutine(DeactivateBullet(currentBullet));
        }
        yield return new WaitForSeconds(attackSpeed);
        StartCoroutine(FireBullets());
    }
    private IEnumerator DeactivateBullet(GameObject currentBullet)
    {
        yield return new WaitForSeconds(bulletLifespan);
        currentBullet.SetActive(false);
    }
    private GameObject GetBullet()
    {
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

        if(notEnoughBulletsInPool)
        {
            GameObject currentBullet = Instantiate(bullet);
            currentBullet.SetActive(false);
            bullets.Add(currentBullet);
            return currentBullet;
        }
        return null;
    }
}