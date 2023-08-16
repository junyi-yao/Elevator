using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;




public class Sensor
{
    public Direction CurrentDirection { get; set; } = Direction.Up;
    public Status CurrentStatus { get; set; } = Status.Stopped;
    public int CurrentFloor { get; set; } = 1;
    
    //when we do not need go further, we should change direction!
    public int NextFloor
    {
        get
        {
            if(CurrentDirection == Direction.Up)
            {
                return CurrentFloor + 1;
            }
            else
            {
                return CurrentFloor - 1;
            }
        }
    }

    public void PrintStatus()
    {
        Console.WriteLine("--------current elevator status-------------");
        Console.WriteLine($"the elevator is at floor {CurrentFloor}, it is {CurrentStatus} with Direction {CurrentDirection}");
    }

}
