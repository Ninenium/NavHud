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
    public class ZenithLines
    {
        private LineRenderer[] _lines;
        //Can only have one linerender per object..
        private GameObject[] _objects;
        //So create one game object for every line.
        private Transform _parent;
        private bool _active = false;
        private bool _hasParent = false;
        private int _length;

        public ZenithLines()
        {
            _lines = new LineRenderer[0];
            _objects = new GameObject[0];
        }

        public void SetValues(Values values)
        {
            _length = values.NumberZenithLinesHalf * 2 + 1;

            double r = values.Distance;
            int oldLength = _lines.Length;
            if (oldLength < _length)
            {
                Array.Resize(ref _lines, _length);
                Array.Resize(ref _objects, _length);

                for (int i = oldLength; i < _lines.Length; i++)
                {
                    _objects[i] = new GameObject();
                    // layers 0,1,15,19 <32
                    _objects[i].layer = 7;

                    // Add line
                    _lines[i] = _objects[i].AddComponent< LineRenderer >() as LineRenderer;
                    _lines[i].GetComponent<Renderer>().material = new Material(Shader.Find("Particles/Additive"));
                }
            }

            float t;
            float p;

            for (int i = 0; i < _length; i++)
            {
                _lines[i].SetWidth(values.LineWidth, values.LineWidth);
                _lines[i].SetVertexCount(values.NumberZenithVerts);

                p = Mathf.PI * ((float)(i + 1) / (_length + 1) - 0.5f);
                for (int j = 0; j < values.NumberZenithVerts; j++)
                {
                    t = 2 * j * Mathf.PI / (values.NumberZenithVerts - 1);
                    _lines[i].SetPosition(j, r * new Vector3d(-Mathf.Sin(t) * Mathf.Cos(p), Mathf.Cos(t) * Mathf.Cos(p), Mathf.Sin(p)));
                }
                _objects[i].SetActive(_active);
            }

            for (int i = _length; i < _objects.Length; i++)
            {
                _objects[i].SetActive(false);
            }

            for (int i = 0; i < values.NumberZenithLinesHalf; i++)
            {
                _lines[i].SetColors(values.LowerHalfColor, values.LowerHalfColor);
            }
            for (int i = values.NumberZenithLinesHalf + 1; i < _length; i++)
            {
                _lines[i].SetColors(values.UpperHalfColor, values.UpperHalfColor);
            }
            _lines[values.NumberZenithLinesHalf].SetColors(values.HorizonColor, values.HorizonColor);

            if (_hasParent)
            {
                SetParent(_parent);
            }
        }

        public void SetActive(bool active)
        {
            _active = active;
            for (int i = 0; i < _length; i++)
            {
                _objects[i].SetActive(_active);
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
            _parent = parent;
            _hasParent = true;
        }

        public void SetPositions(Vector3d up)
        {
            Quaternion rotation = Quaternion.FromToRotation(Vector3d.forward, up);
            for (int i = 0; i < _length; i++)
            {
                _lines[i].transform.localRotation = rotation;
            }
        }
    }
}
