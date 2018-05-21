using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Person : MonoBehaviour
{

    public Mouse mouse;
    public int tier = 0;
    public int nationNum = 5;

    private Vector3 pos;
    public bool hover;
    public GameObject hex;
    public Hexagon hexagon;
    public UIManager uiManager;
    private Hexagon[] hexagons;
    public GameObject unusedHexMaster;
    public GameObject country;
    private Hexagon oldHexagon;
    private Sprite[] sprites;

    void Awake()
    {
        uiManager = GameObject.FindObjectOfType<UIManager>();
        sprites = Resources.LoadAll<Sprite>("People");
        mouse = GameObject.FindObjectOfType<Mouse>();
        this.nationNum = uiManager.turnNumber + 2;
        this.name = "Unit " + uiManager.selectedCountry + " , " + uiManager.selectedCountry.GetComponent<Country>().unitCount;
        this.transform.SetParent(uiManager.selectedCountry.transform);
        uiManager.selectedCountry.GetComponent<Country>().unitCount++;
    }
    void Update()
    {
        if (hover)
            this.transform.position = mouse.mousePos;
    }
    void OnTriggerStay2D(Collider2D col)
    {
        if (hover)
        {
            if (col.gameObject.tag.Equals("hex"))
            {
                hex = col.gameObject;
                hexagon = hex.GetComponent<Hexagon>();
            }
        }
    }
    public void Place()
    {
        this.GetComponent<CircleCollider2D>().radius = .17f;
        Country hexCountry = this.transform.parent.gameObject.GetComponent<Country>();
        pos = hex.transform.position;
        GameObject originalHex = hex;
        this.transform.position = pos;
        hover = false;
        mouse.hasPerson = false;
        if (hex.transform.parent != this.gameObject.transform.parent)
        {
            hexagon.isCountry = true;
            foreach (GameObject g in hexagon.adj)
            {
                Hexagon hex = g.GetComponent<Hexagon>();
                if (hex.nationNum == this.nationNum && g.transform.parent != this.gameObject.transform.parent)
                {
                    if (hex.isCountry)
                    {
                        Hexagon[] hexesToSwitchParent = g.gameObject.transform.parent.gameObject.GetComponent<Country>().hexagons;
                        Person[] unitsToSwitchParent = g.gameObject.transform.parent.gameObject.GetComponent<Country>().PrepareUnitsForParentSwitch();
                        foreach (Hexagon h in hexesToSwitchParent)
                        {
                            h.gameObject.transform.SetParent(uiManager.selectedCountry.transform);
                        }
                        foreach (Person p in unitsToSwitchParent)
                        {
                            p.country = uiManager.selectedCountry;
                            p.gameObject.transform.SetParent(country.transform);
                        }
                    }
                    else { 
                        g.transform.SetParent(uiManager.selectedCountry.transform);
                        g.GetComponent<Hexagon>().isCountry = true;
                    }
                }
            }
            //resets the "hex" variable to the currently clicked hexagon
            hex = originalHex;
            //establishes old country and new country
            GameObject OldHexCountry = hex.transform.parent.gameObject;
            Country OldHexCountryScript = OldHexCountry.GetComponent<Country>();
            hex.transform.SetParent(uiManager.selectedCountry.transform);
            GameObject NewHexCountry = hex.transform.parent.gameObject;
            Country NewHexCountryScript = NewHexCountry.GetComponent<Country>();
            //updates hexagons array of countries
            if (OldHexCountryScript != null){
                OldHexCountryScript.UpdateArray();
            }
            NewHexCountryScript.UpdateArray();
            //Country split function goes here

            hexagon.nationNum = this.nationNum;
            hexagon.ChangeSprite(this.nationNum);
        }
        //update guardedBy feature
        hexagon.hasGuard = true;
        try
        {
            oldHexagon.hasGuard = false;
        }
        catch (Exception e) {}
        hexCountry.UpdateGuard();
        oldHexagon = hexagon;
    }
    public void StartingTier(){
        tier = uiManager.tier;
        this.GetComponent<SpriteRenderer>().sprite = sprites[tier];
        tier += 1;
    }
}
