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
        public Camera HudCam {
            get { return _hudCam; }
            set {
                _hudCam = value;
                SetParent(value.transform);
            }
        }

        private Values _values;
        public Values Values {
            get { return _values; }
            set {
                _values = value;
                SetValues(value);
            }
        }

        private bool _active, _linesActive, _markersActive, _maneuverActive, _targetActive, _alignActive, _waypointActive;
        private bool _enabled, _linesEnabled, _markersEnabled, _waypointEnabled;

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

        public bool WaypointEnabled {
            get { return _waypointEnabled; }
            set { _waypointEnabled = value; }
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
        private EdgeMarkers _edgeMarkers;
        private WaypointMarker _waypointMarker;

        private double _waypointSurfHeight = 0;

        private Vector3 _smoothVel = Vector3.zero;
        //private Vector3 _estAccel = Vector3.zero;

        public MainBehaviour()
        {
            // get camera 0 this is the scaled space camera that is active in both flight and mapview
            _mainCam = Camera.allCameras[0];

            _headingMarker = new HeadingMarker();
            _zenithLines = new ZenithLines();
            _azimuthLines = new AzimuthLines();
            _targetAlignmentMarker = new TargetAlignmentMarker();
            _markers = new Markers();
            _waypointMarker = new WaypointMarker();
            _edgeMarkers = new EdgeMarkers();
        }

        void Start()
        {
        }

        void FixedUpdate()
        {
            Vector3 vel;
            switch (FlightUIController.speedDisplayMode)
            {
            case FlightUIController.SpeedDisplayModes.Surface:
                vel = FlightGlobals.ship_srfVelocity;
                break;

            case FlightUIController.SpeedDisplayModes.Orbit:
                vel = FlightGlobals.ship_obtVelocity;
                break;

            case FlightUIController.SpeedDisplayModes.Target:
                vel = FlightGlobals.ship_tgtVelocity;
                break;

            default:
                throw new ArgumentOutOfRangeException();
            }

            if(_smoothVel.magnitude<1.0f)
            {
                //float speed = vel.magnitude;
                //Vector3 nSmoothVel = (_smoothVel+_estAccel)*(1.0f-speed) + speed*vel;
                //_estAccel = nSmoothVel-_smoothVel;
                //_smoothVel = nSmoothVel;
                _smoothVel += vel.magnitude*(vel-_smoothVel);
            } else {
                //_estAccel = Vector3.zero;
                _smoothVel = vel;
            }
        }

        void OnPreCull()
        {
            if (Enabled && (!MapView.MapIsEnabled || EnabledMap))
            {
                if (!_active)
                {
                    _headingMarker.SetActive(true);
                    _edgeMarkers.SetHeadingActive(true);
                    _active = true;
                }
                Matrix4x4 worldToCamMat = _mainCam.transform.worldToLocalMatrix;
                _hudCam.fieldOfView = _mainCam.fieldOfView;
                Vector3 screenEdge = _hudCam.ScreenPointToRay(Vector3.zero).direction;
                UpdateHeadingMarkers(worldToCamMat, screenEdge);
                if (LinesEnabled)
                {
                    if (!_linesActive)
                    {
                        _zenithLines.SetActive(true);
                        _azimuthLines.SetActive(true);
                        _linesActive = true;
                    }
                    UpdateLines(worldToCamMat);
                } else {
                    if (_linesActive)
                    {
                        _zenithLines.SetActive(false);
                        _azimuthLines.SetActive(false);
                        _linesActive = false;
                    }
                }
                if (MarkersEnabled)
                {
                    if (!_markersActive)
                    {
                        _markers.SetDirectionsActive(true);
                        _edgeMarkers.SetDirectionsActive(true);
                        _markersActive = true;
                    }
                    UpdateMarkers(worldToCamMat, screenEdge);
                    if (FlightGlobals.fetch.VesselTarget != null)
                    {
                        if (!_targetActive)
                        {
                            _markers.SetTargetActive(true);
                            _edgeMarkers.SetTargetActive(true);
                            _targetActive = true;
                        }
                        UpdateTargetMarkers(worldToCamMat, screenEdge);
                        if (FlightGlobals.fetch.VesselTarget.GetTargetingMode() == VesselTargetModes.DirectionVelocityAndOrientation)
                        {
                            if (!_alignActive)
                            {
                                _targetAlignmentMarker.SetActive(true);
                                _edgeMarkers.SetAlignmentActive(true);
                                _alignActive = true;
                            }
                            UpdateAlignMarkers(worldToCamMat, screenEdge);
                        } else {
                            if (_alignActive)
                            {
                                _targetAlignmentMarker.SetActive(false);
                                _edgeMarkers.SetAlignmentActive(false);
                                _alignActive = false;
                            }
                        }
                    } else {
                        if (_targetActive)
                        {
                            _markers.SetTargetActive(false);
                            _edgeMarkers.SetTargetActive(false);
                            _targetActive = false;
                        }
                        if (_alignActive)
                        {
                            _targetAlignmentMarker.SetActive(false);
                            _edgeMarkers.SetAlignmentActive(false);
                            _alignActive = false;
                        }
                    }
                    if (FlightGlobals.ActiveVessel.patchedConicSolver != null && FlightGlobals.ActiveVessel.patchedConicSolver.maneuverNodes.Count > 0)
                    {
                        if (!_maneuverActive)
                        {
                            _markers.SetManeuverActive(true);
                            _edgeMarkers.SetManeuverActive(true);
                            _maneuverActive = true;
                        }
                        UpdateManeuverMarker(worldToCamMat, screenEdge);
                    } else {
                        if (_maneuverActive)
                        {
                            _markers.SetManeuverActive(false);
                            _edgeMarkers.SetManeuverActive(false);
                            _maneuverActive = false;
                        }
                    }
                    if (FinePrint.WaypointManager.navIsActive() && _waypointEnabled)
                    {
                        if (!_waypointActive) 
                        {
                            _waypointMarker.SetActive(true);
                            _edgeMarkers.SetWaypointActive(true);
                            _waypointActive = true;
                            _waypointMarker.LoadTexture();
                            _edgeMarkers.LoadWaypointColor();
                            if (FlightGlobals.ActiveVessel.mainBody.pqsController != null)
                            {
                                NavWaypoint navWp = FinePrint.WaypointManager.navWaypoint;
                                Vector3d pqsRadialVector = QuaternionD.AngleAxis(navWp.longitude, Vector3d.down) * QuaternionD.AngleAxis(navWp.latitude, Vector3d.forward) * Vector3d.right;
                                _waypointSurfHeight = FlightGlobals.ActiveVessel.mainBody.pqsController.GetSurfaceHeight(pqsRadialVector)
                                    - FlightGlobals.ActiveVessel.mainBody.pqsController.radius;
                                if (_waypointSurfHeight < 0)
                                {
                                    _waypointSurfHeight = 0;
                                }
                            }
                        }
                        UpdateWaypointMarker(worldToCamMat, screenEdge);
                    } else {
                        if (_waypointActive)
                        {
                            _waypointMarker.SetActive(false);
                            _edgeMarkers.SetWaypointActive(false);
                            _waypointActive = false;
                        }
                    }

                } else {
                    if (_markersActive)
                    {
                        _markers.SetDirectionsActive(false);
                        _edgeMarkers.SetDirectionsActive(false);
                        _markersActive = false;
                        if (_targetActive)
                        {
                            _markers.SetTargetActive(false);
                            _edgeMarkers.SetTargetActive(false);
                            _targetActive = false;
                        }
                        if (_alignActive)
                        {
                            _targetAlignmentMarker.SetActive(false);
                            _edgeMarkers.SetTargetActive(false);
                            _alignActive = false;
                        }
                        if (_maneuverActive)
                        {
                            _markers.SetManeuverActive(false);
                            _edgeMarkers.SetManeuverActive(false);
                            _maneuverActive = false;
                        }
                        if (_waypointActive)
                        {
                            _waypointMarker.SetActive(false);
                            _edgeMarkers.SetWaypointActive(false);
                            _waypointActive = false;
                        }
                    }
                }
            } else {
                if (_active)
                {
                    _headingMarker.SetActive(false);
                    _edgeMarkers.SetHeadingActive(false);
                    _active = false;
                    if (_linesActive)
                    {
                        _zenithLines.SetActive(false);
                        _azimuthLines.SetActive(false);
                        _linesActive = false;
                    }
                    if (_markersActive)
                    {
                        _markers.SetDirectionsActive(false);
                        _edgeMarkers.SetDirectionsActive(false);
                        _markersActive = false;
                    }
                    if (_targetActive)
                    {
                        _markers.SetTargetActive(false);
                        _edgeMarkers.SetTargetActive(false);
                        _targetActive = false;
                    }
                    if (_alignActive)
                    {
                        _targetAlignmentMarker.SetActive(false);
                        _edgeMarkers.SetAlignmentActive(false);
                        _alignActive = false;
                    }
                    if (_maneuverActive)
                    {
                        _markers.SetManeuverActive(false);
                        _edgeMarkers.SetManeuverActive(false);
                        _maneuverActive = false;
                    }
                    if (_waypointActive)
                    {
                        _waypointMarker.SetActive(false);
                        _edgeMarkers.SetWaypointActive(false);
                        _waypointActive = false;
                    }
                }
            }
        }

        void OnDestroy()
        {
            GameObject.Destroy(_hudCam.gameObject);
        }
        /*
        private void SetActive()
        {
            _zenithLines.SetActive(_active && _linesEnabled);
            _azimuthLines.SetActive(_active && _linesEnabled);
            _markers.SetDirectionsActive(_active && _markersEnabled);
            _targetAlignmentMarker.SetActive(_active && _markersEnabled && _alignActive);
            _markers.SetManeuverActive(_active && _markersEnabled && _maneuverActive);
            _markers.SetTargetActive(_active && _markersEnabled && _targetActive);
            _edgeMarkers.SetDirectionsActive(_active && _markersEnabled && _edgeMarkersEnabled);
            _edgeMarkers.SetAlignmentActive(_active && _markersEnabled && _edgeMarkersEnabled && _alignActive);
            _edgeMarkers.SetManeuverActive(_active && _markersEnabled && _edgeMarkersEnabled && _maneuverActive);
            _edgeMarkers.SetTargetActive(_active && _markersEnabled && _edgeMarkersEnabled && _targetActive);
        }*/

        private void SetValues(Values values)
        {
            _headingMarker.SetValues(values);
            _zenithLines.SetValues(values);
            _azimuthLines.SetValues(values);
            _targetAlignmentMarker.SetValues(values);
            _markers.SetValues(values);
            _waypointMarker.SetValues(values);
            _edgeMarkers.SetValues(values);
        }

        private void SetParent(Transform parent)
        {
            _headingMarker.SetParent(parent);
            _azimuthLines.SetParent(parent);
            _zenithLines.SetParent(parent);
            _targetAlignmentMarker.SetParent(parent);
            _markers.SetParent(parent);
            _waypointMarker.SetParent(parent);
            _edgeMarkers.SetParent(parent);
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

        private void UpdateHeadingMarkers(Matrix4x4 worldToCamMat, Vector3 screenEdge)
        {
            Vector3d hdg = worldToCamMat.MultiplyVector(FlightGlobals.ActiveVessel.ReferenceTransform.up).normalized;
            Vector3d rgt = worldToCamMat.MultiplyVector(FlightGlobals.ActiveVessel.ReferenceTransform.right).normalized;
            Vector3d dwn = worldToCamMat.MultiplyVector(FlightGlobals.ActiveVessel.ReferenceTransform.forward).normalized;
            _headingMarker.SetPositions(hdg, rgt, dwn);
            _edgeMarkers.SetHeading(hdg, screenEdge);
        }

        private void UpdateMarkers(Matrix4x4 worldToCamMat, Vector3 screenEdge)
        {
            Vector3d up = worldToCamMat.MultiplyVector(FlightGlobals.upAxis).normalized;
            /*Vector3d prograde;
            switch (FlightUIController.speedDisplayMode)
            {
            case FlightUIController.SpeedDisplayModes.Surface:
                prograde = FlightGlobals.ship_srfVelocity;
                break;

            case FlightUIController.SpeedDisplayModes.Orbit:
                prograde = FlightGlobals.ship_obtVelocity;
                break;

            case FlightUIController.SpeedDisplayModes.Target:
                prograde = FlightGlobals.ship_tgtVelocity;
                break;

            default:
                throw new ArgumentOutOfRangeException();
            }*/
            Vector3d pgd = worldToCamMat.MultiplyVector(_smoothVel).normalized;
            Vector3d nrm = Vector3.Cross(pgd, up).normalized;
            Vector3d rad = Vector3.Cross(pgd, nrm).normalized;

            _markers.SetDirections(pgd, nrm, rad);
            _edgeMarkers.SetDirections(pgd, nrm, rad, screenEdge);
        }

        private void UpdateTargetMarkers(Matrix4x4 worldToCamMat, Vector3 screenEdge)
        {
            Vector3 target = worldToCamMat.MultiplyVector(FlightGlobals.fetch.vesselTargetTransform.position - FlightGlobals.ActiveVessel.ReferenceTransform.position).normalized;
            _markers.SetTarget(target);
            _edgeMarkers.SetTarget(target, screenEdge);
        }

        private void UpdateAlignMarkers(Matrix4x4 worldToCamMat, Vector3 screenEdge)
        {
            Transform targetTranform = FlightGlobals.fetch.vesselTargetTransform;
            Vector3d fwd = worldToCamMat.MultiplyVector(targetTranform.forward).normalized;
            Vector3d rgt = worldToCamMat.MultiplyVector(targetTranform.right).normalized;
            Vector3d up = worldToCamMat.MultiplyVector(targetTranform.up).normalized;
            _targetAlignmentMarker.SetPositions(fwd, rgt, up);
            _edgeMarkers.SetAlignment(fwd, screenEdge);
        }

        private void UpdateManeuverMarker(Matrix4x4 worldToCamMat, Vector3 screenEdge)
        {
            Vector3d burnvector = FlightGlobals.ActiveVessel.patchedConicSolver.maneuverNodes[0].GetBurnVector(FlightGlobals.ActiveVessel.orbit);
            Vector3 maneuver = worldToCamMat.MultiplyVector(burnvector).normalized;
            _markers.SetManeuver(maneuver);
            _edgeMarkers.SetManeuver(maneuver, screenEdge);
        }

        private void UpdateWaypointMarker(Matrix4x4 worldToCamMat, Vector3 screenEdge){
            NavWaypoint navWp = FinePrint.WaypointManager.navWaypoint;
            //I can't find a way to get the reference body of the waypoint so I'm using the activevessel's mainbody
            Vector3d waypointWorldPos = FlightGlobals.ActiveVessel.mainBody.GetWorldSurfacePosition(navWp.latitude, navWp.longitude, navWp.altitude + _waypointSurfHeight);
            Vector3 waypoint = worldToCamMat.MultiplyVector(waypointWorldPos - FlightGlobals.ActiveVessel.ReferenceTransform.position).normalized;
            _waypointMarker.SetPositions(waypoint);
            _edgeMarkers.SetWaypoint(waypoint, screenEdge);
        }
    }
}
