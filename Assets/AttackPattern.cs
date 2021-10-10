using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attack Pattern", fileName = "Attack Pattern")]
public class AttackPattern : ScriptableObject
{
    [Header("Edit Spawner")]
    [SerializeField] private float attackSpeed = 3f;
    [SerializeField] private float numberOfBullets = 8f;
    [SerializeField] private float angleOffset = 22.5f;

    [Header("Edit Bullets")]
    [SerializeField] private float bulletSpeed = 3f;
    [SerializeField] private float bulletLifespan = 5f;
    [SerializeField] private float bulletSize = 0.3f;

    [Header("Bullet Color")]
    [SerializeField] private Color[] colorList;
    [SerializeField] private bool randomColor;
    [SerializeField] private float timeBetweenColor;

    [SerializeField] private int bulletsOfColor;

    [Header("Edit Attack Pattern")]
    [SerializeField] private float patternLength;

    public float GetAttackSpeed() { return attackSpeed; }
    public float GetNumberOfBullets() { return numberOfBullets; }
    public float GetAngleOffset() { return angleOffset; }

    public float GetBulletSpeed() { return bulletSpeed; }
    public float GetBulleLifespan() { return bulletLifespan; }
    public float GetBulletSize() { return bulletSize; }

    public Color[] GetColorList() { return colorList; }
    public bool GetRandomColor() { return randomColor; }
    public float GetTimeBetweenColor() { return timeBetweenColor; }
    public int GetBulletsOfColor() { return bulletsOfColor; }
    public float GetPatternLength() { return patternLength; }

}
