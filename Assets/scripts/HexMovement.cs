using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexMovement : MonoBehaviour {

	public Mouse mouse;
	private int nationNum;
	public GameObject personObject;
	private bool valid = true;
	private Person person;
    bool placedIsValid;
    private Hexagon hexagon;

    void Start(){
        hexagon = this.gameObject.GetComponent<Hexagon>();
        mouse = GameObject.Find("Mouse").GetComponent<Mouse>();
    }
		void OnTriggerStay2D(Collider2D col) {
        if (col.gameObject.tag.Equals("mouse") && Input.GetMouseButtonUp(0) == true && mouse.hasPerson == true)
        {
            personObject = mouse.personObject;
            person = personObject.GetComponent<Person>();
            if (person.hover)
            {
                placedIsValid = PlacedIsValid();
                if (placedIsValid)
                {
                    mouse.hasPerson = false;
                    hexagon.guardedBy = person.tier;
                    person.Place();
                }
            }
		}
    }
    bool PlacedIsValid (){ //the criteria for placing the unit down to be something the player can do
        Hexagon thisHex = this.gameObject.GetComponent<Hexagon>();
        // criteria for it to be a valid place to move the unit goes here
        print("this hex is guarded by: " + thisHex.guardedBy);
        if (thisHex.isLand == true ) { //requirements regardless of country/color of hexagon
            if (this.gameObject.transform.parent != person.transform.parent) //requirements if it is taking a hexagon of another country
            { 
                if (thisHex.guardedBy < person.tier || thisHex.guardedBy == 0){
                    return true;  
                }
                else
                {
                    print("the unit you tried to occupy is too heavily guarded for that unit to take!");
                    return false;
                }
            }
            return true;
        }
        return false;
    }
}

