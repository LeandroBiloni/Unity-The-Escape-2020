using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoyBlastPower : MonoBehaviour
{
	[SerializeField] float _timeAlive = 0.2f;
	[SerializeField] float _growthAmmount = 3f;
	[SerializeField] float _growthTime = 0.3f;
	float _scaleModifier = 1;

	public void BlastStart()
	{
		gameObject.SetActive(true);
		StartCoroutine(BlastEffect());
	}

	IEnumerator BlastEffect()
	{
		var time = 0f;
		var growthTime = 0.3f;
		var startValue = _scaleModifier;
		var startScale = transform.localScale;

		while(time < growthTime)
		{
			_scaleModifier = Mathf.Lerp(startValue, _growthAmmount, time / growthTime);
			transform.localScale = startScale * _scaleModifier;
			time += Time.deltaTime;
			yield return null;
		}

		transform.localScale = startScale * _growthAmmount;

		_scaleModifier = _growthAmmount;

		yield return new WaitForSecondsRealtime(_timeAlive);

		gameObject.SetActive(false);

		transform.localScale = Vector3.one;
		_scaleModifier = startValue;
	}
}
