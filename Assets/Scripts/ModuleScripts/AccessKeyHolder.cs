using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccessKeyHolder : MonoBehaviour
{
    //These store access keys for the grid data on the individul modules so I can
    //access them later for branch generation/post-generation manipulation.
    public int xAccessKey;
    public int yAccessKey;

    //Putting a string here so I can tell whether a module was added in the branch or
    //trunk phase. Mostly a debugging tool.
    public string phaseDesignation;
}
