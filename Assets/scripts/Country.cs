using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Country : MonoBehaviour {

    private Instantiator instantiator;

    void Awake(){
        this.name = "Country " + instantiator.countryCount;
    }
}
