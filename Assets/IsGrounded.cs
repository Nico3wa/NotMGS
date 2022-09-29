using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IsGrounded : MonoBehaviour
{
    [SerializeField] Transform _raycastRoot;
    [SerializeField] Vector3 raycastDirection;


    // Update is called once per frame
    void Update()
    {
  
        if (Physics.Raycast(_raycastRoot.position, raycastDirection, out RaycastHit hit, raycastDirection.magnitude))
        {
            Debug.DrawLine(_raycastRoot.position , _raycastRoot.position + raycastDirection ,Color.magenta);
        }
        else
        {
            Debug.DrawLine(_raycastRoot.position, _raycastRoot.position + raycastDirection, Color.red);
        }
    }
}
