using System;
using System.Data;
using Npgsql;
public class ArtikelModel
{
    public int ArtikelId { get; set; }
    public int ArtikelNr { get; set; }
    public string Name { get; set; }
    public double Preis { get; set; }
}
public class Artikel : ArtikelModel
{
    public static void Insert(Artikel artikel, NpgsqlConnection conn)
    {
        using (var cmd = new NpgsqlCommand("INSERT INTO artikel (ArtikelNr, Name, Preis) VALUES (@ArtikelNr, @Name, @Preis) RETURNING ArtikelId", conn))
        {
            cmd.Parameters.AddWithValue("ArtikelNr", artikel.ArtikelNr);
            cmd.Parameters.AddWithValue("Name", artikel.Name);
            cmd.Parameters.AddWithValue("Preis", artikel.Preis);

            artikel.ArtikelId = Convert.ToInt32(cmd.ExecuteScalar());
        }
    }
    // public static void WriteToCSV(List<Artikel> artikelList, string filePath)
    // {
    //     try
    //     {
    //         using (StreamWriter sw = new StreamWriter(filePath, false))
    //         {
    //             // Schreibe die Header-Zeile in die CSV-Datei
    //             sw.WriteLine("ArtikelId,ArtikelNr,Name,Preis");

    //             foreach (var artikel in artikelList)
    //             {
    //                 // Schreibe die Daten jeder Zeile in die CSV-Datei
    //                 sw.WriteLine($"{artikel.ArtikelId},{artikel.ArtikelNr},{artikel.Name},{artikel.Preis}");
    //             }

    //             Console.WriteLine("Daten in die Datei geschrieben: " + filePath);
    //         }
    //     }
    //     catch (Exception ex)
    //     {
    //         Console.WriteLine("Fehler beim Schreiben der Datei: " + ex.Message);
    //     }
    // }
}