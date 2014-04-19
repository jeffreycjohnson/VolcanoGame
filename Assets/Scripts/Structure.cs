using UnityEngine;
using System.Collections;

public class Structure : MonoBehaviour {
	public Mesh buildingModel;
	public Material buildingMaterial;
	public Mesh generatorModel;
	public Material generatorMaterial;
	public Mesh wallModel;
	public Material wallMaterial;

	public int buildingCost = 200;
	public int generatorCost = 500;
	public int wallCost = 100;

	enum Type {
		None,
		Building,
		Generator,
		Wall
	}

	Type type = Type.None; 

	void Update () {
		if(transform.parent.GetComponent<Tile>().HasLava && type == Type.Building) {
			renderer.enabled = false;
			type = Type.None;
		}
		switch(type) {
		case Type.Building:
			State.money += .03f;
			break;
		case Type.Generator:
			if(transform.parent.GetComponent<Tile>().HasLava) {
				State.money += .15f;
			}
			State.money += .003f;
			break;
		case Type.Wall:
			State.money += .003f;
			break;
		case Type.None:
			State.money += .003f;
			break;
		}
	}

	public void buildBuilding() {
		if(type != Type.None || transform.parent.GetComponent<Tile>().HasLava || State.money < buildingCost) {
			return;
		}
		State.money -= buildingCost;
		renderer.material = buildingMaterial;
		GetComponent<MeshFilter>().mesh = buildingModel;
		renderer.enabled = true;
		type = Type.Building;
	}

	public void buildGenerator() {
		if(type != Type.None || transform.parent.GetComponent<Tile>().HasLava || State.money < generatorCost) {
			return;
		}
		State.money -= generatorCost;
		renderer.material = generatorMaterial;
		GetComponent<MeshFilter>().mesh = generatorModel;
		renderer.enabled = true;
		type = Type.Generator;
	}

	public void buildWall() {
		if(type != Type.None || transform.parent.GetComponent<Tile>().HasLava || State.money < wallCost) {
			return;
		}
		State.money -= wallCost;
		renderer.material = wallMaterial;
		GetComponent<MeshFilter>().mesh = wallModel;
		renderer.enabled = true;
		type = Type.Wall;
	}
}
