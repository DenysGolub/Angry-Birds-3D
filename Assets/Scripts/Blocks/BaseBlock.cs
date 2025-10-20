using UnityEngine;

public abstract class BaseBlock : MonoBehaviour
{
    public abstract void OnCollisionEnter(Collision collision);
}
