using UnityEngine;

namespace Components.Species
{
    [CreateAssetMenu]
    public class SpeciesInfo : ScriptableObject
    {
        [Header("Species Profile")]
        public string englishName = "English Name";
        public string latinName = "Latin Name";
        public ConservationIndex conservationIndex = ConservationIndex.Unknown;

        [Header("Fact Sheet")]
        public FactData[] facts;

        [Header("Observation Settings")]
        public Vector3 orbitOffset;
        public float defaultOrbitDistance = 2.5f;
    }
}