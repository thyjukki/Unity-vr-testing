

public class OpenBoltGun : Gun_Base {

    public override bool SlideStopped {
        get {
            return controllerEvents && !controllerEvents.triggerClicked;
        }
    }

    private bool loading;
    private bool loaded;
    protected override void LateUpdate() {
        base.LateUpdate();

        if (slide.slidePos > slide.loadDistance && !loading) {
            loading = true;
        } else if (slide.slidePos < slide.loadDistance && loading) {
            loading = false;
            if (attachedMag && attachedMag.CurrentAmmo > 0) {
                loaded = true;
            }
        }
    }

    public override bool canFire { get { return slide.CanFire; } }

    public override void FireBullet() {
        if (attachedMag && canFire && loaded && attachedMag.CurrentAmmo > 0) {
            attachedMag.CurrentAmmo--;
            base.FireBullet();
            loaded = false;
            slide.Fire();
        }
    }
}
