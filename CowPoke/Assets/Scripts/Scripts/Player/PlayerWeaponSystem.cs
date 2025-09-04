public class PlayerWeaponSystem : MonoBehaviour
{
    public WeaponDef[] startingWeapons;
    public ObjectPool pool;
    List<WeaponController> _controllers = new();

    void Start()
    {
        foreach (var def in startingWeapons)
            _controllers.Add(new WeaponController(transform, def, pool));
    }

    void Update()
    {
        float dt = Time.deltaTime;
        for (int i = 0; i < _controllers.Count; i++)
            _controllers[i].Tick(dt);
    }

    public void AddWeapon(WeaponDef def, int level=1)
    {
        var wc = new WeaponController(transform, def, pool);
        wc.SetLevel(level);
        _controllers.Add(wc);
    }

    public void SetLevel(string weaponId, int level)
    {
        foreach (var wc in _controllers)
            if (wc.DefIdEquals(weaponId)) wc.SetLevel(level);
    }
}
