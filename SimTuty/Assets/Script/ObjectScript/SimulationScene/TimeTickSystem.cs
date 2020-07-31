using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;


public delegate void TimeTickHandler(float timeDelta);

public class TimeTickSystem : MonoBehaviour
{
    // pub-sub pattern
    // To subscribe: 
    // TimeTickSystem.onTick += new TimeTickHandler(someFunction(float));
    public static event TimeTickHandler onTick;
    public Text textTime;
    public Text shText;
    public Text textTotal;
    List<Building> b;
    public static float CurrentSimTime { get; private set; }
    public static float TimeInThisDay
    {
        get  {
            var rt = ToRealTime();
            float TimeInDay = CurrentSimTime - rt.Day * 24f * 60f * 60f;
            return TimeInDay;
        }
    }

    // Conversion rate: 1 real hour = <rate> sim-second
    // 3600 real seconds = <rate> sim-second
    // 3600/<rate> real seconds = 1 sim-second
    public static float ConversionRate { get; set; }


    /* Display time */
    public class RealTime
    {
        public int Day { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
    }

    private void Awake()
    {
        CurrentSimTime = 0;
        ConversionRate = 1;
    }

    // Update is called once per frame
    void Update()
    {
        float timeChange = Time.deltaTime;
        CurrentSimTime += timeChange;

        RealTime realTime = ToRealTime();

        if (onTick != null)
            onTick.Invoke(timeChange);
        shText.text = "";
        textTime.text = $"Day: { realTime.Day}, " + $"Hour: {realTime.Hour}, " +$"Minute: {realTime.Minute}";
        int nrrunningtruck=0;
        foreach (Building b in BuildingManager.instance.ListBuildings)
        {
            shText.text += b.ToString() + "\n";
            if(b is Warehouse)
            {
                nrrunningtruck += ((Warehouse)b).NrOfTruckRunning;
            }
        }
        textTotal.text = "Total Revenue: " + BuildingManager.instance.Revenue +"\n" + "Total Number of Goods Sold: "+ BuildingManager.instance.GoodsSold + "\n" + "Number of Running Trucks:" + nrrunningtruck;
        
        
       
        

        //Debug.Log($"Sim time: {currentSimTime}, " +
        //    $"Real time: {realTime}");

        //Debug.Log($"Day: {realTime.Day}, " +
        //            $"Hour: {realTime.Hour}, " +
        //            $"Minute: {realTime.Minute}");
    }

    public static RealTime ToRealTime()
    {                          
        RealTime res = new RealTime();

        float realTime = (CurrentSimTime / (1f * ConversionRate)) * 3600f;

        // to Day
        res.Day = (int)(realTime / (1f * 60 * 60 * 24));
        realTime -= 1f * res.Day * 60 * 60 * 24;

        // To Hour
        res.Hour = (int)(realTime / (1f * 60 * 60));
        realTime -= 1f * res.Hour * 60 * 60;

        // To Minute
        res.Minute = (int)(realTime / (1f * 60));

        return res;
    }
    public static float ToSimTime(float realTime)
    {
        return realTime * ConversionRate / 3600f;
    }
	
}
