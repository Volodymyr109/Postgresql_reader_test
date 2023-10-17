using System;
using System.Data;
using Npgsql;

class Program
{
    // static async System.Threading.Tasks.Task Main(string[] args)
    // {
    //     var connString = "Host=localhost;Port=5432;Username=postgres;Password=1234;Database=test";

    //     await using var conn = new NpgsqlConnection(connString);
    //     await conn.OpenAsync();

    //     // Daten in die Artikel-Tabelle einfügen
    //     Artikel newArtikel = new Artikel { ArtikelNr = 00001, Name = "BMW", Preis = 50000.0 };
    //     Artikel.Insert(newArtikel, conn);

    //     // Daten in die Kunde-Tabelle einfügen
    //     Kunde newKunde = new Kunde { KundenNr = 00010, Name = "HERR Benz", Strasse = "MercedesStrasse", Ort = "HH" };
    //     Kunde.Insert(newKunde, conn);

    //     // Daten in die Rechnung-Tabelle einfügen
    //     Rechnung newRechnung = new Rechnung { KundenId = newKunde.KundenId, ArtikelId = newArtikel.ArtikelId, Anzahl = 3 };
    //     Rechnung.Insert(newRechnung, conn);

    //     // Daten aus den Tabellen in CSV-Dateien schreiben
    //     List<Artikel> artikelList = new List<Artikel> { newArtikel };
    //     Artikel.WriteToCSV(artikelList, "Artikel1.csv");

    //     List<Kunde> kundeList = new List<Kunde> { newKunde };
    //     Kunde.WriteToCSV(kundeList, "Kunde1.csv");

    //     List<Rechnung> rechnungList = new List<Rechnung> { newRechnung };
    //     Rechnung.WriteToCSV(rechnungList, "Rechnung1.csv");
    // }
    static async System.Threading.Tasks.Task Main(string[] args)
    {
        var connString = "Host=localhost;Port=5432;Username=postgres;Password=1234;Database=test";

        await using var conn = new NpgsqlConnection(connString);
        await conn.OpenAsync();

        Console.WriteLine("Daten aus der Artikel-Tabelle:");
        await using (var artikelCmd = new NpgsqlCommand("SELECT * FROM artikel", conn))
        await using (var artikelReader = await artikelCmd.ExecuteReaderAsync())
        {
            while (await artikelReader.ReadAsync())
            {
                Console.WriteLine($"{artikelReader["ArtikelId"]}, {artikelReader["ArtikelNr"]}, {artikelReader["Name"]}, {artikelReader["Preis"]}");
            }
        }

        Console.WriteLine("\nDaten aus der Kunde-Tabelle:");
        await using (var kundeCmd = new NpgsqlCommand("SELECT * FROM kunde", conn))
        await using (var kundeReader = await kundeCmd.ExecuteReaderAsync())
        {
            while (await kundeReader.ReadAsync())
            {
                Console.WriteLine($"{kundeReader["KundenId"]}, {kundeReader["KundenNr"]}, {kundeReader["Name"]}, {kundeReader["Strasse"]}, {kundeReader["Ort"]}");
            }
        }

        Console.WriteLine("\nDaten aus der Rechnung-Tabelle:");
        await using (var rechnungCmd = new NpgsqlCommand("SELECT * FROM rechnung", conn))
        await using (var rechnungReader = await rechnungCmd.ExecuteReaderAsync())
        {
            while (await rechnungReader.ReadAsync())
            {
                Console.WriteLine($"{rechnungReader["RechnungId"]}, {rechnungReader["KundenId"]}, {rechnungReader["ArtikelId"]}, {rechnungReader["Anzahl"]}");
            }
        }
    }
}