using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class tdAnimationEventHandler : MonoBehaviour
{
    tdEntity _entity;
    //public bgEntityEvent Callback;
    private void Awake() {
        _entity = this.GetComponent<tdEntity>();
        if (_entity == null)
            _entity = this.GetComponentInParent<tdEntity>();
    }

    public void OnAnimationEvent(tdMessageType callbackevent) {
        //Callback(callbackevent);
        _entity.SendMessageToBrain(callbackevent);
    }

    public void OnEffectsEvent(tdFX fx) {
        tdDataManager.Instance.AttackFx(fx, _entity);
    }

    //public void AttackFx(int comboNb) {
    //    GameObject fx = Instantiate(AtkFx[comboNb].Prefab);
    //    Vector3 posOffset;
    //    Quaternion rotOffset;
    //    if (_movement.IsFacingRight) {
    //        posOffset = AtkFx[comboNb].PosRightOffset;
    //        rotOffset = Quaternion.Euler(AtkFx[comboNb].RotRightOffset);
    //    } else {
    //        posOffset = AtkFx[comboNb].PosLeftOffset;
    //        rotOffset = Quaternion.Euler(AtkFx[comboNb].RotLeftOffset);
    //    }
    //    fx.transform.position = this.transform.position + posOffset;
    //    fx.transform.rotation = this.transform.rotation * rotOffset;
    //}
}
