using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class OutsideRequest
{
    public int Floor { get; }
    public Direction RequestDirection { get; }
    public OutsideRequest(int floor, Direction requestDirection)
    {
        Floor = floor;
        RequestDirection = requestDirection;
    }
}
