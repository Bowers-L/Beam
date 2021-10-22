using UnityEngine;

namespace Beam.Triggers
{
    class PlaneTrigger : Trigger
    {
        /*
        /* Def. of plane:
         * d - How far from the origin the plane is (in direction of normal vector, can be negative)
         * theta - Angle from x axis to the vector on the xz plane (CCW)
         * phi - -PI/2 to PI/2 starting at the xz plane and going towards the y-axis.
         * theta and phi together determine the normal vector.
         * EX: The xz plane is (0, 0, PI/2), zy plane is (0, 0, 0), xy plane is (0, PI/2, 0)
         * Yup, not doing this rn.
        public Vector3 plane;
        */

        public float killPlaneY;

        public void Update()
        {
            if (transform.position.y < killPlaneY)
            {
                Debug.Log("Destroyed");
                activate();
                Destroy(this.gameObject);
            }
        }
    }
}
