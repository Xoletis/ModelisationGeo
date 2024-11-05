using UnityEngine;

public class Remove : MonoBehaviour
{

    public int potentiel = 255;
    MeshRenderer ren;
    public Material vert, rouge, jaune, orange;

    private void Start()
    {
        int n = Random.Range(1, 255);
        potentiel = n;
        ren = GetComponent<MeshRenderer>();
        UpdateVisibility();
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
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
        else
        {
            gameObject.GetComponent<MeshRenderer>().enabled = true;
        }


        if (potentiel < 64)
        {
            ren.material = rouge;
        }
        else if (potentiel < 128)
        {
            ren.material = orange;
        }
        else if (potentiel < 192)
        {
            ren.material = jaune;
        }
        else
        {
            ren.material = vert;
        }
    }
}
