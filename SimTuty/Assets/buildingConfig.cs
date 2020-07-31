using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class buildingConfig : MonoBehaviour

{
	// Start is called before the first frame update
	public GameObject panel, button,scene;
	public InputField ip,ip1,ip2,ip3,ip4,ip5;//Inputfield|ip:status,ip1:address|ip:Number of products in warehouse
	void Start()
	{
		

	}

	// Update is called once per frame
	void Awake(){
		
	}
	public void closePanel()
	{
		panel.SetActive(false);
		panel.SetActive(false);
		scene.SetActive(true);
		//close panel and save data
		//
		if (BuildingManager.instance.getObjectBuilding() is Shop)
		{
			((Shop)BuildingManager.instance.getObjectBuilding()).Status = (ip.text =="")?" ":ip.text;
			((Shop)BuildingManager.instance.getObjectBuilding()).Address = (ip1.text == "") ? " " : ip1.text;
			((Shop)BuildingManager.instance.getObjectBuilding()).ExpectedSellingPerDay = (ip2.text=="") ? 0 : float.Parse(ip2.text);
			((Shop)BuildingManager.instance.getObjectBuilding()).Capacity = (ip3.text == "") ? 0 : int.Parse(ip3.text);
			((Shop)BuildingManager.instance.getObjectBuilding()).Inventory = (ip4.text == "") ? 0 : int.Parse(ip4.text);
			((Shop)BuildingManager.instance.getObjectBuilding()).Threshold = (ip5.text == "") ? 0 : int.Parse(ip5.text);

			ip1.text = "";
			ip2.text = "";
			ip3.text = "";
			ip4.text = "";
			ip5.text = "";
			ip.text = "";
		}
		else if (BuildingManager.instance.getObjectBuilding() is Warehouse)
		{
			try
			{

				//NrOfBigTruck
				int bigTruck = (ip1.text == "") ? 0 : int.Parse(ip1.text); 
				((Warehouse)BuildingManager.instance.getObjectBuilding()).NrOfBigTruck = bigTruck;
				//NrOfSmallTruck
				int smallTruck = (ip2.text == "") ? 0 : int.Parse(ip2.text);
				((Warehouse)BuildingManager.instance.getObjectBuilding()).NrOfSmallTruck = smallTruck;
				//Name
				string name = (ip.text == "") ? " " : ip.text;
				((Warehouse)BuildingManager.instance.getObjectBuilding()).Name = name;

				ip1.text = "";
				ip2.text = "";
				ip.text = "";

			}
			catch (Exception)
			{
				Debug.Log("Wrong type");
			}

			//((Warehouse)BuildingManager.instance.getObjectBuilding()).Address = ip1.text;
		}
	

	
		
	}
	public void InputChanged()	{
		
		//Get value from input field and assign to object.

	

	
	}
}










/*foreach (Building bd in BuildingManager.instance.ListBuildings)	{

			Tuple<int, int> l = new Tuple<int, int>((int)BuildingManager.instance.temp.First().WorldCoord.x, (int)BuildingManager.instance.temp.First().WorldCoord.y + 5);
// Lower Right Corner
Tuple<int, int> r = new Tuple<int, int>((int)BuildingManager.instance.temp.First().WorldCoord.x + 5, (int)BuildingManager.instance.temp.First().WorldCoord.y);
// Upper Left Corner
Tuple<int, int> bl = new Tuple<int, int>((int)bd.WorldCoord.x, (int)bd.WorldCoord.y + bd.Dimension.Item2);
// Lower Right Corner
Tuple<int, int> br = new Tuple<int, int>((int)bd.WorldCoord.x + bd.Dimension.Item1, (int)bd.WorldCoord.y);

			if (BuildingManager.instance.IsOverlapped(l, r, bl, br)){
				isOverLoaded = true;
				if (bd is Shop)
				{
					((Shop) bd).Status = ip.text;
					((Shop) bd).Address = ip1.text;
					break;
				}
				else
				{
					//((Warehouse)bd).NrOfStock = Convert.ToInt32(ip3.text);

				}
				
			}

		}
		foreach (Building bd in BuildingManager.instance.ListBuildings){
			Debug.Log(((Shop) bd).ToString());
*/		