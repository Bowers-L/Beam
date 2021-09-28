

using UnityEngine;
using UnityEngine.Events;

using Beam.Events;
using Beam.Utility;

using System.Collections;
using System.Collections.Generic;

namespace Beam.Core.Beams
{
    public enum BeamType
    {
        None,
        Grab,
        Swap
    }

    public class BeamShot : UnityEvent<BeamSource, Ray> { }
    public class BeamRelease : UnityEvent<BeamSource> { }
    public class BeamSourceMoved : UnityEvent<BeamSource> { }

    public abstract class BeamSource : MonoBehaviour
    {
        public float maxBeamRange;  //The maximum distance the beam can travel to latch onto an object
        public float maxBeamFlex;   //The maximum angle between an object and the player's cursor before the beam breaks.
        public float beamSnapSpeed;

        public GameObject beamEffectPrefab;
        public GameObject beamEffectPos;

        protected BeamTarget currTarget;
        protected BeamType currBeamType;

        [HideInInspector]
        public BeamSourceEffect beamEffectInst;

        public void FixedUpdate()
        {
            if (currTarget != null)
            {
                Ray beamDir = new Ray(transform.position, currTarget.transform.position - transform.position);

                RaycastHit hitInfo;
                if (Physics.Raycast(beamDir, out hitInfo, currTarget.currBeamDist, GetLayerMask(currBeamType)))
                {
                    if (!hitInfo.transform.gameObject.Equals(currTarget.gameObject))
                    {
                        DeactivateBeam();
                    }
                }
            }

            if (beamEffectPrefab == null)
            {
                Debug.LogWarning("Beam source is missing effect prefab");
            }
        }

        public void GrabBeam(Ray beamRay)
        {
            EventManager.InvokeEvent<BeamShot, BeamSource, Ray>(this, beamRay);
            List<Ray> r1 = new List<Ray>();
            BeamTarget target = FindTarget(beamRay, BeamType.Grab, r1);
            if ( target != null)
            {
                currTarget = target;
                currBeamType = BeamType.Grab;

                GrabBeamEffect();

                target.AttachBeam(this, r1[r1.Count - 1]);
            }
        }

        void GrabBeamEffect()
        {
            beamEffectInst = Instantiate(beamEffectPrefab).GetComponent<BeamSourceEffect>();
            beamEffectInst.GetComponent<BeamSourceEffect>().SetPos(beamEffectPos.transform.position, currTarget.transform.position, transform.forward);
        }

        public void DeactivateBeam()
        {
            if (currTarget != null)
            {
                currTarget.DetachBeam();
                currTarget = null;
                EventManager.InvokeEvent<BeamRelease, BeamSource>(this);
            }

            if (beamEffectInst != null)
            {
                Destroy(beamEffectInst.gameObject);
            }

            currBeamType = BeamType.None;
        }

        public virtual void SwapBeam(Ray beamRay)
        {
            //Note: This function is overrided by the player in PlayerBeamSource.cs
            //Also, it will eventually need to be modified/replaced to account for the time of the VFX.

            List<Ray> r1 = new List<Ray>();
            currTarget = FindTarget(beamRay, BeamType.Swap, r1);


            if (currTarget != null)
            {
                Vector3 tempPos = transform.position;
                transform.position = currTarget.transform.position;
                currTarget.transform.position = tempPos;
                currBeamType = BeamType.Grab;
                DeactivateBeam();
            }

        }

        protected BeamTarget FindTarget(Ray beamRay, BeamType type, List<Ray> output)
        {
            if (output == null || output.Count > 0)
            { 
               output = new List<Ray>();
            }
            output.Add(beamRay);
            RaycastHit hitInfo;
            if (Physics.Raycast(beamRay, out hitInfo, maxBeamRange, GetLayerMask(type)))
            {
                
                if (hitInfo.collider.gameObject.tag == "Mirror")
                {
                    Vector3 pos = hitInfo.point;
                    Vector3 dir = Vector3.Reflect(beamRay.direction, hitInfo.normal);
                    Ray r1 = new Ray(pos, dir);
                    return FindTarget(r1, type, output);
                }
                
                BeamTarget target = hitInfo.collider.GetComponentInParent<BeamTarget>();
                return target;
            }

            return null;
        }

        protected int GetLayerMask(BeamType type)
        {
            int layerMask = UnityEngineExt.GetMaskWithout("Ignore Raycast");
            switch (type)
            {
                case BeamType.Grab:
                    layerMask &= UnityEngineExt.GetMaskWithout("Allows Grab") & UnityEngineExt.GetMaskWithout("Allows Both");
                    break;
                case BeamType.Swap:
                    layerMask &= UnityEngineExt.GetMaskWithout("Allows Swap") & UnityEngineExt.GetMaskWithout("Allows Both");
                    break;
                default:
                    break;
            }

            return layerMask;
        }
    }

}
