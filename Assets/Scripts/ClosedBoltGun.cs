using System;
using UnityEngine;
using VRTK;

public class ClosedBoltGun : Gun_Base {
    public int test;

    private int chamberedRounds;
    public int ChamberedRounds {
        get {
            return chamberedRounds;
        }
        set {
            chamberedRounds = value;

            chamberedBullet.SetActive(chamberedRounds > 0);
        }
    }

    private bool unchambered;
    private bool loading;

    public void Chamber() {

        if (attachedMag && attachedMag.CurrentAmmo > 0) {
            ChamberedRounds++;
            attachedMag.CurrentAmmo--;
        }
    }

    public void Unchamber() {
        if (ChamberedRounds > 0) {
            ChamberedRounds--;
        }
    }

    protected override void Update() {

        //Update slidestop state
        if (attachedMag && attachedMag.IsEmpty) {
            SlideStopped = true;
        } else if (SlideStopped) {
            SlideStopped = slide.SlideLocked;
        } else {
            SlideStopped = false;
        }

        base.Update();

        //Debug.Log(SlideStopped);
    }

    protected override void LateUpdate() {
        base.LateUpdate();

        
        if (slide.slidePos > slide.unchamberDistance && !unchambered && IsGrabbed()) {
            unchambered = true;
            Unchamber();
        } else if (slide.slidePos < slide.unchamberDistance && unchambered) {
            unchambered = false;
        }



        if (slide.slidePos > slide.loadDistance && !loading) {
            loading = true;
        } else if (slide.slidePos < slide.loadDistance && loading) {
            loading = false;
            Chamber();
        }
    }
    public override bool canFire { get { return chamberedRounds > 0 && slide.CanFire; } }

    public override void FireBullet() {
        if (canFire) {
            base.FireBullet();

            chamberedRounds--;
            slide.Fire();
        }
    }
}