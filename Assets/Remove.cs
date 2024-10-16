using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Remove : MonoBehaviour
{

    public int potentiel = 255;

    private void Start()
    {
        potentiel = 255;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Gomme"))
        {
            moveSphere sph = other.GetComponent<moveSphere>();
            if (sph.earase)
            {
                potentiel -= sph.force;
            }
            else
            {
                potentiel += sph.force;
            }
            UpdateVisibility();
        }
    }

    private void UpdateVisibility()
    {
        if (potentiel < SphereVolume.Instance.visiblePotentiel) {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}
