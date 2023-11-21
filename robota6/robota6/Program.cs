// See https://aka.ms/new-console-template for more information
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.CompilerServices;
public class Viewer
{ 
    private readonly int viewerNumber;
    public int ViewerNumber => viewerNumber;
    public Viewer(int number)
    {
        viewerNumber = number;
    }
}
public class Cinema
{
    private readonly int totalSeats;
    private int occupiedSeats;
    public event EventHandler NotPlaces;
    public Cinema(int seats)
    {
        totalSeats = seats;
        occupiedSeats = 0;
    }
    public void PushViewer(Viewer viewer)
    {
        occupiedSeats++;
        Console.ForegroundColor = System.ConsoleColor.Green;
        Console.WriteLine($"Глядач {viewer.ViewerNumber} зайняв своє мiсце.");

        if (occupiedSeats == totalSeats)
        {
            OnNotPlaces();
        }
    }
    protected virtual void OnNotPlaces()
    {
        NotPlaces?.Invoke(this, EventArgs.Empty);
    }
}
public class Security
{
    public void CloseZal(object sender, EventArgs e)
    {
        Console.ForegroundColor = System.ConsoleColor.Red;
        Console.WriteLine("Дежурний закрив зал");
        OnSwitchOff();
    }
    public event EventHandler SwitchOff;
    protected virtual void OnSwitchOff()
    {
        SwitchOff?.Invoke(this, EventArgs.Empty);
    }
}
public class Light
{
    public void Turn(object sender, EventArgs e)
    {
        Console.ForegroundColor = System.ConsoleColor.Yellow;
        Console.WriteLine("Вимикаємо свiтло!");
        OnBegin();
    }
    public event EventHandler Begin;
    protected virtual void OnBegin()
    {
        Begin?.Invoke(this, EventArgs.Empty);
    }
}
public class Hardware
{
    private readonly string filmName;
    public Hardware(string name)
    {
        filmName = name;
    }
    public void FilmOn(object sender, EventArgs e)
    {
        Console.ForegroundColor = System.ConsoleColor.DarkGreen;
        Console.WriteLine($"Починається фiльм {filmName}");
    }
}
class Program
{
    static void Main()
    {
        Console.ForegroundColor = System.ConsoleColor.Blue;
        Console.Write("Введiть кiлькiсть мiсць у залi: ");
        int seats = int.Parse(Console.ReadLine());
        Console.Write("Введiть назву фiльму: ");
        string filmName = Console.ReadLine();
        Cinema cinema = new Cinema(seats);
        Security security = new Security();
        Light light = new Light();
        Hardware hardware = new Hardware(filmName);
        cinema.NotPlaces += security.CloseZal;
        security.SwitchOff += light.Turn;
        light.Begin += hardware.FilmOn;
        for (int i = 1; i <= seats; i++)
        {
            Viewer viewer = new Viewer(i);
            cinema.PushViewer(viewer);
        }
    }
}