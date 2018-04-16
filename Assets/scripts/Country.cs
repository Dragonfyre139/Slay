using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Country : MonoBehaviour {
    public Hexagon[] hexagons;
    public Instantiator instantiator;
    public Hexagon[] HexesInCountry;

    private void Awake(){
        instantiator = GameObject.FindObjectOfType<Instantiator>();
        this.name = "Country " + instantiator.countryCount;
    }
    public void CreateArray(){
        hexagons = this.GetComponentsInChildren<Hexagon>();
    }
    public void DeleteIfEmpty(){
        if (hexagons.Length == 0) {
            instantiator.countrySubtraction++;
            Destroy(this.gameObject);
        }
    }
}
