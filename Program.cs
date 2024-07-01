
using System;
using System.Collections.Generic;
using System.IO; 
// Alap Jármű osztály
public abstract class Jarmu
{
    public string Azonosito { get; private set; }
    public int GyartasiEv { get; private set; }
    public string Rendsz { get; set; }
    public double Fogyasztas { get; set; } // liter / 100 km
    public double OsszKm { get; private set; }
    public bool Szabad { get; private set; }
    public double AlapDij { get; set; }

    public Jarmu(string azonosito, int gyartasiEv, string rendsz, double fogyasztas, double alapDij)
    {
        Azonosito = azonosito;
        GyartasiEv = gyartasiEv;
        Rendsz = rendsz;
        Fogyasztas = fogyasztas;
        AlapDij = alapDij;
        Szabad = true;
    }

    public void Fuvar(int km, double benzinAr)
    {
        if (!Szabad)
        {
            throw new InvalidOperationException("nem szabad a jarmu!");
        }
        OsszKm += km;
        Szabad = false;
    }

    public void Visszahoz()
    {
        Szabad = true;
    }

    public double UtiKoltseg(int km, double benzinAr)
    {
        return (km * Fogyasztas / 100) * benzinAr;
    }

    public abstract double BerletiDij(int km, double benzinAr);

    public override string ToString()
    {
        return $"{Azonosito}, {GyartasiEv}, {Rendsz}, {OsszKm} km, {(Szabad ? "Szabad" : "Foglalt")}";
    }
}

// Busz osztály
public class Busz : Jarmu
{
    public int FerohelySzama { get; set; }
    public static double FerohelyX { get; set; } = 1000; // Egységes szorzó az összes buszra

    public Busz(string azonosito, int gyartasiEv, string rendszam, double fogyasztas, double alapDij, int ferohelyekSzama)
        : base(azonosito, gyartasiEv, rendszam, fogyasztas, alapDij)
    {
        FerohelySzama = ferohelyekSzama;
    }

    public override double BerletiDij(int km, double benzinAr)
    {
        double utiKoltseg = UtiKoltseg(km, benzinAr);
        return AlapDij + (utiKoltseg * 1.1) + (FerohelySzama * FerohelyX); // Haszonkulcs 10%
    }

    public override string ToString()
    {
        return base.ToString() + $", {FerohelySzama} férőhely";
    }
}

// Teherautó osztály
public class Teherauto : Jarmu
{
    public double Teherbiras { get; set; } // tonnában
    public static double TeherbirasX { get; set; } = 500; // Egységes szorzó az összes teherautóra

    public Teherauto(string azonosito, int gyartasiEv, string rendszam, double fogyasztas, double alapDij, double teherbiras)
        : base(azonosito, gyartasiEv, rendszam, fogyasztas, alapDij)
    {
        Teherbiras = teherbiras;
    }

    public override double BerletiDij(int km, double benzin)
    {
        double utiKoltseg = UtiKoltseg(km, benzin);
        return AlapDij + (utiKoltseg * 1.1) + (Teherbiras * TeherbirasX); // Haszonkulcs 10%
    }

    public override string ToString()
    {
        return base.ToString() + $", {Teherbiras} tonna teherbiras";
    }
}

// Főprogram
class Program
{
    static void Main()
    {
        List<Jarmu> jarmuPark = new List<Jarmu>();

        // Néhány jármű hozzáadása
        jarmuPark.Add(new Busz("B001", 2010, "ABC-123", 20, 5000, 50));
        jarmuPark.Add(new Teherauto("T001", 2012, "XYZ-789", 30, 7000, 10));

        // Járművek listázása
        Console.WriteLine("jarmuvek szama a parkban::");
        foreach (var jarmu in jarmuPark)
        {
            Console.WriteLine(jarmu);
        }

        // Fuvar hozzáadása
        Console.WriteLine("\nfuvar hozzaadasa:");
        jarmuPark[0].Fuvar(150, 450);
        Console.WriteLine(jarmuPark[0]);
        Console.WriteLine($"A berleti dij:: {jarmuPark[0].BerletiDij(150, 450)}");

        // Jármű visszahozása
        jarmuPark[0].Visszahoz();
        Console.WriteLine(jarmuPark[0]);

        // Fuvar hozzáadása teherautóval
        Console.WriteLine("\fuvar hozzaadasa teherautoval:");
        jarmuPark[1].Fuvar(200, 450);
        Console.WriteLine(jarmuPark[1]);
        Console.WriteLine($"A berleti dij:: {jarmuPark[1].BerletiDij(200, 450)}");

        // Jármű visszahozása
        jarmuPark[1].Visszahoz();
        Console.WriteLine(jarmuPark[1]);

        Console.ReadKey();
    }
}
