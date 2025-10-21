using Enums;
using UnityEngine;

[CreateAssetMenu(fileName = "New Block", menuName = "Blocks")]
public class BlockSO : ScriptableObject
{
    public int MaxHealth;
    public int DamageMultiplier;
    public float Mass;
    public float LinearDamping;
    public BlockType Type;
}
