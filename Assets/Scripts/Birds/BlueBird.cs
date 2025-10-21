using UnityEngine;

public class BlueBird : BirdBase
{
    void Start()
    {
        _birdType =  BirdType.Blue;
    }
    
    public override void UseSpecialAbility()
    {
        Rigidbody currentBird = GetComponent<Rigidbody>();
        Vector3 positionUp = gameObject.transform.position;
        Vector3 positionDown = gameObject.transform.position;
        
        positionUp.y = positionUp.y + 1f;
        positionDown.y = positionDown.y - 1f;
        
        GameObject firstBird = Instantiate(gameObject, positionUp, transform.rotation);
        GameObject secondBird = Instantiate(gameObject, positionDown, transform.rotation);

        Rigidbody firstBirdRigidbody = firstBird.GetComponent<Rigidbody>();
        Rigidbody secondBirdRigidbody = secondBird.GetComponent<Rigidbody>();
        
        firstBirdRigidbody.linearVelocity = currentBird.linearVelocity;
        secondBirdRigidbody.linearVelocity = currentBird.linearVelocity;
        
        firstBirdRigidbody.angularVelocity = currentBird.angularVelocity;
        secondBirdRigidbody.angularVelocity = currentBird.angularVelocity;

        StartCoroutine(firstBird.GetComponent<BirdBase>().DestroyBird());
        StartCoroutine(secondBird.GetComponent<BirdBase>().DestroyBird());

    }
}
