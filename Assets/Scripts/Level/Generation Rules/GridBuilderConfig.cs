using UnityEngine;

[CreateAssetMenu(fileName = "GridBuilderConfig", menuName = "Level Generation/GridBuilderConfig")]
public class GridBuilderConfig : ScriptableObject
{
    [Header("Grid Builder Parameters")]
    [SerializeField] private float nodeSize = 1f;
    [SerializeField] private LayerMask unwalkableLayerMask;

    public float NodeSize => nodeSize;
    public LayerMask UnwalkableLayerMask => unwalkableLayerMask;
}
