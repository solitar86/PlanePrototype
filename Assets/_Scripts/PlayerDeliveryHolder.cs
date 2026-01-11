using System.Collections.Generic;
using UnityEngine;

public class PlayerDeliveryHolder : MonoBehaviour
{
    private List<Deliverable> _deliverablesList = new();
    private Player_CollisionHandler _collisionHandler;

    private void Start()
    {
        _collisionHandler = GetComponent<Player_CollisionHandler>();
    }

    public void AddDeliverable(Deliverable deliverable)
    {
        if (_deliverablesList.Contains(deliverable)) return;

        _deliverablesList.Add(deliverable);
    }

    public void RemoveDeliberable(Deliverable.Color type)
    {
        if(_deliverablesList.Count > 0)
        {
            for (int i = _deliverablesList.Count - 1; i >= 0; i--)
            {
                if (_deliverablesList[i].DeliverableColor == type)
                {
                    // Score this object
                    _deliverablesList.Remove(_deliverablesList[i]);
                    PlayerScoreManager.AddDeliveryScore(_collisionHandler.lastLandingType);
                }
            }
        }
    }

    private void Update()
    {
        if (_collisionHandler.IsLanded())
        {
            if(Physics.Raycast(transform.position, transform.up * -1, out RaycastHit hitInfo, float.MaxValue))
            {
                if(hitInfo.collider.TryGetComponent<IslandSurface>(out var surface))
                {
                    RemoveDeliberable(surface.IslandColor);
                }
            }
        }
    }
}
