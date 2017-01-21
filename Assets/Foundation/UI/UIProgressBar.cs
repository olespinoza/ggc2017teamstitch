using UnityEngine;
using System.Collections;

public class UIProgressBar : MonoBehaviour
{
	[SerializeField] private UnityEngine.UI.Image _foreground;
	[SerializeField] private UnityEngine.UI.Image _background;
	[SerializeField] private UnityEngine.UI.Text _text;

	[SerializeField] private Color _ColorFull;
	[SerializeField] private Color _ColorEmpty;
	[SerializeField] private Color _ColorNegative;

	[SerializeField] private float _value = 1.0f;
	[SerializeField] private float _max = 1.0f;

	void Update()
	{
		float clampedValue = Mathf.Clamp( _value / _max, -1.0f, 1.0f );

		if( _text != null )
		{
			_text.text = Mathf.FloorToInt( _value ).ToString();
		}

		if( _foreground != null && _background != null )
		{
			float size = Mathf.Abs(clampedValue) * _background.rectTransform.rect.width;
			_foreground.rectTransform.SetSizeWithCurrentAnchors( RectTransform.Axis.Horizontal, size );
			if( _value < 0 )
			{
				_foreground.color = Color.Lerp( _ColorEmpty, _ColorNegative, -clampedValue );
			}
			else
			{
				_foreground.color = Color.Lerp( _ColorEmpty, _ColorFull, clampedValue );
			}
		}
	}
}
