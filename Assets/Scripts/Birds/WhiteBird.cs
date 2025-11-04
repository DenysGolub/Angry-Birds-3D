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
            _eggPrefab.GetComponent<Egg>().IsMultiplayer = _isMultiplayer;
        }
    
        public override void UseSpecialAbility()
        {
            if (!_isMultiplayer)
            {
                Instantiate(_eggPrefab, transform.position, transform.rotation);
            }
            else
            {
                Runner.Spawn(_eggPrefab, transform.position, transform.rotation);
            }
            
            Rb.AddForce(Vector3.up * 10f, ForceMode.Impulse);
        }
    }
}
