using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ISearchAlgorithm
{
    public bool isFind = false;
    public abstract void SearchWay(Coordinate start, Coordinate end);

    public abstract List<Coordinate> BackTrace(Coordinate start, Coordinate end);
}
