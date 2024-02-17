using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flammable : MonoBehaviour
{
    public GameObject trap;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        trap.GetComponent<TrapPetrolOil>().SetOnFire();
    }
}
