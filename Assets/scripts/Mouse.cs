using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Mouse : MonoBehaviour {
	public bool hasPerson = false;
	public Vector3 mousePos;
	public Person person;
	public GameObject personObject;
	public GameObject hexObject;
    public UIManager uiManager;
    bool pickedUpIsValid;
    private GameObject parent;
    private bool clickedHexagonHasCountry = true;

	void Update() {
		mousePos = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 1);
		mousePos = Camera.main.ScreenToWorldPoint (mousePos);
		this.transform.position = mousePos;
	}
	void OnTriggerStay2D(Collider2D col){
        try
        {
            parent = col.gameObject.transform.parent.gameObject;
            clickedHexagonHasCountry = true;
        }
        catch (Exception e) { clickedHexagonHasCountry = false; }
		if (col.gameObject.tag.Equals ("person") && Input.GetMouseButtonDown (0)) {
            person = col.GetComponent<Person>();
            personObject = col.gameObject;
            pickedUpIsValid = PickedUpIsValid();
            if (pickedUpIsValid) { PickUpUnit(); }
		}
        else if (col.gameObject.tag.Equals("hex") && Input.GetMouseButtonDown (0) && uiManager.turnNumber == col.gameObject.GetComponent<Hexagon>().nationNum - 2 && uiManager.selectedCountry != parent && clickedHexagonHasCountry) {
           if (!EventSystem.current.IsPointerOverGameObject()) uiManager.SetCurrentCountry(col.gameObject.transform.parent.gameObject);
        }
	}
	void PickedUpBuffer(){
		hasPerson = true;
	}
    public void PickUpUnit(){
        if (!person.hover)
        {
            if (hasPerson == false)
            {
                personObject.GetComponent<CircleCollider2D>().radius = .01f;
                //do the isvalid method for unit here
                Invoke("PickedUpBuffer", .1f);
                person.hover = true;
            }
        }
    }
    public bool PickedUpIsValid(){
        if (uiManager.selectedCountry == person.country){
            return true;  
        }
        return false;
    }
}
