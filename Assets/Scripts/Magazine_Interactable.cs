using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class Magazine_Interactable : VRTK_InteractableObject {
    

    public int MaxAmmo;

    [SerializeField]
    private int currentAmmo;
    public int CurrentAmmo {
        get {
            return currentAmmo;
        }

        set {
            currentAmmo = value;

            if (Bullets.Count > CurrentAmmo) {
                int count = Bullets.Count - currentAmmo;

                int i = 0;
                foreach (var bullet in Bullets) {
                    if (i < count) {
                        bullet.SetActive(false);
                    } else {
                        bullet.SetActive(true);
                    }
                    i++;
                }
            }
        }
    }

    public List<GameObject> Bullets;

    public bool IsEmpty {
        get {
            return CurrentAmmo == 0;
        }
    }
}
