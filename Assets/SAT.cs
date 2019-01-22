using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SAT : MonoBehaviour
{
    public string SATFormulaAsString = "(a V b) ^ c";

    public TextMeshProUGUI SATFormula;

	// Use this for initialization
	void Start () {
        //SATFormula.text = SATFormulaAsString;
	}
	
}
