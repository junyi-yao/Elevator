using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class RequestHandler
{
    private readonly List<InsideRequest> _insideRequests = new List<InsideRequest>();
    private readonly List<OutsideRequest> _outsideRequests = new List<OutsideRequest>();
    private int _maxFloor = 20;
    private int _minFloor = 1;



    public List<InsideRequest> GetInsideRequests() { return _insideRequests; }
    public List<OutsideRequest> GetOutsideRequests() {  return _outsideRequests; }
    public int GetMaxFloor() { return _maxFloor; }
    public int GetMinFloor() { return _minFloor; }
    public void AddRequest(string? requeststr)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(requeststr))
            {
                throw new ArgumentException("Request cannot be empty");
            }

            requeststr = requeststr.Trim();

            if(requeststr.Length > 3)
            {
                throw new ArgumentException("Not a valid Request");
            }

            int floor;

            bool requestIsNumber = Int32.TryParse(requeststr, out floor);


            if(requestIsNumber)
            {
                //add an Inside request here

                if (floor > _maxFloor || floor < _minFloor)
                {
                    throw new ArgumentException("Floor is not in the right range");
                }

                AddInsideRequest(floor);
            }

            else
            {
                //add an outside request here
                var stringLength = requeststr.Length;
                var floorstr = requeststr.Substring(0, stringLength - 1);
                var direction = requeststr.Substring(stringLength - 1); 
                floor = Convert.ToInt32(floorstr);//an exception will occur if we cannot convert to int

                if (floor > _maxFloor || floor < _minFloor)
                {
                    throw new ArgumentException("Floor is not in the right range");
                }
                direction = direction.ToLower();

                if (direction == "u")
                {
                    if(floor == _maxFloor)
                    {
                        throw new ArgumentException("you cannot go up on top floor");
                    }
                    AddOutsideRequest(floor, Direction.Up);
                }
                else if(direction == "d")
                {
                    if(floor == _minFloor)
                    {
                        throw new ArgumentException("you cannot go down on bottom floor");
                    }
                    AddOutsideRequest(floor, Direction.Down);
                }
                else { throw new ArgumentException("Direction is not valid"); }

            }
            




        }
        catch (Exception ex)
        {
            Console.WriteLine("An Exception Occurred when trying to add a Request");
            Console.WriteLine(ex.Message);
        }
    }


    private void AddInsideRequest(int floor)
    {
        var newRequest = new InsideRequest(floor);
        if (InsideRequestExist(newRequest))
        {
            Console.WriteLine("this Inside Request already exists");
            return;
        }
        _insideRequests.Add(newRequest);
        Console.WriteLine($"***Added Inside request for floor {floor}***");
    }

    private void AddOutsideRequest(int floor, Direction direction)
    {
        var newRequest = new OutsideRequest(floor, direction);

        if(OutsideRequestExist(newRequest)) {

            Console.WriteLine("this Outside Request already exists");
            return; 
        
        }
        _outsideRequests.Add(newRequest);
        Console.WriteLine($"***Added Outside request for floor {floor} with direction {direction}***");
    }

    private bool InsideRequestExist(InsideRequest request)
    {
        foreach (InsideRequest insideRequest in _insideRequests)
        {
            if (request.Floor == insideRequest.Floor)
            {
                return true;
            }
        }
        return false;
    }

    private bool OutsideRequestExist(OutsideRequest request)
    {
        foreach (OutsideRequest outsideRequest in _outsideRequests)
        {
            if (request.Floor == outsideRequest.Floor && request.RequestDirection.Equals(outsideRequest.RequestDirection))
            {
                return true;
            }
        }
        return false;
    }

    public void PrintInsideRequest()
    {
        Console.WriteLine("All InsideRequests:");
        foreach (var request in _insideRequests)
        {
            Console.WriteLine("floor:{0}", request.Floor);
        }
    }
    public void PrintOutsideRequest()
    {
        Console.WriteLine("All OutsideRequests:");
        foreach (var outsideRequest in _outsideRequests)
        {
            Console.WriteLine("floor:{0}, direction:{1}", outsideRequest.Floor, outsideRequest.RequestDirection);
        }
    }
}