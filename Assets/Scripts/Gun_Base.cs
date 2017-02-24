using System;
using UnityEngine;
using VRTK;
using VRTK.GrabAttachMechanics;
using VRTK.SecondaryControllerGrabActions;


[RequireComponent(typeof(VRTK_ChildOfControllerGrabAttach), typeof(VRTK_SwapControllerGrabAction))]
public class Gun_Base : VRTK_InteractableObject {
    public float bulletSpeed;

    //private GameObject bullet;
    public Slide_Interactable slide;
    public VRTK_SnapDropZone magWell;

    protected Magazine_Interactable attachedMag;

    public Transform trigger;

    protected Vector3 triggerRestPos;
    
    public GameObject muzzle;
    public GameObject ejectionPort;

    public GameObject chamberedBullet;
    public GameObject bulletPrefab;
    public GameObject shellPrefab;


    public bool fullAuto;
    public bool magRelease;
    public bool slideRelease;

    protected VRTK_ControllerEvents controllerEvents;



    private bool fired;


    private void ToggleSlide(bool state) {
        if (!state) {
            slide.ForceStopInteracting();
        }
        slide.isGrabbable = state;
        slide.enabled = true;
    }

    protected bool slideStopped;
    public virtual bool SlideStopped {
        get {
            return slideStopped;
        }

        protected set {
            slideStopped = value;
        }
    }

    public override void Grabbed(GameObject currentGrabbingObject) {
        base.Grabbed(currentGrabbingObject);

        controllerEvents = currentGrabbingObject.GetComponent<VRTK_ControllerEvents>();

        ToggleSlide(true);

        //Limit hands grabbing when picked up
        if (VRTK_DeviceFinder.GetControllerHand(currentGrabbingObject) == SDK_BaseController.ControllerHand.Left) {
            allowedTouchControllers = AllowedController.Left_Only;
            allowedUseControllers = AllowedController.Left_Only;

            slide.allowedGrabControllers = AllowedController.Right_Only;
        } else if (VRTK_DeviceFinder.GetControllerHand(currentGrabbingObject) == SDK_BaseController.ControllerHand.Right) {
            allowedTouchControllers = AllowedController.Right_Only;
            allowedUseControllers = AllowedController.Right_Only;

            slide.allowedGrabControllers = AllowedController.Left_Only;
        }
    }

    public override void Ungrabbed(GameObject previousGrabbingObject) {
        base.Ungrabbed(previousGrabbingObject);

        ToggleSlide(false);

        //Unlimit hands
        allowedTouchControllers = AllowedController.Both;
        allowedUseControllers = AllowedController.Both;
        slide.allowedGrabControllers = AllowedController.Both;

        controllerEvents = null;
    }

    public void LoadMagazine(Magazine_Interactable mag) {
        magWell.ForceSnap(mag.gameObject);
        attachedMag = mag;

        Rigidbody magBody = mag.GetComponent<Rigidbody>();
        Collider magCol = mag.GetComponent<Collider>();
        magBody.useGravity = false;
        magBody.isKinematic = true;
        magCol.isTrigger = true;

        mag.isGrabbable = true;
    }

    public void UnLoadMagazine() {

        if (attachedMag) {
            magWell.ForceUnsnap();

            Rigidbody magBody = attachedMag.GetComponent<Rigidbody>();
            Collider magCol = attachedMag.GetComponent<Collider>();
            magBody.useGravity = true;
            magBody.isKinematic = false;
            magCol.isTrigger = false;

            attachedMag.isGrabbable = true;

            attachedMag = null;
        }
    }

    public void OnObjectEnteredSnapDropZone(object sender, SnapDropZoneEventArgs e) {
        Magazine_Interactable mag = e.snappedObject.GetComponent<Magazine_Interactable>();

        if (!mag) {
            mag = e.snappedObject.GetComponentInChildren<Magazine_Interactable>();
        }
        if (!mag) {
            mag = e.snappedObject.GetComponentInParent<Magazine_Interactable>();
        }

        if (!mag) {
            return;
        }


        LoadMagazine(mag);
    }

    protected override void Awake() {
        base.Awake();

    }

    protected virtual void Start() {
        magWell.ObjectEnteredSnapDropZone += OnObjectEnteredSnapDropZone;
        triggerRestPos = trigger.localPosition;


        //Set VRTK_InteractableObject values which will be same for all guns
        disableWhenIdle = false;
#if UNITY_EDITOR
        touchHighlightColor = Color.red;//FIXME(Jukki) remove this
#endif
        allowedTouchControllers = AllowedController.Both;

        isGrabbable = true;
        holdButtonToGrab = false;
        stayGrabbedOnTeleport = true;
        validDrop = ValidDropTypes.Drop_Anywhere;
        grabOverrideButton = VRTK_ControllerEvents.ButtonAlias.Grip_Press;
        allowedGrabControllers = AllowedController.Both;
        grabAttachMechanicScript = GetComponent<VRTK_ChildOfControllerGrabAttach>();
        secondaryGrabActionScript = GetComponent<VRTK_SwapControllerGrabAction>();

        isUsable = false;
        holdButtonToUse = false;
        pointerActivatesUseAction = false;

    }

    protected virtual void InputHandle() {


        if (controllerEvents) {
            trigger.localPosition = new Vector3(triggerRestPos.x + (0.07f * controllerEvents.GetTriggerAxis()), triggerRestPos.y, triggerRestPos.z);

            if (controllerEvents.touchpadPressed) {
                float angle = controllerEvents.GetTouchpadAxisAngle();
                if (angle < 45 || angle > 315) {
                    SlideStopped = false;
                } else if (angle < 225 && angle > 135) {
                    if (magRelease) {
                        UnLoadMagazine();
                    }
                }
            }


            if (controllerEvents.triggerClicked && (!fired || fullAuto)) {
                FireBullet();
                fired = true;
            }

            if (fired && !controllerEvents.triggerClicked) {
                fired = false;
            }
        } else {
            trigger.localPosition = triggerRestPos;
        }
    }

    protected override void Update() {
        base.Update();


        

        InputHandle();
    }

    public virtual bool canFire { get { return true; } }

    public virtual void FireBullet() {

        GameObject bullet = Instantiate(bulletPrefab, muzzle.transform.position, muzzle.transform.rotation) as GameObject;

        bullet.GetComponent<Bullet>().firedFrom = gameObject;

        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(bullet.transform.forward * bulletSpeed);


        GameObject shell = Instantiate(shellPrefab, ejectionPort.transform.position, ejectionPort.transform.rotation) as GameObject;

        Rigidbody shellRb = shell.GetComponent<Rigidbody>();
        shellRb.AddForce(shell.transform.forward * 2 + shell.transform.right * 10 + shell.transform.up * 10);

        var colliders = GetComponentsInChildren<Collider>();

        foreach (var col in colliders) {
            Physics.IgnoreCollision(shell.GetComponent<Collider>(), col);
            Physics.IgnoreCollision(bullet.GetComponent<Collider>(), col);
        }
        Destroy(shell, 5f);
        Destroy(bullet, 10f);
    }
}