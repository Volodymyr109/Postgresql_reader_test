using System;
using System.Data;
using Npgsql;
public class RechnungModel
{
    public int RechnungId { get; set; }
    public int KundenId { get; set; }
    public int ArtikelId { get; set; }
    public int Anzahl { get; set; }
}
public class Rechnung : RechnungModel
{
    public static void Insert(Rechnung rechnung, NpgsqlConnection conn)
    {
        using (var cmd = new NpgsqlCommand("INSERT INTO rechnung (KundenId, ArtikelId, Anzahl) VALUES (@KundenId, @ArtikelId, @Anzahl) RETURNING RechnungId", conn))
        {
            cmd.Parameters.AddWithValue("KundenId", rechnung.KundenId);
            cmd.Parameters.AddWithValue("ArtikelId", rechnung.ArtikelId);
            cmd.Parameters.AddWithValue("Anzahl", rechnung.Anzahl);

            rechnung.RechnungId = Convert.ToInt32(cmd.ExecuteScalar());
        }
    }
    // public static void WriteToCSV(List<Rechnung> rechnungList, string filePath)
    // {
    //     try
    //     {
    //         using (StreamWriter sw = new StreamWriter(filePath, false))
    //         {
    //             // Schreibe die Header-Zeile in die CSV-Datei
    //             sw.WriteLine("RechnungId,KundenId,ArtikelId,Anzahl");

    //             foreach (var rechnung in rechnungList)
    //             {
    //                 // Schreibe die Daten jeder Zeile in die CSV-Datei
    //                 sw.WriteLine($"{rechnung.RechnungId},{rechnung.KundenId},{rechnung.ArtikelId},{rechnung.Anzahl}");
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