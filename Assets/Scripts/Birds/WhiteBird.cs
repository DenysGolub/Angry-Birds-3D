using AngryBirds.Enums;
using UnityEngine;

namespace AngryBirds.Birds
{
    public class WhiteBird : BirdBase
    {
        public GameObject EggPrefab;
        private AudioSource _pushingEgg;
        private void Start()
        {
            _birdType =  BirdType.White;
            _pushingEgg = GetComponent<AudioSource>();
        }

    
        public override void UseSpecialAbility()
        {
            _pushingEgg.Play();
            Rigidbody eggRb = Instantiate(EggPrefab, transform.position, transform.rotation).GetComponent<Rigidbody>();
            _rb.AddForce(Vector3.up * 10f, ForceMode.Impulse);
        }
    }
}
