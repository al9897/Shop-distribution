using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class TruckScript : MonoBehaviour
{
    int counter = 0;
    public GameObject truck;
    public List<Vector3Int> path;
    public Shop shop;
    public int amount;
    public Warehouse warehouse;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        int mod = 5;
        if (counter % mod == 0)
        {
            int index = (int)(counter / mod);

            if (index == path.Count)
            {
                index = 0;
                //counter = 0;

                warehouse.NrOfTruckRunning--;
                Destroy(truck, 0);
                warehouse.NotifyAvailable();
                
            }

            // Truck arrives at the shop
            if (index == path.Count / 2)
            {
                shop.WaitingForDelivery = false;
                shop.Inventory += amount;
            }

            if(truck!=null)
            {
                truck.transform.position = new Vector3(path[index].x + 0.5f,
                                                   path[index].y + 0.5f);

                if (index > 0)
                {
                    Vector3 dir = new Vector3(path[index].x - path[index - 1].x,
                                              path[index].y - path[index - 1].y);
                    var rot = truck.transform.rotation.eulerAngles;
                    rot = Utility.GetRotation(dir, rot);

                    truck.transform.rotation = Quaternion.Euler(rot);
                }
            }
            

            //Debug.Log(index);
            //Debug.Log(truck.transform.rotation);
        }
        counter++;
    }
}
