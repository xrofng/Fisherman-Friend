using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingObjBoomerang : MovingObject {

    public float normalTime = 5.0f;
    public float reachDistance = 2.0f;
    public float rotateSpeed = 2;

    protected override void Start()
    {
        Initialization();
    }

    protected override void Initialization()
    {
        base.Initialization();
    }

    protected override void Update()
    {
        Vector3 moveDirection = direction;
        normalTime -= Time.deltaTime;
        if (normalTime <= 0)
        {
            moveDirection = Vector3.Normalize(HitBox.Owner.gameObject.transform.position - transform.position);
            MoveEnd = CheckEnd();
        }
        transform.Rotate(Vector3.up * rotateSpeed, Space.Self);
        transform.Translate(moveDirection * speed,Space.World);
    }

    protected override bool CheckEnd()
    {
        float distance = Vector3.Distance(HitBox.Owner.gameObject.transform.position, transform.position);
        //transform.LookAt(targetPos, Vector3.up);
        if (distance <= reachDistance)
        {
            Destroy(this.gameObject);
            return true;
        }
        return false;
    }
}
