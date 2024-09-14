using UnityEngine;

[CreateAssetMenu(fileName = "NewMonster", menuName = "Game/Monster Data")]
public class EnemyData : ScriptableObject
{
   
    public float maxhealth = 50f;
    public string enemyName = "Enemy";
    public float defense = 5f;
    public float armorPenetration = 5f;
    public bool isCritical = false;


}