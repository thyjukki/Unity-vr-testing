using VRTK;
using UnityEngine;

public class Slide_Interactable : VRTK_InteractableObject {
    private Vector3 restPosition;
    private float fireTimer = 0f;

    public float boltSpeed;
    public float travelDistance;
    public float slideStop;
    public float unchamberDistance;
    public float loadDistance;

    public Gun_Base gun;

    private bool unchambered;
    private bool loading;

    public float slidePos {
        get {
            return transform.localPosition.x - restPosition.x;
        }
    }

    public bool SlideLocked {
        get;

        private set;
    }

    public void Fire() {
        fireTimer = travelDistance;
    }

    protected override void Awake() {
        base.Awake();
        restPosition = transform.localPosition;
    }

    protected override void LateUpdate() {

        base.LateUpdate();
        float newPos = transform.localPosition.x;


        // return slide back
        if (fireTimer <= 0 && slidePos > 0 && !IsGrabbed()) {
            
            if (!(SlideLocked && gun.SlideStopped && slidePos == slideStop)) {
                newPos = transform.localPosition.x - boltSpeed * Time.deltaTime;
                
                if (slidePos >= slideStop && (newPos - restPosition.x) <= slideStop && gun.SlideStopped) {
                    newPos = restPosition.x + slideStop;
                    SlideLocked = true;
                } else {
                    SlideLocked = false;
                }
            }
        }

        //Fire
        if (fireTimer > 0) {
            float step = boltSpeed * Time.deltaTime;
            newPos = transform.localPosition.x + step;
            fireTimer -= step;
        }

        if (fireTimer < 0) {
            fireTimer = 0;
        }

        //make sure slide does not slide off

        float startLimit = 0.0f;

        if (SlideLocked) {
            startLimit = slideStop;
        }
        if (slidePos < startLimit) {
            newPos = restPosition.x + startLimit;
        } else if (slidePos > travelDistance) {
            newPos = restPosition.x + travelDistance;
        }

        transform.localPosition = new Vector3(newPos, restPosition.y, restPosition.z);

    }


    public bool CanFire {
        get {
            return (transform.localPosition.x == restPosition.x) && !IsGrabbed();
        }
    }
}

