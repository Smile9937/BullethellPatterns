using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attack Pattern", fileName = "Attack Pattern")]
public class AttackPattern : ScriptableObject
{
    [Header("Edit Spawner")]
    [SerializeField] private float attackDelay = 3f;
    [SerializeField] private float numberOfBullets = 8f;
    [SerializeField] private float angleOffset = 22.5f;

    [Header("Edit Bullets")]
    [SerializeField] private float bulletSpeed = 3f;
    [SerializeField] private float bulletLifespan = 5f;
    [SerializeField] private float bulletSize = 0.3f;

    [Header("Bullet Design")]
    [SerializeField] private Sprite bulletSprite;
    [SerializeField] private Color[] colorList;
    [SerializeField] private bool randomColor;
    [SerializeField] private int bulletsOfColor = 0;

    [Header("Edit Attack Pattern")]
    [SerializeField] private float patternLength = 5f;
    [SerializeField] private float timeUntilPatternStart = 0.5f;

    //Edit Spawner
    public float GetAttackDelay() { return attackDelay; }
    public float GetNumberOfBullets() { return numberOfBullets; }
    public float GetAngleOffset() { return angleOffset; }

    //Edit Bullets
    public float GetBulletSpeed() { return bulletSpeed; }
    public float GetBulleLifespan() { return bulletLifespan; }
    public float GetBulletSize() { return bulletSize; }

    //Bullet Design
    public Sprite GetBulletSprite() { return bulletSprite; }
    public Color[] GetColorList() { return colorList; }
    public bool GetRandomColor() { return randomColor; }
    public int GetBulletsOfColor() { return bulletsOfColor; }

    //Edit Attack Pattern
    public float GetPatternLength() { return patternLength; }
    public float GetTimeUntilPatternStart() { return timeUntilPatternStart; }

}
