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
using System;
using KSP;
using UnityEngine;
using System.Collections;

namespace NavHud
{
    public class TargetAlignmentMarker
    {
        private LineRenderer[] _lines;
        private GameObject[] _objects;

        private double _r;
        private float _lineWidth;

        public TargetAlignmentMarker()
        {
            _lines = new LineRenderer[6];
            _objects = new GameObject[6];

            for (int i = 0; i < _lines.Length; i++)
            {
                _objects[i] = new GameObject();
                _objects[i].layer = 5;

                // Add line
                _lines[i] = _objects[i].AddComponent< LineRenderer >() as LineRenderer;
                _lines[i].renderer.material = new Material(Shader.Find("Particles/Additive"));
                _lines[i].SetVertexCount(2);
            }
        }

        public void SetValues(Values values)
        {
            _lineWidth = values.LineWidth;
            _r = values.Distance;

            for (int i = 0; i < _lines.Length; i++)
            {
                _lines[i].SetWidth(_lineWidth, _lineWidth);
                _lines[i].SetColors(values.AlignmentColor, values.AlignmentColor);
            }
        }

        public void SetActive(bool active)
        {
            for (int i = 0; i < _objects.Length; i++)
            {
                _objects[i].SetActive(active);
            }
        }

        public void SetParent(Transform parent)
        {
            for (int i = 0; i < _lines.Length; i++)
            {
                // Parent the object to the camera -- this is probably not nessesary.
                //objects[j].transform.parent = cam.transform;
                _lines[i].transform.parent = parent;
                _lines[i].useWorldSpace = false;
                _lines[i].transform.localPosition = Vector3.zero;
                _lines[i].transform.localEulerAngles = Vector3.zero;
            }
        }

        public void SetPositions(Vector3d forward, Vector3d right, Vector3d up)
        {
            _lines[0].SetPosition(0, _r * (forward - 0.1f * right));
            _lines[0].SetPosition(1, _r * (forward + 0.1f * right));

            _lines[1].SetPosition(0, _r * (forward - _lineWidth * 2 * up));
            _lines[1].SetPosition(1, _r * (forward - 0.1f * up));

            _lines[2].SetPosition(0, _r * (forward + _lineWidth * 2 * up));
            _lines[2].SetPosition(1, _r * (forward + 0.1f * up));

            _lines[3].SetPosition(0, _r * (-forward - 0.1f * right));
            _lines[3].SetPosition(1, _r * (-forward + 0.1f * right));

            _lines[4].SetPosition(0, _r * (-forward - _lineWidth * 2 * up));
            _lines[4].SetPosition(1, _r * (-forward - 0.1f * up));

            _lines[5].SetPosition(0, _r * (-forward + _lineWidth * 2 * up));
            _lines[5].SetPosition(1, _r * (-forward + 0.1f * up));
        }
    }
}
