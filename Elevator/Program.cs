// See https://aka.ms/new-console-template for more information
using System.IO;

Console.WriteLine("Hello, World!");




var ec = new ElevatorControll();



Task elevatorTask = ec.RunAsync();

while (true)
{
    //Console.WriteLine("please enter your request, enter Q to quit");
    var input = Console.ReadLine();
    if (input == "Q")
    {
        ec.ShutDown();
        break;
    }
    ec.AddRequest(input);
    //Console.WriteLine($"you entered request to {input}");
}



await elevatorTask;

//ec.PrintInsideRequest();
//ec.PrintOutsideRequest();




//ec.AddRequest("11");
//ec.AddRequest("25");
//ec.AddRequest("wsd");
//ec.AddRequest("5U");
//ec.AddRequest("6D");
//ec.AddRequest("5U");
//ec.AddRequest("6d");
//ec.AddRequest("55U");
//ec.AddRequest("asd");
//ec.PrintInsideRequest();
//ec.PrintOutsideRequest();
