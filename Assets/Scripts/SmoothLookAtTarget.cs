/*******************************************************
 * 
 * SmoothLookAtTarget.cs
 *  - rotates the GameObject to face a target Transform
 *  - uses a smoothing variable to animate over time
 *  - uses an adjustment angle variable to adjust for artwork facing a different way
 * 
 * 
 * 
 * public variables:
 *  - target
 *    - the target Transform to rotate towards
 *    
 *  - smoothing
 *    - the speed of rotation towards the target
 *  
 *  - adjustmentAngle
 *    - adjusts the angle if the artwork is facing a different rotation
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


/*
 * The MoveTowardsTarget class inherits from Monobehaviour, which lets us add it as a component to a GameObject
 * see link: https://docs.unity3d.com/ScriptReference/MonoBehaviour.html
 */
public class SmoothLookAtTarget : MonoBehaviour
{

    /*
     * target
     * The Transform component of the GameObject we want to rotate towards
     * This could be the player GameObject!
     * see link: https://docs.unity3d.com/ScriptReference/Transform.html
     */
    public Transform target;


    /*
     * smoothing
     * smoothing is a float (a decimal number) to set how fast we rotate towards the target.
     * smoothing has a default setting of "5.0f", this setting can be changed in the editor!
     */
    public float smoothing = 5.0f;


    /*
     * adjustmentAngle
     * this will add a number to the rotation of the GameObject, allowing us to adjust it so our GameObject's 
     * art is facing the right direction. 
     * We can set this in the editor
     */
    public float adjustmentAngle = 0.0f;


    /*
     * Update
     * this method is provided by Monobehaviour that runs CONSTANTLY (30-60 times per second) while this GameObject is active
     * see link: https://docs.unity3d.com/ScriptReference/MonoBehaviour.Update.html
     * we can use this to rotate our GameObject a small amount each time it runs
     */
    private void Update()
    {
        /*
         * CHECK WE HAVE A TARGET TRANSFORM
         * if the public variable, target has NOT been set in the editor we cannot run the movement code!
         * We check here it is set, otherwise we will get an error!
         * In unity, we use "null" to mean "empty" or "no value"
         * We use the != (not equal) operator to check the target is NOT EQUAL to null
         * see link: https://docs.microsoft.com/en-us/dotnet/csharp/language-reference/operators/not-equal-operator
         */
        if (target != null)
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
             * GET OUR TARGET'S CURRENT POSITION
             * Our target variable is a Transform, we can get it's current position using target.position
             * see link: https://docs.unity3d.com/ScriptReference/Transform-position.html
             */
            Vector3 targetPosition = target.position;

            /*
             * GET THE DIFFERENCE BETWEEN THE TARGET POSITION AND OUR CURRENT POSITION
             * To get the angle between the target position and our GameObject position, we minus the 
             * target position from our GameObject position.
             * We store this in a Vector3 variable for use later.
             */
            Vector3 difference = targetPosition - currentPosition;


            /*
             * GET THE ANGLE BETWEEN THE TARGET AND GAMEOBJECT USING ATAN2
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
             * we set our current position (transform.rotation) to rotate a little bit towards the target every update
             * see link: https://docs.unity3d.com/ScriptReference/Quaternion.Lerp.html
             * we give Quaternion.Lerp our current rotation (currentRotation), the new rotation (rotationInRadians) and a speed to rotate by (rotationSpeed)
             */
            transform.rotation = Quaternion.Lerp(currentRotation, rotationInRadians, rotationSpeed);
        }
    }
}
