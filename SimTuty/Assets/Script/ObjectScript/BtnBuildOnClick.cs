using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BtnBuildOnClick : MonoBehaviour
{
	public GameObject Panel;
	//
    bool isPlacing=false;

	public GameObject ButtonHolder;

	public string inputType;
	public Type BuildingType;

	Button button;

	public GameObject PlacementManagerObject;
	public GameObject BuildingTilemap;

	void Start()
	{
		//button = ButtonHolder.GetComponent<Button>();
		//button.onClick.AddListener(() => { onClickHandler(); });

		BuildingType = inputType.ToLower() == "shop" ? typeof(Shop) : typeof(Warehouse);
	}
	
	public void OpenPanel()
	{					
		Panel.SetActive(true);
		Time.timeScale = 0.0f;
		Panel.layer = 3;
	}
	public void ClosePanel()
	{
		Panel.SetActive(false);
	}

	public void onClickHandler()
    {		
        if (isPlacing)
			return;
		
        var tmp = Instantiate(PlacementManagerObject);
        tmp.name = PlacementManagerObject.name;

		var tmpClass = tmp.GetComponent<BuildingPlacementManager>();
		tmpClass.grid = GameObject.Find("MainGrid").GetComponent<Grid>();
		tmpClass.BuildingTilemap = BuildingTilemap;
		tmpClass.DonePlacingEventHandler += new DonePlacingHandler(FinishPlacing);
		tmpClass.BuildingType = BuildingType;

		isPlacing = true;
	}
	
	public void FinishPlacing()
	{
		isPlacing = false;
	}
}
