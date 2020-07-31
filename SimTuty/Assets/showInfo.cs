using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class showInfo : MonoBehaviour
{
	public Text text1, text2, text3, text4, text5, text6;

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		if (BuildingManager.instance.temp[0] != null)
		{
			if (BuildingManager.instance.getObjectBuilding() is Shop)
			{
				try
				{
					if (((Shop)BuildingManager.instance.getObjectBuilding()).Address != "" || ((Shop)BuildingManager.instance.getObjectBuilding()).Status != "")
					{
						text1.text = ((Shop)BuildingManager.instance.getObjectBuilding()).Status;
						text2.text = ((Shop)BuildingManager.instance.getObjectBuilding()).Address;
						text3.text = Convert.ToString(((Shop)BuildingManager.instance.getObjectBuilding()).ExpectedSellingPerDay);
						text4.text = Convert.ToString(((Shop)BuildingManager.instance.getObjectBuilding()).Capacity);
						text5.text = Convert.ToString(((Shop)BuildingManager.instance.getObjectBuilding()).Inventory);
						text6.text = Convert.ToString(((Shop)BuildingManager.instance.getObjectBuilding()).Threshold);
					}
					else
					{
						text1.text = "";
						text2.text = "";
					}
				}
				catch (Exception) { };

			}
			else if (BuildingManager.instance.getObjectBuilding() is Warehouse)
			{
				try
				{
					text2.text = ((Warehouse)BuildingManager.instance.getObjectBuilding()).Name + "";
					text3.text = ((Warehouse)BuildingManager.instance.getObjectBuilding()).NrOfBigTruck + "";
					text4.text = ((Warehouse)BuildingManager.instance.getObjectBuilding()).NrOfSmallTruck + "";
				}
				catch (Exception)
				{
					Debug.Log("Wrong type");
				}

			}
		}
	}
}
