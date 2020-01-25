using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : GameBehaviour
{
	[SerializeField]
	private Light light;

	float defaultIntensity = 1;
	float defaultAngle = 15;

	public bool CanSetLight { get; private set; } = true;

	public void SetLight(float pIntensity, float pAngle, float pDuration)
	{
		if(!CanSetLight && pDuration > 0)
			return;

		CanSetLight = false;

		const float transition_duration = 1;

		LeanTween.value(gameObject, UpdateLightAngle, light.spotAngle, pAngle, transition_duration);

		LeanTween.value(gameObject, UpdateLightIntensity,
			light.intensity, pIntensity, transition_duration)
			.setOnComplete(() => CanSetLight = pDuration < 0);

		if(pDuration > 0)
			DoInTime(() => SetLight(defaultIntensity, defaultAngle, -1), pDuration);
	}

	private void UpdateLightIntensity(float pIntensity)
	{
		light.intensity = pIntensity;
	}

	private void UpdateLightAngle(float pAngle)
	{
		light.spotAngle = pAngle;
	}

}
