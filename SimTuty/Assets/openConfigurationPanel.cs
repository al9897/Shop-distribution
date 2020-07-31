using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class openConfigurationPanel : MonoBehaviour
{
	public GameObject sence, panelConfigureShop,panelConfigureWarehouse;
	public InputField ip;
	List<Building> lstBuilding;

	// Start is called before the first frame update
	void Start()
    {

	}
    // Update is called once per frame
    void Update()
    {
		lstBuilding = BuildingManager.instance.ListBuildings;
		if (Input.GetMouseButtonDown(0))
		{
			Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			var mousePos = new Vector3Int(Mathf.FloorToInt(point.x), Mathf.FloorToInt(point.y), 0);
			if (lstBuilding != null)
			{

				Tuple<int, int> l = new Tuple<int, int>((int)mousePos.x,
											  (int)mousePos.y + 5);
				// Lower Right Corner
				Tuple<int, int> r = new Tuple<int, int>((int)mousePos.x + 5,
														(int)mousePos.y);
				foreach (var build in lstBuilding)
				{			
					// Upper Left Corner
					Tuple<int, int> bl = new Tuple<int, int>((int)build.WorldCoord.x,
															 (int)build.WorldCoord.y + 5);
					// Lower Right Corner
					Tuple<int, int> br = new Tuple<int, int>((int)build.WorldCoord.x + 5,
															 (int)build.WorldCoord.y);
					if (BuildingManager.instance.IsOverlapped(l, r, bl, br))
					{

						BuildingManager.instance.temp[0] = build;
						//Debug.Log(BuildingManager.instance.temp[0].WorldCoord);
							
						//problem:cannot assign value from inputfield to the object yet

						sence.SetActive(false);
						if (build is Shop)
						{											
							panelConfigureShop.SetActive(true);
						}
						else if (build is Warehouse)
						{							
							panelConfigureWarehouse.SetActive(true);
							
						}
						
					}

				}
			}
		}
		else if (Input.GetKeyDown(KeyCode.Escape))
		{
			panelConfigureShop.SetActive(false);
			panelConfigureWarehouse.SetActive(false);
			sence.SetActive(true);
		}
	}
	
	

}
