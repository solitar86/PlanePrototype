using Project.SFX;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeliveryHolder : MonoBehaviour
{
    private List<Deliverable> _deliverablesList = new();
    private Player_CollisionHandler _collisionHandler;

    [Space(15)]
    [SerializeField] private float _inventoryXPos = 10f;
    [SerializeField] private float _inventoryYPos = 160f;
    [SerializeField] private Sound _scoringSound;

    private void Start()
    {
        _collisionHandler = GetComponent<Player_CollisionHandler>();
    }

    public void AddDeliverable(Deliverable deliverable)
    {
        if (_deliverablesList.Contains(deliverable)) return;

        _deliverablesList.Add(deliverable);
    }

    public void ScoreDeliberable(Deliverable.IslandColor type)
    {
        float finalScore = 0f;
        if (_deliverablesList.Count > 0)
        {
            for (int i = _deliverablesList.Count - 1; i >= 0; i--)
            {
                if (_deliverablesList[i].DeliverableColor == type)
                {
                    // Score this object
                    _deliverablesList.Remove(_deliverablesList[i]);
                    finalScore += PlayerScoreManager.AddDeliveryScore(_collisionHandler.lastLandingType);
                }
            }
        }

        if(finalScore ==  0f)return;

        FloatingText.Create(transform.position + transform.up * 2f, finalScore.ToString() + "$", Color.lightGoldenRodYellow);
        AudioPlayer.PlaySoundAtPoint(this, _scoringSound, transform.position, true);
    }

    private void Update()
    {
        if (_collisionHandler.IsLanded() && _deliverablesList.Count > 0)
        {
            if (Physics.Raycast(transform.position, transform.up * -1, out RaycastHit hitInfo, float.MaxValue))
            {
                if (hitInfo.collider.TryGetComponent<IslandSurface>(out var surface))
                {
                    ScoreDeliberable(surface.IslandColor);
                }
            }
        }
    }


    void OnGUI()
    {
        var text = ParseCurrentDeliveryString();
        GUI.Label(new Rect(_inventoryXPos, _inventoryYPos, 100, 300), "<color=#000000>" + text + "</color>");
    }


    private string ParseCurrentDeliveryString()
    {
        int blue = 0;
        int red = 0;
        int green = 0;
        int yellow = 0;
        int orange = 0;
        foreach (var item in _deliverablesList)
        {
            switch (item.DeliverableColor)
            {
                case Deliverable.IslandColor.Blue:
                    blue++;
                    break;
                case Deliverable.IslandColor.Yellow:
                    yellow++;
                    break;
                case Deliverable.IslandColor.Green:
                    green++;
                    break;
                case Deliverable.IslandColor.Orange:
                    orange++;
                    break;
                case Deliverable.IslandColor.Red:
                    red++;
                    break;
            }
        }

        var text = string.Empty;

        text += blue == 0 ? string.Empty : "<color=#0028FF>Blue x" + blue.ToString() + "</color>\n";
        text += green == 0 ? string.Empty : "<color=#029F00>Green x" + green.ToString() + "</color>\n";
        text += red == 0 ? string.Empty : "<color=#8C060A>Red x" + red.ToString() + "</color>\n";
        text += yellow == 0 ? string.Empty : "<color=#D9BE00>Yellow x" + yellow.ToString() + "</color>\n"; ;
        text += orange == 0 ? string.Empty : "<color=#D14D00>Orange x" + orange.ToString() + "</color>\n";

        return text;
    }
}
