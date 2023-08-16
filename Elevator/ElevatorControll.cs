using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ElevatorControll
{
    private readonly Sensor _sensor = new Sensor();
    private readonly RequestHandler _requestHandler = new RequestHandler();
    private bool _appRunning = false;

    public ElevatorControll()
    {
        Console.WriteLine("Elevator is created");
    }



    public void ShutDown()
    {
        _appRunning = false;
        Console.WriteLine("-----------------------------------------");
        Console.WriteLine("The elevator is shut down");
        Console.WriteLine("no more request can be made");
        Console.WriteLine("remaining requests will still be served");
        Console.WriteLine("-----------------------------------------");
    }

    public void AddRequest(string? requeststr)
    {
        _requestHandler.AddRequest(requeststr);
    }

    public void PrintInsideRequest()
    {
        _requestHandler.PrintInsideRequest();
    }

    public void PrintOutsideRequest()
    {
        _requestHandler.PrintOutsideRequest();
    }
    public async Task RunAsync()
    {
        _appRunning = true;
        var hasRequest = true;

        while (_appRunning || hasRequest)//should use apprunning or request not empty
        {
           

            if (_requestHandler.GetInsideRequests().Count == 0 
                && _requestHandler.GetOutsideRequests().Count == 0)
            {
                hasRequest = false;
                //_sensor.PrintStatus();
                Console.WriteLine("+++++Currently no active request+++++");
                await Task.Delay(5000);
                continue;
            }

            hasRequest = true;


            await ServeRequestAsync();

            //_requestHandler.PrintInsideRequest();
            //_requestHandler.PrintOutsideRequest();
            
            
        }


        Console.WriteLine("The elevator has stopped running");
        _sensor.PrintStatus();



    }



    private async Task ServeRequestAsync()
    {
        //_sensor.PrintStatus();
        if(_sensor.CurrentStatus == Status.Stopped)
        {
            await ServeRequestWhenStoppedAsync();
            return;
        }
        await ServeRequestWhenMovingAsync();
        return;

    }

    private async Task ServeRequestWhenStoppedAsync()
    {
        if(_sensor.CurrentDirection == Direction.Up)
        {
            await StoppedUpAsyncs();
            return;
        }
        await StoppedDownAsync();
        return;

    }

    private async Task StoppedDownAsync()
    {
        foreach(var insideReq in _requestHandler.GetInsideRequests())
        {
            if (insideReq.Floor < _sensor.CurrentFloor)
            {
                await MoveDownAsync();
                return;
            }
            
        }
        foreach(var outsideReq in _requestHandler.GetOutsideRequests())
        {
            if(outsideReq.Floor < _sensor.CurrentFloor)
            {
                await MoveDownAsync();
                return;
            }
        }

        //now, the elevator is stopped at this floor with no request to go down
        //it should move up and serve the request to move up for the current floor
        _sensor.CurrentDirection = Direction.Up;
        RemoveOutsideRequest();
        await MoveUpAsync();

    }

    private async Task StoppedUpAsyncs()
    {
        foreach (var insideReq in _requestHandler.GetInsideRequests())
        {
            if (insideReq.Floor > _sensor.CurrentFloor)
            {
                await MoveUpAsync();
                return;
            }

        }
        foreach (var outsideReq in _requestHandler.GetOutsideRequests())
        {
            if (outsideReq.Floor > _sensor.CurrentFloor)
            {
                await MoveUpAsync();
                return;
            }
        }
        //now, the elevator is stopped at this floor with no request to go up
        //it should move down and serve the request to move down for the current floor
        _sensor.CurrentDirection = Direction.Down;
        RemoveOutsideRequest();
        await MoveDownAsync();
    }

    private async Task ServeRequestWhenMovingAsync()
    {
        if (_sensor.CurrentDirection == Direction.Up)
        {
            await MovingUpAsyncs();
            return;
        }
        await MovingDownAsync();
        return;
    }

    private async Task MovingDownAsync()
    {
       foreach(var insideReq in _requestHandler.GetInsideRequests())
        {
            if(insideReq.Floor == _sensor.CurrentFloor)
            {
                await StopAsync();
                return;
            }
        }
       foreach(var outsideReq in _requestHandler.GetOutsideRequests())
        {
            if(outsideReq.Floor == _sensor.CurrentFloor && outsideReq.RequestDirection == Direction.Down)
            {
                await StopAsync();
                return;
            }
        }
        foreach (var insideReq in _requestHandler.GetInsideRequests())
        {
            if (insideReq.Floor < _sensor.CurrentFloor)
            {
                await MoveDownAsync();
                return;
            }
        }
        foreach (var outsideReq in _requestHandler.GetOutsideRequests())
        {
            if (outsideReq.Floor < _sensor.CurrentFloor)
            {
                await MoveDownAsync();
                return;
            }
        }
        //at this point, there should only be the request to go up for the curreent floor
        //we should change the direction to go up and stop here to get the person for up
        _sensor.CurrentDirection = Direction.Up;
        await StopAsync();
    }

    private async Task MovingUpAsyncs()
    {
        foreach (var insideReq in _requestHandler.GetInsideRequests())
        {
            if (insideReq.Floor == _sensor.CurrentFloor)
            {
                await StopAsync();
                return;
            }
        }
        foreach (var outsideReq in _requestHandler.GetOutsideRequests())
        {
            if (outsideReq.Floor == _sensor.CurrentFloor && outsideReq.RequestDirection == Direction.Up)
            {
                await StopAsync();
                return;
            }
        }
        foreach (var insideReq in _requestHandler.GetInsideRequests())
        {
            if (insideReq.Floor > _sensor.CurrentFloor)
            {
                await MoveUpAsync();
                return;
            }
        }
        foreach (var outsideReq in _requestHandler.GetOutsideRequests())
        {
            if (outsideReq.Floor > _sensor.CurrentFloor)
            {
                await MoveUpAsync();
                return;
            }
        }
        //at this point, there should only be the request to go down for the curreent floor
        //we should change the direction to go down and stop here to get the person for down
        _sensor.CurrentDirection = Direction.Down;
        await StopAsync();
    }

    private async Task MoveUpAsync()
    {
        Console.WriteLine("elevator moving up");
        _sensor.CurrentStatus = Status.Moving;
        _sensor.CurrentDirection = Direction.Up;
        await Task.Delay(3000);
        _sensor.CurrentFloor++;
        Console.WriteLine($"arrived at floor {_sensor.CurrentFloor}");

    }

    private async Task MoveDownAsync()
    {
        Console.WriteLine("elevator moving down");
        _sensor.CurrentStatus = Status.Moving;
        _sensor.CurrentDirection = Direction.Down;
        await Task.Delay(3000);
        _sensor.CurrentFloor--;
        Console.WriteLine($"arrived at floor {_sensor.CurrentFloor}");
    }


    private async Task StopAsync()
    {
        Console.WriteLine($"Elevator stopped, door open at floor {_sensor.CurrentFloor}");
        _sensor.CurrentStatus = Status.Stopped;
        RemoveInsideRequest();
        RemoveOutsideRequest();
        await Task.Delay(1000);


    }
    private void RemoveInsideRequest()
    {
        for(int i = 0; i < _requestHandler.GetInsideRequests().Count; i++)
        {
            var insideReq = _requestHandler.GetInsideRequests()[i];
            if (insideReq.Floor == _sensor.CurrentFloor)
            {
                _requestHandler.GetInsideRequests().Remove(insideReq);
            }
        }

    }


    private void RemoveOutsideRequest()
    {

        for (int i = 0; i <  _requestHandler.GetOutsideRequests().Count; i++)
        {
            var outsideReq = _requestHandler.GetOutsideRequests()[i];
            if (outsideReq.Floor == _sensor.CurrentFloor && outsideReq.RequestDirection == _sensor.CurrentDirection)
            {
                _requestHandler.GetOutsideRequests().Remove(outsideReq);
            }

        }

    }


}