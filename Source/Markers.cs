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
	public class Markers
	{
		private GameObject[] _objects;
		
		public const int Prograde   = 0;
		public const int Retrograde = 1;
		public const int Normal     = 2;
		public const int Antinormal = 3;
		public const int Radial     = 4;
		public const int Antiradial = 5;
		public const int Target     = 6;
		public const int Antitarget = 7;
		public const int Maneuver   = 8;
		
		private double _r;
		
		public Markers()
		{
			_objects = new GameObject[9];
			
			_objects[Normal]     = CreateMarker(new Vector2(0.0f, 0.0f));
			_objects[Antinormal] = CreateMarker(new Vector2(1f/3f, 0.0f));
			_objects[Maneuver]   = CreateMarker(new Vector2(2f/3f, 0.0f));
			_objects[Radial]     = CreateMarker(new Vector2(0.0f, 1f/3f));
			_objects[Antiradial] = CreateMarker(new Vector2(1f/3f, 1f/3f));
			_objects[Antitarget] = CreateMarker(new Vector2(2f/3f, 1f/3f));
			_objects[Prograde]   = CreateMarker(new Vector2(0.0f, 2f/3f));
			_objects[Retrograde] = CreateMarker(new Vector2(1f/3f, 2f/3f));
			_objects[Target]     = CreateMarker(new Vector2(2f/3f, 2f/3f));
			
		}
		
		private GameObject CreateMarker(Vector2 textureOffset)
		{
        	GameObject marker = CreateSimplePlane();
        	// Get the texture (code from enhancedNavBall)
        	Texture texture = MapView.ManeuverNodePrefab.GetComponent<ManeuverGizmo>().handleNormal.flag.renderer.sharedMaterial.mainTexture;
        	marker.renderer.material = new Material(Shader.Find( "Particles/Additive" ));
        	marker.renderer.material.mainTexture = texture;
        	marker.renderer.material.mainTextureScale = Vector2.one / 3;
        	marker.renderer.material.mainTextureOffset = textureOffset;
        	return marker;
        }
		
		// code from enhancednavball
        private GameObject CreateSimplePlane()
        {
            Mesh mesh = new Mesh();

            GameObject obj = new GameObject();
            MeshFilter meshFilter = obj.AddComponent<MeshFilter>();
            obj.AddComponent<MeshRenderer>();
			obj.layer = 5;
				
            const float uvize = 1f;

            Vector3 p0 = new Vector3(-1, -1, 0);
            Vector3 p1 = new Vector3( 1, -1, 0);
            Vector3 p2 = new Vector3(-1,  1, 0);
            Vector3 p3 = new Vector3( 1,  1, 0);

            mesh.vertices = new[]
            {
                p0, p1, p2,
                p1, p3, p2
            };

            mesh.triangles = new[]
            {
                0, 1, 2,
                3, 4, 5
            };

            Vector2 uv1 = new Vector2(0, 0);
            Vector2 uv2 = new Vector2(uvize, uvize);
            Vector2 uv3 = new Vector2(0, uvize);
            Vector2 uv4 = new Vector2(uvize, 0);

            mesh.uv = new[]{
                uv1, uv4, uv3,
                uv4, uv2, uv3
            };
            
            mesh.RecalculateNormals();
            mesh.RecalculateBounds();
            mesh.Optimize();
            
            meshFilter.mesh = mesh;

            return obj;
        }
        
        public void SetValues(Values values)
        {
        	_r = values.Distance;
        	_objects[Prograde  ].renderer.material.SetColor("_TintColor", values.ProgradeColor);
			_objects[Retrograde].renderer.material.SetColor("_TintColor", values.ProgradeColor);
			_objects[Normal    ].renderer.material.SetColor("_TintColor", values.NormalColor);
			_objects[Antinormal].renderer.material.SetColor("_TintColor", values.NormalColor);
			_objects[Radial    ].renderer.material.SetColor("_TintColor", values.RadialColor);
			_objects[Antiradial].renderer.material.SetColor("_TintColor", values.RadialColor);
			_objects[Target    ].renderer.material.SetColor("_TintColor", values.TargetColor);
			_objects[Antitarget].renderer.material.SetColor("_TintColor", values.TargetColor);
			_objects[Maneuver  ].renderer.material.SetColor("_TintColor", values.ManeuverColor);
			for(int i=0; i<9; i++){
				_objects[i].transform.localScale = values.VectorSize*Vector3.one;
			}
        }
		
		public void SetParent(Transform parent)
		{
			for(int i=0; i<9; i++) ParentVector(_objects[i], parent);
		}
		
		private void ParentVector(GameObject vector, Transform parent)
        {
        	vector.transform.parent = parent;
        	vector.transform.localPosition = Vector3.zero;
        	vector.transform.localEulerAngles = Vector3.zero;
        }
		
		public void SetDirections(Vector3d prograde, Vector3d normal, Vector3d radial)
		{
			_objects[Prograde  ].transform.localPosition =  _r*prograde;
			_objects[Retrograde].transform.localPosition = -_r*prograde;
			_objects[Normal    ].transform.localPosition =  _r*normal;
			_objects[Antinormal].transform.localPosition = -_r*normal;
			_objects[Radial    ].transform.localPosition =  _r*radial;
			_objects[Antiradial].transform.localPosition = -_r*radial;
		}
		
		public void SetTarget(Vector3d target)
		{
			_objects[Target    ].transform.localPosition =  _r*target;
           	_objects[Antitarget].transform.localPosition = -_r*target;
		}
		
		public void SetManeuver(Vector3d maneuver)
		{
			_objects[Maneuver  ].transform.localPosition = _r*maneuver;
		}
		
		public void SetDirectionsActive(bool active)
		{
			for(int i=0; i<6; i++) _objects[i].SetActive(active);
		}
		
		public void SetTargetActive(bool active)
		{
			_objects[Target    ].SetActive(active);
			_objects[Antitarget].SetActive(active);
		}
		
		public void SetManeuverActive(bool active)
		{
			_objects[Maneuver].SetActive(active);
		}
	}
}
