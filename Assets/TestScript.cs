using UnityEngine;

public class TestScript : MonoBehaviour
{
    private Rigidbody _rb;
    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Force()
    {
        _rb.AddTorque(Vector3.up * 10, ForceMode.Impulse);
    }
    
    
}
