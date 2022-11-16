using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelativePosition : MonoBehaviour
{
    public float offsetX;

    [SerializeField]
    private Transform relativeTo;

    private void Update()
    {
        transform.position = new Vector3(relativeTo.position.x + offsetX, transform.position.y, transform.position.z);
    }
}
