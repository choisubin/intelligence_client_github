using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateSingleNetworkBrainNumberResponse
{
    public int statusCode;
    public List<AnsEquations> ansEquations;
    public List<Distances> distances;
    public TopCoeffs NP;
    public long calcTime;
}
