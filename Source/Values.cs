/*
 * Copyright (c) 2014 Ninenium
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */
using UnityEngine;
using KSP.IO;

namespace NavHud
{
    public class Values
    {
        private bool _iChanged = false;
        public bool IChanged {
            get {
                bool val = _iChanged;
                _iChanged = false;
                return val;
            }
        }

        private Color _upperHalfColor = new Color(0.0f, 0.5f, 0.3f, 0.1f);
        public Color UpperHalfColor {
            get { return _upperHalfColor; }
            set {
                if (_upperHalfColor != value)
                {
                    _upperHalfColor = value;
                    _iChanged = true;
                }
            }
        }

        private Color _lowerHalfColor = new Color(0.3f, 0.5f, 0.0f, 0.1f);
        public Color LowerHalfColor {
            get { return _lowerHalfColor; }
            set {
                if (_lowerHalfColor != value)
                {
                    _lowerHalfColor = value;
                    _iChanged = true;
                }
            }
        }

        private Color _horizonColor = new Color(0.5f, 0.5f, 0.5f, 1.0f);
        public Color HorizonColor {
            get { return _horizonColor; }
            set {
                if (_horizonColor != value)
                {
                    _horizonColor = value;
                    _iChanged = true;
                }
            }
        }

        private Color _azimuthColor = new Color(0.0f, 0.3f, 0.0f, 0.1f);
        public Color AzimuthColor {
            get { return _azimuthColor; }
            set {
                if (_azimuthColor != value)
                {
                    _azimuthColor = value;
                    _iChanged = true;
                }
            }
        }

        private Color _northColor = new Color(0.5f, 0.0f, 0.0f, 1.0f);
        public Color NorthColor {
            get { return _northColor; }
            set {
                if (_northColor != value)
                {
                    _northColor = value;
                    _iChanged = true;
                }
            }
        }

        private Color _eastColor = new Color(0.0f, 0.5f, 0.0f, 1.0f);
        public Color EastColor {
            get { return _eastColor; }
            set {
                if (_eastColor != value)
                {
                    _eastColor = value;
                    _iChanged = true;
                }
            }
        }

        private Color _southColor = new Color(0.0f, 0.5f, 0.0f, 1.0f);
        public Color SouthColor {
            get { return _southColor; }
            set {
                if (_southColor != value)
                {
                    _southColor = value;
                    _iChanged = true;
                }
            }
        }

        private Color _westColor = new Color(0.0f, 0.5f, 0.0f, 1.0f);
        public Color WestColor {
            get { return _westColor; }
            set {
                if (_westColor != value)
                {
                    _westColor = value;
                    _iChanged = true;
                }
            }
        }

        private Color _headingColor = new Color(0.75f, 0.5f, 0.0f, 1.0f);
        public Color HeadingColor {
            get { return _headingColor; }
            set {
                if (_headingColor != value)
                {
                    _headingColor = value;
                    _iChanged = true;
                }
            }
        }

        private Color _alignmentColor = Color.red;
        public Color AlignmentColor {
            get { return _alignmentColor; }
            set {
                if (_alignmentColor != value)
                {
                    _alignmentColor = value;
                    _iChanged = true;
                }
            }
        }

        private Color _progradeColor = Color.green;
        public Color ProgradeColor {
            get { return _progradeColor; }
            set {
                if (_progradeColor != value)
                {
                    _progradeColor = value;
                    _iChanged = true;
                }
            }
        }

        private Color _normalColor = Color.magenta;
        public Color NormalColor {
            get { return _normalColor; }
            set {
                if (_normalColor != value)
                {
                    _normalColor = value;
                    _iChanged = true;
                }
            }
        }

        private Color _radialColor = Color.cyan;
        public Color RadialColor {
            get { return _radialColor; }
            set {
                if (_radialColor != value)
                {
                    _radialColor = value;
                    _iChanged = true;
                }
            }
        }

        private Color _targetColor = Color.magenta;
        public Color TargetColor {
            get { return _targetColor; }
            set {
                if (_targetColor != value)
                {
                    _targetColor = value;
                    _iChanged = true;
                }
            }
        }

        private Color _maneuverColor = Color.blue;
        public Color ManeuverColor {
            get { return _maneuverColor; }
            set {
                if (_maneuverColor != value)
                {
                    _maneuverColor = value;
                    _iChanged = true;
                }
            }
        }

        private int _numberZenithLinesHalf = 6;
        public int NumberZenithLinesHalf {
            get { return _numberZenithLinesHalf; }
            set {
                if (_numberZenithLinesHalf != value)
                {
                    _numberZenithLinesHalf = value;
                    _iChanged = true;
                }
            }
        }

        private int _numberAzimuthLinesQuarter = 8;
        public int NumberAzimuthLinesQuarter {
            get { return _numberAzimuthLinesQuarter; }
            set {
                if (_numberAzimuthLinesQuarter != value)
                {
                    _numberAzimuthLinesQuarter = value;
                    _iChanged = true;
                }
            }
        }

        private int _numberZenithVerts = 128;
        public int NumberZenithVerts {
            get { return _numberZenithVerts; }
            set {
                if (_numberZenithVerts != value)
                {
                    _numberZenithVerts = value;
                    _iChanged = true;
                }
            }
        }

        private int _numberAzimuthVerts = 64;
        public int NumberAzimuthVerts {
            get { return _numberAzimuthVerts; }
            set {
                if (_numberAzimuthVerts != value)
                {
                    _numberAzimuthVerts = value;
                    _iChanged = true;
                }
            }
        }

        private float _vectorSize = 0.03f;
        public float VectorSize {
            get { return _vectorSize; }
            set {
                if (_vectorSize != value)
                {
                    _vectorSize = value;
                    _iChanged = true;
                }
            }
        }

        private float _lineWidth = 0.003f;
        public float LineWidth {
            get { return _lineWidth; }
            set {
                if (_lineWidth != value)
                {
                    _lineWidth = value;
                    _iChanged = true;
                }
            }
        }

        private double _distance = 1f;
        public double Distance {
            get { return _distance; }
            set {
                if (_distance != value)
                {
                    _distance = value;
                    _iChanged = true;
                }
            }
        }

        public Values()
        {
        }

        public void Save(PluginConfiguration config)
        {
            SetColor(config, "upperHalfColor", _upperHalfColor);
            SetColor(config, "lowerHalfColor", _lowerHalfColor);
            SetColor(config, "horizonColor", _horizonColor);
            SetColor(config, "azimuthColor", _azimuthColor);
            SetColor(config, "northColor", _northColor);
            SetColor(config, "eastColor", _eastColor);
            SetColor(config, "southColor", _southColor);
            SetColor(config, "westColor", _westColor);
            SetColor(config, "headingColor", _headingColor);
            SetColor(config, "alignmentColor", _alignmentColor);
            SetColor(config, "progradeColor", _progradeColor);
            SetColor(config, "normalColor", _normalColor);
            SetColor(config, "radialColor", _radialColor);
            SetColor(config, "targetColor", _targetColor);
            SetColor(config, "maneuverColor", _maneuverColor);
            config.SetValue("numberZenithLinesHalf", _numberZenithLinesHalf);
            config.SetValue("numberAzimuthLinesQuarter", _numberAzimuthLinesQuarter);
            config.SetValue("numberZenithVerts", _numberZenithVerts);
            config.SetValue("numberAzimuthVerts", _numberAzimuthVerts);
            config.SetValue("vectorSize", (double)_vectorSize);
            config.SetValue("lineWidth", (double)_lineWidth);
            config.SetValue("distance", (double)_distance);
        }

        public void Load(PluginConfiguration config)
        {
            _upperHalfColor = GetColor(config, "upperHalfColor");
            _lowerHalfColor = GetColor(config, "lowerHalfColor");
            _horizonColor = GetColor(config, "horizonColor");
            _azimuthColor = GetColor(config, "azimuthColor");
            _northColor = GetColor(config, "northColor");
            _eastColor = GetColor(config, "eastColor");
            _southColor = GetColor(config, "southColor");
            _westColor = GetColor(config, "westColor");
            _headingColor = GetColor(config, "headingColor");
            _alignmentColor = GetColor(config, "alignmentColor");
            _progradeColor = GetColor(config, "progradeColor");
            _normalColor = GetColor(config, "normalColor");
            _radialColor = GetColor(config, "radialColor");
            _targetColor = GetColor(config, "targetColor");
            _maneuverColor = GetColor(config, "maneuverColor");
            _numberZenithLinesHalf = config.GetValue<int>("numberZenithLinesHalf");
            _numberAzimuthLinesQuarter = config.GetValue<int>("numberAzimuthLinesQuarter");
            _numberZenithVerts = config.GetValue<int>("numberZenithVerts");
            _numberAzimuthVerts = config.GetValue<int>("numberAzimuthVerts");
            _vectorSize = (float)config.GetValue<double>("vectorSize");
            _lineWidth = (float)config.GetValue<double>("lineWidth");
            _distance = config.GetValue<double>("distance");
            _iChanged = true;
        }

        private void SetColor(PluginConfiguration config, string text, Color color)
        {
            config.SetValue(text + "R", (double)color.r);
            config.SetValue(text + "G", (double)color.g);
            config.SetValue(text + "B", (double)color.b);
            config.SetValue(text + "A", (double)color.a);
        }

        private Color GetColor(PluginConfiguration config, string text)
        {
            float r = (float)config.GetValue<double>(text + "R");
            float g = (float)config.GetValue<double>(text + "G");
            float b = (float)config.GetValue<double>(text + "B");
            float a = (float)config.GetValue<double>(text + "A", 1.0d);
            return new Color(r, g, b, a);
        }
    }
}
