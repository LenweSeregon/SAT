using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour {

    public GameObject plane;
    public GameObject openerPrefab;

    private List<Row> rows;

    public int nbLine;
    public int nbDoor;
    public Material doorMaterial;

    class Row
    {
        public bool[] hasDoor;
        public GameObject[] doors;
        public bool[] isOpened;

        public Row(bool[] hasDoor, bool[] isOpened)
        {
            this.hasDoor = hasDoor;
            this.isOpened = isOpened;
            this.doors = new GameObject[hasDoor.Length];
        }
    }

	// Use this for initialization
	void Start ()
    {
        Generate();
        GenerateButtons();

        GetCNFForm();
    }

    private void Generate()
    {
       rows = new List<Row>();
        for(int i = 0; i < nbLine; i++)
        {
            bool[] doors = new bool[nbDoor];
            bool[] isOpened = new bool[nbDoor];
            for(int j = 0; j < nbDoor; j++)
            {
                int rand = Random.Range(0, 2);
                if (rand == 0)
                {
                    doors[j] = false;
                }
                else
                {
                    doors[j] = true;
                    int rand2 = Random.Range(0, 2);
                    if (rand2 == 0)
                    {
                        isOpened[j] = false;
                    }
                    else
                    {
                        isOpened[j] = true;
                    }
                }
            }
            rows.Add(new Row(doors, isOpened));
        }

        int iterator = 0;
        foreach(Row row in rows)
        {
            GameObject line = new GameObject();
            line.transform.name = "Row";

            int k = 0;
            for(int i = 0; i < nbDoor; i++)
            {
                GameObject wall = GameObject.CreatePrimitive(PrimitiveType.Cube);
                wall.transform.parent = line.transform;
                wall.transform.name = "Wall";
                wall.transform.localScale = new Vector3(10, 4, 1);
                wall.transform.position = new Vector3(i * 20, 2, iterator * 10);

                if (!row.hasDoor[i])
                {
                    GameObject wall2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    wall2.transform.localScale = new Vector3(10, 4, 1);
                    wall2.transform.position = new Vector3(i * 20 + 10, 2, iterator * 10);
                    wall2.transform.parent = line.transform;
                    wall2.transform.name = "Wall";
                    row.doors[i] = null;
                }
                else
                {
                    GameObject door = GameObject.CreatePrimitive(PrimitiveType.Cube);
                    door.transform.localScale = new Vector3(10, 4, 1);
                    door.transform.position = new Vector3(i * 20 + 10, 2, iterator * 10);
                    door.transform.parent = line.transform;
                    door.GetComponent<Renderer>().material = doorMaterial;
                    door.transform.name = "Door";
                    row.doors[i] = door;

                    if(row.isOpened[i])
                    {
                        door.transform.Rotate(new Vector3(0, 90, 0));
                    }
                }
                k = i;
            }
            GameObject wallFinal = GameObject.CreatePrimitive(PrimitiveType.Cube);
            wallFinal.transform.parent = line.transform;
            wallFinal.transform.name = "Wall";
            wallFinal.transform.localScale = new Vector3(10, 4, 1);
            wallFinal.transform.position = new Vector3((k+1) * 20, 2, iterator * 10);
            iterator++;
        }

    }
    private void GenerateButtons()
    {
        for(int i = 0; i < nbDoor; i++)
        {
            GameObject opener = Instantiate(openerPrefab);
            opener.transform.position = new Vector3(i * 20 + 10, 0.5f, -15);
            opener.transform.localScale = new Vector3(6, 1, 6);
            opener.name = "Opener" + i;
            opener.GetComponent<OpenerManager>().generatorRef = this;
        }
    }

    public void SwitchDoors(int index)
    {
        for(int i = 0; i < nbLine; i++)
        {
            int j = index;
            if (rows[i].doors[j] != null)
            {
                if (rows[i].isOpened[j])
                {
                    rows[i].doors[j].transform.Rotate(new Vector3(0, 90, 0));
                    rows[i].isOpened[j] = false;
                }
                else
                {
                    rows[i].doors[j].transform.Rotate(new Vector3(0, -90, 0));
                    rows[i].isOpened[j] = true;
                }
            }
        }

        GetCNFForm();
    }

    public List<string> GetCNFForm()
    {
        List<string> clauses = new List<string>();
        for (int i = 0; i < nbLine; i++)
        {
            string clause = "";
            for (int j = 0; j < nbDoor; j++)
            {
                if (rows[i].doors[j] != null)
                {
                    if (rows[i].isOpened[j])
                    {
                        clause += "a" + j + "V";
                    }
                    else
                    {
                        clause += "~a" + j + "V";
                    }
                }
            }
            clause = clause.Remove(clause.Length - 1);
            clauses.Add(clause);
            Debug.Log(clause);
        }
        return clauses;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
