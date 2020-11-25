using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Animations;


public enum JointDirection
{
    DIRECT=1,
    STRAIGHT=0,
    REVERSE=-1
}

public enum GearApplication{
    Guided,
    Unguided,
    ScaleBased
}



public class Gear : MonoBehaviour
{
    public int ToothCount;
    public float DiameterMillimiters;
    public float ThicknessMillimiters;
    public List<Gear> JointGears;
    public List<JointDirection> Direction;
    public float AngularVelocity;
    public Axis axis;
    public GearApplication app;
    public float torque;
    
    public float ratio=1f;
    //public float ratioPrint=1f;
    
    public bool printer;
    private Vector3 rotate;
    private Rigidbody r;
    private Transform t;
    
    
    // Start is called before the first frame update
    void Start()
    {
        rotate = new Vector3();
        
        r = gameObject.GetComponent<Rigidbody>();
        t = gameObject.GetComponent<Transform>();
        r.maxAngularVelocity = Mathf.Infinity;

        switch (app)
        {
            case GearApplication.Guided:
                DiameterMillimiters = t.localScale.x * 100;
                ThicknessMillimiters = t.localScale.z * 100;
                break;
            case GearApplication.Unguided:
                break;
            case GearApplication.ScaleBased:
                
                Vector3 scale;
                scale.x = DiameterMillimiters / 100;
                scale.y = DiameterMillimiters / 100;
                scale.z = ThicknessMillimiters / 100;
                t.localScale = scale;
                break;
            
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
        rotate =Vector3.zero;

        switch (axis)
        {
            case Axis.X:
                
                rotate.x = AngularVelocity;
                r.angularVelocity = rotate;
                break;
            case Axis.Y:
                
                rotate.y = AngularVelocity;
                r.angularVelocity = rotate;
                break;
            case Axis.Z:
                
                rotate.z = AngularVelocity;
                r.angularVelocity = rotate;
                break;
        }
            
        
        
        if(JointGears.Count==0)
            return;
        int i = 0;
        
        foreach (var g in JointGears)
        {
            ratio = (float) ToothCount / (float) g.ToothCount;
            switch (Direction[i])
            {
                case JointDirection.DIRECT:
                    
                    g.AngularVelocity = AngularVelocity * (ratio);
                    if (app == GearApplication.Unguided)
                    {
                        g.torque = torque / ratio;
                    }
                    break;
                case JointDirection.REVERSE:
                    g.AngularVelocity = -AngularVelocity * (ratio);
                    if (app == GearApplication.Unguided)
                    {
                        g.torque = torque / ratio;
                    }
                    break;
                case JointDirection.STRAIGHT:
                    g.AngularVelocity = AngularVelocity;
                    if (app == GearApplication.Unguided)
                    {
                        g.torque = torque ;
                    }
                    break;
            }

            i++;



        } 
        
        
        
        
    }
}
