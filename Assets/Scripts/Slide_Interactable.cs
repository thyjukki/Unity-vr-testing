using VRTK;
using UnityEngine;

[RequireComponent(typeof(VRTK.GrabAttachMechanics.VRTK_TrackObjectGrabAttach), typeof(VRTK.SecondaryControllerGrabActions.VRTK_SwapControllerGrabAction))]
public class Slide_Interactable : VRTK_InteractableObject {
    private Vector3 restPosition;
    private float fireTimer = 0f;

    public enum SlideDir {
        X,
        Y,
        Z
    }

    public SlideDir direction;
    public bool flipeSide;

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
            return position - rest;
        }
    }

    float rest {
        get {
            switch (direction) {
                case SlideDir.X:
                    return restPosition.x;
                case SlideDir.Y:
                    return restPosition.y;
                case SlideDir.Z:
                    return restPosition.z;
            }

            return 0;
        }
    }

    float position {
        get {
            switch (direction) {
                case SlideDir.X:
                    return transform.localPosition.x;
                case SlideDir.Y:
                    return transform.localPosition.y;
                case SlideDir.Z:
                    return transform.localPosition.z;
            }

            return 0;
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

    protected virtual void Start() {
        disableWhenIdle = false;

        isGrabbable = true;
        holdButtonToGrab = true;
        stayGrabbedOnTeleport = true;
        validDrop = ValidDropTypes.Drop_Anywhere;

        grabOverrideButton = VRTK_ControllerEvents.ButtonAlias.Trigger_Press;

        grabAttachMechanicScript = GetComponent<VRTK.GrabAttachMechanics.VRTK_TrackObjectGrabAttach>();
        secondaryGrabActionScript = GetComponent<VRTK.SecondaryControllerGrabActions.VRTK_SwapControllerGrabAction>();

        isUsable = false;
        holdButtonToUse = false;
        useOnlyIfGrabbed = false;
    }

    protected override void LateUpdate() {

        base.LateUpdate();
        float newPos = position;


        // return slide back
        if (fireTimer <= 0 && slidePos > 0 && !IsGrabbed()) {
            
            if (!(SlideLocked && gun.SlideStopped && slidePos == slideStop)) {
                newPos = position - boltSpeed * Time.deltaTime;
                
                if (slidePos >= slideStop && (newPos - rest) <= slideStop && gun.SlideStopped) {
                    newPos = rest + slideStop;
                    SlideLocked = true;
                } else {
                    SlideLocked = false;
                }
            }
        }

        //Fire
        if (fireTimer > 0) {
            float step = boltSpeed * Time.deltaTime;
            newPos = position + step;
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
            newPos = rest + startLimit;
        } else if (slidePos > travelDistance) {
            newPos = rest + travelDistance;
        }

        Vector3 pos = Vector3.zero;
        if (flipeSide) {
            newPos = -newPos;
        }
        switch (direction) {
            case SlideDir.X:
                pos = new Vector3(newPos, restPosition.y, restPosition.z);
                break;
            case SlideDir.Y:
                pos = new Vector3(restPosition.x, newPos, restPosition.z);
                break;
            case SlideDir.Z:
                pos = new Vector3(restPosition.x, restPosition.y, newPos);
                break;
        }
        transform.localPosition = pos;

    }


    public bool CanFire {
        get {
            return (position == rest) && !IsGrabbed();
        }
    }
}

