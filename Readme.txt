NavHud - 1.2.1 - by Ninenium
Some parts were written by Michel Dusseault (Addle or RealGrep on Github).

A heads up display with the markers from the NavBall and docking alignment indicator.
Compatible with: blizzy78's Toolbar mod.

Note: Holding LeftAlt while hitting the toggle key (default Y) cycles the speed display mode.

Installation:
Copy the content of the 'GameData' folder to the 'GameData' folder of your KSP installation.

License:
MIT

Change-log:
version 1.2.1
 - Added option to hide NavHud display when UI is hidden with F2 (Addle).
 - Added option to disable waypoint.
 - Replaced default button by button in applauncher.
version 1.2.0
 - Updated code for KSP 1.0 changes (Addle)
 - Made the burn time display a little better (minutes and others displayed rather than just total seconds) (Addle)
version 1.1.4
 - Added Waypoint support.
 - Fixed bug "NullReferenceExeption" when patched conics isn't unlocked.
version 1.1.3
 - Fixed bug where map view setting wasn't being saved and restored. (Addle)
 - Burn calculations now take into account reducing mass during the burn. (Addle)
version 1.1.2
 - Added triangles at screen edge that point to markers that are out of view.
 - Added smoothing to markers at speeds below 1 m/s.
 - Added a HUD text display so the navball can be closed completely without missing the information contained on it. Also added settings options for it. Note that holding LeftAlt and hitting the toggle key will cycle through the speed display modes (Surface, Orbit and Target). (Addle)
version 1.1.1
 - Fixed reset button bug (Addle)
 - Fixed alpha channel save bug (Addle)
 - Tweaked default alpha channel setting (Addle)
 - Changed default window position (Addle)
 - Added KSP-AVC support (Addle)
 - Tweaked heading marker
version 1.1
 - Changed settings window layout.
 - Added alpha channel option.
 - Improved target alignment support for mods. (Thanks to taniwha.)
 - The HUD is now rendered by its own camera positioned in the origin. This removes jitter and clipping issues.
 - Added anti-heading marker.
 - HUD is visible in the mapview.
 - Fixed bug: memory leak when settings window is open.
Version 1.0.1
 - Added hot key.
 - Fixed bug: No HUD when reverting flight from mapview.
Version 1.0
 - First release.
