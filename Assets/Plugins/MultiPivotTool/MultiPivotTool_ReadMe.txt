*****MultiPivot Tool*****
by Turmonious Games

This tool will allow you to quickly and easily define multiple pivot points for a GameObject and rotate the object around them in the edtior or in code.

*****Quick Start Guide*****
0) Have the MultiPivot Tool imported into your project
1) Select a GameObject in your Hierarchy that you want to use the MultiPivot Tool on
2) Press ctrl + e, you should see both a rotation handle and position handle on the center of the selected object
3) Drag the position handle
4) Rotate your object around it's new pivot point
5) ???
6) Profit



The MultiPivot Tool defines a component which stores a list of alternative pivot points for that object. You can add this component to any GameObject and start adding Pivots to it. You can subsequently move them around, define their properties, and use them to easily rotate your GameObjects around any of them.

To make your life easier, we have defined a new keyboard shortcut for working with your new pivot points. Pressing Ctrl+E while selecting a GameObject that does not have a PivotSystem component will add one, initialize the list to contain a pivot, and select that pivot. If it does contain one, it will simply select the first pivot. Continuing to press it will cycle which pivot is selected.

*****Pivot Inspector Features*****
-Pivot (Edit Pivot) Button: This is a simple toggle button which controls whether or not you're currently working with the pivot in the
	scene. Only one pivot can be edited at a time. It works like a built-in handle, only one can be used at a time.
-Name: Simply, a name that identifies the pivot point
-Position: The position in world or local space, depending on the simulation mode
-Follow Object: This simply means that the position component of the Pivot handle will orient itself toward your GameObject
-Rotate By: This rotates the object around the pivot by the value entered.


*****Code Features*****
You can search the PivotSystem's pivots and work with it directly using the properties and methods provided. The most important of which is "Rotate(Quaternion pRot)". This method simply accepts a quaternion and rotates the GameObject around it by the amount defined by the quaternion.



*****Version*****

1.0:
-Initial Release