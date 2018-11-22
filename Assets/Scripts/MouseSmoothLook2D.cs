/*******************************************************
 * 
 * MouseSmoothLook2D.cs
 *  - rotates the GameObject to face the mouse cursor
 *  - uses a smoothing variable to animate over time
 *  - uses an adjustment angle variable to adjust for artwork facing a different way
 * 
 * 
 * 
 * public variables:
 *  - theCamera
 *    - the main camera of the game
 *    - used to calculate the mouse position in the game world
 *    
 *  - smoothing
 *    - the speed of rotation towards the mouse cursor
 *  
 *  - adjustmentAngle
 *    - adjusts the angle if the artwork is facing a different rotation
 *    
 *    
 * private variables: none
 *    
 *    
 * Monobehaviour methods
 *  - Update
 *    - runs contantly (30-60 times per second)
 *    - only runs while this script is active
 *    - see link: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html
 * 
 * 
 *******************************************************/
using UnityEngine;

public class MouseSmoothLook2D : MonoBehaviour
{
    /*********************
     * 
     * PUBLIC VARIABLES
     * A "public" variable is declared outside of any methods.
     * We can change these in the editor without changing any of the code.
     * 
     *********************/

    /*
     * theCamera
     * we will want to get the mouse position, the Camera component has funcitonality to do this
     * set this variable in the editor by dragging the a GameObject with a Camera component onto this variable in the editor
     * The Camera we want is the main game Camera
     * see link: https://docs.unity3d.com/ScriptReference/Camera.html
     */
    public Camera theCamera;


    /*
     * smoothing
     * smoothing is a float (a decimal number) to set how fast we rotate towards the target.
     * smoothing has a default setting of "5.0f", this setting can be changed in the editor!
     * NOTE: ALL float number values MUST end with an "f"
     */
    public float smoothing = 5.0f;


    /*
     * adjustmentAngle
     * this will add a number to the rotation of the GameObject, allowing us to adjust it so our GameObject's art is facing the right direction
     * we can set this in the editor
     */
    public float adjustmentAngle = 0.0f;


    /*
     * Update
     * A method (also known as an "Event Function") provided by Monobehaviour that will run constantly.
     * Update is a general purpose update method, used for any types code, but may look a little "jerky" running Physics movement using it.
     * see link: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html
     * 
     * In this method we will be getting our current position, the mouse position and calculating an angle from those two positions.
     * We will animate the rotation to smoothly rotate towards the mouse cursor
     */
    void Update()
    {
        /*
         * GET THIS GAMEOBJECT'S CURRENT POSITION
         * we can get our current position from the Transform component using "transform.position"
         * see link: https://docs.unity3d.com/ScriptReference/Transform-position.html
         * "transform.position" is a Vector3, which has 3 properties: X, Y and Z.
         * see link: https://docs.unity3d.com/ScriptReference/Vector3.html
         * we create a Vector3 variable to store our transform's current position
         */
        Vector3 currentPosition = transform.position;


        /*
         * GET THIS GAMEOBJECT'S CURRENT ROTATION
         * we can get our current rotation from the Transform component using "transform.rotation"
         * see link: https://docs.unity3d.com/ScriptReference/Transform-rotation.html
         * "transform.rotation" is a Quaternion, which has 4 properties: X, Y, Z and W.
         * see link: https://docs.unity3d.com/ScriptReference/Quaternion.html
         * we create a Quaternion variable to store our transform's current rotation
         */
        Quaternion currentRotation = transform.rotation;


        /*
         * GET THE MOUSE POSITION ON SCREEN
         * we can get the screen X and Y coordinates of the mouse using "Input.mousePosition"
         * see link: https://docs.unity3d.com/ScriptReference/Input-mousePosition.html
         * from this we can get the mouse position in the scene (in "3D" space) later!
         */
        Vector3 mousePosition = Input.mousePosition;


        /*
         * GET THE MOUSE POSITION IN THE SCENE
         * the Camera has a "ScreenToWorldPoint" method that will get our mouse position in the scene
         * this returns a Vector3, which we can use later.
         * see link: https://docs.unity3d.com/ScriptReference/Camera.ScreenToWorldPoint.html
         */
        Vector3 targetMousePosition = theCamera.ScreenToWorldPoint(mousePosition);


        /*
         * GET THE DIFFERENCE BETWEEN THE MOUSE POSITION AND OUR CURRENT POSITION
         * To get the angle between the mouse position and our GameObject position, we minus the 
         * mouse position from our GameObject position.
         * We store this in a Vector3 variable for use later.
         */
        Vector3 difference = targetMousePosition - currentPosition;


        /*
         * GET THE ANGLE BETWEEN THE MOUSE AND GAMEOBJECT USING ATAN2
         * We are interested in setting the Z rotation angle on our GameObject
         * We can get the Z angle by using "Mathf.Atan2" if we give it the X and Y positions.
         * see link: https://docs.unity3d.com/ScriptReference/Mathf.Atan2.html
         * "Math.Atan2" will give us an angle in RADIANS, NOT DEGREES!
         * To fix this and covert the angle to degrees, we can multiply our angle by "Mathf.Rad2Deg"
         * see link: https://docs.unity3d.com/ScriptReference/Mathf.Rad2Deg.html
         */
        float angleZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;


        /*
         * CREATE A Vector3 TO STORE OUR ROTATION IN DEGREES
         * We will create a Vector3 and store our "angleZ" in the Z property
         */
        Vector3 rotationInDegrees = new Vector3();

        /*
         * SET THE X AND Y TO ZERO
         * We don't want to rotate our GameObject on X and Y, so we set them to zero
         */
        rotationInDegrees.x = 0;
        rotationInDegrees.y = 0;

        /*
         * SET THE Z ANGLE TO OUR NEW ANGLE AND ADD IN OUR ADJUSTMENT
         * here we set the Z angle to our new "angleZ" and add our pulic variable "adjustmentAngle"
         * this means we can set the correct facing angle in the editor!
         */
        rotationInDegrees.z = angleZ + adjustmentAngle;


        /*
         * CONVERT THE ROTATION FROM DEGREES TO RADIANS
         * Here we turn the "rotationInDegrees" into a Quaternion, and also covert it's properties from degrees to radians.
         * We can do this using "Quaternion.Euler"
         * see link: https://docs.unity3d.com/ScriptReference/Quaternion.Euler.html
         * Our "transform.rotation" will require a Quaternion with radians instead of degrees.
         * we create a new Quaternion and use "Quaternion.Euler" to convert our Vector3 into a Quaternion with radians instead of degrees
         * 
         * NOTE: unity refers to degree rotations as "Euler Angles"
         */
        Quaternion rotationInRadians = Quaternion.Euler(rotationInDegrees);


        /*
         * SET THE ROTATION SPEED
         * we want to set our speed to the public variable "smoothing" with the amount of time between updates, "Time.deltaTime"
         * see link: https://docs.unity3d.com/ScriptReference/Time-deltaTime.html
         */
        float rotationSpeed = Time.deltaTime * smoothing;


        /*
         * USE "LERP" TO MOVE TO THE NEW ROTATION
         * "Lerp" is short for "Linear Interpolation" - or movement over time!
         * we set our current position (transform.rotation) to rotate a little bit towards the mouse cursor every update
         * see link: https://docs.unity3d.com/ScriptReference/Quaternion.Lerp.html
         * we give Quaternion.Lerp our current rotation (currentRotation), the new rotation (rotationInRadians) and a speed to rotate by (rotationSpeed)
         */
        transform.rotation = Quaternion.Lerp(currentRotation, rotationInRadians, rotationSpeed);
    }
}
