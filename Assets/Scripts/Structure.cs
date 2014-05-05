using UnityEngine;
using System.Collections;

public class Structure : MonoBehaviour {
	public Mesh BuildingModel;
	public Material BuildingMaterial;
	public Mesh GeneratorModel;
	public Material GeneratorMaterial;
	public Mesh WallModel;
	public Material WallMaterial;
	public Mesh BaseModel;
	public Material BaseMaterial;

	public int BuildingCost = 1000;
    public int GeneratorCost = 400;
	public int WallCost = 100;

	public Transform Fire;
	public Transform Explosion;
    public Transform SmallExplosion;
	public Transform GeneratorRunning;

	private bool _dying = false;
	private GameObject _child;
    public int Health = 15;
    private int _health = 0;

	public enum Type {
		None,
		Building,
		Generator,
		Wall,
		Base
	}

	public Type type { get; private set; }

    void Start()
    {
        _health = Health;
        type = Type.None;
    }

	private bool hasBuilding {
		get {
			int realX = transform.parent.GetComponent<Tile>().X;
			int realY = transform.parent.GetComponent<Tile>().Y;
			for(int x = -2; x <= 2; x++) {
				for(int y = -2; y <= 2; y++) {
                    if (realY + y >= State.level.LevelHeight || realY + y < 0)
                    {
						continue;
					}
                    Type tileType = transform.parent.parent.GetComponent<Level>().GetTile((x + realX) % State.level.LevelWidth, y + realY).
                        transform.FindChild("Structure").GetComponent<Structure>().type;
					if(tileType == Type.Building || tileType == Type.Base) {
						return true;
					}
				}
			}
			return false;
		}
	}

	void Update () {
		if(transform.parent.GetComponent<Tile>().HasLava && !_dying) {
			if(type == Type.Generator && hasBuilding) {
				_child.particleSystem.Play();
			}
			else if(type == Type.Building || type == Type.Wall || type == Type.Base) {
				StartCoroutine("die");
			}
		}
		if(type == Type.Generator && (!transform.parent.GetComponent<Tile>().HasLava || !hasBuilding || _dying)) {
			_child.particleSystem.Stop();
		}
		switch(type) {
		case Type.Generator:
			if(transform.parent.GetComponent<Tile>().HasLava && hasBuilding) {
				State.money += 15f * Time.deltaTime;
			}
			break;
		case Type.Base:
		case Type.Wall:
            if (_health <= 0 && !_dying)
            {
                StartCoroutine(die());
                _health = Health;
            }
			break;
		case Type.Building:
		case Type.None:
			break;
		}
	}

	IEnumerator die()
	{
		float time = 1.75f;
		_dying = true;
		Destroy(Instantiate(Fire.gameObject, transform.position, transform.rotation), time);
		yield return new WaitForSeconds(time);
		Destroy(Instantiate(Explosion.gameObject, transform.position, transform.rotation), 1);
#if !(UNITY_STANDALONE || UNITY_WEBPLAYER)
		Handheld.Vibrate();
#endif
		renderer.enabled = false;
		if(type == Type.Base) {
			State.defeated = true;
		}
		type = Type.None;
		_dying = false;
	}

	public void buildBuilding(bool free = false) {
        if (type != Type.None || transform.parent.GetComponent<Tile>().HasLava) return;
        if (!free)
        {
            if (State.money < BuildingCost) return;
            State.money -= BuildingCost;
        }
		renderer.material = BuildingMaterial;
		GetComponent<MeshFilter>().mesh = BuildingModel;
		renderer.enabled = true;
		type = Type.Building;
	}

    public void buildGenerator(bool free = false)
    {
        if (type != Type.None || transform.parent.GetComponent<Tile>().HasLava) return;
        if (!free)
        {
            if (State.money < GeneratorCost) return;
            State.money -= GeneratorCost;
        }
		renderer.material = GeneratorMaterial;
		GetComponent<MeshFilter>().mesh = GeneratorModel;
		renderer.enabled = true;
		type = Type.Generator;
		_child = Instantiate(GeneratorRunning.gameObject, transform.position, transform.rotation) as GameObject;
		_child.transform.parent = transform;
		_child.particleSystem.Stop();
		_child.particleSystem.Clear();
	}

    public void buildWall(bool free = false)
    {
        if (type != Type.None || transform.parent.GetComponent<Tile>().HasLava) return;
        if (!free)
        {
            if (State.money < WallCost) return;
            State.money -= WallCost;
        }
        
		renderer.material = WallMaterial;
		GetComponent<MeshFilter>().mesh = WallModel;
		renderer.enabled = true;
		type = Type.Wall;
	}

	public void buildBase()
	{
		renderer.material = BaseMaterial;
		GetComponent<MeshFilter>().mesh = BaseModel;
		renderer.enabled = true;
		type = Type.Base;
	}
	
    public void Hurt(int amount)
    {
        if (_dying) return;
        _health -= amount;
        if (!_dying) Destroy(Instantiate(SmallExplosion.gameObject, transform.position, transform.rotation), 1);
    }
}
