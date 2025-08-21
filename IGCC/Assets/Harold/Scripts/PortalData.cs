using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Room/PortalData")]
public class PortalData : ScriptableObject
{
    [SerializeField]
    private List<PortalEntry> _portalDataList = new List<PortalEntry>();

    public RoomData getRandomRoom()
    {
        float totalWeight = 0;
        foreach (var drop in _portalDataList)
        {
            totalWeight += drop.Weight;
        }

        float randomChance = Random.Range(0, totalWeight);

        float currentWeight = 0;
        foreach (var drop in _portalDataList)
        {
            currentWeight += drop.Weight;

            if (randomChance <= currentWeight)
                return drop.Room;
        }

        return null;
    }
}

[System.Serializable]
class PortalEntry
{
    [SerializeField]
    RoomData _room;

    public RoomData Room => _room;

    [SerializeField]
    float _weight;
    public float Weight => _weight;
}