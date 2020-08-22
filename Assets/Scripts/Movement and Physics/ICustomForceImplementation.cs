using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public interface ICustomForceImplementation
{

    Vector3 GetCurrentForceVector(CustomForce parentForce, ForceObject objectAppliedTo);

}
