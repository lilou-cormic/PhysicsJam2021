using PurpleCable;
using UnityEngine;

public class PopPool : Pool<Pop>
{
    private static PopPool _current = null;

    [SerializeField] Pop Prefab = null;

    [SerializeField] AudioClip PopSound = null;

    protected override void Awake()
    {
        base.Awake();

        _current = this;
    }

    protected override Pop CreateItem()
    {
        Pop pop = Instantiate(Prefab, transform);
        ((IPoolable)pop).SetAsAvailable();

        return pop;
    }

    public static void ShowPop(Color color, Vector3 position, bool playSound = true)
    {
        var pop = _current.GetItem();
        pop.Color = color;
        pop.transform.position = position;

        if (playSound)
            _current.PopSound.PlayRandomPitch();
    }
}
