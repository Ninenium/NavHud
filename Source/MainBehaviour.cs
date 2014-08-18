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
	public class MainBehaviour: MonoBehaviour
	{		
        private Camera _hudCam, _mainCam;
        public Camera HudCam{
        	get {return _hudCam;}
        	set {
        		_hudCam = value;
        		SetParent(value.transform);
        	}
        }
        private Values _values;
        public Values Values {
        	get {return _values;}
        	set {
        		_values = value;
        		SetValues(value);
        	}
        }
        private bool _active, _linesActive, _markersActive, _maneuverActive, _targetActive, _alignActive;
        private bool _enabled, _linesEnabled, _markersEnabled;
        public bool Enabled {
        	get { return _enabled; }
        	set { _enabled = value; }
        }
        public bool LinesEnabled {
        	get { return _linesEnabled; }
        	set { _linesEnabled = value; }
        }
        public bool MarkersEnabled {
        	get { return _markersEnabled; }
        	set { _markersEnabled = value; }
        }
        private bool _enabledMap = false;
        public bool EnabledMap {
			get { return _enabledMap; }
			set { _enabledMap = value; }
		}
       	private ZenithLines _zenithLines;
       	private AzimuthLines _azimuthLines;
       	private HeadingMarker _headingMarker;
       	private TargetAlignmentMarker _targetAlignmentMarker;
       	private Markers _markers;
        
        public MainBehaviour()
        {   
        	// get camera 0 this is the scaled space camera that is active in both flight and mapview
        	_mainCam = Camera.allCameras[0];
        	
        	_headingMarker = new HeadingMarker();
        	_zenithLines = new ZenithLines();
        	_azimuthLines = new AzimuthLines();
        	_targetAlignmentMarker = new TargetAlignmentMarker();
        	_markers = new Markers();
        	
        }
        
        void Start()
        {	
        }
        
        void OnPreCull()
        {
        	if(Enabled && ( !MapView.MapIsEnabled || EnabledMap ) )
        	{
        		if(!_active)
        		{
        			_headingMarker.SetActive(true);
        			_active = true;
        		}
        		Matrix4x4 worldToCamMat = _mainCam.transform.worldToLocalMatrix;
        		_hudCam.fieldOfView = _mainCam.fieldOfView;
        		UpdateHeadingMarkers(worldToCamMat);
        		if(LinesEnabled)
        		{
        			if(!_linesActive)
        			{
        				_zenithLines.SetActive(true);
        				_azimuthLines.SetActive(true);
        				_linesActive = true;
        			}
        			UpdateLines(worldToCamMat);
        		} else {
        			if(_linesActive)
        			{
        				_zenithLines.SetActive(false);
        				_azimuthLines.SetActive(false);
        				_linesActive = false;
        			}
        		}
        		if(MarkersEnabled)
        		{
        			if(!_markersActive)
        			{
        				_markers.SetDirectionsActive(true);
        				_markersActive = true;
        			}
        			UpdateMarkers(worldToCamMat);
        			if(FlightGlobals.fetch.VesselTarget!=null)
        			{
        				if(!_targetActive)
        				{
        					_markers.SetTargetActive(true);
        					_targetActive = true;
        				}
        				UpdateTargetMarkers(worldToCamMat);
        				if(FlightGlobals.fetch.VesselTarget.GetTargetingMode() == VesselTargetModes.DirectionVelocityAndOrientation)
        				{
        					if(!_alignActive)
        					{
        						_targetAlignmentMarker.SetActive(true);
        						_alignActive = true;
        					}
        					UpdateAlignMarkers(worldToCamMat);
        				}else{
        					if(_alignActive)
        					{
        						_targetAlignmentMarker.SetActive(false);
        						_alignActive = false;
        					}
        				}
        			}else{
        				if(_targetActive)
        				{
        					_markers.SetTargetActive(false);
        					_targetActive = false;
        				}
        				if(_alignActive)
        				{
        					_targetAlignmentMarker.SetActive(false);
        					_alignActive = false;
        				}
        			}
        			if(FlightGlobals.ActiveVessel.patchedConicSolver.maneuverNodes.Count > 0)
        			{
        				if(!_maneuverActive)
        				{
        					_markers.SetManeuverActive(true);
        					_maneuverActive = true;
        				}
        				UpdateManeuverMarker(worldToCamMat);
        			}else{
        				if(_maneuverActive)
        				{
        					_markers.SetManeuverActive(false);
        					_maneuverActive = false;
        				}
        			}
        		} else {
        			if(_markersActive)
        			{
        				_markers.SetDirectionsActive(false);
        				_markersActive = false;
        				if(_targetActive)
        				{
        					_markers.SetTargetActive(false);
        					_targetActive = false;
        				}
        				if(_alignActive)
        				{
        					_targetAlignmentMarker.SetActive(false);
        					_alignActive = false;
        				}
        				if(_maneuverActive)
        				{
        					_markers.SetManeuverActive(false);
        					_maneuverActive = false;
        				}
        			}
        		}
        	} else {
        		if(_active)
        		{
        			_headingMarker.SetActive(false);
        			_active = false;
        			if(_linesActive)
        			{
        				_zenithLines.SetActive(false);
        				_azimuthLines.SetActive(false);
        				_linesActive = false;
        			}
        			if(_markersActive)
        			{
        				_markers.SetDirectionsActive(false);
        				_markersActive = false;
        			}
        			if(_targetActive)
        			{
        				_markers.SetTargetActive(false);
        				_targetActive = false;
        			}
        			if(_alignActive)
        			{
        				_targetAlignmentMarker.SetActive(false);
        				_alignActive = false;
        			}
        			if(_maneuverActive)
        			{
        				_markers.SetManeuverActive(false);
        				_maneuverActive = false;
        			}
        		}
        	}
        }

        void OnDestroy()
        {
        	GameObject.Destroy(_hudCam.gameObject);
        }
        
        private void SetActive()
        {
        	_zenithLines.SetActive(_active&_linesEnabled);
        	_azimuthLines.SetActive(_active&_linesEnabled);
        	_markers.SetDirectionsActive(_active&_markersEnabled);
        	_targetAlignmentMarker.SetActive(_active&_markersEnabled&_alignActive);
        	_markers.SetManeuverActive(_active&_markersEnabled&_maneuverActive);
        	_markers.SetTargetActive(_active&_markersEnabled&_targetActive);
        }
        
        private void SetValues(Values values)
        {	
        	_headingMarker.SetValues(values);
        	_zenithLines.SetValues(values);
        	_azimuthLines.SetValues(values);
        	_targetAlignmentMarker.SetValues(values);
        	_markers.SetValues(values);
        }
        
        private void SetParent(Transform parent)
        {
        	_headingMarker.SetParent(parent);
        	_azimuthLines.SetParent(parent);
       		_zenithLines.SetParent(parent);
        	_targetAlignmentMarker.SetParent(parent);
       		_markers.SetParent(parent);
        }
        
		private void UpdateLines(Matrix4x4 worldToCamMat)
		{	
        	// Vectors pointing up and east (not normalized)
        	Vector3d position = FlightGlobals.ActiveVessel.findWorldCenterOfMass();
        	Vector3d worldUp = position - FlightGlobals.ActiveVessel.mainBody.position;
        	Vector3d worldEast = FlightGlobals.ActiveVessel.mainBody.getRFrmVel(position);
        	
            Vector3d up = worldToCamMat.MultiplyVector(worldUp).normalized;
            Vector3d east = worldToCamMat.MultiplyVector(worldEast).normalized;
            
            _zenithLines.SetPositions(up);
            _azimuthLines.SetPositions(up, east);          
        }
        
        private void UpdateHeadingMarkers(Matrix4x4 worldToCamMat)
        {        	
        	Vector3d hdg = worldToCamMat.MultiplyVector(FlightGlobals.ActiveVessel.ReferenceTransform.up).normalized;
           	Vector3d rgt = worldToCamMat.MultiplyVector(FlightGlobals.ActiveVessel.ReferenceTransform.right).normalized;
           	Vector3d dwn = worldToCamMat.MultiplyVector(FlightGlobals.ActiveVessel.ReferenceTransform.forward).normalized;           	
           	_headingMarker.SetPositions(hdg,rgt,dwn);
        }
        
        private void UpdateMarkers(Matrix4x4 worldToCamMat){
            Vector3d up = worldToCamMat.MultiplyVector(FlightGlobals.upAxis).normalized;
            Vector3d prograde;
            switch (FlightUIController.speedDisplayMode)
            {
                case FlightUIController.SpeedDisplayModes.Surface:
                    prograde = FlightGlobals.ship_srfVelocity.normalized;
                    break;

                case FlightUIController.SpeedDisplayModes.Orbit:
                    prograde = FlightGlobals.ship_obtVelocity.normalized;
                    break;

                case FlightUIController.SpeedDisplayModes.Target:
                    prograde = FlightGlobals.ship_tgtVelocity.normalized;
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
           	Vector3d pgd = worldToCamMat.MultiplyVector(prograde).normalized;
           	Vector3d nrm = Vector3.Cross(pgd,up).normalized;
           	Vector3d rad = Vector3.Cross(pgd,nrm).normalized;
           	           	
           	_markers.SetDirections(pgd, nrm, rad);
        }
        
        private void UpdateTargetMarkers(Matrix4x4 worldToCamMat)
        {
        	Transform targetTranform = FlightGlobals.fetch.vesselTargetTransform;	
           	_markers.SetTarget(worldToCamMat.MultiplyVector(targetTranform.position-FlightGlobals.ActiveVessel.ReferenceTransform.position).normalized);
        }
        
        private void UpdateAlignMarkers(Matrix4x4 worldToCamMat)
        {
        	Transform targetTranform = FlightGlobals.fetch.vesselTargetTransform;
        	Vector3d fwd = worldToCamMat.MultiplyVector(targetTranform.forward).normalized;
           	Vector3d rgt = worldToCamMat.MultiplyVector(targetTranform.right).normalized;
           	Vector3d up = worldToCamMat.MultiplyVector(targetTranform.up).normalized;
           	_targetAlignmentMarker.SetPositions(fwd, rgt, up);
        }
        
        private void UpdateManeuverMarker(Matrix4x4 worldToCamMat)
        {
        	Vector3d burnvector = FlightGlobals.ActiveVessel.patchedConicSolver.maneuverNodes[0].GetBurnVector(FlightGlobals.ActiveVessel.orbit);
           	_markers.SetManeuver(worldToCamMat.MultiplyVector(burnvector).normalized);
        }
	}
}
