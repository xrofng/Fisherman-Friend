using DG.Tweening;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class MovingObjToTarget : MovingObject
{
    public float rotationSpeed = 5;
    public float initialWait = 1;
    public bool initial = true;

    public Transform target;
    public GameObject hitParticle;
    private float multiplier = 1;

    public float projectileAutoDestroy = 5;

    public Transform Pivot;
    public Transform Trail;

    private List<GameObject> _hitEnemies = new List<GameObject>();

    // Start is called before the first frame update
    protected override void Start()
    {
        multiplier = Random.Range(1, 3);
        StartCoroutine(InitialWait());
        Destroy(gameObject, projectileAutoDestroy);
    }

    // Update is called once per frame
    protected override void Update()
    {
        if (initial)
        {
            transform.eulerAngles += Vector3.forward * Time.deltaTime * (rotationSpeed / 2);
            transform.position += transform.up * Time.deltaTime * (speed / 8f);
        }
        else
        {
            if (target == null)
            {
                return;
            }
            var targetRotation = Quaternion.LookRotation(target.transform.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, .5f);
            //transform.LookAt(target.position);
            transform.position += transform.forward * Time.deltaTime * (speed * multiplier);
            Pivot.eulerAngles += Vector3.forward * Time.deltaTime * rotationSpeed * 1.5f;
        }
        if (_hitEnemies.Count > 0)
        {
            foreach (GameObject hit in _hitEnemies)
            {
                Destroy(hit.gameObject);
            }
            _hitEnemies.Clear();
        }
    }

    IEnumerator InitialWait()
    {
        yield return new WaitForSeconds(initialWait / 2);
        Trail.DOLocalMoveY(1, .2f);
        yield return new WaitForSeconds(initialWait / 2);
        DOVirtual.Float(rotationSpeed, rotationSpeed * 1.5f, .3f, SetRotationSpeed);
        initial = false;
    }

    private void SetRotationSpeed(float x)
    {
        rotationSpeed = x;
    }

}
