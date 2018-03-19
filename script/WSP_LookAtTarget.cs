using UnityEngine;
using System.Collections;

public class WSP_LookAtTarget : MonoBehaviour
{
    GameObject Capsule;
    private Transform myTransform;
    public Transform TargetTransform;

    void Start()
    {
        Capsule = GameObject.Find("Capsule");
        myTransform = gameObject.transform;
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(Capsule.transform.position, Vector3.up, 0.2f);
        //Debug.Log("LookatTarget");
        //if (TargetTransform != null)
        //{
        //    myTransform.LookAt(TargetTransform.position);
        //}
    }
}

