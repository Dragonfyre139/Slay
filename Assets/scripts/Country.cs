using System;
using UnityEngine;

public class Country : MonoBehaviour
{
    public Hexagon[] hexagons;
    public Instantiator instantiator;
    public int unitCount = 0;
    public int food;
    public int ymax;
    public int ymin;
    public int xmax;
    public int xmin;
    private Person[] people;

    private void Awake()
    {
        instantiator = GameObject.FindObjectOfType<Instantiator>();
        this.name = "Country " + instantiator.countryCount;
       
    }
    public void UpdateArray()
    {
        hexagons = this.GetComponentsInChildren<Hexagon>();
        if (hexagons.Length > 0)
        {
            xmax = hexagons[0].HexPosX;
            xmin = xmax;
            ymax = hexagons[0].HexPosY;
            ymin = ymax;
            foreach (Hexagon h in hexagons)
            {
                int currentX = h.HexPosX;
                int currentY = h.HexPosY;
                if (currentX < xmin) xmin = currentX;
                if (currentX > xmax) xmax = currentX;
                if (currentY < ymin) ymin = currentY;
                if (currentY > ymax) ymax = currentY;
            }
        }
    }
    public void DeleteIfEmpty()
    {
        if (hexagons.Length == 0)
        {
            Invoke("RemoveFromCountryLists", 1);
        }
    }
    void RemoveFromCountryLists(){
        instantiator.countryscripts.Remove(this.GetComponent<Country>());
        instantiator.countries.Remove(this.gameObject);
        Destroy(this.gameObject);
    }
    public Person[] PrepareUnitsForParentSwitch()
    {
        Person[] unitsForSwitch;
        unitsForSwitch = this.GetComponentsInChildren<Person>();
        return unitsForSwitch;
    }
    public void AssignBeginningFood(){
        food = hexagons.Length * 5;
    }
    public void GiveRoundFood(){
        food += hexagons.Length;
        people = this.GetComponentsInChildren<Person>();
        foreach (Person p in people)
        {
            food -= 2 * (int)Math.Pow(3,(p.tier - 1));
        //    if (food < 0) Starve();
        }
    }
    public void UpdateGuard(){
        print("UpdateGuard");
        foreach (Hexagon h in hexagons){
            if (h.hasGuard == false) h.guardedBy = 0;
        }
        foreach (Hexagon h in hexagons){
            if (h.hasGuard){
                foreach (GameObject g in h.adj)
                {
                    if (g.transform.parent == h.transform.parent) {
                        Hexagon adjHexScript = g.GetComponent<Hexagon>();
                        if (adjHexScript.guardedBy < h.guardedBy) adjHexScript.guardedBy = h.guardedBy; 
                    }
                }
            }
        }
    }
}

