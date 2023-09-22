using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
public class PlayerInfo 
{
    public Vector3 position;
    public bool green;
    public bool pink;
    public bool yellow;
    
    public PlayerInfo()
    {
        this.position = new Vector3();
        this.green = false;
        this.pink = false;
        this.yellow = false;
    }
    public PlayerInfo(Vector3 trans, bool gr, bool pi, bool ylw)
    {
        this.position = trans;
        this.green = gr;
        this.pink = pi;
        this.yellow = ylw;
    }
}
