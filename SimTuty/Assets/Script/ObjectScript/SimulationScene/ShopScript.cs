using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ShopScript : MonoBehaviour
{
    public GameObject ShopInventoryBar;
    public Shop ThisShop { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        if (SceneManager.GetActiveScene().name != "SimulationScene")
            return;
        var iBar = Instantiate(ShopInventoryBar, this.gameObject.transform);
        Vector3 renderCoord = ThisShop.WorldCoord;
        renderCoord.y--;
        renderCoord.x += 2.5f;
        iBar.transform.position = renderCoord;
        
        ShopInventoryBar = iBar;
    }

    // Update is called once per frame
    void Update()
    {
        if (SceneManager.GetActiveScene().name != "SimulationScene")
            return;
        // Update Inventory Bar
        float percentInventory;

        if (ThisShop.Inventory == 0)
            percentInventory = 0;
        else
            percentInventory = 1f * ThisShop.Inventory / ThisShop.Capacity;

        Debug.Log(percentInventory);
        // get the bar child
        GameObject tmpBar = ShopInventoryBar.transform.GetChild(2).gameObject;
        // rescale
        var currentScale = tmpBar.transform.localScale;
        currentScale.x = percentInventory;
        tmpBar.transform.localScale = currentScale; // set new scale

        if (ThisShop.Inventory <= ThisShop.Threshold)
            tmpBar.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.red;
        else
            tmpBar.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.green;
    }

    void ShowInventoryBar()
    {

    }
}
