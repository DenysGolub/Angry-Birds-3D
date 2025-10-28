using AngryBirds.Enums;
using UnityEngine;

namespace AngryBirds.Birds
{
    public class WhiteBird : BirdBase
    {
        [SerializeField] private GameObject _eggPrefab;
        private void Start()
        {
            BirdType =  BirdType.White;
        }
    
        public override void UseSpecialAbility()
        {
            Rigidbody eggRb = Instantiate(_eggPrefab, transform.position, transform.rotation).GetComponent<Rigidbody>();
            Rb.AddForce(Vector3.up * 10f, ForceMode.Impulse);
        }
    }
}
