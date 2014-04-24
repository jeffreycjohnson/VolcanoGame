﻿using UnityEngine;
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

	public Transform fire;
	public Transform explosion;
    public Transform smallexplosion;
	public Transform generatorRunning;

	private bool dying = false;
	private GameObject child;
    public int Health = 15;
    private int _health = 0;

	public enum Type {
		None,
		Building,
		Generator,
		Wall
	}

	Type type = Type.None;

    public Type GetStructureType()
    {
        return type;
    }

    void Start()
    {
        _health = Health;
    }

	private bool hasBuilding {
		get {
			int realX = transform.parent.GetComponent<Tile>()._x;
			int realY = transform.parent.GetComponent<Tile>()._y;
			for(int x = -2; x <= 2; x++) {
				for(int y = -2; y <= 2; y++) {
					if(realY + y >= Level.LevelHeight || realY + y < 0) {
						continue;
					}
					if(transform.parent.parent.GetComponent<Level>().GetTile((x + realX) % Level.LevelWidth, y + realY).
					   transform.FindChild("Structure").GetComponent<Structure>().type == Type.Building) {
						return true;
					}
				}
			}
			return false;
		}
	}

	void Update () {
		if(transform.parent.GetComponent<Tile>().HasLava && !dying) {
			if(type == Type.Generator && hasBuilding) {
				child.particleSystem.Play();
			}
			else {
				StartCoroutine("die");
			}
		}
		if((!transform.parent.GetComponent<Tile>().HasLava || !hasBuilding || dying) && type == Type.Generator) {
			child.particleSystem.Stop();
		}

		switch(type) {
		case Type.Generator:
			if(transform.parent.GetComponent<Tile>().HasLava && hasBuilding) {
				State.money += 15f * Time.deltaTime;
			}
			break;
		case Type.Wall:
            if (_health <= 0 && !dying)
            {
                StartCoroutine(die());
                _health = Health;
            }
			break;
		case Type.Building:
			break;
		case Type.None:
			break;
		}
	}

	IEnumerator die()
	{
		float time;
		switch(type) {
		case Type.Building:
			time = 1.75f;
			break;
		case Type.Wall:
			time = 1.75f;
			break;
		default:
			yield break;
		}
		dying = true;
		Destroy(Instantiate(fire.gameObject, transform.position, transform.rotation), time);
		yield return new WaitForSeconds(time);
		Destroy(Instantiate(explosion.gameObject, transform.position, transform.rotation), 1);
		Handheld.Vibrate();
		renderer.enabled = false;
		type = Type.None;
		dying = false;
	}

	public void buildBuilding(bool free = false) {
        if (type != Type.None || transform.parent.GetComponent<Tile>().HasLava) return;
        if (!free)
        {
            if (State.money < buildingCost) return;
            State.money -= buildingCost;
        }
		renderer.material = buildingMaterial;
		GetComponent<MeshFilter>().mesh = buildingModel;
		renderer.enabled = true;
		type = Type.Building;
	}

    public void buildGenerator(bool free = false)
    {
        if (type != Type.None || transform.parent.GetComponent<Tile>().HasLava) return;
        if (!free)
        {
            if (State.money < generatorCost) return;
            State.money -= generatorCost;
        }
		renderer.material = generatorMaterial;
		GetComponent<MeshFilter>().mesh = generatorModel;
		renderer.enabled = true;
		type = Type.Generator;
		child = Instantiate(generatorRunning.gameObject, transform.position, transform.rotation) as GameObject;
		child.transform.parent = transform;
		child.particleSystem.Stop();
		child.particleSystem.Clear();
	}

    public void buildWall(bool free = false)
    {
        if (type != Type.None || transform.parent.GetComponent<Tile>().HasLava) return;
        if (!free)
        {
            if (State.money < wallCost) return;
            State.money -= wallCost;
        }
        
		renderer.material = wallMaterial;
		GetComponent<MeshFilter>().mesh = wallModel;
		renderer.enabled = true;
		type = Type.Wall;
	}

    public void Hurt(int amount)
    {
        if (dying) return;
        _health -= amount;
        if (!dying) Destroy(Instantiate(smallexplosion.gameObject, transform.position, transform.rotation), 1);
    }
}
