using UnityEngine;
using System.Collections;

public class Grid
{

	public static readonly float Size = 3;

    public static Vector3 StepDelta(int x, int z)
    {
        return new Vector3(x * Grid.Size, 0, z * Grid.Size);
    }

}
