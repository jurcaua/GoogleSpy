using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipGun : MonoBehaviour {

    // when gun object collides with the player, set to right position and enable ability to shoot for player
    void OnTriggerEnter(Collider c) {
        if (LayerMask.LayerToName(c.gameObject.layer).Equals("Player")) {

            GetComponent<Rotater>().enabled = false;

            PlayerMovement pm =  c.gameObject.GetComponent<PlayerMovement>();
            pm.canShoot = true;
            for (int i = 0; i < pm.transform.childCount; i++) {
                if (pm.transform.GetChild(i).name == "GunArm") {
                    transform.parent = pm.transform.GetChild(i);
                    transform.localPosition = new Vector3(0.103999f, -0.224f, -0.2169999f);
                    transform.localRotation = Quaternion.Euler(0f, 90f, 0f);
                }
            }
        }
    }
}
