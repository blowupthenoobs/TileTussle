using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileItemScript : MonoBehaviour
{
    [SerializeField] int hp = 1;
    
    public void AttackItem()
    {
        hp--;

        if(hp <= 0)
            DestroyItem();
    }

    public void DestroyItem()
    {
        transform.parent.gameObject.GetComponent<TileScript>().RelinquishItem();
        Destroy(gameObject);
    }
}
