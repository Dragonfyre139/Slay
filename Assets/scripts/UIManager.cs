using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIManager : MonoBehaviour
{
    private Person person;
    public Mouse mouse;
    public int turnNumber;
    public static int roundNumber;
    public GameObject selectedCountry;
    public Instantiator instantiator;
    private Country selectedCountryScript;
    public GameObject unusedHexMaster;
    public int tier;

    void Start()
    {
        turnNumber = 1;
        roundNumber = 1;
    }


    public void SpawnPerson()
    {
        if (selectedCountry != null || unusedHexMaster)
        {
            GameObject personObject = (GameObject)Instantiate(Resources.Load("Person"), new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f), Quaternion.identity);
            string button = EventSystem.current.currentSelectedGameObject.name;
            char[] tierList = button.ToCharArray();
            tier = (int)char.GetNumericValue(tierList[18]);
            if (selectedCountryScript.food >= 10 * tier)
            {
                selectedCountryScript.food -= 10 * tier;
                tier -= 1;
                person = personObject.GetComponent<Person>();
                person.country = selectedCountry;
                personObject.transform.SetParent(selectedCountry.transform);
                person.StartingTier();
                mouse.personObject = personObject;
                mouse.person = personObject.GetComponent<Person>();
                mouse.PickUpUnit();
            }
            else {
                Destroy(personObject);
                print("you do not have enough food to create a new unit!"); 
            }
        }
        else print("you must select a country before you can create a unit!");
    }
    public void NextTurn(GameObject g)
    {
        Text text = g.GetComponent<Text>();
        turnNumber++;
        if (turnNumber == 6)
        {
            turnNumber = 1;
            roundNumber++;
            foreach(Country c in instantiator.countryscripts){
                c.GiveRoundFood();
            }
        }
        text.text = turnNumber.ToString();
    }
    public void SetCurrentCountry(GameObject g)
    {
        if (!mouse.hasPerson)
        {
            print("SetCurrentCounty");
            selectedCountry = g;
            selectedCountryScript = g.GetComponent<Country>();
        }
        else print("You can't switch countries while you have a unit equipped!");
    }
}