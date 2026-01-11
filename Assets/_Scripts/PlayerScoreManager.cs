using UnityEngine;

public class PlayerScoreManager : MonoBehaviour
{
    private static float _currentScore;

    private static float _perfectLandingMultiplier = 2;
    private static float _smoothLandingMultiplier = 1.25f;
    private static float _roughLandingMultiplier = 0.85f;


    public static void AddDeliveryScore(Player_CollisionHandler.LandingType landingType)
    {
        float multiplier = 1f;
        float singleDeliveryScore = 100f;

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
                multiplier = 0f;
                break;
        }

        _currentScore += singleDeliveryScore * multiplier;

        Debug.Log("ADDED " + singleDeliveryScore * multiplier);
        Debug.Log("Current Score: " + _currentScore);
    }
}
