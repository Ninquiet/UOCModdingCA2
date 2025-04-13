using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
	
	private float _lastAnimationChange = 0f;
    
    private void Update()
    {
        CheckForAnimationChange();
    }

	private void CheckForAnimationChange()
	{
		if (Time.time - _lastAnimationChange > 0.1f)
        {
            _lastAnimationChange = Time.time;
        }
	}
}
