using UnityEngine;

namespace Components.Species
{
    [CreateAssetMenu]
    public class SpeciesInfo : ScriptableObject
    {
        [Header("Basic Profile")]
        [SerializeField] public string englishName = "Species English Name Here";
        [SerializeField] public string latinName = "Species Latin Name Here";
        [SerializeField] public ConservationIndex conservationIndex = ConservationIndex.Unknown;

        [Header("Fact Sheet")]
        [SerializeField] public FactData[] facts;

        [Header("Observation Settings")]
        [SerializeField] public Vector3 orbitOffset = Vector3.zero;
        [SerializeField] public float defaultCameraZoom = 2.5f;
    }
}