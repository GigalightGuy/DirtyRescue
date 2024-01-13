using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapFishnet : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Player.Instance.Root(2);
    }
}
