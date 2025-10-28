using AngryBirds.Enums;
using UnityEngine;

namespace AngryBirds.Birds
{
    public class WhiteBird : BirdBase
    {
        [SerializeField] private GameObject _eggPrefab;
        private void Start()
        {
            _birdType =  BirdType.White;
        }
    
        public override void UseSpecialAbility()
        {
            Rigidbody eggRb = Instantiate(_eggPrefab, transform.position, transform.rotation).GetComponent<Rigidbody>();
            _rb.AddForce(Vector3.up * 10f, ForceMode.Impulse);
        }
    }
}
