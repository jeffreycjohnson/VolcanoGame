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

    public Type GetType()
    {
        return type;
    }

    void Start()
    {
        _health = Health;
    }

	void Update () {
		if(transform.parent.GetComponent<Tile>().HasLava && !dying) {
			if(type == Type.Generator) {
				child.particleSystem.Play();
			}
			else {
				StartCoroutine("die");
			}
		}
		else if(type == Type.Generator) {
			child.particleSystem.Stop();
		}

		switch(type) {
		case Type.Building:
			//State.money += .03f;
			break;
		case Type.Generator:
			if(transform.parent.GetComponent<Tile>().HasLava) {
				State.money += .3f;
			}
			//State.money += .003f;
			break;
		case Type.Wall:
            if (_health <= 0 && !dying)
            {
                StartCoroutine(die());
                _health = Health;
            }
			//State.money += .003f;
			break;
		case Type.None:
			//State.money += .003f;
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
