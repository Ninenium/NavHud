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
	public class AzimuthLines
	{
		private LineRenderer[] _lines; 		//Can only have one linerender per object..
        private GameObject[] _objects;		//So create one game object for every line.
        private Transform _parent;
        private bool _active = false;
        private bool _hasParent = false;
  		private int _length;
  		
		public AzimuthLines()
		{			
  			_lines = new LineRenderer[0];
        	_objects = new GameObject[0];
		}
		
		public void SetValues(Values values)
		{
			_length = values.NumberAzimuthLinesQuarter*4;
			
  			double r = values.Distance;
  			int oldLength = _lines.Length;
  			if(oldLength<_length){
  				
  				Array.Resize(ref _lines, _length);
  				Array.Resize(ref _objects, _length);
  				
  				for(int i=oldLength; i<_lines.Length; i++){
        			_objects[i] = new GameObject();
        			// layers 0,1,15,19 <32
        			_objects[i].layer = 5;
        		        		
        			// Add line
        			_lines[i] = _objects[i].AddComponent< LineRenderer >() as LineRenderer;
        			_lines[i].renderer.material = new Material( Shader.Find( "Particles/Additive" ));
  				}
  			}
  			
  			float t;
            float p;
        	            
            for(int i=0; i<_lines.Length; i++){
        		_lines[i].SetWidth( values.LineWidth, values.LineWidth );
        		_lines[i].SetVertexCount(values.NumberAzimuthVerts);
        		
            	p = 2*i*Mathf.PI/_length;
            	for (int j=0; j<values.NumberAzimuthVerts; j++){
            		t = Mathf.PI*((float)j / (values.NumberAzimuthVerts-1) - 0.5f);
            		_lines[i].SetPosition(j, r* new Vector3d(Mathf.Cos(p)*Mathf.Cos(t), Mathf.Sin(p)*Mathf.Cos(t), Mathf.Sin(t)));
            	}
            	_objects[i].SetActive(_active);
        	} 
            
            for(int i=_length; i<_objects.Length; i++){
            	_objects[i].SetActive(false);
            }
            		
			for(int i=0; i<_length; i++) if(i%values.NumberAzimuthLinesQuarter!=0) _lines[i].SetColors(values.AzimuthColor, values.AzimuthColor);
			_lines[0].SetColors(values.NorthColor, values.NorthColor);
			_lines[values.NumberAzimuthLinesQuarter].SetColors(values.EastColor, values.EastColor);
			_lines[2*values.NumberAzimuthLinesQuarter].SetColors(values.SouthColor, values.SouthColor);
			_lines[3*values.NumberAzimuthLinesQuarter].SetColors(values.WestColor, values.WestColor);
			
			if(_hasParent) SetParent(_parent);
		}
		
		public void SetActive(bool active)
		{
			_active = active;
			for(int i=0; i<_length; i++) _objects[i].SetActive(active);
		}
		
		public void SetParent(Transform parent)
		{
			for(int i=0; i<_lines.Length; i++){
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
		
		public void SetPositions(Vector3d up, Vector3d east)
		{     
            Quaternion rotation = Quaternion.LookRotation(up, east);
            for(int i=0; i<_length; i++){
            	_lines[i].transform.localRotation = rotation;
            }
		}
	}
}
