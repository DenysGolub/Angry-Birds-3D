using System;
using System.Collections;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slingshot : MonoBehaviour
{
    [Header("Slingshot setup")] public Transform Pivot;
    public Transform LaunchPoint;
    public float MaxStretch = 3f;
    public float PowerMultiplier = 30f;

    [Header("Flight Camera Setup")] public CinemachineCamera FlightCamera;

    public float WaitSecondsAfterShot = 2f;

    [Header("Bands Visuals")] public LineRenderer LeftBand;
    public LineRenderer RightBand;
    public Transform LeftAnchor;
    public Transform RightAnchor;

    [Header("Flight Path")] public LineRenderer FlightPath;
    public int PathResolution = 30;
    public float PathTimeStep = 0.1f;

    private Rigidbody CurrentProjectile;
    private SpringJoint Joint;
    private bool IsDragging = false;

    [Header("First ammo setup")]
    public GameObject CurrentProjectilePrefab;
    public static event Action OnShotFired;
    

    void Start()
    {
        OnShotFired.Invoke();
        CreateProjectile();
    }

    private void OnEnable()
    {
        GameManager.SetNextBirdToSlingshotAction += GetProjectile;
    }

    private void OnDisable()
    {
        GameManager.SetNextBirdToSlingshotAction -= GetProjectile;
    }

    void GetProjectile(GameObject bird)
    {
        CurrentProjectilePrefab = bird;
    }

    //Logic: we start to drag our bird and the more we drag to back the more power it will get
    //assume that bird flight always have parabola trajectory

    void Update()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }


        if (CurrentProjectile == null)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            AudioManager.Instance.PlaySlingshotStretch();

            IsDragging = true;
        }

        if (Input.GetMouseButtonUp(0))
        {
            AudioManager.Instance.PlayLaunchSlingshot();
            AudioManager.Instance.PlayBirdLaunch(Bird.Red);
            IsDragging = false;
            FlightPath.enabled = false;
            StartCoroutine(Release());
        }

        if (IsDragging)
        {
            DragProjectile();
            UpdateFlightPath();
        }
    }

    void LateUpdate()
    {
        UpdateBands();
    }

    void CreateProjectile()
    {
        
        FlightCamera.Follow = Pivot;

        if (CurrentProjectilePrefab != null)
        {
            CurrentProjectile = Instantiate(CurrentProjectilePrefab.GetComponent<Rigidbody>(), LaunchPoint.position, Quaternion.identity);
            Joint = CurrentProjectile.gameObject.AddComponent<SpringJoint>();
            Joint.connectedAnchor = Pivot.position;
            Joint.spring = 40f;
            Joint.damper = 5f;
            Joint.autoConfigureConnectedAnchor = false;
            CurrentProjectile.isKinematic = true; 
        }
    }

    void DragProjectile()
    {
        if (!CurrentProjectile.isKinematic)
            CurrentProjectile.isKinematic = true;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane plane = new Plane(Vector3.up, Pivot.position);
        if (plane.Raycast(ray, out float distance))
        {
            Vector3 point = ray.GetPoint(distance);
            Vector3 dir = point - Pivot.position;
            if (dir.magnitude > MaxStretch)
                dir = dir.normalized * MaxStretch;

            CurrentProjectile.position = Pivot.position + dir;
        }
    }

    IEnumerator Release()
    {
        CurrentProjectilePrefab = null;
        OnShotFired.Invoke();
        Destroy(Joint);
        CurrentProjectile.isKinematic = false;

        FlightCamera.Follow = CurrentProjectile.transform;

        Vector3 forceDir = (Pivot.position - CurrentProjectile.position);
        float stretch = forceDir.magnitude / MaxStretch;
        float heightBoost = 0.5f + stretch * 2f;
        forceDir.y += heightBoost;

        float forceMag = forceDir.magnitude * PowerMultiplier;
        CurrentProjectile.AddForce(forceDir.normalized * forceMag, ForceMode.Impulse);

        CurrentProjectile = null;
        yield return new WaitForSeconds(WaitSecondsAfterShot);
        CreateProjectile();
    }

    void UpdateBands()
    {
        if (LeftBand && RightBand && CurrentProjectile)
        {
            LeftBand.enabled = true;
            RightBand.enabled = true;

            LeftBand.SetPosition(0, LeftAnchor.position);
            LeftBand.SetPosition(1, CurrentProjectile.position);

            RightBand.SetPosition(0, RightAnchor.position);
            RightBand.SetPosition(1, CurrentProjectile.position);

            float stretch = Vector3.Distance(CurrentProjectile.position, Pivot.position) / MaxStretch;
            float width = Mathf.Lerp(0.08f, 0.02f, stretch);
            LeftBand.startWidth = LeftBand.endWidth = width;
            RightBand.startWidth = RightBand.endWidth = width;
        }
        else
        {
            if (LeftBand) LeftBand.enabled = false;
            if (RightBand) RightBand.enabled = false;
        }
    }

    void UpdateFlightPath()
    {
        if (FlightPath == null || CurrentProjectile == null) return;

        FlightPath.enabled = true;
        FlightPath.positionCount = PathResolution;

        Vector3 startPos = CurrentProjectile.position;
        Vector3 forceDir = (Pivot.position - CurrentProjectile.position);
        float stretch = forceDir.magnitude / MaxStretch;
        float heightBoost = 0.5f + stretch * 2f;
        forceDir.y += heightBoost;
        float forceMag = forceDir.magnitude * PowerMultiplier;
        Vector3 initialVelocity = forceDir.normalized * forceMag / CurrentProjectile.mass;

        for (int i = 0; i < PathResolution; i++)
        {
            float t = i * PathTimeStep;
            Vector3 pos = startPos + initialVelocity * t + 0.5f * Physics.gravity * t * t;
            FlightPath.SetPosition(i, pos);
        }
    }
}