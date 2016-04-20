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
    /// <summary>
    /// Description of Heading.
    /// </summary>
    public class HeadingMarker
    {
        private LineRenderer[] _lines;
        private GameObject[] _objects;
        private float _size;
        private double _r;

        public HeadingMarker()
        {
            _lines = new LineRenderer[4];
            _objects = new GameObject[4];

            for (int i = 0; i < _lines.Length; i++)
            {
                _objects[i] = new GameObject();
                _objects[i].layer = 7;
                // Add line
                _lines[i] = _objects[i].AddComponent< LineRenderer >() as LineRenderer;
                _lines[i].GetComponent<Renderer>().material = new Material(Shader.Find("Particles/Additive"));
            }
            _lines[0].SetVertexCount(6);
            _lines[1].SetVertexCount(2);
            _lines[2].SetVertexCount(10);
            _lines[3].SetVertexCount(2);
        }

        public void SetValues(Values values)
        {
            _r = values.Distance;
            _size = values.VectorSize;

            _lines[0].SetWidth(values.LineWidth, values.LineWidth);
            _lines[1].SetWidth(1f * _size, 0.0f);
            _lines[2].SetWidth(values.LineWidth, values.LineWidth);
            _lines[3].SetWidth(1f * _size, 0.0f);

            for (int i = 0; i < _lines.Length; i++)
            {
                _lines[i].SetColors(values.HeadingColor, values.HeadingColor);
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

        public void SetPositions(Vector3d heading, Vector3d right, Vector3d down)
        {
            float a = _size * 3.0f;
            float b = _size * 0.7f;
            float c = _size * 1.0f;
            Vector3d down2 = new Vector3d(down.x, down.y, 0);
            _lines[0].SetPosition(0, _r * (heading + a * right));
            _lines[0].SetPosition(1, _r * (heading + b * right));
            _lines[0].SetPosition(2, _r * (heading + b * down + 0.001f * right));
            _lines[0].SetPosition(3, _r * (heading + b * down - 0.001f * right));
            _lines[0].SetPosition(4, _r * (heading - b * right));
            _lines[0].SetPosition(5, _r * (heading - a * right));

            _lines[1].SetPosition(0, _r * (heading - 0.5 * c * down2));
            _lines[1].SetPosition(1, _r * (heading));

            _lines[2].SetPosition(0, _r * (-heading + a * right));
            _lines[2].SetPosition(1, _r * (-heading + c * right));
            _lines[2].SetPosition(2, _r * (-heading + 0.5 * c * (-down + 1.001f * right)));
            _lines[2].SetPosition(3, _r * (-heading + 0.5 * c * (-down + 0.999f * right)));
            _lines[2].SetPosition(4, _r * (-heading + 0.001f * right));
            _lines[2].SetPosition(5, _r * (-heading - 0.001f * right));
            _lines[2].SetPosition(6, _r * (-heading + 0.5 * c * (-down - 0.999f * right)));
            _lines[2].SetPosition(7, _r * (-heading + 0.5 * c * (-down - 1.001f * right)));
            _lines[2].SetPosition(8, _r * (-heading - c * right));
            _lines[2].SetPosition(9, _r * (-heading - a * right));

            _lines[3].SetPosition(0, _r * (-heading + 0.5 * c * down2));
            _lines[3].SetPosition(1, _r * (-heading));
        }
    }
}
