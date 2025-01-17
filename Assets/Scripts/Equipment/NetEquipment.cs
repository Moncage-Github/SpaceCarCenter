using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetEquipment : BaseEquipment
{
    private VehicleInventory _inventory;
    [SerializeField] float _netSize;
    [SerializeField] float _netInterval
;
    // Start is called before the first frame update
    void Start()
    {
        _inventory = Vehicle.GetComponent<VehicleInventory>();
        BoxCollider2D col = transform.GetComponent<BoxCollider2D>();
        col.size = new Vector2(_netSize, 1f);
        col.offset = new Vector2(0, -1 * _netInterval);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("item"))
        {
            Debug.Log("그물 걸림");
            _inventory.AddItemToInventory(collision.GetComponent<CollectableItem>().ItemCode);
            Destroy(collision.gameObject);
        }
    }
}
