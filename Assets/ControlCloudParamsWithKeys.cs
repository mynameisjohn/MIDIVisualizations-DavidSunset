using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class ControlCloudParamsWithKeys : MonoBehaviour
{
    public Volume _Volume;
    VolumeProfile _profile;
    VolumetricClouds _clouds;
    ColorAdjustments _colorAdjustments;

    public float _Delta = 0.01f;

    public KeyCode _DensityDecreaseKey = KeyCode.DownArrow;
    public KeyCode _DensityIncreaseKey = KeyCode.UpArrow;
    public float _MinDensityCurve = 0;
    public float _MaxDensityCurve = 1;
    float _densityValue;

    public KeyCode _ShapeDecreaseKey = KeyCode.LeftArrow;
    public KeyCode _ShapeIncreaseKey = KeyCode.RightArrow;
    public float _MinShapeCurve = 0.875f;
    public float _MaxShapeCurve = 1.0f;
    float _shapeValue;

    public KeyCode _ErosionDecreaseKey = KeyCode.RightAlt;
    public KeyCode _ErosionIncreaseKey = KeyCode.RightControl;
    public float _MinErosionCurve = 0;
    public float _MaxErosionCurve = 1;
    float _erosionValue;

    public Gradient _CloudScatteringTintGradient;
    public KeyCode _CloudScatteringTintDecreaseKey = KeyCode.A;
    public KeyCode _CloudScatteringTintIncreaseKey = KeyCode.S;
    float _scatteringTingGradientValue;

    public Light _Sun;
    public Gradient _SunColorGradient;
    public KeyCode _SunColorGradientDecreaseKey = KeyCode.Z;
    public KeyCode _SunColorGradientIncreaseKey = KeyCode.X;
    float _sunColorGradientValue;

    public KeyCode _SaturationDecreaseKey = KeyCode.Q;
    public KeyCode _SaturationIncreaseKey = KeyCode.W;
    public float _MinSaturationValue = 0;
    public float _MaxSaturationValue = 100;
    float _saturationValue;

    bool _recompute;

    // Start is called before the first frame update
    void Start()
    {
        _profile = _Volume.sharedProfile;
        _profile.TryGet(out _clouds);
        _profile.TryGet(out _colorAdjustments);

        _densityValue = (_clouds.densityMultiplier.value - _MinDensityCurve) / (_MaxDensityCurve - _MinDensityCurve);
        _shapeValue = (_clouds.densityMultiplier.value - _MinShapeCurve) / (_MaxShapeCurve - _MinShapeCurve);
        _erosionValue = (_clouds.densityMultiplier.value - _MinErosionCurve) / (_MaxErosionCurve - _MinErosionCurve);

        recomputeCloudValues();
    }

    // Update is called once per frame
    void Update()
    {
        _recompute = false;
        if (Input.GetKey(_DensityDecreaseKey))
        {
            decreaseDensity();
        }
        if (Input.GetKey(_DensityIncreaseKey))
        {
            increaseDensity();
        }
        if (Input.GetKey(_ShapeDecreaseKey))
        {
            decreaseShape();
        }
        if (Input.GetKey(_ShapeIncreaseKey))
        {
            increaseShape();
        }
        if (Input.GetKey(_ErosionDecreaseKey))
        {
            decreaseErosion();
        }
        if (Input.GetKey(_ErosionIncreaseKey))
        {
            increaseErosion();
        }

        if (Input.GetKey(_SunColorGradientDecreaseKey))
        {
            decreaseSunGradient();
        }
        if (Input.GetKey(_SunColorGradientIncreaseKey))
        {
            increaseSunGradient();
        }

        if (Input.GetKey(_CloudScatteringTintDecreaseKey))
        {
            decreaseScatteringTint();
        }
        if (Input.GetKey(_CloudScatteringTintIncreaseKey))
        {
            increaseScatteringTint();
        }

        if (Input.GetKey(_SaturationDecreaseKey))
        {
            decreaseSaturation();
        }
        if (Input.GetKey(_SaturationIncreaseKey))
        {
            increaseSaturation();
        }

        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);
            Vector2 pos = t.position;
            
            Debug.Log(pos);
            if (pos.x > Screen.width / 2)
            {
                increaseDensity();
                increaseShape();
                increaseErosion();
            }
            else if (pos.x < Screen.width / 2)
            {
                decreaseDensity();
                decreaseShape();
                decreaseErosion();
            }

            if (pos.y > Screen.height / 2)
            {
                increaseSunGradient();
                increaseScatteringTint();
                increaseSaturation();
            }
            else if (pos.y < Screen.height / 2)
            {
                decreaseSunGradient();
                decreaseScatteringTint();
                decreaseSaturation();
            }

            _recompute = true;
        }

        if (_recompute)
        {
            recomputeCloudValues();
        }
    }

    private void increaseSaturation()
    {
        _saturationValue = Mathf.Clamp01(_saturationValue + _Delta);
        _recompute = true;
    }

    private void decreaseSaturation()
    {
        _saturationValue = Mathf.Clamp01(_saturationValue - _Delta);
        _recompute = true;
    }

    private void increaseScatteringTint()
    {
        _scatteringTingGradientValue = Mathf.Clamp01(_scatteringTingGradientValue + _Delta);
        _recompute = true;
    }

    private void decreaseScatteringTint()
    {
        _scatteringTingGradientValue = Mathf.Clamp01(_scatteringTingGradientValue - _Delta);
        _recompute = true;
    }

    private void increaseSunGradient()
    {
        _sunColorGradientValue = Mathf.Clamp01(_sunColorGradientValue + _Delta);
        _recompute = true;
    }

    private void decreaseSunGradient()
    {
        _sunColorGradientValue = Mathf.Clamp01(_sunColorGradientValue - _Delta);
        _recompute = true;
    }

    private void increaseErosion()
    {
        _erosionValue = Mathf.Clamp01(_erosionValue + _Delta);
        _recompute = true;
    }

    private void decreaseErosion()
    {
        _erosionValue = Mathf.Clamp01(_erosionValue - _Delta);
        _recompute = true;
    }

    private void increaseShape()
    {
        _shapeValue = Mathf.Clamp01(_shapeValue + _Delta);
        _recompute = true;
    }

    private void decreaseShape()
    {
        _shapeValue = Mathf.Clamp01(_shapeValue - _Delta);
        _recompute = true;
    }

    private void increaseDensity()
    {
        _densityValue = Mathf.Clamp01(_densityValue + _Delta);
        _recompute = true;
    }

    private void decreaseDensity()
    {
        _densityValue = Mathf.Clamp01(_densityValue - _Delta);
        _recompute = true;
    }

    void recomputeCloudValues()
    {
        _clouds.densityMultiplier.value = Mathf.Lerp(_MinDensityCurve, _MaxDensityCurve, _densityValue);
        _clouds.shapeFactor.value = Mathf.Lerp(_MinShapeCurve, _MaxShapeCurve, _shapeValue);
        _clouds.erosionFactor.value = Mathf.Lerp(_MinErosionCurve, _MaxErosionCurve, _erosionValue);

        Debug.Log("Density: " + _clouds.densityMultiplier.value);
        Debug.Log("Shape: " + _clouds.shapeFactor.value);
        Debug.Log("Erosion: " + _clouds.erosionFactor.value);

        _clouds.scatteringTint.value = _CloudScatteringTintGradient.Evaluate(_scatteringTingGradientValue);
        Debug.Log("Scattering Tint Value: " + _scatteringTingGradientValue);

        _Sun.color = _SunColorGradient.Evaluate(_sunColorGradientValue);
        Debug.Log("Sun Value: " + _sunColorGradientValue);

        Debug.Log("Saturation Value: " + _saturationValue);
        _colorAdjustments.saturation.value = Mathf.Lerp(_MinSaturationValue, _MaxSaturationValue, _saturationValue);
    }
}
