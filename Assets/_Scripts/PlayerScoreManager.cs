using UnityEngine;

public class PlayerScoreManager : MonoBehaviour
{
    private static float _currentScore;
    public float CurrentScore => _currentScore;

    private static float _perfectLandingMultiplier = 2;
    private static float _smoothLandingMultiplier = 1.25f;
    private static float _roughLandingMultiplier = 0.85f;


    public static float AddDeliveryScore(Player_CollisionHandler.LandingType landingType, Deliverable.IslandColor color = Deliverable.IslandColor.Blue)
    {
        float multiplier = 1f;
        float singleDeliveryScore = 100f;

        if(GameManager._isGameOver) { return 0; }

        switch (landingType)
        {
            case Player_CollisionHandler.LandingType.Perfect:
                multiplier = _perfectLandingMultiplier;
                break;
            case Player_CollisionHandler.LandingType.Smooth:
                multiplier = _smoothLandingMultiplier;
                break;
            case Player_CollisionHandler.LandingType.Rough:
                multiplier = _roughLandingMultiplier;
                break;
            case Player_CollisionHandler.LandingType.Crash:
                multiplier = 0.2f;
                break;
        }

        if (color == Deliverable.IslandColor.Orange) multiplier *= 2;

        _currentScore += singleDeliveryScore * multiplier;

        return singleDeliveryScore * multiplier;
    }
}
