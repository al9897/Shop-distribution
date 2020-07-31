using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SliderManager : MonoBehaviour
{
	// Start is called before the first frame update
	public Slider slider;
    void Start()
    {
	
	}

    // Update is called once per frame
    void Update()
    {
		Debug.Log(slider.value + " Time");
	}
	public void OnValueChanged()
	{
		TimeTickSystem.ConversionRate = slider.value;
	}
}
